<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SolicitudesCredito_Detalles.aspx.cs" Inherits="SolicitudesCredito_Detalles" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <title>Detalles de la solicitud</title>
    <link href="/Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/Content/css/style.css" rel="stylesheet" />
    <link href="/Content/css/icons.css" rel="stylesheet" />
    <link href="/Scripts/plugins/iziToast/css/iziToast.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/unitegallery/css/unitegallery.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/unitegallery/themes/default/ug-theme-default.css" rel="stylesheet" />
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

        .card-header {
            background-color: rgb(255,255,255) !important;
        }

        .tr-exito {
            color: #00a803;
        }
    </style>
</head>
<body>
    <form id="frmPrincipal" runat="server">
        <div class="card">
            <div class="card-header">
                <div class="row justify-content-between align-items-end">
                    <div class="col-lg-auto col-md-auto col-sm-auto col-auto">
                        <div class="form-inline p-0">
                            <div class="spinner-border" role="status" id="divCargandoAnalisis" style="display: none;">
                                <span class="sr-only">Cargando</span>
                            </div>
                            <asp:Image runat="server" ID="imgLogo" class="LogoPrestamo align-self-center d-none d-sm-block d-sm-none d-md-block d-md-none d-lg-block" alt="Logo del Producto" Style="display: none;" />
                            <asp:Label runat="server" ID="lblProducto" CssClass="h6 font-weight-bold align-self-end"></asp:Label>
                        </div>
                    </div>
                    <div class="col-lg-auto col-md-auto col-sm-auto col-auto">
                        <div class="form-inline">
                            <div class="button-items pb-2">
                                <button runat="server" id="btnHistorialExterno" type="button" class="btn btn-secondary waves-effect waves-light">
                                    <i class="fas fa-clipboard-check"></i>
                                    Buró Externo
                                </button>
                                <button runat="server" id="btnDocumentacionModal" type="button" data-toggle="modal" data-target="#modalDocumentacion" class="btn btn-secondary waves-effect waves-light">
                                    <i class="far fa-file-alt"></i>
                                    Documentos
                                </button>
                                <button runat="server" id="btnMasDetalles" type="button" class="btn btn-secondary waves-effect waves-light">
                                    <i class="fas fa-info-circle"></i>
                                    Más detalles
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row justify-content-between bg-light">
                    <div class="col-lg-auto col-md-auto col-sm-auto col-auto">
                        <i class="mdi mdi-account mdi-24px mt-1"></i>
                        <asp:Label ID="lblNombreCliente" CssClass="h6 font-weight-bold" runat="server"></asp:Label>
                    </div>
                    <div class="col-lg-auto col-md-auto col-sm-auto col-auto align-self-end">
                        <asp:Label runat="server" class="h6 font-weight-bold">Identidad:</asp:Label>
                        <asp:Label ID="lblIdentidadCliente" CssClass="h6 font-weight-bold" runat="server"></asp:Label>
                    </div>
                    <div class="col-lg-12">
                        <table class="table table-condensed mb-0">
                            <tbody>
                                <tr>
                                    <th class="text-center pt-1 pb-1">No. Solicitud:
                                        <asp:Label ID="lblNoSolicitud" CssClass="col-form-label" runat="server"></asp:Label></th>
                                    <th class="text-center pt-1 pb-1">Tipo Solicitud:
                                        <asp:Label ID="lblTipoSolicitud" CssClass="col-form-label" runat="server"></asp:Label></th>
                                    <th class="text-center pt-1 pb-1">Agente de Ventas:
                                        <asp:Label ID="lblAgenteDeVentas" CssClass="col-form-label" runat="server"></asp:Label></th>
                                    <th class="text-center pt-1 pb-1">Agencia:
                                        <asp:Label ID="lblAgencia" CssClass="col-form-label" runat="server"></asp:Label></th>
                                    <th class="text-center pt-1 pb-1">Gestor:
                                        <asp:Label ID="lblNombreGestor" CssClass="col-form-label" runat="server"></asp:Label></th>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div class="col-12">
                        <div class="table-responsive">
                            <table class="table table-sm m-0 text-center" id="tblEstadoSolicitud" runat="server">
                                <thead>
                                    <tr>
                                        <th class="text-center">Ingreso</th>
                                        <th class="text-center">Recepción</th>
                                        <th class="text-center">Analisis</th>
                                        <th class="text-center">Campo</th>
                                        <th class="text-center">Condic.</th>
                                        <th class="text-center">Reprog.</th>
                                        <th class="text-center">Validación</th>
                                        <th class="text-center">Resolución</th>
                                    </tr>
                                </thead>
                                <tbody>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
            <div class="card-body">
                <div class="collapse-group">
                    <div class="panel panel-default">
                        <div class="panel-heading p-1 bg-light border-bottom" role="tab" id="headingOne">
                            <h6 class="panel-title m-0 font-14">
                                <a role="button" data-toggle="collapse" href="#collapseInformacionPersonal" class="text-dark h6 trigger collapsed font-weight-bold" aria-expanded="true" aria-controls="collapseOne">
                                    <i class="mdi mdi-account-circle mdi-24px"></i>
                                    Informacion Personal
                                </a>
                            </h6>
                        </div>
                        <div id="collapseInformacionPersonal" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingOne">
                            <div class="panel-body">
                                <div class="row mb-0" id="divInformacionPersonal" runat="server">
                                    <div class="col-md-6">
                                        <div class="form-group row">
                                            <div class="col-12">
                                                <label class="col-form-label">RTN numérico</label>
                                                <asp:TextBox ID="txtRTNCliente" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Número de teléfono</label>
                                                <asp:TextBox ID="txtTelefonoCliente" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Nacionalidad</label>
                                                <asp:TextBox ID="txtNacionalidad" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Fecha de nacimiento</label>
                                                <asp:TextBox ID="txtFechaNacimientoCliente" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Edad</label>
                                                <asp:TextBox ID="txtEdadCliente" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Correo Electrónico</label>
                                                <asp:TextBox ID="txtCorreoCliente" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Profesion u oficio</label>
                                                <asp:TextBox ID="txtProfesionCliente" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Sexo</label>
                                                <asp:TextBox ID="txtSexoCliente" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Estado civil</label>
                                                <asp:TextBox ID="txtEstadoCivilCliente" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-6 col-md-6 border-left border-gray">
                                        <h6 class="font-weight-bold">Documentación</h6>
                                        <div class="form-group row">
                                            <div class="col-12">
                                                <!-- Div donde se muestran las imágenes de informacion personal -->
                                                <div id="divDocumentacionCedula" runat="server" style="display: none;">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-default">
                        <div class="panel-heading p-1 bg-light border-bottom" role="tab" id="headingTwo">
                            <h6 class="panel-title m-0 font-14">
                                <a href="#collapseInformacionDomiciliar" class="text-dark collapsed h6 collapsed font-weight-bold" data-toggle="collapse" aria-expanded="false" aria-controls="collapseTwo">
                                    <i class="mdi mdi-home-variant mdi-24px"></i>
                                    Informacion Domicilio
                                </a>
                            </h6>
                        </div>
                        <div id="collapseInformacionDomiciliar" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingTwo">
                            <div class="panel-body">
                                <div class="row mb-0" id="divInformacionDomicilio" runat="server">
                                    <div class="col-lg-6 col-md-6">
                                        <div class="form-group row">
                                            <div class="col-6">
                                                <label class="col-form-label">Tipo de vivienda</label>
                                                <asp:TextBox ID="txtTipoDeVivienda" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Tiempo de residir</label>
                                                <asp:TextBox ID="txtTiempoDeResidir" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Departamento</label>
                                                <asp:TextBox ID="txtDepartamentoDomicilio" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Municipio</label>
                                                <asp:TextBox ID="txtMunicipioDomicilio" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Ciudad/Poblado</label>
                                                <asp:TextBox ID="txtCiudadPobladoDomicilio" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Barrio/colonia</label>
                                                <asp:TextBox ID="txtBarrioColoniaDomicilio" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-12">
                                                <label class="col-form-label">Dirección detallada</label>
                                                <textarea id="txtDireccionDetalladaDomicilio" runat="server" readonly="readonly" class="form-control form-control-sm"></textarea>
                                            </div>
                                            <div class="col-12">
                                                <label class="col-form-label">Referencias del domicilio</label>
                                                <textarea id="txtReferenciasDomicilio" runat="server" readonly="readonly" class="form-control form-control-sm"></textarea>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-6 col-md-6 border-left border-gray">
                                        <h6 class="font-weight-bold">Documentación</h6>
                                        <div class="form-group row">
                                            <div class="col-12">
                                                <!-- Div donde se muestran las imágenes de informacion domicilio -->
                                                <div class="align-self-center" id="divDocumentacionDomicilio" runat="server" style="display: none;">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-default" id="divPanelInformacionConyugal" runat="server">
                        <div class="panel-heading p-1 bg-light border-bottom" role="tab" id="headingThree">
                            <h6 class="panel-title m-0 font-14">
                                <a href="#collapseThree" class="text-dark collapsed h6 font-weight-bold" data-toggle="collapse" aria-expanded="false" aria-controls="collapseThree">
                                    <i class="mdi mdi-account-multiple mdi-24px"></i>
                                    Informacion Conyugal
                                </a>
                            </h6>
                        </div>
                        <div id="collapseThree" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingThree">
                            <div class="panel-body">
                                <div class="row mb-0" id="divInformacionConyugal" runat="server">
                                    <div class="col-lg-6 col-md-12 border-right border-gray">
                                        <div class="form-group row">

                                            <div class="col-12">
                                                <label class="col-form-label">Nombre completo</label>
                                                <asp:TextBox ID="txtNombreDelConyugue" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Identidad</label>
                                                <asp:TextBox ID="txtIdentidadConyugue" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Telefono</label>
                                                <asp:TextBox ID="txtTelefonoConyugue" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Fecha de nacimiento</label>
                                                <asp:TextBox ID="txtFechaNacimientoConyugue" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Lugar de trabajo</label>
                                                <asp:TextBox ID="txtLugarDeTrabajoConyugue" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Ingreso mensual</label>
                                                <asp:TextBox ID="txtIngresosMensualesConyugue" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>

                                            <div class="col-6">
                                                <label class="col-form-label">Teléfono del trabajo</label>
                                                <asp:TextBox ID="txtTelefonoTrabajoConyugue" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-default">

                        <div class="panel-heading p-1 bg-light border-bottom" role="tab" id="headingFour">
                            <h6 class="panel-title m-0 font-14">
                                <a href="#collapseInformacionLaboral" class="text-dark collapsed h6 font-weight-bold" data-toggle="collapse" aria-expanded="false" aria-controls="collapseThree">
                                    <i class="mdi mdi-briefcase-check mdi-24px"></i>
                                    Informacion Laboral
                                </a>
                            </h6>
                        </div>
                        <div id="collapseInformacionLaboral" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour">
                            <div class="panel-body">
                                <div class="row mb-0" id="divInformacionLaboral" runat="server">
                                    <div class="col-lg-6 col-md-6">
                                        <div class="form-group row">
                                            <div class="col-12">
                                                <label class="col-form-label">Nombre del trabajo</label>
                                                <asp:TextBox ID="txtNombreTrabajoCliente" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Puesto asignado</label>
                                                <asp:TextBox ID="txtPuestoAsignado" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Ingresos mensuales</label>
                                                <asp:TextBox ID="txtIngresosMensuales" CssClass="form-control form-control-sm text-right" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Fecha de ingreso</label>
                                                <asp:TextBox ID="txtFechaIngreso" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Arraigo Laboral</label>
                                                <asp:TextBox ID="txtArraigoLaboral" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-lg-6 col-md-6 col-sm-6 col-12">
                                                <label class="col-form-label">Teléfono empresa</label>
                                                <asp:TextBox ID="txtTelefonoEmpresa" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-lg-3 col-md-3 col-sm-3 col-6">
                                                <label class="col-form-label">Extensión cliente</label>
                                                <asp:TextBox ID="txtExtensionCliente" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-lg-3 col-md-3 col-sm-3 col-6">
                                                <label class="col-form-label">Extensión RRHH</label>
                                                <asp:TextBox ID="txtExtensionRecursosHumanos" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Departamento</label>
                                                <asp:TextBox ID="txtDepartamentoEmpresa" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Municipio</label>
                                                <asp:TextBox ID="txtMunicipioEmpresa" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Ciudad/Poblado</label>
                                                <asp:TextBox ID="txtCiudadPobladoEmpresa" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Barrio/colonia</label>
                                                <asp:TextBox ID="txtBarrioColoniaEmpresa" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-12">
                                                <label class="col-form-label">Dirección detallada</label>
                                                <textarea id="txtDireccionDetalladaEmpresa" runat="server" readonly="readonly" class="form-control form-control-sm"></textarea>
                                            </div>
                                            <div class="col-12">
                                                <label class="col-form-label">Referencias la dirección</label>
                                                <textarea id="txtReferenciaDetalladaEmpresa" runat="server" readonly="readonly" class="form-control form-control-sm"></textarea>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Fuente de otros ingresos</label>
                                                <asp:TextBox ID="txtFuenteDeOtrosIngresos" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Valor de otros ingresos</label>
                                                <asp:TextBox ID="txtValorDeOtrosIngresos" CssClass="form-control form-control-sm text-right" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-6 col-md-6 border-left border-gray">
                                        <h6 class="font-weight-bold">Documentación</h6>
                                        <div class="form-group row">
                                            <div class="col-12">
                                                <!-- Div donde se muestran las imágenes de informacion laboral -->
                                                <div class="align-self-center" id="divDocumentacionLaboral" runat="server" style="display: none;">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-default">
                        <div class="panel-heading p-1 bg-light border-bottom" role="tab" id="headingFive">
                            <h6 class="m-0 font-14">
                                <a href="#collapseReferenciasPersonales" class="text-dark collapsed h6 font-weight-bold" data-toggle="collapse" aria-expanded="false" aria-controls="collapseFive">
                                    <i class="mdi mdi-phone-log mdi-24px"></i>
                                    Referencias Personales del Cliente
                                </a>
                            </h6>
                        </div>
                        <div id="collapseReferenciasPersonales" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFive">
                            <div class="panel-body">
                                <div class="row mb-0" id="divReferenciasPersonales" runat="server">
                                    <div class="col-12">

                                        <h6>Referencias personales</h6>
                                        <div class="form-group row mr-1 ml-1">
                                            <div class="table-responsive">
                                                <table class="table table-sm table-hover cursor-pointer" id="tblReferencias" runat="server">
                                                    <thead>
                                                        <tr>
                                                            <th>Nombre referencia</th>
                                                            <th>Lugar de trabajo</th>
                                                            <th>Tiempo de conocer</th>
                                                            <th>Telefono</th>
                                                            <th>Parentesco</th>
                                                            <th></th>
                                                            <th></th>
                                                        </tr>
                                                    </thead>
                                                    <tbody class="table-condensed">
                                                    </tbody>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-default" id="panelInformacionGarantia" runat="server" visible="true">
                        <div class="panel-heading p-1 bg-light border-bottom" role="tab" id="headingGarantia">
                            <h6 class="m-0 font-14">
                                <a href="#collapseInformacionGarantia" class="text-dark collapsed h6 font-weight-bold" data-toggle="collapse" aria-expanded="false" aria-controls="collapseSeven">
                                    <i class="mdi mdi-certificate mdi-24px"></i>
                                    Información de la garantía
                                </a>
                            </h6>
                        </div>
                        <div id="collapseInformacionGarantia" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingGarantia">
                            <div class="panel-body">
                                <div class="row mb-0" id="divInformacionGarantia" runat="server">
                                    <div class="col-lg-6">
                                        <h6 class="m-0 pt-2 font-weight-bold">Características físicas</h6>

                                        <div class="form-group row">
                                            <div class="col-sm-4 col-12">
                                                <label class="col-form-label">VIN</label>
                                                <asp:TextBox ID="txtVIN" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-4 col-6">
                                                <label class="col-form-label">Tipo de garantía</label>
                                                <asp:TextBox ID="txtTipoDeGarantia" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-4 col-6">
                                                <label class="col-form-label">Tipo de vehículo</label>
                                                <asp:TextBox ID="txtTipoDeVehiculo" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-4 col-6">
                                                <label class="col-form-label">Marca</label>
                                                <asp:TextBox ID="txtMarca" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-4 col-6">
                                                <label class="col-form-label">Modelo</label>
                                                <asp:TextBox ID="txtModelo" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-4 col-6">
                                                <label class="col-form-label">Año</label>
                                                <asp:TextBox ID="txtAnio" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-4 col-6">
                                                <label class="col-form-label">Color</label>
                                                <asp:TextBox ID="txtColor" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-4 col-6">
                                                <label class="col-form-label">Matrícula</label>
                                                <asp:TextBox ID="txtMatricula" CssClass="form-control form-control-sm" ReadOnly="true" type="text" required="required" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-4 col-6">
                                                <label class="col-form-label">Serie Motor</label>
                                                <asp:TextBox ID="txtSerieMotor" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Serie Chasis</label>
                                                <asp:TextBox ID="txtSerieChasis" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">GPS</label>
                                                <asp:TextBox ID="txtGPS" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                                            </div>
                                        </div>

                                        <h6 class="font-weight-bold m-0">Características mecánicas</h6>

                                        <div class="form-group row">
                                            <div class="col-sm-4">
                                                <label class="col-form-label">Cilindraje</label>
                                                <asp:TextBox ID="txtCilindraje" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-4">
                                                <label class="col-form-label">Recorrido</label>
                                                <asp:TextBox ID="txtRecorrido" CssClass="form-control form-control-sm" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-4">
                                                <label class="col-form-label">Transmisión</label>
                                                <asp:TextBox ID="txtTransmision" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-4">
                                                <label class="col-form-label">Tipo de combustible</label>
                                                <asp:TextBox ID="txtTipoDeCombustible" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-4">
                                                <label class="col-form-label">Serie 1</label>
                                                <asp:TextBox ID="txtSerieUno" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-4">
                                                <label class="col-form-label">Serie 2</label>
                                                <asp:TextBox ID="txtSerieDos" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-12">
                                                <label class="col-form-label">Comentario</label>
                                                <textarea id="txtComentario" runat="server" readonly="readonly" class="form-control form-control-sm"></textarea>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-6 border-left border-gray">

                                        <h6 class="font-weight-bold">Fotografías de la garantía</h6>

                                        <div class="form-group row">
                                            <div class="col-12">
                                                <!-- Div donde se muestran las imágenes de la garantía-->
                                                <div class="align-self-center" id="divGaleriaGarantia" runat="server" style="display: none;">
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-lg-6 border-gray">
                                        <h6 class="m-0 pt-2 font-weight-bold border-top border-gray">Propietario de la garantía</h6>

                                        <div class="form-group row">
                                            <div class="col-sm-6">
                                                <label class="col-form-label">Nombre</label>
                                                <asp:TextBox ID="txtNombrePropietarioGarantia" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-6">
                                                <label class="col-form-label">Identidad</label>
                                                <asp:TextBox ID="txtIdentidadPropietarioGarantia" CssClass="form-control form-control-sm" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Nacionalidad</label>
                                                <asp:TextBox ID="txtNacionalidadPropietarioGarantia" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Estado civil</label>
                                                <asp:TextBox ID="txtEstadoCivilPropietarioGarantia" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-6 border-left border-gray">
                                        <h6 class="m-0 pt-2 font-weight-bold border-top border-gray">Vendedor de la garantía</h6>
                                        <div class="form-group row">
                                            <div class="col-6">
                                                <label class="col-form-label">Nombre</label>
                                                <asp:TextBox ID="txtNombreVendedorGarantia" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Identidad</label>
                                                <asp:TextBox ID="txtIdentidadVendedorGarantia" CssClass="form-control form-control-sm" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Nacionalidad</label>
                                                <asp:TextBox ID="txtNacionalidadVendedorGarantia" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Estado civil</label>
                                                <asp:TextBox ID="txtEstadoCivilVendedorGarantia" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Informacion del prestamo requerido y el calculo del mismo -->
                    <div class="panel panel-default">
                        <div class="panel-heading p-1 bg-light border-bottom" role="tab" id="headingSix">
                            <h6 class="panel-title m-0 font-14">
                                <a href="#collapsePrestamoRequerido" class="text-dark collapsed h6 font-weight-bold" data-toggle="collapse" aria-expanded="false" aria-controls="collapseSix">
                                    <i class="mdi mdi-cash-multiple mdi-24px"></i>
                                    Información del Préstamo Requerido
                                </a>
                            </h6>
                        </div>
                        <div id="collapsePrestamoRequerido" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingSix">
                            <div class="panel-body">
                                <div class="row mb-0">

                                    <!-- Capacidad de pago del cliente -->
                                    <div class="col-lg-6 col-md-6 col-12" id="divCapacidadDePagoPrecalificado" runat="server">

                                        <h6 class="font-weight-bold">Capacidad de Pago - Precalificado</h6>

                                        <div class="form-group row">
                                            <div class="col-12">
                                                <label class="col-form-label">Ingresos</label>
                                                <asp:TextBox ID="txtIngresosPrecalificado" CssClass="form-control form-control-sm text-right" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Obligaciones</label>
                                                <asp:TextBox ID="txtObligacionesPrecalificado" CssClass="form-control form-control-sm text-right" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Disponible</label>
                                                <asp:TextBox ID="txtDisponiblePrecalificado" CssClass="form-control form-control-sm text-right" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Capacidad de pago (Mensual)</label>
                                                <asp:TextBox ID="txtCapacidadDePagoMensual" CssClass="form-control form-control-sm text-right" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Capacidad de pago (Quincenal)</label>
                                                <asp:TextBox ID="txtCapacidadDePagoQuincenal" CssClass="form-control form-control-sm text-right" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                        </div>

                                        <!-- Recalculo de capacidad de pago cuando se modifiquen los ingresos -->

                                        <div runat="server" id="divRecalculoCapacidadDePago" visible="false">

                                            <label class="font-weight-bold">Recalculo capacidad de pago - Ingresos reales</label>

                                            <div class="form-group row">
                                                <div class="col-12">
                                                    <label class="col-form-label">Ingresos</label>
                                                    <asp:TextBox ID="txtIngresos_Recalculo" CssClass="form-control form-control-sm text-right" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="col-6">
                                                    <label class="col-form-label">Obligaciones</label>
                                                    <asp:TextBox ID="txtObligaciones_Recalculo" CssClass="form-control form-control-sm text-right" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="col-6">
                                                    <label class="col-form-label">Disponible</label>
                                                    <asp:TextBox ID="txtDisponible_Recalculo" CssClass="form-control form-control-sm text-right" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="col-6">
                                                    <label class="col-form-label">Capacidad de pago (Mensual)</label>
                                                    <asp:TextBox ID="txtCapacidadDePagoMensual_Recalculo" CssClass="form-control form-control-sm text-right" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="col-6">
                                                    <label class="col-form-label">Capacidad de pago (Quincenal)</label>
                                                    <asp:TextBox ID="txtCapacidadDePagoQuicenal_Recalculo" CssClass="form-control form-control-sm text-right" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                        <div id="divPrestamosSueridos_CapacidadDePagoReal" runat="server" visible="false">

                                            <h6 class="font-weight-bold">Préstamos sugeridos - Capacidad de pago real</h6>

                                            <div class="form-group row">
                                                <div class="col-12" id="divTablaNuevosPrestamosSugeridos" runat="server" visible="false">
                                                    <div class="table-responsive">
                                                        <table class="table table-condensed table-striped table-hover cursor-pointer" id="tblPrestamosSugeridosReales" runat="server">
                                                            <thead>
                                                                <tr>
                                                                    <th>Monto a financiar</th>
                                                                    <th>Plazo</th>
                                                                    <th>Cuota</th>
                                                                </tr>
                                                            </thead>
                                                            <tbody class="table-condensed">
                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </div>
                                                <div class="col-12" runat="server" id="divSinCapacidadDePago" visible="false">
                                                    <h6 class="font-weight-bold text-danger d-block">Capacidad de pago insuficiente</h6>
                                                    No hay préstamos seguridos para la capacidad de pago del cliente.
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <!-- Préstamo solicitado -->
                                    <div class="col-lg-6 col-md-6 col-12 border-left border-gray" id="divPrestamoSolicitado" runat="server">

                                        <h6 class="font-weight-bold">Préstamo inicial solicitado</h6>

                                        <div class="form-group row">
                                            <div class="col-6">
                                                <label class="col-form-label">Valor a financiar</label>
                                                <asp:TextBox ID="txtValorAFinanciarSeleccionado" CssClass="form-control form-control-sm text-right" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Moneda</label>
                                                <asp:TextBox ID="txtMonedaSolicitada" CssClass="form-control form-control-sm text-right" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Valor de la garantía</label>
                                                <asp:TextBox ID="txtValorGarantia" CssClass="form-control form-control-sm text-right" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Valor de la prima</label>
                                                <asp:TextBox ID="txtValorPrima" CssClass="form-control form-control-sm text-right" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>

                                            <div class="col-6">
                                                <label class="col-form-label">Plazo <span id="lblTipoDePlazo_Solicitado" class="font-weight-bold" runat="server"></span></label>
                                                <asp:TextBox ID="txtPlazoSeleccionado" CssClass="form-control form-control-sm text-right" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Origen</label>
                                                <asp:TextBox ID="txtOrigen" CssClass="form-control form-control-sm text-right" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div id="divCalculoPrestamoSolicitado" runat="server">

                                            <label class="font-weight-bold">Cálculo del préstamo solicitado</label>

                                            <!-- Calculo del prestamo -->
                                            <!--
                                                EN CASO DE QUE SE HAYAN MODIFICADO LOS INGRESOS DEL CLIENTE DEBIDO A INCONGRUENCIA CON EL PRECALIFICADO Y LOS COMPROBANTES DE PAGO
                                                MOSTRAR EL RECALCULO CON LAS CANTIDADES REALES
                                            -->
                                            <div class="form-group row">
                                                <div class="col-sm-12 col-6">
                                                    <label class="col-form-label">Monto total a financiar</label>
                                                    <asp:TextBox ID="txtMontoTotalAFinanciar_Calculo" CssClass="form-control form-control-sm text-right" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="col-sm-4 col-6">
                                                    <label class="col-form-label">Couta del préstamo</label>
                                                    <asp:TextBox ID="txtCuotaDelPrestamo_Calculo" CssClass="form-control form-control-sm text-right" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="col-sm-4 col-6">
                                                    <label class="col-form-label">Couta del seguro</label>
                                                    <asp:TextBox ID="txtCuotaDelSeguro_Calculo" CssClass="form-control form-control-sm text-right" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="col-sm-4 col-6">
                                                    <label class="col-form-label">Cuota del GPS</label>
                                                    <asp:TextBox ID="txtCuotaGPS_Calculo" CssClass="form-control form-control-sm text-right" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="col-12">
                                                    <label class="col-form-label">Cuota total</label>
                                                    <asp:TextBox ID="txtCuotaTotal_Calculo" CssClass="form-control form-control-sm text-right" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="col-6">
                                                    <label class="col-form-label">Costo aparato GPS</label>
                                                    <asp:TextBox ID="txtCostoAparatoGPS_Calculo" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="col-6">
                                                    <label class="col-form-label">Gastos de cierre</label>
                                                    <asp:TextBox ID="txtGastosDeCierre_Calculo" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                                </div>

                                                <div class="col-6">
                                                    <label class="col-form-label">Tasa anual aplicada</label>
                                                    <asp:TextBox ID="txtTasaAnualAplicada_Calculo" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="col-6">
                                                    <label class="col-form-label">Tasa mensual aplicada</label>
                                                    <asp:TextBox ID="txtTasaMensualAplicada_Calculo" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                                </div>
                                            </div>

                                        </div>


                                        <div id="divPrestamoFinalAprobado" runat="server">

                                            <h6 class="font-weight-bold">Monto final a financiar <span class="font-weight-bold" runat="server" id="lblEstadoDelMontoFinalAFinanciar">(En análisis)</span></h6>

                                            <div class="form-group row">
                                                <div class="col-4">
                                                    <label class="col-form-label">Monto total a financiar</label>
                                                    <asp:TextBox ID="txtMontoTotalAFinanciar_FinalAprobado" CssClass="form-control form-control-sm text-right border-success" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="col-4">
                                                    <label class="col-form-label">Plazo <span id="lblTipoDePlazo_FinalAprobado" class="font-weight-bold" runat="server">Quincenal</span></label>
                                                    <asp:TextBox ID="txtPlazoFinal_FinalAprobado" CssClass="form-control form-control-sm text-right border-success" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="col-4">
                                                    <label class="col-form-label">Couta del préstamo</label>
                                                    <asp:TextBox ID="txtCuotaDelPrestamo_FinalAprobado" CssClass="form-control form-control-sm text-right border-success" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="col-4">
                                                    <label class="col-form-label">Couta del seguro</label>
                                                    <asp:TextBox ID="txtCuotaDelSeguro_FinalAprobado" CssClass="form-control form-control-sm text-right" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="col-4">
                                                    <label class="col-form-label">Cuota del GPS</label>
                                                    <asp:TextBox ID="txtCuotaGPS_FinalAprobado" CssClass="form-control form-control-sm text-right" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="col-4">
                                                    <label class="col-form-label">Cuota total</label>
                                                    <asp:TextBox ID="txtCuotaTotal_FinalAprobado" CssClass="form-control form-control-sm text-right" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="col-6">
                                                    <label class="col-form-label">Costo aparato GPS</label>
                                                    <asp:TextBox ID="txtCostoAparatoGPS_FinalAprobado" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="col-6">
                                                    <label class="col-form-label">Gastos de cierre</label>
                                                    <asp:TextBox ID="txtGastosDeCierre_FinalAprobado" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="col-6">
                                                    <label class="col-form-label">Tasa anual aplicada</label>
                                                    <asp:TextBox ID="txtTasaAnualAplicada_FinalAprobado" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="col-6">
                                                    <label class="col-form-label">Tasa mensual aplicada</label>
                                                    <asp:TextBox ID="txtTasaMensualAplicada_FinalAprobado" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>


                    <div class="panel panel-default">
                        <div class="panel-heading p-1 bg-light border-bottom" role="tab" id="headingSeven">
                            <h6 class="m-0 font-14">
                                <a href="#collapseInformacionAnalisis" class="text-dark collapsed h6 font-weight-bold" data-toggle="collapse" aria-expanded="false" aria-controls="collapseSeven">
                                    <i class="mdi mdi-account-search mdi-24px"></i>
                                    Información de Perfil
                                </a>
                            </h6>
                        </div>
                        <div id="collapseInformacionAnalisis" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingSeven">
                            <div class="panel-body">
                                <div class="row mb-0" id="div1" runat="server">
                                    <div class="col-lg-6 col-md-6 col-12 border-right border-gray">
                                        <div class="form-group row">
                                            <div class="col-12">
                                                <label class="col-form-label">Tipo de empresa</label>
                                                <asp:TextBox ID="txtTipoDeEmpresa" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-12">
                                                <label class="col-form-label">Tipo de perfil</label>
                                                <asp:TextBox ID="txtTipoDePerfil" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-12">
                                                <label class="col-form-label">Tipo de empleo</label>
                                                <asp:TextBox ID="txtTipoDeEmpleo" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-12">
                                                <label class="col-form-label">Buro actual</label>
                                                <asp:TextBox ID="txtBuroActual" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-default" id="divInformaciondeCampo" runat="server" visible="false">
                        <div class="panel-heading p-1 bg-light border-bottom" role="tab" id="headingEight">
                            <h6 class="panel-title m-0 font-14">
                                <a href="#collapseInformaciondeCampo" class="text-dark collapsed h6 collapsed font-weight-bold" data-toggle="collapse" aria-expanded="false" aria-controls="collapseTwo">
                                    <i class="mdi mdi-home-variant mdi-24px"></i>
                                    Investigación de campo
                                </a>
                            </h6>
                        </div>
                        <div id="collapseInformaciondeCampo" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingEight">
                            <div class="panel-body">
                                <div class="row mb-0 border-bottom border-gray" id="divResolucionDomicilio" runat="server" visible="false">
                                    <div class="col-lg-6 col-md-6">

                                        <h6>Resolución del Domicilio <small>(<span id="lblResolucionCampoDomicilio" runat="server" class="font-weight-bold"></span>)</small></h6>

                                        <div class="form-group row">

                                            <div class="col-6">
                                                <label class="col-form-label">Gestor validador</label>
                                                <asp:TextBox ID="txtGestorValidador_Domicilio" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Resultado de la investigación</label>
                                                <asp:TextBox ID="txtResultadoInvestigacionCampo_Domicilio" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>

                                            <div class="col-6">
                                                <label class="col-form-label">Fecha recibida</label>
                                                <asp:TextBox ID="txtFechaRecibida_Domicilio" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Fecha rendida</label>
                                                <asp:TextBox ID="txtFechaValidacion_Domicilio" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-12">
                                                <label class="col-form-label">Observaciones del gestor</label>
                                                <textarea id="txtObservacionesDeCampo_Domicilio" runat="server" readonly="readonly" class="form-control form-control-sm"></textarea>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-6 col-md-6 border-left border-gray">
                                        <h6 class="font-weight-bold">Fotografías de la investigación</h6>
                                        <div class="form-group row">
                                            <div class="col-12">
                                                <!-- Div donde se muestran las imágenes de informacion domicilio -->
                                                <div class="align-self-center" id="divDocumentacionCampoDomicilio" runat="server" style="display: none;">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row mb-0 mt-1" id="divResolucionTrabajo" runat="server" visible="false">
                                    <div class="col-lg-6 col-md-6">

                                        <h6>Resolución del Trabajo <small>(<span id="lblResolucionCampoTrabajo" runat="server" class="font-weight-bold"></span>)</small></h6>

                                        <div class="form-group row">

                                            <div class="col-6">
                                                <label class="col-form-label">Gestor validador</label>
                                                <asp:TextBox ID="txtGestorValidador_Trabajo" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Resultado de la investigación</label>
                                                <asp:TextBox ID="txtResultadoInvestigacion_Trabajo" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>

                                            <div class="col-6">
                                                <label class="col-form-label">Fecha recibida</label>
                                                <asp:TextBox ID="txtFechaRecibida_Trabajo" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Fecha rendida</label>
                                                <asp:TextBox ID="txtFechaRendida_Trabajo" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-12">
                                                <label class="col-form-label">Observaciones del gestor</label>
                                                <textarea id="txtObservacionesDeCampo_Trabajo" runat="server" readonly="readonly" class="form-control form-control-sm"></textarea>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-6 col-md-6 border-left border-gray">
                                        <h6 class="font-weight-bold">Fotografías de la investigación</h6>
                                        <div class="form-group row">
                                            <div class="col-12">
                                                <!-- Div donde se muestran las imágenes de informacion domicilio -->
                                                <div class="align-self-center" id="divDocumentacionCampoTrabajo" runat="server" style="display: none;">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <!--/ panel body -->
                        </div>
                        <!--/ panel-collapse collapse -->
                    </div>
                    <!--/ panel investigacion de campo-->
                </div>
                <!--/ collapse-group -->
            </div>
            <!--/ card-header -->
        </div>
        <!--/ card -->

        <!-- modal estado del procesamiento de la solicitud -->
        <div id="modalEstadoSolicitud" class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="modalEstadoSolicitudLabel" aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <h6 class="modal-title mt-0" id="modalEstadoSolicitudLabel">Estado del procesamiento de la solicitud</h6>
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    </div>
                    <div class="modal-body pb-0">
                        <!-- tab -->
                        <ul class="nav nav-tabs nav-tabs-custom" role="tablist">
                            <li class="nav-item">
                                <a class="nav-link active" data-toggle="tab" href="#tabTiempos" role="tab">
                                    <span class="d-block d-sm-none"><i class="fa fa-clock"></i></span>
                                    <span class="d-none d-sm-block">Procesos</span>
                                </a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" data-toggle="tab" href="#tabDetalles" role="tab">
                                    <span class="d-block d-sm-none"><i class="far fa-comments"></i></span>
                                    <span class="d-none d-sm-block">Más detalles</span>
                                </a>
                            </li>
                            <li class="nav-item" runat="server" id="pestanaListaSolicitudCondiciones" style="display: none;">
                                <a class="nav-link" data-toggle="tab" href="#listaCondiciones" role="tab">
                                    <span class="d-none d-sm-block">Condiciones de la solictud</span>
                                </a>
                            </li>
                        </ul>
                        <!-- Tab panes -->
                        <div class="tab-content">
                            <div class="tab-pane active pr-0 pl-0 pt-3 pb-3" id="tabTiempos" role="tabpanel">
                                <div class="form-group row justify-content-center" id="divSolicitudInactiva" style="display: none;">
                                    <h6 class="text-danger">Solicitud Inactiva</h6>
                                </div>
                                <div class="table-responsive">
                                    <table class="table table-hover table-sm table-bordered mb-0 cursor-pointer" id="tblDetalleEstado">
                                        <thead class="thead-light">
                                            <tr>
                                                <th>Proceso</th>
                                                <th>Inicio</th>
                                                <th>Fin</th>
                                                <th>Tiempo transcurrido</th>
                                                <th>Usuario</th>
                                            </tr>
                                        </thead>
                                        <tbody></tbody>
                                    </table>
                                </div>
                            </div>
                            <div class="tab-pane pr-0 pl-0 pt-3" id="tabDetalles" role="tabpanel">
                                <div class="row">
                                    <div class="col-md-12">
                                        <h5 class="text-center font-weight-bold" id="lblEstadoSolicitud" runat="server">En Recepción</h5>

                                        <div class="form-group text-center mt-4" id="divNoHayMasDetalles" runat="server" visible="true">
                                            <label class="col-form-label font-weight-bold">No hay más detalles de esta solicitud...</label>
                                        </div>

                                        <ul class="timeline pl-3 mb-0" id="divLineaDeTiempo" runat="server" style="display: none">
                                            <li id="liObservacionesReprogramacion" runat="server" visible="true">
                                                <label class="mb-0">Observaciones de reprogramación (<span class="font-weight-bold" id="lblUsuario_ComentarioReprogramacion" runat="server"></span>)</label>
                                                <label class="float-right" runat="server" id="lblFecha_ComentarioReprogramacion"></label>
                                                <p runat="server" id="lblComentario_Reprogramacion"></p>
                                            </li>
                                            <li id="liObservaciones_OtrosCondicionamientos" runat="server" visible="true">
                                                <label class="mb-0">Observaciones de otros condicionamientos (<span class="font-weight-bold" id="lblUsuario_ComentarioOtrosCondicionamientos" runat="server"></span>)</label>
                                                <label class="float-right" runat="server" id="lblFecha_OtrosCondicionamientos"></label>
                                                <p runat="server" id="lblComentario_OtrosCondicionamientos"></p>
                                            </li>
                                            <li id="liObservacionesInformacionPersonal" runat="server" visible="true">
                                                <label class="mb-0">Observaciones información personal (<span class="font-weight-bold" id="lblUsuario_ComentarioInformacionPerosnal" runat="server"></span>)</label>
                                                <label class="float-right" runat="server" id="lblFecha_ComentarioInformacionPersonal"></label>
                                                <p runat="server" id="lblComentario_InformacionPersonal"></p>
                                            </li>
                                            <li id="liObservacionesInformacionLaboral" runat="server" visible="true">
                                                <label class="mb-0">Observaciones información laboral (<span class="font-weight-bold" id="lblUsuario_ComentarioInformacionLaboral" runat="server"></span>)</label>
                                                <label class="float-right" runat="server" id="lblFecha_ComentarioInformacionLaboral"></label>
                                                <p runat="server" id="lblComentario_InformacionLaboral"></p>
                                            </li>
                                            <li id="liObservacionesReferenciasPersonales" runat="server" visible="true">
                                                <label class="mb-0">Observaciones de referencias personales (<span class="font-weight-bold" id="lblUsuario_ComentarioReferenciasPersonales" runat="server"></span>)</label>
                                                <label class="float-right" runat="server" id="lblFecha_ComentarioReferenciasPersonales"></label>
                                                <p runat="server" id="lblComentario_ReferenciasPersonales"></p>
                                            </li>
                                            <li id="liObservacionesDocumentacion" runat="server" visible="true">
                                                <label class="mb-0">Observaciones de la documentación (<span class="font-weight-bold" id="lblUsuario_ComentarioDocumentacion" runat="server"></span>)</label>
                                                <label class="float-right" runat="server" id="lblFecha_ComentarioDocumentacion"></label>
                                                <p runat="server" id="lblComentario_Documentacion"></p>
                                            </li>
                                            <li id="liObservacionesParaGestoria" runat="server" visible="true">
                                                <label class="mb-0">Observaciones para gestoría (<span class="font-weight-bold" id="lblUsuario_ComentarioParaGestoria" runat="server"></span>)</label>
                                                <label class="float-right" runat="server" id="lblFecha_ComentarioParaGestoria"></label>
                                                <p runat="server" id="lblComentario_ParaGestoria"></p>
                                            </li>
                                            <li id="liObservacionesDeGestoria" runat="server" visible="true">
                                                <label class="mb-0">Observaciones de gestoría (<span class="font-weight-bold" id="lblUsuario_ComentarioDeGestoria" runat="server"></span>)</label>
                                                <label class="float-right" runat="server" id="lblFecha_ComentarioDeGestoria"></label>
                                                <p runat="server" id="lblComentario_DeGestoria"></p>
                                            </li>
                                            <li id="liComentariosDeLaResolucion" runat="server" visible="true">
                                                <label class="mb-0">Comentarios de la resolución (<span class="font-weight-bold" id="lblUsuario_ComentarioDeLaResolucion" runat="server"></span>)</label>
                                                <label class="float-right" runat="server" id="lblFecha_ComentarioDeLaResolucion"></label>
                                                <p runat="server" id="lblComentario_Resolicion"></p>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                            </div>
                            <div class="tab-pane pr-0 pl-0 pt-3" id="listaCondiciones" role="tabpanel">
                                <table id="tblListaSolicitudCondiciones" runat="server" class="table table-hover mb-0 cursor-pointer">
                                    <thead class="thead-light">
                                        <tr>
                                            <th>Tipo Condición</th>
                                            <th>Descripción</th>
                                            <th>Comentario Adicional</th>
                                            <th>Estado</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                        <!-- termina tab -->
                    </div>
                    <div class="modal-footer">
                        <button type="button" data-dismiss="modal" class="btn btn-secondary waves-effect">
                            Cerrar
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <!-- modal comentario sobre una referencia personal -->
        <div id="modalComentarioReferencia" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="modalComentarioReferenciaLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header pt-3 pb-2">
                        <h6 class="modal-title mt-0" id="modalComentarioReferenciaLabel">Observaciones de referencia personal</h6>
                    </div>
                    <form action="#" id="frmObservacionReferencia">
                        <div class="modal-body">
                            <div class="form-group">
                                <label class="col-form-label">Observaciones del departamento de crédito</label>
                                <textarea id="txtObservacionesReferencia" required="required" class="form-control" rows="2"></textarea>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="reset" data-dismiss="modal" class="btn btn-secondary waves-effect">
                                Cerrar
                            </button>
                        </div>
                    </form>
                </div>
            </div>
        </div>

        <div id="modalDocumentacion" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="modalDocumentacionLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h6 class="modal-title mt-0" id="modalDocumentacionLabel">Documentación de la solicitud</h6>
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-md-12">
                                <label class="mt-0 header-title text-center">Documentación identidad</label>
                                <div class="align-self-center" id="divDocumentacionCedulaModal" runat="server">
                                </div>
                            </div>
                            <div class="col-md-12">
                                <label class="mt-0 header-title text-center">Documentación domicilio</label>
                                <div class="align-self-center" id="divDocumentacionDomicilioModal" runat="server">
                                </div>
                            </div>
                            <div class="col-md-12">
                                <label class="mt-0 header-title text-center">Documentación laboral</label>
                                <div class="align-self-center" id="divDocumentacionLaboralModal" runat="server">
                                </div>
                            </div>
                            <div class="col-md-12">
                                <label class="mt-0 header-title text-center">Solicitud fisica</label>
                                <div class="align-self-center" id="divDocumentacionSoliFisicaModal" runat="server">
                                </div>
                            </div>
                            <div class="col-sm-12" id="divContenedorCampoDomicilioModal" runat="server" visible="false">
                                <label class="mt-0 header-title text-center">Documentación de campo (Domicilio)</label>
                                <div class="align-self-center" id="divDocumentacionCampoDomicilioModal" runat="server">
                                </div>
                            </div>
                            <div class="col-sm-12" id="divContenedorCampoTrabajoModal" runat="server" visible="false">
                                <label class="mt-0 header-title text-center">Documentación de campo (Trabajo)</label>
                                <div class="align-self-center" id="divDocumentacionCampoTrabajoModal" runat="server">
                                </div>
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
    </form>

    <script src="/Scripts/js/jquery.min.js"></script>
    <script src="/Scripts/js/bootstrap.bundle.min.js"></script>
    <script src="/Scripts/plugins/iziToast/js/iziToast.min.js"></script>
    <script src="/Scripts/plugins/unitegallery/js/unitegallery.min.js"></script>
    <script src="/Scripts/plugins/unitegallery/themes/default/ug-theme-default.js"></script>
    <script src="/Scripts/plugins/unitegallery/themes/tilesgrid/ug-theme-tilesgrid.js"></script>
    <script src="/Scripts/plugins/unitegallery/themes/tiles/ug-theme-tiles.js"></script>
    <script src="/Scripts/plugins/parsleyjs/parsley.js"></script>
    <script src="/Scripts/plugins/select2/js/select2.full.min.js"></script>
    <script src="/Scripts/plugins/countdownjs/countdown.min.js"></script>
    <script src="/Scripts/plugins/moment/moment.js"></script>
    <script src="/Scripts/plugins/moment/moment-with-locales.min.js"></script>
    <script src="/Scripts/app/solicitudes/SolicitudesCredito_Detalles.js?v=20201228115752"></script>
    <script>
        InicializarGaleria('divDocumentacionCedula');

        InicializarGaleria('divDocumentacionCedulaModal');

        InicializarGaleria('divDocumentacionDomicilio');

        InicializarGaleria('divDocumentacionDomicilioModal');

        InicializarGaleria('divDocumentacionLaboral');

        InicializarGaleria('divDocumentacionLaboralModal');

        InicializarGaleria('divDocumentacionSoliFisicaModal');

        InicializarGaleria('divDocumentacionCampoDomicilio');

        InicializarGaleria('divDocumentacionCampoDomicilioModal');

        InicializarGaleria('divDocumentacionCampoTrabajo');

        InicializarGaleria('divDocumentacionCampoTrabajoModal');

        $("#divGaleriaGarantia").unitegallery({
            tile_width: 180,
            tile_height: 120
        });

        function InicializarGaleria(idGaleria) {

            $('#' + idGaleria + '').unitegallery({
                gallery_theme: "tilesgrid",
                tile_width: 180,
                tile_height: 120,
                lightbox_type: "compact",
                grid_num_rows: 15,
                tile_enable_textpanel: true,
                tile_textpanel_title_text_align: "center"
            });
        }
    </script>
</body>
</html>
