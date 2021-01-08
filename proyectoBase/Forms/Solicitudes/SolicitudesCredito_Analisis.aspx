<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SolicitudesCredito_Analisis.aspx.cs" Inherits="SolicitudesCredito_Analisis" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <title>Análisis de solicitud de crédito</title>
    <link href="/Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/Content/css/style.css" rel="stylesheet" />
    <link href="/Content/css/icons.css" rel="stylesheet" />
    <link href="/Scripts/plugins/iziToast/css/iziToast.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/unitegallery/css/unitegallery.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/unitegallery/themes/default/ug-theme-default.css" rel="stylesheet" />
    <link href="/Scripts/plugins/select2/css/select2.min.css" rel="stylesheet" />
    <link href="/CSS/Estilos_CSS.css" rel="stylesheet" />
    <link href="/CSS/SolicitudesCredito_Analisis.css" rel="stylesheet" />
    <link href="/Scripts/plugins/sweet-alert2/sweetalert2.min.css" rel="stylesheet" />
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
                            <div class="dropdown pb-2 pr-2">
                                <button class="btn btn-secondary" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                    <i class="fa fa-bars"></i>
                                    Más opciones
                                </button>
                                <div class="dropdown-menu" aria-labelledby="dropdownMenuButton">
                                    <button type="button" class="dropdown-item pl-3" id="btnResumen" data-toggle="modal" data-target="#modalResumen"><i class="far fa-file-alt"></i>Resumen solicitud</button>
                                    <button type="button" class="dropdown-item pl-3" id="btnImprimirReporte" onclick="ExportHtmlToPdf('#ReporteSolicitud','Reporte_Solitud','Reporte de la Solicitud')"><i class="far fa-file-pdf"></i>Generar PDF</button>
                                    <button type="button" class="dropdown-item pl-3" id="btnHistorialInterno" title="Ver Historial interno del cliente" disabled="disabled"><i class="fas fa-history"></i>Historial Interno </button>
                                    <button type="button" class="dropdown-item pl-3" id="btnCondicionarSolicitud"><i class="far fa-edit"></i>Condicionar solicitud</button>
                                </div>
                            </div>
                            <div class="button-items pb-2">
                                <button runat="server" id="btnResolucionSolicitud" type="button" class="btn btn-secondary">
                                    <i class="far fa-edit"></i>
                                    Resolución
                                </button>
                                <button runat="server" id="btnHistorialExterno" type="button" class="btn btn-secondary">
                                    <i class="far fa-credit-card"></i>
                                    Buró Externo
                                </button>
                                <button runat="server" id="btnDocumentacionModal" type="button" data-toggle="modal" data-target="#modalDocumentacion" class="btn btn-warning">
                                    <i class="far fa-file-alt"></i>
                                    Documentos
                                </button>
                                <button runat="server" id="btnMasDetalles" type="button" class="btn btn-secondary">
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
                                        <asp:Label ID="lblNoSolicitud" CssClass="col-form-label" runat="server"></asp:Label>
                                    </th>
                                    <th class="text-center pt-1 pb-1">Tipo Solicitud:
                                        <asp:Label ID="lblTipoSolicitud" CssClass="col-form-label" runat="server"></asp:Label>
                                    </th>
                                    <th class="text-center pt-1 pb-1">Agente de Ventas:
                                        <asp:Label ID="lblAgenteDeVentas" CssClass="col-form-label" runat="server"></asp:Label>
                                    </th>
                                    <th class="text-center pt-1 pb-1">Agencia:
                                        <asp:Label ID="lblAgencia" CssClass="col-form-label" runat="server"></asp:Label>
                                    </th>
                                    <th class="text-center pt-1 pb-1">Gestor:
                                        <asp:Label ID="lblNombreGestor" CssClass="col-form-label" runat="server"></asp:Label>
                                    </th>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div class="col-12">
                        <div class="table-responsive">
                            <asp:Table runat="server" ID="tblEstadoSolicitud" CssClass="table table-sm m-0 text-center">
                                <asp:TableHeaderRow TableSection="TableHeader">
                                    <asp:TableHeaderCell CssClass="text-center">Ingreso</asp:TableHeaderCell>
                                    <asp:TableHeaderCell CssClass="text-center">Recepción</asp:TableHeaderCell>
                                    <asp:TableHeaderCell CssClass="text-center">Analisis</asp:TableHeaderCell>
                                    <asp:TableHeaderCell CssClass="text-center">Campo</asp:TableHeaderCell>
                                    <asp:TableHeaderCell CssClass="text-center">Condic.</asp:TableHeaderCell>
                                    <asp:TableHeaderCell CssClass="text-center">Reprog.</asp:TableHeaderCell>
                                    <asp:TableHeaderCell CssClass="text-center">Validación</asp:TableHeaderCell>
                                    <asp:TableHeaderCell CssClass="text-center">Resolución</asp:TableHeaderCell>
                                </asp:TableHeaderRow>
                            </asp:Table>
                        </div>
                    </div>
                </div>
            </div>
            <div class="card-body">
                <div class="collapse-group">
                    <!-- Informacion personal -->
                    <div class="panel panel-default">
                        <div class="panel-heading p-1 bg-light border-bottom" role="tab" id="headingOne">
                            <div class="row justify-content-between align-items-end">
                                <div class="col-auto">
                                    <h6 class="panel-title m-0 font-14">
                                        <a role="button" data-toggle="collapse" href="#collapseInformacionPersonal" class="text-dark h6 trigger collapsed font-weight-bold" aria-expanded="true" aria-controls="collapseOne">
                                            <i class="mdi mdi-account-circle mdi-24px"></i>
                                            Informacion Personal
                                        </a>
                                    </h6>
                                </div>
                                <div class="col-xl-2 col-lg-3 col-md-4 col-sm-6 col-auto">
                                    <button id="btnValidarInformacionPersonal" type="button" class="btn btn-sm btn-warning btn-block" data-tipovalidacion="ValidarInformacionPersonal">
                                        <i class="far fa-check-square"></i>
                                        Validar información personal
                                    </button>
                                </div>
                            </div>
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

                    <!-- Informacion domicilio -->
                    <div class="panel panel-default">
                        <div class="panel-heading p-1 bg-light border-bottom" role="tab" id="headingTwo">
                            <h6 class="panel-title m-0 font-14">
                                <a href="#collapseInformacionDomicilio" class="text-dark collapsed h6 collapsed font-weight-bold" data-toggle="collapse" aria-expanded="false" aria-controls="collapseTwo">
                                    <i class="mdi mdi-home-variant mdi-24px"></i>
                                    Informacion Domicilio
                                </a>
                            </h6>
                        </div>
                        <div id="collapseInformacionDomicilio" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingTwo">
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

                    <!-- Informacion conyugal -->
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

                    <!-- Informacion laboral -->
                    <div class="panel panel-default">
                        <div class="panel-heading p-1 bg-light border-bottom" role="tab" id="headingFour">
                            <div class="row justify-content-between align-items-end">
                                <div class="col-auto">
                                    <h6 class="panel-title m-0 font-14">
                                        <a role="button" data-toggle="collapse" href="#collapseInformacionLaboral" class="text-dark h6 trigger collapsed font-weight-bold" aria-expanded="true" aria-controls="collapseFour">
                                            <i class="mdi mdi-briefcase-check mdi-24px"></i>
                                            Informacion Laboral
                                        </a>
                                    </h6>
                                </div>
                                <div class="col-xl-2 col-lg-3 col-md-4 col-sm-6 col-auto">
                                    <button id="btnValidarInformacionLaboral" type="button" class="btn btn-sm btn-warning btn-block" data-tipovalidacion="ValidarInformacionLaboral">
                                        <i class="far fa-check-square"></i>
                                        Validar información laboral
                                    </button>
                                </div>
                            </div>
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

                    <!-- Referencias personales -->
                    <div class="panel panel-default">
                        <div class="panel-heading p-1 bg-light border-bottom" role="tab" id="headingFive">
                            <div class="row justify-content-between align-items-end">
                                <div class="col-auto">
                                    <h6 class="panel-title m-0 font-14">
                                        <a role="button" data-toggle="collapse" href="#collapseReferenciasPersonales" class="text-dark h6 trigger collapsed font-weight-bold" aria-expanded="true" aria-controls="collapseFive">
                                            <i class="mdi mdi-phone-log mdi-24px"></i>
                                            Referencias Personales del Cliente
                                        </a>
                                    </h6>
                                </div>
                                <div class="col-xl-2 col-lg-3 col-md-4 col-sm-6 col-auto">
                                    <button id="btnValidarReferenciasPersonales" type="button" class="btn btn-sm btn-warning btn-block" data-tipovalidacion="ValidarInformacionLaboral">
                                        <i class="far fa-check-square"></i>
                                        Validar referencias personales
                                    </button>
                                </div>
                            </div>
                        </div>
                        <div id="collapseReferenciasPersonales" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFive">
                            <div class="panel-body pt-2">
                                <div class="row justify-content-between" id="divReferenciasPersonales" runat="server">
                                    <div class="col-auto">
                                        <h6>Lista de referencias personales</h6>
                                    </div>
                                    <div class="col-auto">
                                        <button type="button" id="btnNuevaReferenciaPersonal" class="btn btn-info"><i class="fas fa-plus-circle"></i>&nbsp; Agregar nuevo</button>
                                    </div>
                                    <div class="col-12">
                                        <div class="table-responsive">
                                            <asp:Table runat="server" ID="tblReferenciasPersonales" CssClass="table table-sm table-bordered table-hover cursor-pointer">
                                                <asp:TableHeaderRow TableSection="TableHeader" CssClass="thead-light">
                                                    <asp:TableHeaderCell CssClass="text-center">Nombre de la referencia</asp:TableHeaderCell>
                                                    <asp:TableHeaderCell CssClass="text-center">Lugar de trabajo</asp:TableHeaderCell>
                                                    <asp:TableHeaderCell CssClass="text-center">Tiempo de conocer</asp:TableHeaderCell>
                                                    <asp:TableHeaderCell CssClass="text-center">Telefono</asp:TableHeaderCell>
                                                    <asp:TableHeaderCell CssClass="text-center">Parentesco</asp:TableHeaderCell>
                                                    <asp:TableHeaderCell CssClass="text-center">Estado</asp:TableHeaderCell>
                                                    <asp:TableHeaderCell CssClass="text-center"></asp:TableHeaderCell>
                                                </asp:TableHeaderRow>
                                            </asp:Table>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Información de la garantía -->
                    <div class="panel panel-default" id="panelInformacionGarantia" runat="server" visible="true">
                        <div class="panel-heading p-1 bg-light border-bottom" role="tab" id="headingGarantia">
                            <h6 class="m-0 font-14">
                                <a href="#collapseInformacionGarantia" class="text-dark collapsed h6 font-weight-bold" data-toggle="collapse" aria-expanded="false" aria-controls="collapseSix">
                                    <i class="mdi mdi-certificate mdi-24px"></i>
                                    Información de la garantía
                                </a>
                            </h6>
                        </div>
                        <div id="collapseInformacionGarantia" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingGarantia">
                            <div class="panel-body">
                                <div class="row mb-0" id="divInformacionGarantia" runat="server">
                                    <div class="col-lg-6">
                                        <h6 class="font-weight-bold">Características físicas</h6>
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
                                        <h6 class="font-weight-bold m-0 pt-2">Características mecánicas</h6>
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

                    <!-- Información del préstamo (Pendiente) -->
                    <%--<div class="panel panel-default">
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
<div class="row">
<div class="col-md-12">
<div class="form-group row">
<button id="btnDigitarValoresManualmente" title="Digitar monto manualmente" type="button" class="btn btn-success col-sm-3 btn-block">
Digitar monto manualmente
</button>
</div>
</div>
<!-- INFORMACION DEL PRECALIFICADO -->
<div class="col-md-6 border">
<div class="form-group row">
<label class="col-sm-12 h6 text-center p-t-10">Capacidad de Pago - Precalificado</label>

<label class="col-sm-6 col-form-label">Ingresos precalificado</label>
<asp:Label ID="lblIngresosPrecalificado" CssClass="col-sm-6" runat="server"></asp:Label>

<label class="col-sm-6 col-form-label">Obligaciones precalificado</label>
<asp:Label ID="lblObligacionesPrecalificado" CssClass="col-sm-6 text-danger" runat="server"></asp:Label>

<label class="col-sm-6 col-form-label">Disponible precalificado</label>
<asp:Label ID="lblDisponiblePrecalificado" CssClass="col-sm-6" runat="server"></asp:Label>

<label class="col-sm-6 col-form-label">Capacidad de pago mensual</label>
<asp:Label ID="lblCapacidadPagoMensual" CssClass="col-sm-6" runat="server"></asp:Label>

<label class="col-sm-6 col-form-label">Capacidad de quincenal</label>
<asp:Label ID="lblCapacidadPagoQuincenal" CssClass="col-sm-6" runat="server"></asp:Label>
</div>
</div>
<!-- INFORMACION DEL PRESTAMO CON LOS DATOS DEL PRECALIFICADO-->
<div class="col-md-6 border">
<div class="form-group row">
<label class="col-sm-12 h6 text-center p-t-10">Préstamo Sugerido - Precalificado</label>

<label class="col-sm-6 col-form-label" id="lblTituloValorPMO">PRESTAMO APROBADO</label>
<asp:Label ID="lblValorPmoSugeridoSeleccionado" CssClass="col-sm-6" runat="server"></asp:Label>

<label id="lblValorVehiculo" class="col-sm-6 col-form-label" style="display: none;">Valor del vehiculo</label>
<asp:Label ID="lblMontoValorVehiculo" CssClass="col-sm-6" runat="server"></asp:Label>

<label id="lblPrima" class="col-sm-6 col-form-label">Prima</label>
<asp:Label ID="lblMontoPrima" CssClass="col-sm-6" runat="server"></asp:Label>
</div>
<div class="form-group row" id="divCargando">
<div class="col-sm-12 text-center p-t-10">
<div class="spinner-border" role="status">
<span class="sr-only">Calculando...</span>
</div>
<br />
Calculando...
</div>
</div>
<!-- DIV CALCULO PRESTAMO EFECTIVO-->
<div class="form-group row" id="divPrestamoEfectivo" style="display: none;">

<strong class="col-sm-6 col-form-label">Monto a financiar</></strong>
<strong>
<asp:Label ID="lblMontoFinanciarEfectivo" CssClass="col-sm-6" runat="server"></asp:Label></strong>

<label class="col-sm-6 col-form-label" id="lblTituloCuotaEfectivo">X Cuotas</label>
<strong>
<asp:Label ID="lblMontoCuotaEfectivo" CssClass="col-sm-6" runat="server"></asp:Label></strong>
</div>
<!-- DIV CALCULO PRESTAMO VEHICULO MOTO -->
<div class="form-group row" id="divPrestamoMoto" style="display: none;">
<label class="col-sm-6 col-form-label"><strong>Monto a financiar</strong></label>
<strong>
<asp:Label ID="lblMontoFinanciarMoto" CssClass="col-sm-6" runat="server"></asp:Label></strong>

<label class="col-sm-6 col-form-label" id="lblTituloCuotaMoto">X Cuotas</label>
<strong>
<asp:Label ID="lblMontoCuotaMoto" CssClass="col-sm-6" runat="server"></asp:Label></strong>
</div>
<!-- DIV CALCULO PRESTAMO VEHICULO AUTO -->
<div class="form-group row" id="divPrestamoAuto" style="display: none;">
<label class="col-sm-6 col-form-label"><strong>Monto a financiar</strong></label>
<strong>
<asp:Label ID="lblMontoFinanciarAuto" CssClass="col-sm-6" runat="server"></asp:Label></strong>

<label class="col-sm-6 col-form-label" id="lblTituloCuotaAuto">X Cuotas</label>
<strong>
<asp:Label ID="lblMontoCuotaTotalAuto" CssClass="col-sm-6" runat="server"></asp:Label></strong>
</div>
</div>
<!--
EN CASO DE QUE SE HAYAN MODIFICADO LOS INGRESOS DEL CLIENTE DEBIDO A INCONGRUENCIA CON EL PRECALIFICADO Y LOS COMPROBANTES DE PAGO
MOSTRAR EL RECALCULO CON LAS CANTIDADES REALES
-->
<div class="col-md-6 border" id="divRecalculoReal" style="display: none;">
<div class="form-group row">

<label class="col-sm-12 h6 text-center p-t-10">Recalculo de Capacidad de Pago - Ingresos Reales</label>

<label class="col-sm-6 col-form-label">Ingresos reales</label>
<asp:Label ID="lblIngresosReales" CssClass="col-sm-6" runat="server"></asp:Label>

<label class="col-sm-6 col-form-label">Obligaciones</label>
<asp:Label ID="lblObligacionesReales" CssClass="col-sm-6" runat="server"></asp:Label>

<label class="col-sm-6 col-form-label">Disponible real</label>
<asp:Label ID="lblDisponibleReal" CssClass="col-sm-6" runat="server"></asp:Label>

<label class="col-sm-6 col-form-label">Capacidad de pago mensual</label>
<asp:Label ID="lblCapacidadPagoMensualReal" CssClass="col-sm-6" runat="server"></asp:Label>

<label class="col-sm-6 col-form-label">Capacidad de pago quincenal</label>
<asp:Label ID="lblCapacidadPagoQuincenalReal" CssClass="col-sm-6" runat="server"></asp:Label>
</div>
</div>
<!-- PRESTAMOS SUGERIDOS CON LOS INGRESOS REALES-->
<div class="col-md-6 border border-success" id="divPmoSugeridoReal">

<div class="form-group row" id="divPrestamoElegido" style="display: none;">
<label class="col-sm-12 h6 text-center p-t-10">Monto a Financiar Actual</label>

<label class="col-sm-6 col-form-label">Monto final a financiar</label>
<asp:Label ID="lblMontoPrestamoEscogido" CssClass="col-sm-6" runat="server"></asp:Label>

<label class="col-sm-6 col-form-label">Plazo final aprobado</label>
<asp:Label ID="lblPlazoEscogido" CssClass="col-sm-6" runat="server"></asp:Label>
</div>
<div class="form-group row" id="cargandoPrestamosSugeridosReales" style="display: none;">
<div class="col-sm-12 text-center p-t-10">
<div class="spinner-border" role="status">
<span class="sr-only">Cargando..</span>
</div>
<br />
Cargando Préstamos Sugeridos...
</div>
</div>
<div class="form-group row" id="divPrestamosSugeridosReales" style="display: none;">
<label class="col-sm-12 h6 text-center p-t-10" id="lbldivPrestamosSugeridosReales">Préstamos Sugeridos - Real</label>
<div class="col-sm-12">
<table class="table table-sm table-striped" id="tblPMOSugeridosReales">
<thead class="thead-light">
<tr>
<th>Monto a financiar</th>
<th>Plazo</th>
<th>Cutoa</th>
<th></th>
</tr>
</thead>
<tbody>
</tbody>
</table>
</div>
</div>
<div class="form-group row" id="divSinCapacidadPago" style="display: none;">
<label class="col-sm-12 h6 text-center p-t-10" id="lbldivSinCapacidadPago">Incapacidad de pago</label>
<br />
<div class="col-sm-12">
<label>
No hay prestamos sugeridos para esta capacidad de pago.<br />
¿Desea ingresar un monto manualmente?</label>
</div>
<div class="col-sm-6">
<button id="btnDigitarMontoManualmente" disabled="disabled" title="Digitar monto manualmente" type="button" class="btn btn-success btn-block">
Digitar monto
</button>
</div>
<div class="col-sm-6">
<button id="btnRechazarIncapacidadPagoModal" data-toggle="modal" data-target="#modalRechazarPorIncapcidadPago" disabled="disabled" title="Rechazar solicitud por incapacidad de pago" type="button" class="btn btn-danger btn-block">
Rechazar solicitud
</button>
</div>
</div>
</div>
</div>
</div>
</div>
</div>--%>

                    <!-- Información del préstamo -->
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
                                                        <table class="table table-sm table-hover cursor-pointer" id="tblPrestamosSugeridosReales" runat="server">
                                                            <thead class="thead-light">
                                                                <tr>
                                                                    <th>Monto a financiar</th>
                                                                    <th>Plazo</th>
                                                                    <th>Cuota</th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
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
                    <!-- Información de perfil -->
                    <div class="panel panel-default">
                        <div class="panel-heading p-1 bg-light border-bottom" role="tab" id="headingSeven">
                            <div class="row justify-content-between align-items-end">
                                <div class="col-auto">
                                    <h6 class="panel-title m-0 font-14">
                                        <a role="button" data-toggle="collapse" href="#collapseInformacionAnalisis" class="text-dark h6 trigger collapsed font-weight-bold" aria-expanded="true" aria-controls="collapseEight">
                                            <i class="mdi mdi-account-search mdi-24px"></i>
                                            Información de Perfil
                                        </a>
                                    </h6>
                                </div>
                                <div class="col-xl-2 col-lg-3 col-md-4 col-sm-6 col-auto">
                                    <button id="btnEnviarACampo" type="button" class="btn btn-sm btn-warning btn-block" data-tipovalidacion="ValidarInformacionLaboral">
                                        <i class="far fa-check-square"></i>
                                        Enviar a campo
                                    </button>
                                </div>
                            </div>
                        </div>
                        <div id="collapseInformacionAnalisis" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingSeven">
                            <div class="panel-body">
                                <div class="row mb-0" id="divInformacionPerfil" runat="server">
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

                    <!-- Investigación de campo -->
                    <div class="panel panel-default" id="divInformaciondeCampo" runat="server" visible="false">
                        <div class="panel-heading p-1 bg-light border-bottom" role="tab" id="headingEight">
                            <h6 class="panel-title m-0 font-14">
                                <a href="#collapseInformaciondeCampo" class="text-dark collapsed h6 collapsed font-weight-bold" data-toggle="collapse" aria-expanded="false" aria-controls="collapseNine">
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
            <!--/ card-body -->
        </div>
        <!--/ card -->

        <!-- modal validar informacion personal -->
        <div id="modalFinalizarValidarPersonal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="modalFinalizarValidarPersonalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h6 class="modal-title mt-0" id="modalFinalizarValidarPersonalLabel">Terminar validación</h6>
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    </div>
                    <div class="modal-body">
                        ¿Está seguro de que desea terminar la validación de la información personal del cliente?<br />
                        <div class="form-group">
                            <label class="col-form-label">Observaciones</label>
                            <input id="comentariosInfoPersonal" class="form-control" data-parsley-maxlength="150" type="text" value="" />
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" id="btnValidoInfoPersonalConfirmar" class="btn btn-primary mr-1 validador">
                            Confirmar
                        </button>
                        <button type="reset" data-dismiss="modal" class="btn btn-secondary">
                            Cancelar
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <!-- modal validar informacion laboral -->
        <div id="modalFinalizarValidarLaboral" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="modalFinalizarValidarLaboralLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h6 class="modal-title mt-0" id="modalFinalizarValidarLaboralLabel">Terminar validación</h6>
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    </div>
                    <div class="modal-body">
                        ¿Está seguro de que desea terminar la validación de la información laboral del cliente?<br />
                        <div class="form-group">
                            <label class="col-form-label">Observaciones</label>
                            <input id="comentariosInfoLaboral" class="form-control" type="text" data-parsley-maxlength="150" value="" />
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" id="btnValidoInfoLaboralConfirmar" class="btn btn-primary mr-1 validador">
                            Confirmar
                        </button>
                        <button type="reset" data-dismiss="modal" class="btn btn-secondary">
                            Cancelar
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <!-- modal documentacion de la solicitud -->
        <div id="modalDocumentacion" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="modalDocumentacionLabel" aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <h6 class="modal-title mt-0" id="modalDocumentacionLabel">Documentación de la solicitud de crédito</h6>
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    </div>
                    <div class="modal-body pt-0">
                        <div class="row border-bottom justify-content-between bg-light pt-2">
                            <div class="col-auto">
                                <label class="header-title font-weight-bold">Documentación identidad</label>
                            </div>
                            <div class="col-auto">
                                <button id="btnValidarDocumentacionIdentidad" type="button" class="btn btn-warning btn-block" data-tipovalidacion="ValidarDocumentosIdentidad">
                                    <i class="far fa-check-square"></i>
                                    Validar documentación
                                </button>
                            </div>
                            <div class="col-12">
                                <div id="divDocumentacionCedulaModal" runat="server">
                                </div>
                            </div>
                        </div>
                        <div class="row border-bottom justify-content-between pt-2">
                            <div class="col-auto">
                                <label class="header-title font-weight-bold">Documentación domicilio</label>
                            </div>
                            <div class="col-auto">
                                <button id="btnValidarDocumentacionDomicilio" type="button" class="btn btn-warning btn-block" data-tipovalidacion="ValidarDocumentosDomicilio">
                                    <i class="far fa-check-square"></i>
                                    Validar documentación
                                </button>
                            </div>
                            <div class="col-12">
                                <div id="divDocumentacionDomicilioModal" runat="server">
                                </div>
                            </div>
                        </div>
                        <div class="row border-bottom justify-content-between bg-light pt-2">
                            <div class="col-auto">
                                <label class="header-title font-weight-bold">Documentación laboral</label>
                            </div>
                            <div class="col-auto">
                                <button id="btnValidarDocumentacionLaboral" type="button" class="btn btn-warning btn-block" data-tipovalidacion="ValidarDocumentosLaboral">
                                    <i class="far fa-check-square"></i>
                                    Validar documentación
                                </button>
                            </div>
                            <div class="col-12">
                                <div id="divDocumentacionLaboralModal" runat="server">
                                </div>
                            </div>
                        </div>
                        <div class="row justify-content-between pt-2">
                            <div class="col-auto">
                                <label class="header-title font-weight-bold">Solicitud física</label>
                            </div>
                            <div class="col-auto">
                                <button id="btnValidarDocumentacionSolicitudFisica" type="button" class="btn btn-warning btn-block" data-tipovalidacion="ValidarDocumentosSolicitudFisica">
                                    <i class="far fa-check-square"></i>
                                    Validar documentación
                                </button>
                            </div>
                            <div class="col-12">
                                <div id="divDocumentacionSoliFisicaModal" runat="server">
                                </div>
                            </div>
                        </div>
                        <div class="row border-top justify-content-between bg-light pt-2" id="divContenedorCampoDomicilioModal" runat="server" visible="false">
                            <div class="col-auto">
                                <label class="header-title font-weight-bold">Documentación de campo (Domicilio)</label>
                            </div>
                            <div class="col-12">
                                <div id="divDocumentacionCampoDomicilioModal" runat="server">
                                </div>
                            </div>
                        </div>
                        <div class="row border-top justify-content-between pt-2" id="divContenedorCampoTrabajoModal" runat="server" visible="false">
                            <div class="col-auto">
                                <label class="header-title font-weight-bold">Documentación de campo (Trabajo)</label>
                            </div>
                            <div class="col-12">
                                <div id="divDocumentacionCampoTrabajoModal" runat="server">
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="reset" data-dismiss="modal" class="btn btn-secondary">
                            Cerrar
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <!-- modal validar referencias -->
        <div id="modalFinalizarValidarReferencias" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="modalFinalizarValidarReferenciasLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h6 class="modal-title mt-0" id="modalFinalizarValidarReferenciasLabel">Terminar validación</h6>
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    </div>
                    <div class="modal-body">
                        ¿Está seguro de que desea terminar la validación de las referencias personales del cliente?<br />
                        <div class="form-group">
                            <label>Observaciones</label>
                            <input id="comentarioReferenciasPersonales" class="form-control" type="text" value="" data-parsley-maxlength="150" />
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" id="btnValidoReferenciasConfirmar" class="btn btn-primary mr-1 validador">
                            Confirmar
                        </button>
                        <button type="reset" data-dismiss="modal" class="btn btn-secondary">
                            Cancelar
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <!-- modal condicionar solicitud -->
        <div id="modalCondicionarSolicitud" class="modal fade bs-example-modal-lg" role="dialog" aria-labelledby="modalCondicionarSolicitudLabel" aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <h6 class="modal-title mt-0" id="modalCondicionarSolicitudLabel">Condicionar solicitud</h6>
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    </div>
                    <div class="modal-body">
                        <!-- Nav tabs -->
                        <ul class="nav nav-tabs nav-tabs-custom" role="tablist">
                            <li class="nav-item">
                                <a class="nav-link active" data-toggle="tab" href="#pestanaAgregarCondiciones" role="tab">
                                    <span class="d-none d-sm-block">Agregar nuevas condiciones</span>
                                </a>
                            </li>
                            <li class="nav-item" runat="server" id="pestanaListaSolicitudCondiciones" style="display: none;">
                                <a class="nav-link" data-toggle="tab" href="#tbllistaCondiciones" role="tab">
                                    <span class="d-none d-sm-block">Condiciones de la solictud</span>
                                </a>
                            </li>
                        </ul>
                        <!-- Tab panes -->
                        <div class="tab-content">
                            <div class="tab-pane active pt-3" id="pestanaAgregarCondiciones" role="tabpanel">
                                <div class="form-group row">
                                    <div class="col-sm-12">
                                        <select id="ddlCondiciones" required="required" class="form-control form-control-sm" data-parsley-group="agregarCondiciones"></select>
                                    </div>
                                </div>
                                <div class="form-group row">
                                    <div class="col-sm-12">
                                        <textarea id="txtComentarioAdicional" required="required" class="form-control form-control-sm" placeholder="comentario adicional" data-parsley-maxlength="128" data-parsley-minlength="15" data-parsley-group="agregarCondiciones"></textarea>
                                    </div>
                                </div>
                                <div class="form-group row justify-content-end">
                                    <div class="col-auto">
                                        <button type="button" id="btnAgregarNuevaCondicion" class="btn btn-block btn-primary validador">Agregar condición</button>
                                    </div>
                                </div>
                                <div class="form-group row">
                                    <div class="col-sm-12">
                                        <table id="tblNuevasCondiciones" class="table table-sm table-bordered table-hover cursor-pointer mb-0">
                                            <caption class="pt-1">Listado de nuevas condiciones</caption>
                                            <thead class="thead-light">
                                                <tr>
                                                    <th>Condicion</th>
                                                    <th>Comentario adicional</th>
                                                    <th>Acciones</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label>Otras condiciones/observaciones</label>
                                    <input id="razonCondicion" required="required" class="form-control" type="text" value="" data-parsley-maxlength="128" />
                                </div>
                            </div>
                            <div class="tab-pane pt-3" id="tbllistaCondiciones" role="tabpanel">
                                <table id="tblListaSolicitudCondiciones" class="table table-sm table-bordered table-hover cursor-pointer">
                                    <thead class="thead-light">
                                        <tr>
                                            <th>Tipo Condición</th>
                                            <th>Descripción</th>
                                            <th>Comentario Adicional</th>
                                            <th>Estado</th>
                                            <th>Acciones</th>
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
                        <button type="button" id="btnCondicionarSolicitudConfirmar" class="btn btn-danger mr-1 validador">
                            Confirmar
                        </button>
                        <button type="reset" data-dismiss="modal" class="btn btn-secondary">
                            Cancelar
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <!-- modal enviar solicitud a campo -->
        <div id="modalEnviarCampo" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="modalEnviarCampoLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h6 class="modal-title mt-0" id="modalEnviarCampoLabel">Enviar a campo</h6>
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    </div>
                    <div class="modal-body">
                        ¿Está seguro de que desea enviar a campo la solicitud?<br />
                        <div class="form-group">
                            <label>Comentario adicional para gestor</label>
                            <input id="comentarioAdicional" class="form-control" type="text" value="" data-parsley-maxlength="150" />
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" id="btnEnviarCampoConfirmar" class="btn btn-primary mr-1 validador">
                            Confirmar
                        </button>
                        <button type="reset" data-dismiss="modal" class="btn btn-secondary">
                            Cancelar
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <!-- modal resumen de la solicitud -->
        <div id="modalResumen" class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="modalResumenLabel" aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header text-center">
                        <h6 class="modal-title w-100 mt-0" id="modalResumenLabel" style="text-align: center">Resumen de la solicitud - Información relevante</h6>
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    </div>
                    <div class="modal-body" id="ResumenSolicitud">
                        <div class="form-group row">
                            <h6 class="col-sm-12 text-center mt-0">Información del cliente</h6>
                            <label class="col-sm-6 EliminarEspacios">Cliente</label>
                            <asp:Label ID="lblResumenCliente" CssClass="col-sm-6 EliminarEspacios" runat="server"></asp:Label>
                            <label class="col-sm-6 EliminarEspacios">Edad</label>
                            <asp:Label ID="lblResumenEdad" CssClass="col-sm-6 EliminarEspacios" runat="server"></asp:Label>
                            <label class="col-sm-6 EliminarEspacios">Trabajo</label>
                            <asp:Label ID="lblResumenTrabajo" CssClass="col-sm-6 EliminarEspacios" runat="server"></asp:Label>
                            <label class="col-sm-6 EliminarEspacios">Puesto</label>
                            <asp:Label ID="lblResumenPuesto" CssClass="col-sm-6 EliminarEspacios" runat="server"></asp:Label>
                            <label class="col-sm-6 EliminarEspacios">Capacidad de Pago Mensual</label>
                            <asp:Label ID="lblResumenCapacidadPagoMensual" CssClass="col-sm-6 EliminarEspacios" runat="server"></asp:Label>
                            <label class="col-sm-6 EliminarEspacios">Capacidad de Pago Quincenal</label>
                            <asp:Label ID="lblResumenCapacidadPagoQuincenal" CssClass="col-sm-6 EliminarEspacios" runat="server"></asp:Label>
                        </div>
                        <hr />
                        <div class="form-group row">
                            <h6 class="col-sm-12 text-center mt-0">Información del domicilio</h6>
                            <label class="col-sm-6 EliminarEspacios">Departamento</label>
                            <asp:Label ID="lblResumenDeptoResidencia" CssClass="col-sm-6 EliminarEspacios" runat="server"></asp:Label>
                            <label class="col-sm-6 EliminarEspacios">Municipio Residencia</label>
                            <asp:Label ID="lblResumenMuniResidencia" CssClass="col-sm-6 EliminarEspacios" runat="server"></asp:Label>
                            <label class="col-sm-6 EliminarEspacios">Bo. o Colonia</label>
                            <asp:Label ID="lblResumenColResidencia" CssClass="col-sm-6 EliminarEspacios" runat="server"></asp:Label>
                            <label class="col-sm-6 EliminarEspacios">Vivienda</label>
                            <asp:Label ID="lblResumenTipoVivienda" CssClass="col-sm-6 EliminarEspacios" runat="server"></asp:Label>
                            <label class="col-sm-6 EliminarEspacios">Tiempo de Residir</label>
                            <asp:Label ID="lblResumenTiempoResidir" CssClass="col-sm-6 EliminarEspacios" runat="server"></asp:Label>
                        </div>
                        <hr />
                        <div class="form-group row">
                            <h6 class="col-sm-12 text-center mt-0">Información del Préstamo</h6>
                            <label class="col-sm-6 EliminarEspacios">Préstamo Aprobado (Asesor de ventas)</label>
                            <asp:Label ID="lblResumenPrestamoSugeridoSeleccionado" CssClass="col-sm-6 EliminarEspacios" runat="server"> - </asp:Label>
                            <label id="lblResumenValorGarantiaTitulo" class="col-sm-6 EliminarEspacios" style="display: none;">Valor de la Garantía</label>
                            <asp:Label ID="lblResumenValorGarantia" CssClass="col-sm-6 EliminarEspacios" runat="server" Style="display: none;"> - </asp:Label>
                            <label id="lblResumenValorPrimaTitulo" class="col-sm-6 EliminarEspacios" style="display: none;">Valor de la Prima</label>
                            <asp:Label ID="lblResumenValorPrima" CssClass="col-sm-6 EliminarEspacios" runat="server" Style="display: none;"> - </asp:Label>
                            <label class="col-sm-6 EliminarEspacios">Monto a Financiar (Asesor de ventas)</label>
                            <asp:Label ID="lblResumenValorFinanciar" CssClass="col-sm-6 EliminarEspacios" runat="server"> - </asp:Label>
                            <label id="lblResumenCuotaTitulo" class="col-sm-6 EliminarEspacios">N Cuotas</label>
                            <asp:Label ID="lblResumenCuota" CssClass="col-sm-6 EliminarEspacios" runat="server"> - </asp:Label>

                            <label class="col-sm-6 EliminarEspacios">Monto Final a Financiar</label>
                            <asp:Label ID="lblResumenMontoFinalFinanciar" CssClass="col-sm-6 EliminarEspacios" runat="server"> - </asp:Label>
                            <label id="lblResumenCuotaFinalFinanciarTitulo" class="col-sm-6 EliminarEspacios">Cuotas</label>
                            <asp:Label ID="lblResumenCuotaFinalFinanciar" CssClass="col-sm-6 EliminarEspacios" runat="server"> - </asp:Label>
                        </div>
                        <hr />
                        <div class="form-group row">
                            <h6 class="col-sm-12 text-center mt-0">Información de Perfil</h6>
                            <label class="col-sm-6 EliminarEspacios">Tipo de Empresa</label>
                            <asp:Label ID="lblResumenTipoEmpresa" CssClass="col-sm-6 EliminarEspacios" runat="server"> - </asp:Label>
                            <label class="col-sm-6 EliminarEspacios">Tipo de Perfil</label>
                            <asp:Label ID="lblResumenTipoPerfil" CssClass="col-sm-6 EliminarEspacios" runat="server"> - </asp:Label>
                            <label class="col-sm-6 EliminarEspacios">Tipo de Empleo</label>
                            <asp:Label ID="lblResumenTipoEmpleo" CssClass="col-sm-6 EliminarEspacios" runat="server"> - </asp:Label>
                            <label class="col-sm-6 EliminarEspacios">Buro Actual</label>
                            <asp:Label ID="lblResumenBuroActual" CssClass="col-sm-6 EliminarEspacios" runat="server"> - </asp:Label>
                        </div>
                        <hr />
                        <div class="form-group row">
                            <h6 class="col-sm-12 text-center mt-0">Personal validador</h6>
                            <label class="col-sm-6 EliminarEspacios">Asesor de Ventas</label>
                            <asp:Label ID="lblResumenVendedor" CssClass="col-sm-6 EliminarEspacios" runat="server"> - </asp:Label>
                            <label class="col-sm-6 EliminarEspacios">Analista</label>
                            <asp:Label ID="lblResumenAnalista" CssClass="col-sm-6 EliminarEspacios" runat="server"> - </asp:Label>
                            <label class="col-sm-6 EliminarEspacios">Gestor Domicilio</label>
                            <asp:Label ID="lblResumenGestorDomicilio" CssClass="col-sm-6 EliminarEspacios" runat="server"> - </asp:Label>
                            <label class="col-sm-6 EliminarEspacios">Gestor Trabajo</label>
                            <asp:Label ID="lblResumenGestorTrabajo" CssClass="col-sm-6 EliminarEspacios" runat="server"> - </asp:Label>

                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" data-dismiss="modal" class="btn btn-secondary">
                            Cerrar
                        </button>
                        <button type="button" class="btn btn-secondary" onclick="ExportHtmlToPdf('#ResumenSolicitud','Resumen de la solicitud','Resumen de la solicitud - Información Relevante')">
                            Guardar PDF
                        </button>
                    </div>
                </div>
            </div>
        </div>

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
                            <li class="nav-item" runat="server" id="tabListaCondicionesDeLaSolicitud" style="display: none;">
                                <a class="nav-link" data-toggle="tab" href="#tabListaCondiciones" role="tab">
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
                            <div class="tab-pane pr-0 pl-0 pt-3" id="tabListaCondiciones" role="tabpanel">
                                <table id="tblListaCondicionesDeLaSolicitud" class="table table-sm table-bordered table-hover mb-0 cursor-pointer">
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
                        <button type="button" data-dismiss="modal" class="btn btn-secondary">
                            Cerrar
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <!-- modal rechazar una solicitud -->
        <div id="modalRechazar" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="modalRechazarLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h6 class="modal-title mt-0 font-weight-bold" id="modalRechazarLabel">Resolución final</h6>
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    </div>
                    <div class="modal-body">
                        ¿Está seguro de que desea <strong>RECHAZAR</strong> esta solicitud?<br />
                        <br />
                        <div class="form-group">
                            <label class="col-form-label">Observaciones</label>
                            <input id="comentarioRechazar" required="required" class="form-control" type="text" value="" data-parsley-maxlength="150" />
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" id="btnConfirmarRechazar" class="btn btn-danger mr-1">
                            Confirmar
                        </button>
                        <button type="reset" data-dismiss="modal" class="btn btn-secondary">
                            Cancelar
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <!-- modal aprobar una solicitud -->
        <div id="modalAprobar" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="modalAprobarLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h6 class="modal-title mt-0 font-weight-bold" id="modalAprobarLabel">Aprobar solicitud de crédito</h6>
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    </div>
                    <div class="modal-body">
                        ¿Está seguro de que desea <strong>APROBAR</strong> esta solicitud?<br />
                        <br />
                        <div class="form-group">
                            <label class="col-form-label">Observaciones</label>
                            <input id="comentarioAprobar" required="required" class="form-control" type="text" value="" data-parsley-maxlength="150" />
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" id="btnConfirmarAprobar" class="btn btn-success mr-1">
                            Confirmar
                        </button>
                        <button type="reset" data-dismiss="modal" class="btn btn-secondary">
                            Cancelar
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <!-- modal agregar comentario sobre una referencia personal -->
        <div id="modalObservacionesReferenciaPersonal" class="modal fade bs-example-modal-lg" role="dialog" aria-labelledby="modalComentarioReferenciaLabel" aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <h6 class="modal-title mt-0" id="modalComentarioReferenciaLabel">Observaciones/Comentarios del departamento de crédito</h6>
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    </div>
                    <div class="modal-body">
                        <div class="form-group row">
                            <div class="col-4">
                                <label class="col-form-label">Referencia personal</label>
                                <asp:TextBox ID="txtNombreReferenciaModal" ReadOnly="true" CssClass="form-control form-control-sm col-form-label" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-4">
                                <label class="col-form-label">Analista de crédito</label>
                                <asp:TextBox ID="txtAnalistaDeCredito" ReadOnly="true" CssClass="form-control form-control-sm col-form-label" runat="server" Text=""></asp:TextBox>
                            </div>
                            <div class="col-4">
                                <label class="col-form-label">Fecha de análisis</label>
                                <asp:TextBox ID="txtFechaDeAnalisis" ReadOnly="true" CssClass="form-control form-control-sm col-form-label" Text="" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="d-block">Observaciones/Comentarios</label>
                            <textarea id="txtObservacionesReferencia" required="required" class="form-control" data-parsley-maxlength="500" rows="2"></textarea>
                        </div>
                        <div class="m-0 p-0">
                            <label class="col-form-label font-weight-bold">Sin comunicación&nbsp;</label>
                            <input type="checkbox" id="cbSinComunicacion" switch="danger" class="align-bottom mb-1" />
                            <label for="cbSinComunicacion" data-on-label="SI" data-off-label="NO" class="align-bottom mb-1"></label>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <div class="button-items pb-2">
                            <button type="button" id="btnActualizarObservacionReferencia" class="btn btn-info validador">
                                Actualizar
                            </button>
                            <button type="reset" data-dismiss="modal" class="btn btn-secondary">
                                Cancelar
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- modal aprobar una solicitud -->
        <div id="modalEliminarReferencia" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="modalEliminarReferenciaLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h6 class="modal-title mt-0" id="modalEliminarReferenciaLabel">Eliminar Referencia Personal</h6>
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    </div>
                    <div class="modal-body">
                        ¿Está seguro de eliminar esta referencia personal?
                    </div>
                    <div class="modal-footer">
                        <button type="button" id="btnEliminarReferenciaConfirmar" class="btn btn-danger mr-1">
                            Confirmar
                        </button>
                        <button type="reset" data-dismiss="modal" class="btn btn-secondary">
                            Cancelar
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <!-- modal actualizar ingresos del cliente -->
        <div id="modalActualizarIngresos" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="modalActualizarIngresosLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <form id="formActualizarIngresos" action="#">
                        <div class="modal-header">
                            <h6 class="modal-title mt-0" id="modalActualizarIngresosLabel">Editar ingresos</h6>
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        </div>
                        <div class="modal-body">
                            <div class="form-group row">
                                <label class="col-sm-4 col-form-label">Ingresos reales</label>
                                <div class="col-sm-8">
                                    <input class="form-control col-form-label MascaraCantidad" id="txtIngresosReales" required="required" type="text" value="" />
                                </div>
                            </div>
                            <div class="form-group row">
                                <label class="col-sm-4 col-form-label">Bonos y comisiones</label>
                                <div class="col-sm-8">
                                    <input class="form-control col-form-label MascaraCantidad" id="txtBonosComisiones" required="required" type="text" value="" />
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="submit" class="btn btn-primary mr-1 validador">
                                Confirmar
                            </button>
                            <button type="reset" data-dismiss="modal" class="btn btn-secondary">
                                Cancelar
                            </button>
                        </div>
                    </form>
                </div>
            </div>
        </div>

        <!-- modal establecer monto a financiar de la lista de montos ofertados -->
        <div id="modalMontoFinanciar" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="modalMontoFinanciarLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h6 class="modal-title mt-0" id="modalMontoFinanciarLabel">Seleccionar Plazo y Monto Financiar</h6>
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    </div>
                    <div class="modal-body">
                        ¿Está seguro de seleccionar este plazo y monto a financiar?
                    </div>
                    <div class="modal-footer">
                        <button type="button" id="btnConfirmarPrestamoAprobado" class="btn btn-primary mr-1 validador">
                            Confirmar
                        </button>
                        <button type="reset" data-dismiss="modal" class="btn btn-secondary">
                            Cancelar
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <!-- modal tipo de documentos -->
        <div id="modalValidarTipoDocs" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="modalFinalizarValidarPersonalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h6 class="modal-title mt-0">Terminar validación</h6>
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    </div>
                    <div class="modal-body">
                        ¿Está seguro de llevar a cabo esta validación?
                    </div>
                    <div class="modal-footer">
                        <button type="button" id="btnValidarTipoDocConfirmar" class="btn btn-primary mr-1 validador">
                            Confirmar
                        </button>
                        <button type="reset" data-dismiss="modal" class="btn btn-secondary">
                            Cancelar
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <!-- modal rechazar por incapacidad de pago (pendiente) -->
        <div id="modalRechazarPorIncapcidadPago" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="modalRechazarPorIncapcidadPagoLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h6 class="modal-title mt-0" id="modalRechazarPorIncapcidadPagoLabel">Rechazar solicitud</h6>
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    </div>
                    <div class="modal-body">
                        ¿Está seguro de rechazar la solicitud por incapacidad de pago?
                    </div>
                    <div class="modal-footer">
                        <button type="button" id="btnRechazarIncapPagoConfirmar" class="btn btn-primary mr-1 validador">
                            Confirmar
                        </button>
                        <button type="reset" data-dismiss="modal" class="btn btn-secondary">
                            Cancelar
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <!-- modal digitar monto a financiar manualmente (pendiente) -->
        <div id="modalDigitarMontoManualmente" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="modalDigitarMontoManualmenteLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h6 class="modal-title mt-0" id="modalDigitarMontoManualmenteLabel">Sugerir Monto</h6>
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    </div>
                    <div class="modal-body">
                        <div class="form-group row">
                            <label class="col-sm-4 col-form-label">Valor Global</label>
                            <div class="col-sm-8">
                                <input class="form-control col-form-label MascaraCantidad" id="txtValorGlobalManual" required="required" type="text" value="" />
                            </div>
                        </div>
                        <div class="form-group row" id="divValorPrimaManual" <%-- style="display: none;"--%>>
                            <label class="col-sm-4 col-form-label">Valor Prima</label>
                            <div class="col-sm-8">
                                <input class="form-control col-form-label MascaraCantidad" id="txtValorPrimaManual" required="required" type="text" value="" <%--disabled="disabled"--%> />
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-4 col-form-label">Plazo</label>
                            <div class="col-sm-8">
                                <input class="form-control col-form-label MascaraEnteros" id="txtValorPlazoManual" required="required" type="text" value="" />
                            </div>
                        </div>
                        <div class="form-group row" id="divCalculandoCuotaManual" style="/*display: none; */">
                            <div class="col-sm-12 text-center">
                                <div class="spinner-border" role="status">
                                    <span class="sr-only">Cargando</span>
                                </div>
                                <br />
                                Calculando Cuota...
                            </div>
                        </div>
                        <div class="form-group row" id="divMostrarCalculoCuotaManual" style="/*display: none; */">
                            <label class="col-sm-4 col-form-label">Monto a Financiar</label>
                            <div class="col-sm-8">
                                <input class="form-control col-form-label MascaraCantidad" id="txtMontoaFinanciarManual" readonly="readonly" required="required" type="text" value="" />
                                <br />
                            </div>
                            <label class="col-sm-4 col-form-label" id="lblTituloCantidadCuotaManual">X Cuotas</label>
                            <div class="col-sm-8">
                                <input class="form-control col-form-label MascaraCantidad" id="txtValorCuotaManual" disabled="disabled" required="required" type="text" value="" />
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" id="btnActualizarMontoManualmente" class="btn btn-primary mr-1 validador btn-default" disabled="disabled">
                            Confirmar
                        </button>
                        <button type="reset" data-dismiss="modal" class="btn btn-secondary">
                            Cancelar
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <!-- Agregar referencia personal -->
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
                        <button id="btnAgregarReferenciaConfirmar" type="button" class="btn btn-primary waves-effect waves-light mr-1">
                            Agregar
                        </button>
                        <button type="reset" data-dismiss="modal" class="btn btn-secondary waves-effect">
                            Cancelar
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <!-- Eliminar referencia personal -->
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

        <!-- Editar referencia personal -->
        <div class="modal fade" id="modalEditarReferenciaPersonal" tabindex="-1" role="dialog" aria-labelledby="modalEditarReferenciaPersonalLabel" aria-hidden="true">
            <div class="modal-dialog">
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
    <script src="/Scripts/plugins/mascarasDeEntrada/js/jquery.inputmask.bundle.js"></script>
    <script src="/Scripts/plugins/kendo/jszip.min.js"></script>
    <script src="/Scripts/plugins/kendo/kendo.all.min.js"></script>
    <script src="/Scripts/plugins/kendo/PrintHtmlToPDF.js"></script>
    <script src="/Scripts/plugins/sweet-alert2/sweetalert2.min.js"></script>
    <script src="/Scripts/app/solicitudes/SolicitudesCredito_Utilitarios.js"></script>
    <script src="/Scripts/app/solicitudes/SolicitudesCredito_Analisis.js?v=20200903152956"></script>
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
    <script type="x/kendo-template" id="page-template">
        <div class="page-template">
            <div class="header">
                <img src="/Imagenes/LogoPrestaditoMediano.png" style="width:150px; float:left;" />
                <h3 id="titleTemplate"></h3>
            </div>
            <div class="footer">
                Pagina #: pageNum # de #: totalPages #
            </div>
        </div>
    </script>
</body>
</html>
