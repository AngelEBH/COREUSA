﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Solicitudes_CANEX_Detalles.aspx.cs" Inherits="Solicitudes_CANEX_Detalles" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <title>Detalles de la solicitud CANEX</title>
    <!-- BOOTSTRAP -->
    <link href="/Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/Content/css/icons.css" rel="stylesheet" />
    <link href="/Content/css/style.css" rel="stylesheet" />
    <!-- ARCHIVOS NECESARIOS PARA EL FUNCIONAMIENTO DE LA PAGINA -->
    <link href="/Scripts/plugins/iziToast/css/iziToast.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/magnific-popup/magnific-popup.css" rel="stylesheet" />
    <link href="/Scripts/plugins/select2/css/select2.min.css" rel="stylesheet" />
    <link href="/CSS/Estilos_CSS.css" rel="stylesheet" />
    <style>
        .card-header {
            background-color: #ffffff;
        }

        .seccion-header {
            background-color: #e9ecef;
            border-bottom: 1px solid rgba(0,0,0,.125);
            font-weight: 500;
        }
    </style>
</head>
<body class="EstiloBody" style="height: calc(100vh - 5px);">
    <form id="form1" runat="server">
        <div class="card">
            <div class="card-header">
                <div class="row">
                    <!-- Información del producto -->
                    <div class="col-md-6 form-inline p-0">
                        <div class="spinner-border" role="status" id="divCargandoAnalisis">
                            <span class="sr-only">Cargando</span>
                        </div>
                        <asp:Image runat="server" ID="LogoPrestamo" class="float-left LogoPrestamo" alt="Logo del Producto" Style="display: none;" />
                        <asp:Label runat="server" ID="lblTipoPrestamo" CssClass="h3"></asp:Label>
                    </div>
                    <!-- Acciones generales -->
                    <div class="col-md-6 form-inline justify-content-end pr-0">
                        <div class="button-items">
                            <button runat="server" ID="btnHistorialExterno" class="btn btn-sm btn-success waves-effect waves-light" type="button">
                                Buro externo
                            </button>

                            <button runat="server" ID="btnValidoDocumentacionModal" type="button" data-toggle="modal" data-target="#modalDocumentos" class="btn btn-success btn-sm waves-effect waves-light">
                                <small>Ver docs</small>
                            </button>

                            <button runat="server" ID="btnCondicionarSolicitud" class="btn btn-sm btn-warning validador" runat="server" data-toggle="modal" data-target="#modalCondicionarSolicitud" type="button">
                                Condicionar
                            </button>
                            <button runat="server" ID="btnRechazar" class="btn btn-sm btn-danger waves-effect waves-light validador" data-toggle="modal" data-target="#modalResolucionRechazar" type="button">
                                Rechazar
                            </button>
                            <button runat="server" id="btnAceptarSolicitud" class="btn btn-sm btn-warning waves-effect waves-light validador" disabled="disabled" data-toggle="modal" data-target="#modalResolucionAprobar" title="" type="button">
                                Aceptar
                            </button>
                        </div>
                    </div>
                    <!-- Información del cliente -->
                    <div class="col-md-6 seccion-header border-bottom">
                        <i class="mdi mdi-account mdi-24px mt-1"></i>
                        <asp:Label ID="spanNombreCliente" CssClass="col-form-label-lg EliminarEspacios mt-1" runat="server"></asp:Label>
                    </div>
                    <div class="col-md-6 seccion-header border-bottom text-right">
                        <label class="col-form-label-lg EliminarEspacios mt-1">Identidad:&nbsp;</label>
                        <asp:Label ID="spanIdentidadCliente" CssClass="col-form-label-lg EliminarEspacios mt-1" runat="server"></asp:Label>
                    </div>
                    <!-- Información de la solicitud -->
                    <div class="col-md-2 form-inline seccion-header border-bottom-0">
                        No. Solicitud:&nbsp;
                        <asp:Label ID="lblNoSolicitud" CssClass="col-form-label" runat="server"></asp:Label>
                    </div>
                    <div class="col-md-2 form-inline seccion-header border-bottom-0">
                        No. Cliente:&nbsp;
                        <asp:Label ID="lblNoCliente" CssClass="col-form-label" runat="server"></asp:Label>
                    </div>
                    <div class="col-md-2 form-inline seccion-header border-bottom-0">
                        Tipo Solicitud:&nbsp;
                        <asp:Label ID="lblTipoSolicitud" CssClass="col-form-label" runat="server"></asp:Label>
                    </div>
                    <div class="col-md-2 form-inline seccion-header border-bottom-0">
                        Agente Ventas:&nbsp;
                        <asp:Label ID="lblAgenteDeVentas" CssClass="col-form-label" runat="server"></asp:Label>
                    </div>
                    <div class="col-md-2 form-inline seccion-header border-bottom-0">
                        Agencia:&nbsp;
                        <asp:Label ID="lblAgencia" CssClass="col-form-label" runat="server"></asp:Label>
                    </div>
                    <div class="col-md-2 form-inline seccion-header border-bottom-0">
                        Estado:&nbsp;
                        <asp:Label ID="lblEstadoSolicitud" runat="server"></asp:Label>
                    </div>
                </div>
            </div>
            <div class="card-body">
                <div class="row">
                    <div class="col-12 p-0">
                        <div class="collapse-group">

                            <!-- INFORMACION PERSONAL -->
                            <div class="panel panel-default">
                                <div class="panel-heading card-header p-1 seccion-header" role="tab" id="headingOne">
                                    <h6 class="panel-title m-0 font-14">
                                        <a role="button" data-toggle="collapse" href="#collapseInformacionPersonal" class="text-dark h5 trigger collapsed"
                                            aria-expanded="true"
                                            aria-controls="collapseOne">
                                            <i class="mdi mdi-account-circle mdi-24px"></i>&nbsp;Informacion Personal
                                        </a>
                                    </h6>
                                </div>
                                <div id="collapseInformacionPersonal" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingOne">
                                    <div class="panel-body">
                                        <div class="card-body">
                                            <div class="row">
                                                <div class="col-md-8">
                                                    <div class="form-group row">
                                                        <label class="col-sm-6">RTN Cliente</label>
                                                        <asp:Label ID="lblRtnCliente" CssClass="col-sm-6" runat="server"></asp:Label>

                                                        <label class="col-sm-6">Teléfono primario</label>
                                                        <asp:HyperLink ID="lblNumeroTelefono" NavigateUrl="tel:+55599999999" CssClass="col-sm-6" runat="server"></asp:HyperLink>

                                                        <label class="col-sm-6">Teléfono alternativo</label>
                                                        <asp:HyperLink ID="lblNumeroTelefonoAlternativo" NavigateUrl="tel:+55599999999" CssClass="col-sm-6" runat="server"></asp:HyperLink>

                                                        <label class="col-sm-6">Nacionalidad</label>
                                                        <asp:Label ID="lblNacionalidad" CssClass="col-sm-6" runat="server"></asp:Label>

                                                        <label class="col-sm-6">Fecha de nacimiento</label>
                                                        <asp:Label ID="lblFechaNacimientoCliente" CssClass="col-sm-6" runat="server"></asp:Label>

                                                        <label class="col-sm-6">Edad</label>
                                                        <asp:Label ID="lblEdadCliente" CssClass="col-sm-6" runat="server"></asp:Label>

                                                        <label class="col-sm-6">Profesion u oficio</label>
                                                        <asp:Label ID="lblProfesionCliente" CssClass="col-sm-6" runat="server"></asp:Label>

                                                        <label class="col-sm-6">Sexo</label>
                                                        <asp:Label ID="lblSexoCliente" CssClass="col-sm-6" runat="server"></asp:Label>

                                                        <label class="col-sm-6">Estado civil</label>
                                                        <asp:Label ID="lblEstadoCivilCliente" CssClass="col-sm-6" runat="server"></asp:Label>
                                                    </div>
                                                </div>
                                                <div class="col-md-3">
                                                    <label class="header-title TituloDivDocumentacion">Documentación</label>
                                                    <div runat="server" class="container" id="divDocumentacionCedula">
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!-- INFORMACION DOMICILIO -->
                            <div class="panel panel-default">
                                <div class="panel-heading card-header p-1 seccion-header" role="tab" id="headingTwo">
                                    <h6 class="panel-title m-0 font-14">
                                        <a href="#collapseInformacionDomiciliar" class="text-dark collapsed h5 collapsed" data-toggle="collapse"
                                            aria-expanded="false"
                                            aria-controls="collapseTwo">
                                            <i class="mdi mdi-home-variant mdi-24px"></i>
                                            Informacion Domicilio
                                        </a>
                                    </h6>
                                </div>
                                <div id="collapseInformacionDomiciliar" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingTwo">
                                    <div class="panel-body">
                                        <div class="card-body">
                                            <div class="row">
                                                <div class="col-md-8">
                                                    <div class="form-group row">
                                                        <label class="col-sm-6">Vivienda</label>
                                                        <asp:Label ID="lblViviendaCliente" CssClass="col-sm-6" runat="server"></asp:Label>

                                                        <label class="col-sm-6">Tiempo de residir</label>
                                                        <asp:Label ID="lblTiempoResidirCliente" CssClass="col-sm-6" runat="server"></asp:Label>

                                                        <label class="col-sm-6">Departamento</label>
                                                        <asp:Label ID="lblDeptoCliente" CssClass="col-sm-6" runat="server"></asp:Label>

                                                        <label class="col-sm-6">Municipio</label>
                                                        <asp:Label ID="lblMunicipioCliente" CssClass="col-sm-6" runat="server"></asp:Label>

                                                        <label class="col-sm-6">Ciudad</label>
                                                        <asp:Label ID="lblCiudadCliente" CssClass="col-sm-6" runat="server"></asp:Label>

                                                        <label class="col-sm-6">Barrio/colonia</label>
                                                        <asp:Label ID="lblBarrioColoniaCliente" CssClass="col-sm-6" runat="server"></asp:Label>

                                                        <label class="col-sm-6">Dirección detallada</label>
                                                        <asp:Label ID="lblDireccionDetalladaCliente" CssClass="col-sm-6" runat="server"></asp:Label>

                                                        <label class="col-sm-6">Referencias del domicilio</label>
                                                        <asp:Label ID="lblReferenciaDomicilioCliente" CssClass="col-sm-6" runat="server"></asp:Label>
                                                    </div>
                                                </div>
                                                <div class="col-md-3">
                                                    <label class="header-title TituloDivDocumentacion">Documentación</label>
                                                    <div runat="server" class="container" id="divDocumentacionDomicilio">
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!-- INFORMACION CONYUGAL -->
                            <div runat="server" class="panel panel-default" id="divConyugueCliente">
                                <div class="panel-heading card-header p-1 seccion-header" role="tab" id="headingThree">
                                    <h6 class="m-0 font-14">
                                        <a href="#collapseThree" class="text-dark collapsed h5" data-toggle="collapse"
                                            aria-expanded="false"
                                            aria-controls="collapseThree">
                                            <i class="mdi mdi-account-multiple mdi-24px"></i>
                                            Informacion Conyugal
                                        </a>
                                    </h6>
                                </div>
                                <div id="collapseThree" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingThree">
                                    <div class="panel-body">
                                        <div class="card-body">
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group row">
                                                        <label class="col-sm-4">Nombre del conyugue</label>
                                                        <asp:Label ID="lblNombreConyugue" CssClass="col-sm-8" runat="server"></asp:Label>

                                                        <label class="col-sm-4">Fecha de nacimiento</label>
                                                        <asp:Label ID="lblFechaNacimientoConygue" CssClass="col-sm-8" runat="server"></asp:Label>

                                                        <label class="col-sm-4">Telefono conyugue</label>
                                                        <asp:HyperLink ID="lblTelefonoConyugue" CssClass="col-sm-8" NavigateUrl="tel:55599999999" runat="server"></asp:HyperLink>

                                                        <label class="col-sm-4">Profesion u Oficio </label>
                                                        <asp:Label ID="lblProfesionOficioConyugue" CssClass="col-sm-8" runat="server"></asp:Label>

                                                        <label class="col-sm-4">Ocupación</label>
                                                        <asp:Label ID="lblOcupacionConyugue" CssClass="col-sm-8" runat="server"></asp:Label>

                                                        <label class="col-sm-4">Lugar de trabajo conyugue</label>
                                                        <asp:Label ID="lblLugarTrabajoConyugue" CssClass="col-sm-8" runat="server"></asp:Label>

                                                        <label class="col-sm-4">Puesto asignado</label>
                                                        <asp:Label ID="lblPuestoAsignadoConyugue" CssClass="col-sm-8" runat="server"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!-- INFORMACION LABORAL -->
                            <div class="panel panel-default">
                                <div class="panel-heading card-header p-1 seccion-header" role="tab" id="headingFour">
                                    <h6 class="m-0 font-14">
                                        <a href="#collapseInformacionLaboral" class="text-dark collapsed h5" data-toggle="collapse"
                                            aria-expanded="false"
                                            aria-controls="collapseThree">
                                            <i class="mdi mdi-briefcase-check mdi-24px"></i>
                                            Informacion Laboral
                                        </a>
                                    </h6>
                                </div>
                                <div id="collapseInformacionLaboral" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour">
                                    <div class="panel-body">
                                        <div class="card-body">
                                            <div class="row">
                                                <div class="col-md-8">
                                                    <div class="form-group row">
                                                        <label class="col-sm-6">Nombre del trabajo</label>
                                                        <asp:Label ID="lblNombreTrabajoCliente" CssClass="col-sm-6" runat="server"></asp:Label>

                                                        <label class="col-sm-6">Ingresos mensuales base</label>
                                                        <asp:Label ID="lblIngresosMensualesCliente" CssClass="col-sm-6" runat="server"></asp:Label>

                                                        <asp:Label ID="lblComisionesClienteTitulo" CssClass="col-sm-6" runat="server">Ingresos mensuales comisiones</asp:Label>
                                                        <asp:Label ID="lblComisionesCliente" CssClass="col-sm-6" runat="server"></asp:Label>

                                                        <label class="col-sm-6">Puesto asigando</label>
                                                        <asp:Label ID="lblPuestoAsignadoCliente" CssClass="col-sm-6" runat="server"></asp:Label>

                                                        <label class="col-sm-6">Teléfono empresa</label>
                                                        <asp:HyperLink ID="lblTelefonoEmpresaCliente" CssClass="col-sm-6" NavigateUrl="tel:55599999999" runat="server"></asp:HyperLink>

                                                        <label class="col-sm-6">Extensión</label>
                                                        <asp:Label ID="lblExtension" CssClass="col-sm-6" runat="server"></asp:Label>

                                                        <label class="col-sm-6">Departamento empresa</label>
                                                        <asp:Label ID="lblDeptoEmpresa" CssClass="col-sm-6" runat="server"></asp:Label>

                                                        <label class="col-sm-6">Municipio empresa</label>
                                                        <asp:Label ID="lblMunicipioEmpresa" CssClass="col-sm-6" runat="server"></asp:Label>

                                                        <label class="col-sm-6">Ciudad empresa</label>
                                                        <asp:Label ID="lblCiudadEmpresa" CssClass="col-sm-6" runat="server"></asp:Label>

                                                        <label class="col-sm-6">Barrio/colonia empresa</label>
                                                        <asp:Label ID="lblBarrioColoniaEmpresa" CssClass="col-sm-6" runat="server"></asp:Label>

                                                        <label class="col-sm-6">Direccion detallada empresa</label>
                                                        <asp:Label ID="lblDireccionDetalladaEmpresa" CssClass="col-sm-6" runat="server"></asp:Label>

                                                        <label class="col-sm-6">Referencias ubicación</label>
                                                        <asp:Label ID="lblReferenciaUbicacionEmpresa" CssClass="col-sm-6" runat="server"></asp:Label>
                                                    </div>
                                                </div>
                                                <div class="col-md-3">
                                                    <label class="header-title TituloDivDocumentacion">Documentación</label>
                                                    <div runat="server" class="container" id="divDocumentacionLaboral">
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!-- REFERENCIAS PERSONALES DEL CLIENTE -->
                            <div class="panel panel-default">
                                <div class="panel-heading card-header p-1 seccion-header" role="tab" id="headingFive">
                                    <h6 class="m-0 font-14">
                                        <a href="#collapseReferenciasPersonales" class="text-dark collapsed h5" data-toggle="collapse"
                                            aria-expanded="false"
                                            aria-controls="collapseFive">
                                            <i class="mdi mdi-phone-log mdi-24px"></i>
                                            Referencias Personales del Cliente
                                        </a>
                                    </h6>
                                </div>
                                <div id="collapseReferenciasPersonales" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFive">
                                    <div class="panel-body">
                                        <div class="card-body">
                                            <div class="form-group row">
                                                <h4>Referencias personales</h4>
                                                <div class="table-responsive">
                                                    <table runat="server" class="table-condensed table-bordered table-striped" style="width: 100%" id="tblReferencias">
                                                        <thead class="thead-light">
                                                            <tr>
                                                                <th>Nombre referencia</th>
                                                                <th>Lugar de trabajo</th>
                                                                <th>Tiempo de conocer ref</th>
                                                                <th>Telefono ref</th>
                                                                <th>Parentesco ref</th>
                                                            </tr>
                                                        </thead>
                                                        <tbody class="table-condensed">
                                                        </tbody>
                                                    </table>
                                                </div>
                                            </div>
                                        <%--<div class="form-group row" id="divAval">
                                                <h4>Aval</h4>
                                                <div class="table-responsive">
                                                    <table runat="server" class="table table-condensed" id="tblAvales">
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
                                                        <tbody>
                                                        </tbody>
                                                    </table>
                                                </div>
                                            </div>--%>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!-- INFORMACION DEL PRESTAMO Y EL CALCULO DEL MISMO -->
                            <div class="panel panel-default">
                                <div class="panel-heading card-header p-1 seccion-header" role="tab" id="headingSix">
                                    <h6 class="m-0 font-14">
                                        <a href="#collapsePrestamoRequerido" class="text-dark collapsed h5" data-toggle="collapse"
                                            aria-expanded="false"
                                            aria-controls="collapseSix">
                                            <i class="mdi mdi-cash-multiple mdi-24px"></i>
                                            Información del Préstamo Requerido
                                        </a>
                                    </h6>
                                </div>
                                <div id="collapsePrestamoRequerido" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingSix">
                                    <div class="panel-body">
                                        <div class="card-body">
                                            <div class="row">
                                                <!-- INFORMACION DEL PRECALIFICADO -->
                                                <div class="col-md-6 border">
                                                    <div class="form-group row">
                                                        <label class="col-sm-12 h6 text-center p-t-10">Capacidad de Pago - Precalificado</label>

                                                        <label class="col-sm-6">Ingresos precalificado</label>
                                                        <asp:Label ID="lblIngresosPrecalificado" CssClass="col-sm-6" runat="server"></asp:Label>

                                                        <label class="col-sm-6">Obligaciones precalificado</label>
                                                        <asp:Label ID="lblObligacionesPrecalificado" CssClass="col-sm-6 text-danger" runat="server"></asp:Label>

                                                        <label class="col-sm-6">Disponible precalificado</label>
                                                        <asp:Label ID="lblDisponiblePrecalificado" CssClass="col-sm-6" runat="server"></asp:Label>

                                                        <label class="col-sm-6">Capacidad de pago mensual</label>
                                                        <asp:Label ID="lblCapacidadPagoMensual" CssClass="col-sm-6" runat="server"></asp:Label>

                                                        <label class="col-sm-6">Capacidad de quincenal</label>
                                                        <asp:Label ID="lblCapacidadPagoQuincenal" CssClass="col-sm-6" runat="server"></asp:Label>
                                                    </div>
                                                </div>
                                                <!-- INFORMACION DEL PRESTAMO CON LOS DATOS DEL PRECALIFICADO-->
                                                <div class="col-md-6 border">
                                                    <div class="form-group row">
                                                        <label class="col-sm-12 h6 text-center p-t-10">Informacion Préstamo Requerido</label>

                                                        <label id="lblTituloValorPMO" class="col-sm-6">Valor Global</label>
                                                        <asp:Label ID="lblValorGlobal" CssClass="col-sm-6" runat="server"></asp:Label>

                                                        <label id="lblValorPrimaTitulo" class="col-sm-6">Valor Prima</label>
                                                        <asp:Label ID="lblValorPrima" CssClass="col-sm-6" runat="server"></asp:Label>

                                                        <asp:Label ID="lblPlazoTitulo" CssClass="col-sm-6" Text="Plazo" runat="server" Style="font-weight: 500; margin-bottom: .5rem;"></asp:Label>
                                                        <asp:Label ID="lblPlazo" CssClass="col-sm-6" runat="server"></asp:Label>

                                                        <label class="col-sm-6">Monto Financiar</label>
                                                        <asp:Label ID="lblMontoFinanciar" CssClass="col-sm-6" runat="server"></asp:Label>
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
            </div>
        </div>

        <!-- modal documentacion -->
        <div id="modalDocumentos" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="modalDocumentosLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title mt-0" id="modalDocumentosLabel">Documentación</h5>
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
                                    <span class="d-none d-sm-block">Agregar Nuevas Condiciones</span>
                                </a>
                            </li>
                            <li class="nav-item" runat="server" id="pestanaListaSolicitudCondiciones" style="display:none;">
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
                                            <input id="txtComentarioAdicional" required="required" type="text" class="form-control form-control-sm" placeholder="comentario adicional" data-parsley-maxlength="128" />
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
                        ¿Está seguro de que desea <strong>RECHAZAR</strong> esta solicitud?<br />
                        <br />
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
                        ¿Está seguro de que desea <strong>ACEPTAR</strong> esta solicitud (Se importará a la bandeja de solicitudes)?<br />
                        <br />
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
    </form>
    <script src="/Scripts/js/jquery.min.js"></script>
    <script src="/Scripts/js/bootstrap.bundle.min.js"></script>
    <!-- ARCHIVOS NECESARIOS PARA LA PANTALLA -->
    <script src="/Scripts/plugins/iziToast/js/iziToast.min.js"></script>
    <script src="/Scripts/plugins/parsleyjs/parsley.js"></script>
    <script src="/Scripts/plugins/imgBox/jquery.imgbox.js"></script>
    <script src="/Scripts/plugins/select2/js/select2.full.min.js"></script>
    <script>
        $(document).ready(function () {
            IDPais = <%=this.IDPais%>;
            IDAgencia = <%=this.IDAgencia%>;
            IDSocio = <%=this.IDSocio%>;
            IDEstado = <%=this.IDEstadoSolicitud%>;
            IDSolicitudImportada = <%=this.IDSolicitudPrestadito%>;
        });
    </script>
    <script src="/Scripts/app/Solicitudes_CANEX/Solicitudes_CANEX_Detalles.js"></script>
</body>
</html>
