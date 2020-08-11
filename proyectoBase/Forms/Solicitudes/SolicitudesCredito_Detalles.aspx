<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SolicitudesCredito_Detalles.aspx.cs" Inherits="SolicitudesCredito_Detalles" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <title>Detalles de la solicitud</title>
    <!-- BOOTSTRAP -->
    <link href="../../Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../Content/css/icons.css" rel="stylesheet" />
    <link href="../../Content/css/style.css" rel="stylesheet" />
    <!-- ARCHIVOS NECESARIOS PARA EL FUNCIONAMIENTO DE LA PAGINA -->
    <link href="../../Scripts/plugins/iziToast/css/iziToast.min.css" rel="stylesheet" />
    <link href="../../Scripts/plugins/magnific-popup/magnific-popup.css" rel="stylesheet" />
    <link href="../../Scripts/plugins/select2/css/select2.min.css" rel="stylesheet" />
    <link href="/CSS/Estilos_CSS.css" rel="stylesheet" />
</head>
<body class="EstiloBody">
    <div class="row">
        <div class="col-12">
            <div class="card">
                <div class="card-header">
                    <div class="form-group row EliminarEspacios">
                        <!-- Icono del prestamo -->
                        <div class="col-md-6">
                            <div class="spinner-border" role="status" id="divCargandoAnalisis">
                                <span class="sr-only">Cargando</span>
                            </div>
                            <asp:Image runat="server" ID="LogoPrestamo" class="float-left LogoPrestamo" alt="Logo del Producto" Style="display: none;" />
                            <asp:Label runat="server" ID="lblTipoPrestamo" CssClass="h3"></asp:Label>
                        </div>
                        <!-- Botones de acceso directo al historial interno del cliente, buro externo y boton para condicionar la solicitud -->
                        <div class="col-md-6">
                            <div class="form-group row">
                                <div class="col-sm-3"></div>
                                <div class="col-sm-3">
                                    <button id="btnHistorialInterno" disabled="disabled" type="button" class="btn btn-sm btn-success btn-block waves-effect waves-light">
                                        Historial interno
                                    </button>
                                </div>
                                <div class="col-sm-3">
                                    <button id="btnHistorialExterno" disabled="disabled" type="button" class="btn btn-sm btn-success btn-block waves-effect waves-light">
                                        Buro externo
                                    </button>
                                </div>
                                <div class="col-sm-3">
                                    <button id="btnValidoDocumentacionModal" type="button" class="btn btn-sm btn-success btn-block waves-effect waves-light float-right">
                                        <small>Ver docs</small>
                                    </button>
                                </div>
                            </div>
                        </div>
                        <!-- Tabla estado del procesamiento de la solicitud -->
                        <div class="col-12">
                            <div class="table-responsive">
                                <table class="table table-condensed m-0">
                                    <thead class="thead-light">
                                        <tr>
                                            <th>
                                                <i class="mdi mdi-account mdi-24px"></i>
                                                <asp:Label ID="spanNombreCliente" CssClass="h4" runat="server"></asp:Label>
                                            </th>
                                            <th class="text-right">
                                                <label class="h4 EliminarEspacios">Identidad:&nbsp;</label>
                                                <asp:Label ID="spanIdentidadCliente" CssClass=" h4 EliminarEspacios" runat="server"></asp:Label>
                                            </th>
                                        </tr>
                                    </thead>
                                </table>
                            </div>
                            <div class="table-responsive">
                                <table class="table table-condensed table-borderless m-0">
                                    <thead class="thead-light">
                                        <tr>
                                            <th class="text-center">No. Solicitud:
                                                <asp:Label ID="lblNoSolicitud" runat="server"></asp:Label>
                                            </th>
                                            <th class="text-center">No. Cliente:
                                                <asp:Label ID="lblNoCliente" runat="server"></asp:Label>
                                            </th>
                                            <th class="text-center">Tipo Solicitud:
                                                <asp:Label ID="lblTipoSolicitud" runat="server"></asp:Label>
                                            </th>
                                            <th class="text-center">Agente Ventas:
                                                <asp:Label ID="lblAgenteDeVentas" runat="server"></asp:Label>
                                            </th>
                                            <th class="text-center">Agencia:
                                                <asp:Label ID="lblAgencia" runat="server"></asp:Label>
                                            </th>
                                        </tr>
                                    </thead>
                                </table>
                            </div>
                            <div class="table-responsive">
                                <table class="table table-condensed m-0" id="tblEstadoSolicitud">
                                    <thead class="thead-light">
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
                    <div class="row">
                        <div class="col-12">
                            <div class="collapse-group">

                                <!-- INFORMACION PERSONAL -->
                                <div class="panel panel-default">
                                    <div class="panel-heading card-header p-1 container-fluid" role="tab" id="headingOne">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <h6 class="panel-title m-0 font-14">
                                                    <a role="button" data-toggle="collapse" href="#collapseInformacionPersonal" class="text-dark h5 trigger collapsed"
                                                        aria-expanded="true"
                                                        aria-controls="collapseOne">
                                                        <i class="mdi mdi-account-circle mdi-24px"></i>&nbsp;Informacion Personal
                                                    </a>
                                                </h6>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="collapseInformacionPersonal" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingOne">
                                        <div class="panel-body">
                                            <div class="card-body">
                                                <div class="container-fluid">
                                                    <div class="row">
                                                        <div class="col-md-8">
                                                            <div class="form-group row">
                                                                <label class="col-sm-6">RTN Cliente</label>
                                                                <asp:Label ID="lblRtnCliente" CssClass="col-sm-6" runat="server"></asp:Label>

                                                                <label class="col-sm-6">Numero de telefono</label>
                                                                <asp:HyperLink ID="lblNumeroTelefono" NavigateUrl="tel:+55599999999" CssClass="col-sm-6" runat="server"></asp:HyperLink>

                                                                <label class="col-sm-6">Nacionalidad</label>
                                                                <asp:Label ID="lblNacionalidad" CssClass="col-sm-6" runat="server"></asp:Label>

                                                                <label class="col-sm-6">Fecha de nacimiento</label>
                                                                <asp:Label ID="lblFechaNacimientoCliente" CssClass="col-sm-6" runat="server"></asp:Label>

                                                                <label class="col-sm-6">Edad</label>
                                                                <asp:Label ID="lblEdadCliente" CssClass="col-sm-6" runat="server"></asp:Label>

                                                                <label class="col-sm-6">Correo electrónico</label>
                                                                <asp:HyperLink ID="lblCorreoCliente" CssClass="col-sm-6" NavigateUrl="mailto:correo@gmail.com" runat="server"></asp:HyperLink>

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
                                                            <div class="container" id="divDocumentacionCedula">
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
                                    <div class="panel-heading card-header p-1 container-fluid" role="tab" id="headingTwo">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <h6 class="panel-title m-0 font-14">
                                                    <a href="#collapseInformacionDomiciliar" class="text-dark collapsed h5 collapsed" data-toggle="collapse"
                                                        aria-expanded="false"
                                                        aria-controls="collapseTwo">
                                                        <i class="mdi mdi-home-variant mdi-24px"></i>
                                                        Informacion Domicilio
                                                    </a>
                                                </h6>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="collapseInformacionDomiciliar" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingTwo">
                                        <div class="panel-body">
                                            <div class="card-body">
                                                <div class="container-fluid">
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
                                                            <div class="container" id="divDocumentacionDomicilio">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <!-- INFORMACION CONYUGAL -->
                                <div class="panel panel-default" id="divConyugueCliente">
                                    <div class="panel-heading card-header p-1 container-fluid" role="tab" id="headingThree">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <h6 class="m-0 font-14">
                                                    <a href="#collapseThree" class="text-dark collapsed h5" data-toggle="collapse"
                                                        aria-expanded="false"
                                                        aria-controls="collapseThree">
                                                        <i class="mdi mdi-account-multiple mdi-24px"></i>
                                                        Informacion Conyugal
                                                    </a>
                                                </h6>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="collapseThree" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingThree">
                                        <div class="panel-body">
                                            <div class="card-body">
                                                <div class="container-fluid">
                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <div class="form-group row">
                                                                <label class="col-sm-4">Nombre del conyugue</label>
                                                                <asp:Label ID="lblNombreConyugue" CssClass="col-sm-8" runat="server"></asp:Label>

                                                                <label class="col-sm-4">Identidad del conyugue</label>
                                                                <asp:Label ID="lblIdentidadConyuge" CssClass="col-sm-8" runat="server"></asp:Label>

                                                                <label class="col-sm-4">Fecha de nacimiento</label>
                                                                <asp:Label ID="lblFechaNacimientoConygue" CssClass="col-sm-8" runat="server"></asp:Label>

                                                                <label class="col-sm-4">Telefono conyugue</label>
                                                                <asp:HyperLink ID="lblTelefonoConyugue" CssClass="col-sm-8" NavigateUrl="tel:55599999999" runat="server"></asp:HyperLink>

                                                                <label class="col-sm-4">Lugar de trabajo conyugue</label>
                                                                <asp:Label ID="lblLugarTrabajoConyugue" CssClass="col-sm-8" runat="server"></asp:Label>

                                                                <label class="col-sm-4">Telefono trabajo conyugue</label>
                                                                <asp:Label ID="lblTelefonoTrabajoConyugue" CssClass="col-sm-8" runat="server"></asp:Label>

                                                                <label class="col-sm-4">Ingresos mensuales conyugue</label>
                                                                <asp:Label ID="lblIngresosConyugue" CssClass="col-sm-8" runat="server"></asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <!-- INFORMACION LABORAL -->
                                <div class="panel panel-default">
                                    <div class="panel-heading card-header p-1 container-fluid" role="tab" id="headingFour">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <h6 class="m-0 font-14">
                                                    <a href="#collapseInformacionLaboral" class="text-dark collapsed h5" data-toggle="collapse"
                                                        aria-expanded="false"
                                                        aria-controls="collapseThree">
                                                        <i class="mdi mdi-briefcase-check mdi-24px"></i>
                                                        Informacion Laboral
                                                    </a>
                                                </h6>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="collapseInformacionLaboral" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour">
                                        <div class="panel-body">
                                            <div class="card-body">
                                                <div class="container-fluid">
                                                    <div class="row">
                                                        <div class="col-md-8">
                                                            <div class="form-group row">
                                                                <label class="col-sm-6">Nombre del trabajo</label>
                                                                <asp:Label ID="lblNombreTrabajoCliente" CssClass="col-sm-6" runat="server"></asp:Label>

                                                                <label class="col-sm-6">Ingresos mensuales</label>
                                                                <asp:Label ID="lblIngresosMensualesCliente" CssClass="col-sm-6" runat="server"></asp:Label>

                                                                <label class="col-sm-6">Puesto asigando</label>
                                                                <asp:Label ID="lblPuestoAsignadoCliente" CssClass="col-sm-6" runat="server"></asp:Label>

                                                                <label class="col-sm-6">Fecha de ingreso</label>
                                                                <asp:Label ID="lblFechaIngresoCliente" CssClass="col-sm-6" runat="server"></asp:Label>

                                                                <label class="col-sm-6">Arraigo laboral</label>
                                                                <asp:Label ID="lblArraigoLaboral" CssClass="col-sm-6" runat="server"></asp:Label>

                                                                <label class="col-sm-6">Teléfono empresa</label>
                                                                <asp:Label ID="lblTelefonoEmpresaCliente" CssClass="col-sm-6" runat="server"></asp:Label>

                                                                <label class="col-sm-6">Extensión RRHH</label>
                                                                <asp:Label ID="lblExtensionRecursosHumanos" CssClass="col-sm-6" runat="server"></asp:Label>

                                                                <label class="col-sm-6">Extensión cliente</label>
                                                                <asp:Label ID="lblExtensionCliente" CssClass="col-sm-6" runat="server"></asp:Label>

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

                                                                <label class="col-sm-6">Fuente de otros Ingresos</label>
                                                                <asp:Label ID="lblDescripcionOtrosIngresos" CssClass="col-sm-6" runat="server"></asp:Label>

                                                                <label class="col-sm-6">Valor otros ingresos</label>
                                                                <asp:Label ID="lblValorOtrosIngresos" CssClass="col-sm-6" runat="server"></asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-3">
                                                            <label class="header-title TituloDivDocumentacion">Documentación</label>
                                                            <div class="container" id="divDocumentacionLaboral">
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
                                    <div class="panel-heading card-header p-1 container-fluid" style="background-color: #e9ecef;" role="tab" id="headingSix">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <h6 class="m-0 font-14">
                                                    <a href="#collapsePrestamoRequerido" class="text-dark collapsed h5" data-toggle="collapse"
                                                        aria-expanded="false"
                                                        aria-controls="collapseSix">
                                                        <i class="mdi mdi-cash-multiple mdi-24px"></i>
                                                        Información del Préstamo Requerido
                                                    </a>
                                                </h6>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="collapsePrestamoRequerido" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingSix">
                                        <div class="panel-body">
                                            <div class="card-body">
                                                <div class="container-fluid">
                                                    <div class="row">
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
                                                                    <table class="table table-condensed table-striped" id="tblPMOSugeridosReales">
                                                                        <thead>
                                                                            <tr>
                                                                                <th>Monto a financiar</th>
                                                                                <th>Plazo</th>
                                                                                <th>Cutoa</th>
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
                                                                    </label>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <!-- INFORMACION DE ANALISIS -->
                                <div class="panel panel-default">
                                    <div class="panel-heading card-header p-1 container-fluid" style="background-color: #e9ecef;" role="tab" id="headingSeven">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <h6 class="m-0 font-14">
                                                    <a href="#collapseInformacionAnalisis" class="text-dark collapsed h5" data-toggle="collapse"
                                                        aria-expanded="false"
                                                        aria-controls="collapseSeven">
                                                        <i class="mdi mdi-account-search mdi-24px"></i>
                                                        Información de Perfil
                                                    </a>
                                                </h6>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="collapseInformacionAnalisis" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingSeven">
                                        <div class="panel-body">
                                            <div class="card-body">
                                                <div class="container-fluid">
                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <form id="frmInfoAnalisis">
                                                                <div class="form-group row">
                                                                    <label class="col-sm-3 col-form-label">Tipo de empresa</label>
                                                                    <div class="col-sm-3">
                                                                        <select id="tipoEmpresa" required="required" class="form-control">
                                                                            <option value="" selected="selected">Seleccionar</option>
                                                                            <option value="Privada">Privada</option>
                                                                            <option value="Publica">Publica</option>
                                                                            <option value="Propia">Propia</option>
                                                                        </select>
                                                                    </div>
                                                                </div>
                                                                <div class="form-group row">
                                                                    <label class="col-sm-3 col-form-label">Tipo de perfil</label>
                                                                    <div class="col-sm-3">
                                                                        <select id="tipoPerfil" required="required" class="form-control">
                                                                            <option value="" selected="selected">Seleccionar</option>
                                                                            <option value="Formal">Formal</option>
                                                                            <option value="Informa">Informal</option>
                                                                        </select>
                                                                    </div>
                                                                </div>
                                                                <div class="form-group row">
                                                                    <label class="col-sm-3 col-form-label">Tipo de empleo</label>
                                                                    <div class="col-sm-3">
                                                                        <select id="tipoEmpleo" required="required" class="form-control">
                                                                            <option value="" selected="selected">Seleccionar</option>
                                                                            <option value="Asalariado">Asalariado</option>
                                                                            <option value="Comerciante">Comerciante</option>
                                                                        </select>
                                                                    </div>
                                                                </div>
                                                                <div class="form-group row">
                                                                    <label class="col-sm-3 col-form-label">Buro actual</label>
                                                                    <div class="col-sm-3">
                                                                        <select id="buroActual" required="required" class="form-control">
                                                                            <option value="" selected="selected">Seleccionar</option>
                                                                            <option value="Con Historial">Con Historial</option>
                                                                            <option value="Sin Historial">Sin Historial</option>
                                                                        </select>
                                                                    </div>
                                                                </div>
                                                            </form>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <!-- REFERENCIAS PERSONALES DEL CLIENTE -->
                                <div class="panel panel-default">
                                    <div class="panel-heading card-header p-1 container-fluid" role="tab" id="headingFive">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <h6 class="m-0 font-14">
                                                    <a href="#collapseReferenciasPersonales" class="text-dark collapsed h5" data-toggle="collapse"
                                                        aria-expanded="false"
                                                        aria-controls="collapseFive">
                                                        <i class="mdi mdi-phone-log mdi-24px"></i>
                                                        Referencias Personales del Cliente
                                                    </a>
                                                </h6>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="collapseReferenciasPersonales" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFive">
                                        <div class="panel-body">
                                            <div class="card-body">
                                                <div class="form-group row">
                                                    <h4>Referencias personales</h4>
                                                    <div class="table-responsive">
                                                        <table class="table table-condensed" id="tblReferencias">
                                                            <thead class="thead-light">
                                                                <tr>
                                                                    <th>Nombre referencia</th>
                                                                    <th>Lugar de trabajo</th>
                                                                    <th>Tiempo de conocer ref</th>
                                                                    <th>Telefono ref</th>
                                                                    <th>Parentesco ref</th>
                                                                    <th></th>
                                                                </tr>
                                                            </thead>
                                                            <tbody class="table-condensed">
                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </div>
                                                <div class="form-group row" id="divAval">
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
                                                            <tbody>
                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="panel panel-default" id="divInformaciondeCampo" style="display: none;">
                                    <div class="panel-heading card-header p-1 container-fluid" role="tab" id="headingEight">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <h6 class="m-0 font-14">
                                                    <a href="#collapseInformaciondeCampo" class="text-dark collapsed h5" data-toggle="collapse"
                                                        aria-expanded="false"
                                                        aria-controls="collapseThree">
                                                        <i class="mdi mdi-account-check mdi-24px"></i>
                                                        Informacion de Campo
                                                    </a>
                                                </h6>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="collapseInformaciondeCampo" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingEight">
                                        <div class="panel-body">
                                            <div class="card-body">
                                                <div class="container-fluid">

                                                    <div class="row" id="divResolucionDomicilio" style="display: none;">
                                                        <div class="col-md-8">
                                                            <h5>Resolución del Domicilio <small id="lblResolucionCampoDomicilio"></small></h5>
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
                                                            <div class="container" id="divDocumentacionCampoDomicilio">
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="row" id="divResolucionTrabajo" style="display: none;">
                                                        <div class="col-md-8">
                                                            <h5>Resolución del Trabajo <small id="lblResolucionCampoTrabajo"></small></h5>
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

            <!--modal de detalles del registro -->
            <div id="modalEstadoSolicitud" class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="modalEstadoSolicitudLabel" aria-hidden="true">
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title mt-0" id="modalEstadoSolicitudLabel">Estado del procesamiento de la solicitud</h5>
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        </div>
                        <div class="modal-body">
                            <!-- tab -->
                            <ul class="nav nav-tabs nav-tabs-custom" role="tablist">
                                <li class="nav-item">
                                    <a class="nav-link active" data-toggle="tab" href="#home1" role="tab">
                                        <span class="d-block d-sm-none"><i class="fas fa-clock"></i></span>
                                        <span class="d-none d-sm-block">Procesos</span>
                                    </a>
                                </li>
                                <li class="nav-item">
                                    <a class="nav-link" data-toggle="tab" href="#profile1" role="tab">
                                        <span class="d-block d-sm-none"><i class="far fa-comment-alt"></i></span>
                                        <span class="d-none d-sm-block">Más detalles</span>
                                    </a>
                                </li>
                            </ul>
                            <!-- Tab panes -->
                            <div class="tab-content">
                                <div class="tab-pane active p-3" id="home1" role="tabpanel">
                                    <div class="form-group row justify-content-center" id="divSolicitudInactiva" style="display: none;">
                                        <h6 class="text-danger">Solicitud Inactiva</h6>
                                    </div>
                                    <div class="table-responsive">
                                        <table class="table mb-0" id="tblDetalleEstado">
                                            <thead class="thead-light">
                                                <tr>
                                                    <th>Proceso</th>
                                                    <th>Inicio</th>
                                                    <th>Fin</th>
                                                    <th>Tiempo (D:H:M:S)</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                                <div class="tab-pane p-3" id="profile1" role="tabpanel">
                                    <div class="form-group row" id="divDetalleResolucion">
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
                                    </div>
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
        </div>
    </div>

    <div id="modalComentarioReferencia" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="modalComentarioReferenciaLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title mt-0" id="modalComentarioReferenciaLabel">Observaciones de referencia personal</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                </div>
                <div class="modal-body">
                    ¿Está seguro de actualizar las observaciones de la referencia personal 
                    <strong>
                        <label id="lblNombreReferenciaModal"></label>
                    </strong>?
                    <br />
                    <div class="form-group">
                        <label class="col-form-label">Observaciones</label>
                        <textarea id="txtObservacionesReferencia" required="required" class="form-control" maxlength="225" rows="2" data-parsley-group="informacionLaboral"></textarea>
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

    <!-- modal validar documentacion -->
    <div id="modalFinalizarValidarDocumentacion" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="modalFinalizarValidarDocumentacionLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title mt-0" id="modalFinalizarValidarDocumentacionLabel">Toda la documentación</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                </div>
                <div class="modal-body">
                    Documentos adjuntados en la solicitud

                    <div class="container-fluid">
                        <div class="row">
                            <div class="col-md-12">
                                <label class="mt-0 header-title text-center">Documentación identidad</label>
                                <div class="popup-gallery" id="divDocumentacionCedulaModal">
                                </div>
                            </div>
                            <div class="col-md-12">
                                <label class="mt-0 header-title text-center">Documentación domicilio</label>
                                <div class="popup-gallery" id="divDocumentacionDomicilioModal">
                                </div>
                            </div>
                            <div class="col-md-12">
                                <label class="mt-0 header-title text-center">Documentación laboral</label>
                                <div class="popup-gallery" id="divDocumentacionLaboralModal">
                                </div>
                            </div>
                            <div class="col-md-12">
                                <label class="mt-0 header-title text-center">Solicitud fisica</label>
                                <div class="popup-gallery" id="divDocumentacionSoliFisicaModal">
                                </div>
                            </div>
                            <div class="col-sm-12" id="tituloCampoDomicilioModal">
                                <label class="mt-0 header-title text-center">Documentación de campo (Domicilio)</label>
                                <div class="popup-gallery" id="divDocumentacionCampoDomicilioModal">
                                </div>
                            </div>

                            <div class="col-sm-12" id="tituloCampoTrabajoModal">
                                <label class="mt-0 header-title text-center">Documentación de campo (Trabajo)</label>
                                <div class="popup-gallery" id="divDocumentacionCampoTrabajoModal">
                                </div>
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
    <script src="../../Scripts/js/jquery.min.js"></script>
    <script src="../../Scripts/js/bootstrap.bundle.min.js"></script>
    <script src="../../Scripts/js/metisMenu.min.js"></script>
    <script src="../../Scripts/js/jquery.slimscroll.js"></script>
    <script src="../../Scripts/js/waves.min.js"></script>
    <script src="../../Scripts/plugins/jquery-sparkline/jquery.sparkline.min.js"></script>
    <script src="../../Scripts/js/app.js"></script>
    <!-- ARCHIVOS NECESARIOS PARA LA PANTALLA -->
    <script src="../../Scripts/plugins/iziToast/js/iziToast.js"></script>
    <script src="../../Scripts/plugins/iziToast/js/iziToast.min.js"></script>
    <script src="../../Scripts/plugins/parsleyjs/parsley.js"></script>
    <script src="../../Scripts/plugins/magnific-popup/jquery.magnific-popup.min.js"></script>
    <script src="../../Scripts/plugins/imgBox/jquery.imgbox.js"></script>
    <script src="../../Scripts/plugins/select2/js/select2.full.min.js"></script>
    <script src="../../Scripts/app/solicitudes/SolicitudesCredito_Utilitarios.js"></script>
    <script src="../../Scripts/app/solicitudes/SolicitudesCredito_Detalles.js"></script>
</body>
</html>
