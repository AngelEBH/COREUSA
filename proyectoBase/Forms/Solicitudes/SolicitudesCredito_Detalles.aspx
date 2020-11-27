<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SolicitudesCredito_Detalles.aspx.cs" Inherits="SolicitudesCredito_Detalles" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head>
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
    <link href="/Scripts/plugins/imgBox/style.css" rel="stylesheet" />
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
    <form runat="server">
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
                                <button id="btnHistorialInterno" disabled="disabled" type="button" class="btn btn-success btn-sm">
                                    Historial interno
                                </button>
                                <button id="btnHistorialExterno" disabled="disabled" type="button" class="btn btn-success btn-sm">
                                    Buro externo
                                </button>
                                <button id="btnValidoDocumentacionModal" type="button" class="btn btn-success btn-sm">
                                    <small>Ver docs</small>
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
                    <div class="col-lg-12">
                        <div class="table-responsive">
                            <table class="table table-condensed m-0 text-center" id="tblEstadoSolicitud" runat="server">
                                <thead>
                                    <tr>
                                        <th>Ingreso</th>
                                        <th>Recepción</th>
                                        <th>Analisis</th>
                                        <th>Campo</th>
                                        <th>Condic.</th>
                                        <th>Reprog.</th>
                                        <th>Validación</th>
                                        <th>Resolución</th>
                                    </tr>
                                </thead>
                                <tbody class="bg-white"></tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
            <div class="card-body">
                <div class="collapse-group">
                    <!-- Información personal -->
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
                                                <label class="col-form-label">Nacionalidad</label>
                                                <asp:TextBox ID="txtNacionalidadCliente" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
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
                                                <label class="col-form-label">Correo electrónico</label>
                                                <asp:TextBox ID="txtCorreoCliente" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Profesión u Oficio</label>
                                                <asp:TextBox ID="txtProfesionOficioCliente" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Sexo</label>
                                                <asp:TextBox ID="txtSexoCliente" CssClass="form-control form-control-sm" ReadOnly="true" type="text" runat="server"></asp:TextBox>
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
                                                <div class="align-self-center" id="divDocumentacionCedula" runat="server" style="display: none;">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Información de domicilio -->
                    <div class="panel panel-default">
                        <div class="panel-heading p-1 bg-light border-bottom" role="tab" id="headingTwo">
                            <h6 class="panel-title m-0 font-14">
                                <a href="#collapseInformacionDomiciliar" class="text-dark collapsed h6 collapsed font-weight-bold" data-toggle="collapse"
                                    aria-expanded="false"
                                    aria-controls="collapseTwo">
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
                                                <label class="col-form-label">Lugar de trabajo</label>
                                                <asp:TextBox ID="txtLugarDeTrabajoConyugue" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Telefono trabajo</label>
                                                <asp:TextBox ID="txtTelefonoTrabajoConyugue" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-12">
                                                <label class="col-form-label">Ingresos mensuales</label>
                                                <asp:TextBox ID="txtIngresosMensualesConyugue" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
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
                                                <asp:TextBox ID="txtNombreDelTrabajo" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Puesto asignado</label>
                                                <asp:TextBox ID="txtPuestoAsignado" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Ingresos mensuales</label>
                                                <asp:TextBox ID="txtIngresosMensuales" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
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
                                                <asp:TextBox ID="txtValorDeOtrosIngresos" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
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

                                <div class="row mb-0" id="divCapacidadDePagoPrecalificado" runat="server">
                                    <div class="col-lg-6 col-md-6 col-12 border-right border-gray">

                                        <h6 class="font-weight-bold">Capacidad de Pago - Precalificado</h6>

                                        <div class="form-group row">
                                            <div class="col-12">
                                                <label class="col-form-label">Ingresos (Precalificado)</label>
                                                <asp:TextBox ID="txtIngresosPrecalificado" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-12">
                                                <label class="col-form-label">Obligaciones (Precalificado)</label>
                                                <asp:TextBox ID="txtObligacionesPrecalificado" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-12">
                                                <label class="col-form-label">Disponible (Precalificado)</label>
                                                <asp:TextBox ID="txtDisponiblePrecalificado" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Capacidad de pago (Mensual)</label>
                                                <asp:TextBox ID="txtCapacidadDePagoMensual" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Capacidad de pago (Quincenal)</label>
                                                <asp:TextBox ID="txtCapacidadDePagoQuincenal" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-6 col-md-6 border-left border-gray">
                                        <h6 class="font-weight-bold">Préstamo solicitado</h6>

                                        <div class="form-group row">
                                            <div class="col-6">
                                                <label class="col-form-label">Monto a financiar seleccionado</label>
                                                <asp:TextBox ID="txtMontoFinanciarSeleccionado" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Plazo seleccionado</label>
                                                <asp:TextBox ID="txtPlazoSeleccionado" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Valor garantía</label>
                                                <asp:TextBox ID="txtValorGarantia" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Valor prima</label>
                                                <asp:TextBox ID="txtValorPrima" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="form-group row">
                                            <div class="col-4">
                                                <label class="col-form-label">Monto a financiar</label>
                                                <asp:TextBox ID="txtCalculoMontoFinanciar" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-4">
                                                <label class="col-form-label">Plazo a financiar</label>
                                                <asp:TextBox ID="txtCalculoPlazo" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-4">
                                                <label class="col-form-label">Cuota</label>
                                                <asp:TextBox ID="txtCalculoCuota" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Cuota GPS</label>
                                                <asp:TextBox ID="txtCalculoCuotaGPS" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Cuota seguro</label>
                                                <asp:TextBox ID="txtCalculoCuotaSeguro" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Costo aparato GPS</label>
                                                <asp:TextBox ID="txtCalculoCostoAparatoGPS" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Gastos de cierre</label>
                                                <asp:TextBox ID="txtCalculoGastosDeCiere" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                        </div>

                                    </div>

                                    <div class="col-lg-12 col-md-12 col-12" runat="server" id="divPrestamoFinalAprobado" visible="false">
                                        <h6 class="font-weight-bold border-top pt-2 pb-0 mb-0">Préstamo final aprobado</h6>

                                        <div class="form-group row">
                                            <div class="col-12">
                                                <label class="col-form-label">Monto a financiar</label>
                                                <asp:TextBox ID="txtMontoFinanciarAprobado" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Plazo a financiar</label>
                                                <asp:TextBox ID="txtPlazoAprobado" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Valor de la cuota</label>
                                                <asp:TextBox ID="txtValorCuotaAprobado" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Cuota GPS</label>
                                                <asp:TextBox ID="txtCuotaGPSAprobado" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Cuota seguro</label>
                                                <asp:TextBox ID="txtCuotaSeguroAprobado" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Costo aparato GPS</label>
                                                <asp:TextBox ID="txtCostoAparatoGPSAprobado" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Gastos de cierre</label>
                                                <asp:TextBox ID="txtGastosDeCierreAprobado" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                        </div>

                                    </div>
                                </div>

                                <%--<div class="row">
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
<div class="form-group row" id="divCargando">
<div class="col-sm-12 text-center p-t-10">
<div class="spinner-border" role="status">
<span class="sr-only">Calculando...</span>
</div>
<br />
Calculando...
</div>
</div>
</div>

<!-- DIV CALCULO PRESTAMO EFECTIVO-->
<div class="form-group row" id="divPrestamoEfectivo" style="display: none;">

<label class="col-sm-6 col-form-label"><strong>Monto a financiar</strong></label>
<asp:Label ID="lblMontoFinanciarEfectivo" Font-Bold="true" CssClass="col-sm-6" runat="server"></asp:Label>

<label class="col-sm-6 col-form-label" id="lblTituloCuotaEfectivo">X Cuotas</label>
<asp:Label ID="lblMontoCuotaEfectivo" Font-Bold="true" CssClass="col-sm-6" runat="server"></asp:Label>
</div>
<!-- DIV CALCULO PRESTAMO VEHICULO MOTO -->
<div class="form-group row" id="divPrestamoMoto" style="display: none;">
<label class="col-sm-6 col-form-label"><strong>Monto a financiar</strong></label>
<asp:Label ID="lblMontoFinanciarMoto" Font-Bold="true" CssClass="col-sm-6" runat="server"></asp:Label>

<label class="col-sm-6 col-form-label" id="lblTituloCuotaMoto">X Cuotas</label>
<asp:Label ID="lblMontoCuotaMoto" Font-Bold="true" CssClass="col-sm-6" runat="server"></asp:Label>
</div>
<!-- DIV CALCULO PRESTAMO VEHICULO AUTO -->
<div class="form-group row" id="divPrestamoAuto" style="display: none;">
<label class="col-sm-6 col-form-label"><strong>Monto a financiar</strong></label>
<asp:Label ID="lblMontoFinanciarAuto" Font-Bold="true" CssClass="col-sm-6" runat="server"></asp:Label>

<label class="col-sm-6 col-form-label" id="lblTituloCuotaAuto">X Cuotas</label>
<asp:Label ID="lblMontoCuotaTotalAuto" Font-Bold="true" CssClass="col-sm-6" runat="server"></asp:Label>
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
<table class="table table-condensed table-striped" id="tblPMOSugeridosReales">
<thead>
<tr>
<th>Monto a financiar</th>
<th>Plazo</th>
<th>Cutoa</th>
</tr>
</thead>
<tbody></tbody>
<tfoot></tfoot>
</table>
</div>
</div>
<div class="form-group row" id="divSinCapacidadPago" style="display: none;">
<label class="col-sm-12 h6 text-center p-t-10" id="lbldivSinCapacidadPago">Incapacidad de pago</label>
<br />
<div class="col-sm-12">
<label>
No hay prestamos sugeridos para esta capacidad de pago.<br />
</label>
</div>
</div>
</div>
</div>--%>
                            </div>
                        </div>
                    </div>

                    <!-- INFORMACION DE ANALISIS -->
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
                                    <div class="col-lg-6 col-md-6 col-6">
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
                                <h6>Referencias personales</h6>
                                <div class="table-responsive">
                                    <table class="table table-condensed table-striped" id="tblReferencias" runat="server">
                                        <thead class="bg-light">
                                            <tr>
                                                <th>Nombre completo</th>
                                                <th>Lugar de trabajo</th>
                                                <th>Tiempo de conocer</th>
                                                <th>Telefono</th>
                                                <th>Parentesco</th>
                                                <th>Acciones</th>
                                            </tr>
                                        </thead>
                                        <tbody></tbody>
                                    </table>
                                </div>
                                <div id="divAval" runat="server" visible="false">
                                    <h4>Aval</h4>
                                    <div class="table-responsive">
                                        <table class="table table-condensed" id="tblAvales">
                                            <thead class="thead-light">
                                                <tr>
                                                    <th>Nombre completo</th>
                                                    <th>Identidad</th>
                                                    <th>Telefono</th>
                                                    <th>Lugar de trabajo</th>
                                                    <th>Puesto asignado</th>
                                                    <th>Ingresos</th>
                                                    <th>Estado</th>
                                                    <th></th>
                                                </tr>
                                            </thead>
                                            <tbody></tbody>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- INFORMACION DE CAMPO -->
                    <div class="panel panel-default" id="divInformaciondeCampo" style="display: none;" runat="server">
                        <div class="panel-heading p-1 bg-light border-bottom" role="tab" id="headingEight">
                            <h6 class="m-0 font-14">
                                <a href="#collapseInformaciondeCampo" class="text-dark collapsed h6 font-weight-bold" data-toggle="collapse" aria-expanded="false" aria-controls="collapseThree">
                                    <i class="mdi mdi-account-check mdi-24px"></i>
                                    Informacion de Campo
                                </a>
                            </h6>
                        </div>
                        <div id="collapseInformaciondeCampo" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingEight">
                            <div class="panel-body">
                                <div class="row mb-0" id="divResolucionDomicilio" runat="server" visible="true">
                                    <div class="col-lg-6 col-md-6">
                                        <h6>Resolución del Domicilio <small id="lblResolucionCampoDomicilio"></small></h6>
                                        <div class="form-group row">
                                            <div class="col-12">
                                                <label class="col-form-label">Gestor validador</label>
                                                <asp:TextBox ID="txtGestorValidadorDomicilio" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Resolución</label>
                                                <asp:TextBox ID="txtResolucionCampoDomicilio" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Fecha de validación</label>
                                                <asp:TextBox ID="txtFechaValidacionDomicilio" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-12">
                                                <label class="col-form-label">Observaciones</label>
                                                <textarea id="txtObservacionesCampoDomicilio" runat="server" readonly="readonly" class="form-control form-control-sm"></textarea>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-6 col-md-6 border-left border-bottom border-gray">
                                        <h6 class="font-weight-bold">Documentación</h6>
                                        <div class="form-group row">
                                            <div class="col-12">
                                                <!-- Div donde se muestran las imágenes de informacion de campo del domicilio -->
                                                <div class="align-self-center" id="divDocumentacionCampoDomicilio" runat="server" style="display: none;">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <%--<div class="row" id="" style="display: none;">
<div class="col-md-8">

<div class="form-group row">
<label class="col-sm-6">Gestor validador</label>
<asp:Label ID="lblGestorValidadorDomicilio" CssClass="col-sm-6" runat="server"></asp:Label>

<label class="col-sm-6">Resolucion</label>
<asp:Label ID="lblResolucionDomicilio" CssClass="col-sm-6" runat="server"></asp:Label>

<label class="col-sm-6">Fecha de validacion</label>
<asp:Label ID="lblFechaValidacionDomicilio" CssClass="col-sm-6" runat="server"></asp:Label>

<label class="col-sm-6">Observaciones</label>
<asp:Label ID="lblObservacionesCampoDomicilio" CssClass="col-sm-6" runat="server"></asp:Label>
</div>
</div>
<div class="col-md-3">
<label class="header-title tituloDocumentacionCampoDomicilio">Documentación</label>
<div class="container" id="">
</div>
</div>
</div>--%>

                                <div class="row mb-0" id="divResolucionTrabajo" runat="server" visible="true">
                                    <div class="col-lg-6 col-md-6">
                                        <h6>Resolución del Trabajo <small id="lblResolucionCampoTrabajo"></small></h6>
                                        <div class="form-group row">
                                            <div class="col-12">
                                                <label class="col-form-label">Gestor validador</label>
                                                <asp:TextBox ID="txtGestorValidadorTrabajo" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Resolución</label>
                                                <asp:TextBox ID="txtResolucionCampoTrabajo" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Fecha de validación</label>
                                                <asp:TextBox ID="txtFechaValidacionTrabajo" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-12">
                                                <label class="col-form-label">Observaciones</label>
                                                <textarea id="txtObservacionesCampoTrabajo" runat="server" readonly="readonly" class="form-control form-control-sm"></textarea>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-6 col-md-6 border-left border-gray">
                                        <h6 class="font-weight-bold">Documentación</h6>
                                        <div class="form-group row">
                                            <div class="col-12">
                                                <!-- Div donde se muestran las imágenes de informacion de campo del trabajo -->
                                                <div class="align-self-center" id="divDocumentacionCampoTrabajo" runat="server" style="display: none;">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>


                                <%--<div class="row" id="" style="display: none;">
<div class="col-md-8">

<div class="form-group row">
<label class="col-sm-6">Gestor validador</label>
<asp:Label ID="lblGestorValidadorTrabajo" CssClass="col-sm-6" runat="server"></asp:Label>

<label class="col-sm-6">Resolucion</label>
<asp:Label ID="lblResolucionTrabajo" CssClass="col-sm-6" runat="server"></asp:Label>

<label class="col-sm-6">Fecha de validacion</label>
<asp:Label ID="lblFechaValidacionTrabajo" CssClass="col-sm-6" runat="server"></asp:Label>

<label class="col-sm-6">Observaciones</label>
<asp:Label ID="lblObservacionesCampoTrabajo" CssClass="col-sm-6" runat="server"></asp:Label>
</div>
</div>
<div class="col-md-3">
<label class="header-title tituloDocumentacionCampoLaboral">Documentación</label>
<div class="container" id="divDocumentacionCampoTrabajo">
</div>
</div>
</div>--%>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>

    <!--modal de detalles del registro -->
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
                        <div class="tab-pane active p-3" id="tabTiempos" role="tabpanel">
                            <div class="form-group row justify-content-center" id="divSolicitudInactiva" style="display: none;">
                                <h6 class="text-danger">Solicitud Inactiva</h6>
                            </div>
                            <div class="table-responsive">
                                <table class="table mb-0 table-striped" id="tblDetalleEstado">
                                    <thead class="thead-light">
                                        <tr>
                                            <th>Proceso</th>
                                            <th>Inicio</th>
                                            <th>Fin</th>
                                            <th>Tiempo (D:H:M:S)</th>
                                        </tr>
                                    </thead>
                                    <tbody></tbody>
                                </table>
                            </div>
                        </div>
                        <div class="tab-pane p-3" id="tabDetalles" role="tabpanel">
                            <div class="row">
                                <div class="col-md-12">
                                    <h5 class="text-danger text-center font-weight-bold">Rechazado por analistas</h5>
                                    <ul class="timeline pl-3 mb-0">
                                        <li>
                                            <label class="mb-0">Razón de reprogramación <span class="font-weight-bold">(Wilmer Zavala)</span></label>
                                            <label class="float-right">09 Noviembre, 2020</label>
                                            <p>No se presentó comprobante de domicilio</p>
                                        </li>
                                        <li>
                                            <label class="mb-0">Observaciones información personal <span class="font-weight-bold">(Willian Díaz)</span></label>
                                            <label class="float-right">09 Noviembre, 2020</label>
                                            <p>Se validaron documentos adjuntados por vendedores.</p>
                                        </li>
                                        <li>
                                            <label class="mb-0">Observaciones información laboral <span class="font-weight-bold">(Willian Díaz)</span></label>
                                            <label class="float-right">09 Noviembre, 2020</label>
                                            <p>Se validaron documentos adjuntados por vendedores.</p>
                                        </li>
                                        <li>
                                            <label class="mb-0">Observaciones de referencias personales <span class="font-weight-bold">(Keyla Hernandez)</span></label>
                                            <label class="float-right">09 Noviembre, 2020</label>
                                            <p>No se presentó comprobante de pago</p>
                                        </li>
                                        <li>
                                            <label class="mb-0">Observaciones de la documentación <span class="font-weight-bold">(Keyla Hernandez)</span></label>
                                            <label class="float-right">09 Noviembre, 2020</label>
                                            <p>No se presentó comprobante de pago</p>
                                        </li>
                                        <li>
                                            <label class="mb-0">Observaciones para gestoría <span class="font-weight-bold">(Willian Díaz)</span></label>
                                            <label class="float-right">09 Noviembre, 2020</label>
                                            <p>Validar campo trabajo y domicilio, cliente recompra formal y buro activo.</p>
                                        </li>
                                        <li>
                                            <label class="mb-0">Observaciones de gestoría <span class="font-weight-bold">(Wilmer Zavala)</span></label>
                                            <label class="float-right">09 Noviembre, 2020</label>
                                            <p>Se validaron documentos adjuntados por vendedores.</p>
                                        </li>
                                        <li>
                                            <label class="mb-0">Comentarios de la resolución <span class="font-weight-bold">(Keyla Hernandez)</span></label>
                                            <label class="float-right">09 Noviembre, 2020</label>
                                            <p class="mb-0">Se validaron documentos adjuntados por vendedores.</p>
                                        </li>
                                    </ul>
                                </div>
                            </div>


                            <%--<div class="form-group row" id="divDetalleResolucion">
<div class="col-sm-12 text-center">
<label id="lblResolucion" class="col-form-label text-warning">En recepción</label>
</div>
</div>
<div class="form-group row" id="divNoHayMasDetalles" style="display: none;">
<div class="col-sm-12 text-center">
<label class="col-form-label">No hay más detalles de esta solicitud</label>
</div>
</div>
<div class="form-group row" id="divReprogramado" style="display: none;">
<label class="col-sm-5 col-form-label"><strong>Razón reprogramado:</strong></label>
<div class="col-sm-7">
<label id="lblReprogramado" class="col-form-label">No se presentó comprobante de ingresos</label>
</div>
</div>
<div class="form-group row" id="divCondicionado" style="display: none;">
<label class="col-sm-5 col-form-label"><strong>Razón condicionado:</strong></label>
<div class="col-sm-7">
<label id="lblCondicionado" class="col-form-label">No se presentó comprobante de ingresos</label>
</div>
</div>
<div class="form-group row" id="divDocumentacionComentario" style="display: none;">
<label class="col-sm-5 col-form-label"><strong>Observaciones documentación:</strong></label>
<div class="col-sm-7">
<label id="lblDocumentacionComentario" class="col-form-label">No se presentó comprobante de ingresos ni croquis de domicilio</label>
</div>
</div>
<div class="form-group row" id="divInfoPersonal" style="display: none;">
<label class="col-sm-5 col-form-label"><strong>Observaciones info. personal:</strong></label>
<div class="col-sm-7">
<label id="lblInfoPersonalComentario" class="col-form-label">No se presentó comprobante de ingresos</label>
</div>
</div>
<div class="form-group row" id="divInfoLaboral" style="display: none;">
<label class="col-sm-5 col-form-label"><strong>Observaciones info. laboral:</strong></label>
<div class="col-sm-7">
<label id="lblInfoLaboralComentario" class="col-form-label">No se presentó comprobante de ingresos</label>
</div>
</div>
<div class="form-group row" id="divReferencias" style="display: none;">
<label class="col-sm-5 col-form-label"><strong>Observaciones referencias:</strong></label>
<div class="col-sm-7">
<label id="lblReferenciasComentario" class="col-form-label">No se presentó comprobante de ingresos</label>
</div>
</div>
<div class="form-group row" id="divCampo" style="display: none;">
<label class="col-sm-5 col-form-label"><strong>Observaciones para campo:</strong></label>
<div class="col-sm-7">
<label id="lblCampoComentario" class="col-form-label">No se presentó comprobante de ingresos</label>
</div>
</div>
<div class="form-group row" id="divGestoria" style="display: none;">
<label class="col-sm-5 col-form-label"><strong>Observaciones de gestoria:</strong></label>
<div class="col-sm-7">
<label id="lblGestoriaComentario" class="col-form-label">No se presentó comprobante de ingresos</label>
</div>
</div>
<div class="form-group row" id="divComentarioResolucion" style="display: none;">
<label class="col-sm-5 col-form-label"><strong>Comentario resolución:</strong></label>
<div class="col-sm-7">
<label id="lblResolucionComentario" class="col-form-label">El cliente se arrepintió</label>
</div>
</div>--%>
                        </div>
                        <div class="tab-pane p-3" id="listaCondiciones" role="tabpanel">
                            <table id="tblListaSolicitudCondiciones" runat="server" class="table table-condensed table-striped">
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

    <div id="modalComentarioReferencia" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="modalComentarioReferenciaLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h6 class="modal-title mt-0" id="modalComentarioReferenciaLabel">Observaciones del departamento de crédito</h6>
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                </div>
                <div class="modal-body">



                    <div class="ug-thumb-wrapper ug-tile ug-tile-clickable" style="z-index: 3; opacity: 1; background-color: rgb(240, 240, 240); border-width: 3px; border-style: solid; border-color: rgb(240, 240, 240); border-radius: 2px; box-shadow: rgb(139, 139, 139) 1px 1px 3px 2px; width: 150px; height: 99px; position: absolute; margin: 0px; left: 350px; top: 10px;">
                        <img src="http://172.20.3.140/Documentos/Solicitudes/Solicitud490/CTES490D18-20201405T091432-1.png" alt="Segunda ID frontal (Licencia, RTN, etc)" class="ug-thumb-image ug-trans-enabled" style="opacity: 1; width: 144px; height: 231px; left: 0px; top: -69px;">
                    </div>


                    <label>Referencia personal: <span class="font-weight-bold" id="lblNombreReferenciaModal"></span></label>
                    <div class="form-group">
                        <label class="col-form-label">Observaciones/Comentarios <span class="font-weight-bold">(Keyla Hernandez)</span></label>
                        <textarea id="txtObservacionesReferencia" readonly="readonly" class="form-control" maxlength="225" rows="2" data-parsley-group="informacionLaboral"></textarea>
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

    <div id="modalFinalizarValidarDocumentacion" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="modalFinalizarValidarDocumentacionLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h6 class="modal-title mt-0" id="modalFinalizarValidarDocumentacionLabel">Documentación de la solicitud</h6>
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

    <script src="/Scripts/js/jquery.min.js"></script>
    <script src="/Scripts/js/bootstrap.bundle.min.js"></script>
    <script src="/Scripts/plugins/iziToast/js/iziToast.min.js"></script>
    <script src="/Scripts/plugins/unitegallery/js/unitegallery.min.js"></script>
    <script src="/Scripts/plugins/unitegallery/themes/default/ug-theme-default.js"></script>
    <script src="/Scripts/plugins/unitegallery/themes/tilesgrid/ug-theme-tilesgrid.js"></script>
    <script src="/Scripts/plugins/unitegallery/themes/tiles/ug-theme-tiles.js"></script>
    <script src="/Scripts/plugins/imgBox/jquery-rotate.min.js"></script>
    <script src="/Scripts/plugins/imgBox/jquery.imgbox.js?v=20200903161022"></script>
    <script src="../../Scripts/app/solicitudes/SolicitudesCredito_Utilitarios.js"></script>
    <script src="../../Scripts/app/solicitudes/SolicitudesCredito_Detalles.js?v=202008211455"></script>

    <script>
//$("#divDocumentacionCedula").unitegallery({
// gallery_theme: "tilesgrid",
// tile_width: 150,
// tile_height: 97,
// grid_num_rows: 15
//});

//$("#divDocumentacionDomicilio").unitegallery({
// gallery_theme: "tilesgrid",
// tile_width: 180,
// tile_height: 120,
// grid_num_rows: 15
//});

//$("#divDocumentacionLaboral").unitegallery({
// gallery_theme: "tilesgrid",
// tile_width: 180,
// tile_height: 120,
// grid_num_rows: 15
//});

//$("#divDocumentacionCampoDomicilio").unitegallery({
// gallery_theme: "tilesgrid",
// tile_width: 180,
// tile_height: 120,
// grid_num_rows: 15
//});

//$("#divDocumentacionCampoTrabajo").unitegallery({
// gallery_theme: "tilesgrid",
// tile_width: 180,
// tile_height: 120,
// grid_num_rows: 15
//});

//$("#divDocumentacionCedula").unitegallery({
// gallery_theme: "tilesgrid",
// tile_width: 150,
// tile_height: 97,
// grid_num_rows: 15
//});

//$(document).on('click', 'div.ug-thumb-overlay', function () {
// alert('enter full screen');
// $('html').append(btnRotarImagenes);
//});

//$(document).on('click', 'img', function () {
// alert('clickkkkk en imagen');
//});
    </script>

    <%--<script>

var btnRotarImagenes = $('<button class="check-original-image btn btn-hover"><i class="mdi mdi-rotate-right"></i></button>').on('click', function () {
RotarImagenes();
});

angle = 90;

function RotarImagenes() {
var image = $('.ug-item-wrapper').children('img');
$(image).rotate(angle);
angle += 90;
}

function ReiniciarAnguloRotacion() {
angle = 0;
}
</script>--%>

    <script type="text/javascript">

        var btnRotarImagenes = $('<button class="check-original-image btn btn-hover"><i class="mdi mdi-rotate-right"></i></button>').on('click', function () {
            RotarImagenes();
        });


        var api;
        jQuery(document).ready(function () {
            api = jQuery("#divDocumentacionCedula").unitegallery();

            api.on("enter_fullscreen", function () {
                alert('enter_fullscreen');
                $('html').append(btnRotarImagenes);
            });

            api.on("exit_fullscreen", function () {
                alert('exit_fullscreen');
                btnRotarImagenes.remove();
            });

        });

        function RotarImagenes() {
            var image = $('.ug-item-wrapper').children('img');
            $(image).rotate(angle);
            angle += 90;
        }

        function ReiniciarAnguloRotacion() {
            angle = 0;
        }
    </script>

</body>
</html>
