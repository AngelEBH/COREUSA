<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Solicitudes_CANEX_Detalles.aspx.cs" Inherits="Solicitudes_CANEX_Detalles" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <title>Detalles de la solicitud CANEX</title>
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
                                <button id="btnDetallesResolucion" runat="server" visible="false" data-toggle="modal" data-target="#modalEstado" type="button" class="btn btn-sm btn-success waves-effect waves-light float-right">
                                    Detalles del estado
                                </button>
                                <button runat="server" id="btnHistorialExterno" type="button" class="btn btn-secondary waves-effect waves-light">
                                    <i class="fas fa-clipboard-check"></i>
                                    Buró Externo
                                </button>
                                <button runat="server" id="btnValidoDocumentacionModal" type="button" data-toggle="modal" data-target="#modalDocumentos" class="btn btn-secondary waves-effect waves-light">
                                    <i class="fas fa-file-alt"></i>
                                    Documentos
                                </button>
                                <button runat="server" id="btnCondicionarSolicitud" type="button" data-toggle="modal" data-target="#modalCondicionarSolicitud" class="btn btn-warning waves-effect waves-light validador">
                                    <i class="fas fa-pen-square"></i>
                                    Condicionar
                                </button>
                                <button runat="server" id="btnRechazar" type="button" data-toggle="modal" data-target="#modalResolucionRechazar" class="btn btn-danger waves-effect waves-light validador">
                                    <i class="fas fa-thumbs-down"></i>
                                    Rechazar
                                </button>
                                <button runat="server" id="btnAceptarSolicitud" type="button" data-toggle="modal" disabled="disabled" data-target="#modalResolucionAprobar" title="" class="btn btn-success waves-effect waves-light validador">
                                    <i class="fas fa-thumbs-up"></i>
                                    Aceptar
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
                                    <th class="text-center pt-1 pb-1">No. Cliente:
                                        <asp:Label ID="lblNoCliente" CssClass="col-form-label" runat="server"></asp:Label>
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
                                    <th class="text-center pt-1 pb-1">Estado:
                                        <asp:Label ID="lblEstadoSolicitud" CssClass="col-form-label" runat="server"></asp:Label>
                                    </th>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
            <div class="card-body">
                <div class="collapse-group">
                    <!-- INFORMACION PERSONAL -->
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
                                    <div class="col-lg-6 col-md-6">
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
                                                <label class="col-form-label">Teléfono alternativo</label>
                                                <asp:TextBox ID="txtNumeroTelefonoAlternativo" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
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
                                                <label class="col-form-label">Nacionalidad</label>
                                                <asp:TextBox ID="txtNacionalidad" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
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

                    <!-- INFORMACION DOMICILIO -->
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
                                                <label class="col-form-label">Vivienda</label>
                                                <asp:TextBox ID="txtVivienda" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
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

                    <!-- INFORMACION CONYUGAL -->
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
                                    <div class="col-lg-12 col-md-12">
                                        <div class="form-group row">
                                            <div class="col-6">
                                                <label class="col-form-label">Nombre del conyugue</label>
                                                <asp:TextBox ID="txtNombreDelConyugue" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Identidad</label>
                                                <asp:TextBox ID="txtIdentidadConyugue" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Fecha de nacimiento</label>
                                                <asp:TextBox ID="txtFechaNacimientoConyugue" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Telefono</label>
                                                <asp:TextBox ID="txtTelefonoConyugue" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Profesion u Oficio</label>
                                                <asp:TextBox ID="txtProfesionOficioConyugue" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Ocupación</label>
                                                <asp:TextBox ID="txtOcupacionConyugue" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Lugar de trabajo</label>
                                                <asp:TextBox ID="txtLugarTrabajoConyugue" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Puesto asignado</label>
                                                <asp:TextBox ID="txtPuestoAsignadoConyugue" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- INFORMACION LABORAL -->
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
                                            <div class="col-12" id="divComisionesCliente" runat="server">
                                                <label class="col-form-label">Ingresos mensuales por comisiones</label>
                                                <asp:TextBox ID="txtComisionesCliente" CssClass="form-control form-control-sm text-right" type="text" ReadOnly="true" runat="server"></asp:TextBox>
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

                    <!-- REFERENCIAS PERSONALES DEL CLIENTE -->
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
                                                <table class="table tabla-compacta table-striped" id="tblReferencias" runat="server">
                                                    <thead class="bg-light">
                                                        <tr>
                                                            <th>Nombre referencia</th>
                                                            <th>Lugar de trabajo</th>
                                                            <th>Tiempo de conocer</th>
                                                            <th>Telefono</th>
                                                            <th>Parentesco</th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- INFORMACION DEL PRESTAMO Y EL CALCULO DEL MISMO -->
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
                                    <!-- INFORMACION DEL PRECALIFICADO -->
                                    <div class="col-lg-6 col-md-6 col-12" id="divCapacidadDePagoPrecalificado" runat="server">

                                        <h6 class="font-weight-bold">Capacidad de Pago - Precalificado</h6>

                                        <div class="form-group row">
                                            <div class="col-12">
                                                <label class="col-form-label">Ingresos (Precalificado)</label>
                                                <asp:TextBox ID="txtIngresosPrecalificado" CssClass="form-control form-control-sm text-right" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-12">
                                                <label class="col-form-label">Obligaciones (Precalificado)</label>
                                                <asp:TextBox ID="txtObligacionesPrecalificado" CssClass="form-control form-control-sm text-right" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-12">
                                                <label class="col-form-label">Disponible (Precalificado)</label>
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
                                    </div>
                                    <!-- INFORMACION DEL PRESTAMO CON LOS DATOS DEL PRECALIFICADO-->
                                    <div class="col-lg-6 col-md-6 border-left border-gray">

                                        <h6 class="font-weight-bold">Préstamo solicitado</h6>

                                        <div class="form-group row">
                                            <div class="col-6">
                                                <label class="col-form-label">Valor Global</label>
                                                <asp:TextBox ID="txtValorGlobal" CssClass="form-control form-control-sm text-right" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Valor prima</label>
                                                <asp:TextBox ID="txtValorPrima" CssClass="form-control form-control-sm text-right" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label" id="lblPlazoTitulo" runat="server">Plazo seleccionado</label>
                                                <asp:TextBox ID="txtPlazoSeleccionado" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Monto Financiar</label>
                                                <asp:TextBox ID="txtMontoAFinanciar" CssClass="form-control form-control-sm text-right" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- modal documentacion -->
        <div id="modalDocumentos" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="modalDocumentosLabel" aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <h6 class="modal-title mt-0" id="modalDocumentosLabel">Documentación</h6>
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    </div>
                    <div class="modal-body">

                        <div class="row">
                            <div class="col-md-12">
                                <label class="mt-0 header-title text-center">Documentación identidad</label>
                                <div runat="server" class="popup-gallery" id="divDocumentacionCedulaModal">
                                </div>
                            </div>
                            <div class="col-md-12">
                                <label class="mt-0 header-title text-center">Documentación domicilio</label>
                                <div runat="server" class="popup-gallery" id="divDocumentacionDomicilioModal">
                                </div>
                            </div>
                            <div class="col-md-12">
                                <label class="mt-0 header-title text-center">Documentación laboral</label>
                                <div runat="server" class="popup-gallery" id="divDocumentacionLaboralModal">
                                </div>
                            </div>
                            <div class="col-md-12">
                                <label class="mt-0 header-title text-center">Solicitud fisica</label>
                                <div runat="server" class="popup-gallery" id="divDocumentacionSoliFisicaModal">
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

        <div id="modalCondicionarSolicitud" class="modal fade bs-example-modal-lg" role="dialog" aria-labelledby="modalCondicionarSolicitudLabel" aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title mt-0" id="modalCondicionarSolicitudLabel">Condicionar solicitud</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    </div>
                    <div class="modal-body">
                        <ul class="nav nav-tabs nav-tabs-custom" role="tablist">
                            <li class="nav-item">
                                <a class="nav-link active" data-toggle="tab" href="#pestanaAgregarCondiciones" role="tab">
                                    <span class="d-none d-sm-block">Agregar nuevas condiciones</span>
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
                            <div class="tab-pane active p-3" id="pestanaAgregarCondiciones" role="tabpanel">
                                <br />
                                <form id="frmAgregarCondicion">
                                    <div class="form-group row">
                                        <div class="col-sm-12">
                                            <asp:DropDownList ID="ddlCondiciones" runat="server" required="required" class="form-control form-control-sm"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <div class="col-sm-12">
                                            <input id="txtComentarioAdicional" required="required" type="text" class="form-control form-control-sm" placeholder="comentario adicional" data-parsley-maxlength="255" />
                                        </div>
                                    </div>
                                </form>
                                <div class="form-group row">
                                    <div class="col-sm-9"></div>
                                    <div class="col-sm-3">
                                        <button type="button" id="btnAgregarCondicion" class="btn btn-block btn-primary validador">Agregar</button>
                                    </div>
                                </div>
                                <div class="form-group row">
                                    <div class="col-sm-12">
                                        <table id="tblCondiciones" class="table table-condensed table-striped">
                                            <thead>
                                                <tr>
                                                    <th>Condicion</th>
                                                    <th>Comentario adicional</th>
                                                    <th></th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                            </div>
                            <div class="tab-pane p-3" id="listaCondiciones" role="tabpanel">
                                <table id="tblListaSolicitudCondiciones" runat="server" class="table table-condensed table-striped">
                                    <thead>
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
                        <button type="button" id="btnCondicionarSolicitudConfirmar" class="btn btn-primary waves-effect waves-light mr-1 validador">
                            Confirmar
                        </button>
                        <button type="reset" data-dismiss="modal" class="btn btn-secondary waves-effect">
                            Cancelar
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <!-- modal RESOLUCION de la solicitud -->
        <div id="modalResolucionRechazar" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="modalResolucionRechazarLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title mt-0" id="modalResolucionRechazarLabel">Rechazar solicitud</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    </div>
                    <div class="modal-body">
                        ¿Está seguro de <strong>RECHAZAR</strong> esta solicitud?<br />
                        <br />
                        <div class="form-group">
                            <label class="col-form-label">Observaciones</label>
                            <div>
                                <textarea id="txtComentarioRechazar" required="required" class="form-control" data-parsley-maxlength="255" rows="2"></textarea>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button id="btnRechazarConfirmar" class="btn btn-danger waves-effect waves-light mr-1">
                            Confirmar
                        </button>
                        <button type="reset" data-dismiss="modal" class="btn btn-secondary waves-effect">
                            Cancelar
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <div id="modalResolucionAprobar" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="modalResolucionAprobarLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title mt-0" id="modalResolucionAprobarLabel">Aceptar solicitud</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    </div>
                    <div class="modal-body">
                        ¿Está seguro de <strong>ACEPTAR</strong> esta solicitud?<br />
                        *Se importará a la bandeja de solicitudes*<br />
                        <br />
                        <div class="form-group">
                            <label class="col-form-label">Observaciones</label>
                            <div>
                                <textarea id="txtComentarioAceptar" required="required" class="form-control" data-parsley-maxlength="255" rows="2"></textarea>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button id="btnAceptarSolicitudConfirmar" class="btn btn-primary waves-effect waves-light mr-1">
                            Confirmar
                        </button>
                        <button type="reset" data-dismiss="modal" class="btn btn-secondary waves-effect">
                            Cancelar
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <div id="modalEstado" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="modalDocumentosLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h6 class="modal-title mt-0" id="modalEstadoLabel">Detalles del estado de la solicitud</h6>
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <div class="text-center">
                                <asp:Label ID="lblEstadoSolicitudModal" runat="server"></asp:Label>
                            </div>
                            <div class="mt-1">
                                <label class="col-form-label">
                                    Analista:
                                </label>
                                <asp:Label ID="lblNombreAnalista" runat="server" CssClass="col-form-label font-weight-bold" />
                            </div>
                            <div class="mt-1">
                                <label class="col-form-label">Detalles</label>

                                <textarea id="lblDetalleEstado" runat="server" readonly="readonly" required="required" class="form-control form-control-sm col-form-label" data-parsley-maxlength="255" rows="2"></textarea>
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
    <script>
        const idPais = <%=this.IdPais%>;
        const idAgencia = <%=this.IdAgencia%>;
        const idSocio = <%=this.IdSocio%>;
        idEstado = <%=this.IdEstadoSolicitud%>;
        const idSolicitudImportada = <%=this.IdSolicitudPrestadito%>;
    </script>
    <script src="/Scripts/app/Solicitudes_CANEX/Solicitudes_CANEX_Detalles.js?v=20201221090925"></script>

    <script>        
        InicializarGaleria('divDocumentacionCedula', 150, 97);
        InicializarGaleria('divDocumentacionCedulaModal', 180, 120);
        InicializarGaleria('divDocumentacionDomicilio', 180, 120);
        InicializarGaleria('divDocumentacionDomicilioModal', 180, 120);
        InicializarGaleria('divDocumentacionLaboral', 180, 120);
        InicializarGaleria('divDocumentacionLaboralModal', 180, 120);
        InicializarGaleria('divDocumentacionSoliFisicaModal', 180, 120);

        function InicializarGaleria(idGaleria, tile_width, tile_height) {

            $('#' + idGaleria + '').unitegallery({
                gallery_theme: "tilesgrid",
                tile_width: tile_width,
                tile_height: tile_height,
                lightbox_type: "compact",
                grid_num_rows: 15,
                tile_enable_textpanel: true,
                tile_textpanel_title_text_align: "center"
            });
        }
    </script>
</body>
</html>
