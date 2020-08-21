<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SolicitudesCredito_Analisis.aspx.cs" Inherits="SolicitudesCredito_Analisis" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <title>Analisis de solicitud de crédito</title>
    <!-- BOOTSTRAP -->
    <link href="/CSS/Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/CSS/Content/css/icons.css" rel="stylesheet" />
    <link href="/CSS/Content/css/style.css" rel="stylesheet" />
    <!-- ARCHIVOS NECESARIOS PARA EL FUNCIONAMIENTO DE LA PAGINA -->
    <link href="/Scripts/plugins/iziToast/css/iziToast.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/magnific-popup/magnific-popup.css" rel="stylesheet" />
    <link href="/Scripts/plugins/select2/css/select2.min.css" rel="stylesheet" />
    <link href="/CSS/Estilos_CSS.css" rel="stylesheet" />
    <style>
        .tr-exito {
            color: #00a803;
        }

        .tr-error {
            color: #f5c6cb;
        }

        .page-template {
            font-family: "DejaVu Sans", "Arial", sans-serif;
            position: absolute;
            width: 100%;
            height: 100%;
            top: 0;
            left: 0;
        }

            .page-template .header {
                font-family: "DejaVu Sans";
                position: absolute;
                top: 20px;
                left: 30px;
                right: 30px;
                border-bottom: 1px solid #888;
                color: #888;
                margin-bottom: 50px;
                text-align: center;
            }

            .page-template .footer {
                position: absolute;
                bottom: 30px;
                left: 30px;
                right: 30px;
                border-top: 1px solid #888;
                text-align: center;
                color: #888;
            }

            .page-template .watermark {
                font-weight: bold;
                font-size: 400%;
                text-align: center;
                margin-top: 30%;
                color: #aaaaaa;
                opacity: 0.1;
                transform: rotate(-35deg) scale(1.7, 1.5);
            }

        @font-face {
            font-family: "DejaVu Sans";
            src: url("/CSS/Content/fonts/DejaVu/DejaVuSans.ttf") format("truetype");
        }

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
<body class="EstiloBody">
    <div class="card">
        <div class="card-header">
            <div class="row">
                <!-- Información del producto -->
                <div class="col-md-6 form-inline p-0">
                    <div class="spinner-border" role="status" id="divCargandoAnalisis">
                        <span class="sr-only">Cargando</span>
                    </div>
                    <asp:Image runat="server" ID="LogoPrestamo" class="float-left LogoPrestamo" alt="Logo del Producto" Style="display: none;" />
                    <asp:Label runat="server" ID="lblTipoPrestamo" CssClass="h4 EliminarEspacios"></asp:Label>
                </div>
                <!-- Acciones generales -->
                <div class="col-md-6 form-inline justify-content-end pr-0">
                    <div class="button-items">
                        <button id="btnValidoDocumentacionModal" type="button" class="btn btn-sm btn-warning waves-effect waves-light float-right">
                            <small>Validar Docs</small>
                        </button>
                        <button id="btnRechazar" disabled="disabled" title="No se puede determinar la resolución de la solicitud" type="button" class="btn btn-sm btn-danger waves-effect waves-light validador">
                            Rechazar
                        </button>
                        <button id="btnAprobar" disabled="disabled" title="No se puede determinar la resolución de la solicitud" type="button" class="btn btn-sm btn-warning waves-effect waves-light validador">
                            Aprobar
                        </button>
                        <button id="btnGroupVerticalDrop1" type="button" class="btn btn-sm btn-secondary" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                            Otras opciones
                            <li class="mdi mdi-arrow-down"></li>
                        </button>
                        <div class="dropdown-menu EliminarEspacios" aria-labelledby="btnGroupVerticalDrop1">
                            <button id="btnResumen" type="button" data-toggle="modal" data-target="#modalResumen" class="btn btn-sm btn-success btn-block m-0" style="border-radius: 0;">
                                Resumen Sol.
                            </button>
                            <button id="btnImprimirReporte" type="button" class="btn btn-sm btn-success btn-block m-0" onclick="ExportHtmlToPdf('#ReporteSolicitud','Reporte_Solitud','Reporte de la Solicitud')" style="border-radius: 0;">
                                Generar PDF
                            </button>
                            <button id="btnHistorialInterno" title="Ver Historial interno del cliente" disabled="disabled" type="button" class="btn btn-sm btn-success btn-block waves-effect waves-light m-0" style="border-radius: 0;">
                                Historial I
                            </button>
                            <button id="btnHistorialExterno" disabled="disabled" type="button" class="btn btn-sm btn-success btn-block waves-effect waves-light m-0" style="border-radius: 0;">
                                Buro externo
                            </button>
                            <button id="btnCondicionarSolicitud" type="button" class="btn btn-sm btn-warning btn-block validador m-0" style="border-radius: 0;">
                                Condicionar
                            </button>
                        </div>
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

                <table class="table table-condensed m-0">
                    <thead class="thead-light">
                        <tr>
                            <th class="text-center">No. Solicitud:</th>
                            <th class="text-center">
                                <asp:Label ID="lblNoSolicitud" CssClass="col-form-label" runat="server"></asp:Label>
                            </th>
                            <th class="text-center">Tipo Solicitud:</th>
                            <th class="text-center">
                                <asp:Label ID="lblTipoSolicitud" CssClass="col-form-label" runat="server"></asp:Label>
                            </th>
                            <th class="text-center">Agente de Ventas:</th>
                            <th class="text-center">
                                <asp:Label ID="lblAgenteDeVentas" CssClass="col-form-label" runat="server"></asp:Label>
                            </th>
                            <th class="text-center">Agencia:</th>
                            <th class="text-center">
                                <asp:Label ID="lblAgencia" CssClass="col-form-label" runat="server"></asp:Label>
                            </th>
                            <th class="text-center">Gestor:</th>
                            <th class="text-center">
                                <asp:Label ID="lblNombreGestor" CssClass="col-form-label" runat="server"></asp:Label>
                            </th>
                        </tr>
                    </thead>
                </table>
                <!-- Información del procesamiento de la solicitud -->
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
                        <tbody></tbody>
                        <tfoot></tfoot>
                    </table>
                </div>
            </div>
        </div>
        <div class="card-body">
            <div class="row">
                <div class="col-12 p-0">
                    <div class="collapse-group">
                        <!-- INFORMACION PERSONAL -->
                        <div class="panel panel-default">
                            <div class="panel-heading p-1 seccion-header" role="tab" id="headingOne">
                                <div class="row">
                                    <div class="col-md-9">
                                        <h6 class="panel-title m-0 font-14">
                                            <a role="button" data-toggle="collapse" href="#collapseInformacionPersonal" class="text-dark h5 trigger collapsed"
                                                aria-expanded="true"
                                                aria-controls="collapseOne">
                                                <i class="mdi mdi-account-circle mdi-24px"></i>&nbsp;Informacion Personal
                                            </a>
                                        </h6>
                                    </div>
                                    <div class="col-md-3">
                                        <button id="btnValidoInfoPersonalModal" type="button" class="btn btn-sm btn-warning btn-block waves-effect waves-light validador">
                                            Validar información personal
                                        </button>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="collapseInformacionPersonal" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingOne">
                            <div class="panel-body">
                                <div class="card-body">
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
                    <!-- INFORMACION DEL DOMICILIO -->
                    <div class="panel panel-default">
                        <div class="panel-heading p-1 seccion-header" role="tab" id="headingTwo">
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
                                            <div class="container" id="divDocumentacionDomicilio">
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <!-- INFORMACION DEL CONYUGUE -->
                    <div class="panel panel-default" id="divConyugueCliente">
                        <div class="panel-heading p-1 seccion-header" role="tab" id="headingThree">
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
                    <!-- INFORMACION LABORAL -->
                    <div class="panel panel-default">
                        <div class="panel-heading p-1 seccion-header" role="tab" id="headingFour">
                            <div class="row">
                                <div class="col-md-9">
                                    <h6 class="m-0 font-14">
                                        <a href="#collapseInformacionLaboral" class="text-dark collapsed h5" data-toggle="collapse"
                                            aria-expanded="false"
                                            aria-controls="collapseThree">
                                            <i class="mdi mdi-briefcase-check mdi-24px"></i>
                                            Informacion Laboral
                                        </a>
                                    </h6>
                                </div>
                                <div class="col-md-3">
                                    <button id="btnValidoInfoLaboralModal" type="button" class="btn btn-sm btn-warning btn-block waves-effect waves-light validador">
                                        Validar información laboral
                                    </button>
                                </div>
                            </div>
                        </div>
                        <div id="collapseInformacionLaboral" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour">
                            <div class="panel-body">
                                <div class="card-body">
                                    <div class="row">
                                        <div class="col-md-8">
                                            <div class="form-group row">
                                                <label class="col-sm-6">Nombre del trabajo</label>
                                                <asp:Label ID="lblNombreTrabajoCliente" CssClass="col-sm-6" runat="server"></asp:Label>

                                                <label class="col-sm-6">Ingresos mensuales</label>
                                                <div class="col-sm-6">
                                                    <asp:Label ID="lblIngresosMensualesCliente" runat="server"></asp:Label>
                                                    <asp:HyperLink ID="actualizarIngresosCliente" CssClass="mdi mdi-pencil mdi-18px text-info" title="Editar" NavigateUrl="#" runat="server" role="button"></asp:HyperLink>
                                                    <li id="pencilOff" class="mdi mdi-pencil-off mdi-18px" style="display: none"></li>
                                                </div>

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

                    <!-- INFORMACION DEL PRESTAMO-->
                    <div class="panel panel-default">
                        <div class="panel-heading p-1 seccion-header" role="tab" id="headingSix">
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
                                        <div class="col-md-12">
                                            <div class="form-group row">
                                                <button id="btnDigitarValoresManualmente" title="Digitar monto manualmente" type="button" class="btn btn-success col-sm-3 btn-block waves-effect waves-light validador">
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
                                                    <table class="table table-condensed table-striped" id="tblPMOSugeridosReales">
                                                        <thead>
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
                                                    <button id="btnDigitarMontoManualmente" disabled="disabled" title="Digitar monto manualmente" type="button" class="btn btn-success btn-block waves-effect waves-light">
                                                        Digitar monto
                                                    </button>
                                                </div>
                                                <div class="col-sm-6">
                                                    <button id="btnRechazarIncapacidadPagoModal" data-toggle="modal" data-target="#modalRechazarPorIncapcidadPago" disabled="disabled" title="Rechazar solicitud por incapacidad de pago" type="button" class="btn btn-danger btn-block waves-effect waves-light">
                                                        Rechazar solicitud
                                                    </button>
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
                        <div class="panel-heading p-1 seccion-header" role="tab" id="headingSeven">
                            <div class="row">
                                <div class="col-md-9">
                                    <h6 class="m-0 font-14">
                                        <a href="#collapseInformacionAnalisis" class="text-dark collapsed h5" data-toggle="collapse"
                                            aria-expanded="false"
                                            aria-controls="collapseSeven">
                                            <i class="mdi mdi-account-search mdi-24px"></i>
                                            Información de Perfil
                                        </a>
                                    </h6>
                                </div>
                                <div class="col-md-3">
                                    <button id="btnEnviarCampo" disabled="disabled" type="button" class="btn btn-sm btn-warning btn-block waves-effect waves-light validador">
                                        Enviar a campo
                                    </button>
                                </div>
                            </div>
                        </div>
                        <div id="collapseInformacionAnalisis" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingSeven">
                            <div class="panel-body">
                                <div class="card-body">
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
                                                        <option value="Informal">Informal</option>
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
                    <!-- REFERENCIAS PERSONALES DEL CLIENTE -->
                    <div class="panel panel-default">
                        <div class="panel-heading p-1 seccion-header" role="tab" id="headingFive">
                            <div class="row">
                                <div class="col-md-9">
                                    <h6 class="m-0 font-14">
                                        <a href="#collapseReferenciasPersonales" class="text-dark collapsed h5" data-toggle="collapse"
                                            aria-expanded="false"
                                            aria-controls="collapseFive">
                                            <i class="mdi mdi-phone-log mdi-24px"></i>
                                            Referencias Personales del Cliente
                                        </a>
                                    </h6>
                                </div>
                                <div class="col-md-3">
                                    <button id="btnValidoReferenciasModal" type="button" class="btn btn-sm btn-warning btn-block waves-effect waves-light validador">
                                        Validar referencias personales
                                    </button>
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
                        <div class="panel-heading p-1 seccion-header" role="tab" id="headingEight">
                            <h6 class="m-0 font-14">
                                <a href="#collapseInformaciondeCampo" class="text-dark collapsed h5" data-toggle="collapse"
                                    aria-expanded="false"
                                    aria-controls="collapseThree">
                                    <i class="mdi mdi-account-check mdi-24px"></i>
                                    Informacion de Campo
                                </a>
                            </h6>
                        </div>
                        <div id="collapseInformaciondeCampo" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingEight">
                            <div class="panel-body">
                                <div class="card-body">
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
        <!-- ROW DEL TAB-->
    </div>

    <!-- modal validar informacion personal -->
    <div id="modalFinalizarValidarPersonal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="modalFinalizarValidarPersonalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title mt-0" id="modalFinalizarValidarPersonalLabel">Terminar validación</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                </div>
                <div class="modal-body">
                    ¿Está seguro de que desea terminar la validación de la información personal del cliente?<br />
                    <br />
                    <div class="form-group">
                        <label class="col-form-label">Observaciones</label>
                        <div>
                            <input id="comentariosInfoPersonal" class="form-control" data-parsley-maxlength="150" type="text" value="" />
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button id="btnValidoInfoPersonalConfirmar" class="btn btn-primary waves-effect waves-light mr-1 validador">
                        Confirmar
                    </button>
                    <button type="reset" data-dismiss="modal" class="btn btn-secondary waves-effect">
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
                    <h5 class="modal-title mt-0" id="modalFinalizarValidarLaboralLabel">Terminar validación</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                </div>
                <div class="modal-body">
                    ¿Está seguro de que desea terminar la validación de la información laboral del cliente?<br />
                    <br />
                    <div class="form-group">
                        <label class="col-form-label">Observaciones</label>
                        <div>
                            <input id="comentariosInfoLaboral" class="form-control" type="text" data-parsley-maxlength="150" value="" />
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button id="btnValidoInfoLaboralConfirmar" class="btn btn-primary waves-effect waves-light mr-1 validador">
                        Confirmar
                    </button>
                    <button type="reset" data-dismiss="modal" class="btn btn-secondary waves-effect">
                        Cancelar
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
                    <h5 class="modal-title mt-0" id="modalFinalizarValidarDocumentacionLabel">Terminar validación</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                </div>
                <div class="modal-body">
                    ¿Está seguro de realizar la validación de la documentación?<br />
                    <br />
                    <div class="form-group row">
                        <div class="col-sm-12">
                            <button id="btnValidarIdentidades" data-id="1" style="background-color: transparent; border: none;"><i class="mdi mdi-check mdi-24px"></i></button>
                            <label class="header-title">Documentación identidad</label>
                            <div class="popup-gallery" id="divDocumentacionCedulaModal">
                            </div>
                        </div>
                        <div class="col-sm-12">
                            <button id="btnValidarDomiciliar" data-id="2" style="background-color: transparent; border: none;"><i class="mdi mdi-check mdi-24px"></i></button>
                            <label class="mt-0 header-title text-center">Documentación domiciliar</label>
                            <div class="popup-gallery" id="divDocumentacionDomicilioModal">
                            </div>
                        </div>
                        <div class="col-sm-12">
                            <button id="btnValidarLaboral" data-id="3" style="background-color: transparent; border: none;"><i class="mdi mdi-check mdi-24px"></i></button>
                            <label class="mt-0 header-title text-center">Documentación laboral</label>
                            <div class="popup-gallery" id="divDocumentacionLaboralModal">
                            </div>
                        </div>
                        <div class="col-sm-12">
                            <button id="btnValidarSoliFisica" data-id="4" style="background-color: transparent; border: none;"><i class="mdi mdi-check mdi-24px"></i></button>
                            <label class="mt-0 header-title text-center">Solicitud fisica</label>
                            <div class="popup-gallery" id="divDocumentacionSoliFisicaModal">
                            </div>
                        </div>

                        <div class="col-sm-12" id="tituloCampoDomicilioModal" style="display: none;">
                            <label class="mt-0 header-title text-center">Documentación de campo (Domicilio)</label>
                            <div class="popup-gallery" id="divDocumentacionCampoDomicilioModal">
                            </div>
                        </div>

                        <div class="col-sm-12" id="tituloCampoTrabajoModal" style="display: none;">
                            <label class="mt-0 header-title text-center">Documentación de campo (Trabajo)</label>
                            <div class="popup-gallery" id="divDocumentacionCampoTrabajoModal">
                            </div>
                        </div>
                    </div>
                    <br />
                    <div class="form-group">
                        <label class="col-form-label">Observaciones</label>
                        <div>
                            <input id="comentariosDocumentacion" class="form-control" type="text" value="" data-parsley-maxlength="150" />
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button id="btnValidoDocumentacionConfirmar" class="btn btn-primary waves-effect waves-light mr-1 validador">
                        Confirmar
                    </button>
                    <button type="reset" data-dismiss="modal" class="btn btn-secondary waves-effect">
                        Cancelar
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
                    <h5 class="modal-title mt-0" id="modalFinalizarValidarReferenciasLabel">Terminar validación</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                </div>
                <div class="modal-body">
                    ¿Está seguro de que desea terminar la validación de las referencias personales del cliente?<br />
                    <br />
                    <div class="form-group">
                        <label>Observaciones</label>
                        <div>
                            <input id="comentarioReferenciasPersonales" class="form-control" type="text" value="" data-parsley-maxlength="150" />
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button id="btnValidoReferenciasConfirmar" class="btn btn-primary waves-effect waves-light mr-1 validador">
                        Confirmar
                    </button>
                    <button type="reset" data-dismiss="modal" class="btn btn-secondary waves-effect">
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
                    <h5 class="modal-title mt-0" id="modalCondicionarSolicitudLabel">Condicionar solicitud</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                </div>
                <div class="modal-body">
                    ¿Está seguro de que desea condicionar la solicitud?<br />
                    <br />
                    <form id="frmAddCondicion">
                        <div class="form-group row">
                            <div class="col-sm-12">
                                <select id="ddlCondiciones" required="required" class="form-control form-control-sm">
                                </select>
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
                            <button id="btnAgregarCondicion" class="btn btn-block btn-primary validador">Agregar</button>
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
                                    <tr>
                                        <td>Documentacion</td>
                                        <td>Adjuntar segunda identidad</td>
                                        <td>La que se adjuntó estaba borrosa</td>
                                        <td>
                                            <button class="btn btn-sm btn-danger">Quitar</button>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                    <div class="form-group">
                        <label>Otros</label>
                        <div>
                            <input id="razonCondicion" required="required" class="form-control" type="text" value="" data-parsley-maxlength="128" />
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button id="btnCondicionarSolicitudConfirmar" class="btn btn-primary waves-effect waves-light mr-1 validador">
                        Confirmar
                    </button>
                    <button type="reset" data-dismiss="modal" class="btn btn-secondary waves-effect">
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
                    <h5 class="modal-title mt-0" id="modalEnviarCampoLabel">Enviar a campo</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                </div>
                <div class="modal-body">
                    ¿Está seguro de que desea enviar a campo la solicitud?<br />
                    <br />
                    <div class="form-group">
                        <label>Comentario adicional</label>
                        <input id="comentarioAdicional" class="form-control" type="text" value="" data-parsley-maxlength="150" />
                    </div>
                </div>
                <div class="modal-footer">
                    <button id="btnEnviarCampoConfirmar" class="btn btn-primary waves-effect waves-light mr-1 validador">
                        Confirmar
                    </button>
                    <button type="reset" data-dismiss="modal" class="btn btn-secondary waves-effect">
                        Cancelar
                    </button>
                </div>
            </div>
        </div>
    </div>

    <!-- modal estado del resumen de la solicitud -->
    <div id="modalResumen" class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="modalResumenLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header text-center">
                    <h5 class="modal-title w-100 mt-0" id="modalResumenLabel" style="text-align: center">Resumen de la solicitud - Información relevante</h5>
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
                    <button type="button" data-dismiss="modal" class="btn btn-secondary waves-effect">
                        Cerrar
                    </button>
                    <button type="button" class="btn btn-secondary waves-effect" onclick="ExportHtmlToPdf('#ResumenSolicitud','Resumen de la solicitud','Resumen de la solicitud - Información Relevante')">
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
                    <h5 class="modal-title mt-0" id="modalEstadoSolicitudLabel">Estado del procesamiento de la solicitud</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                </div>
                <div class="modal-body">
                    <!-- tab -->
                    <ul class="nav nav-tabs nav-tabs-custom" role="tablist">
                        <li class="nav-item">
                            <a class="nav-link active" data-toggle="tab" href="#PestanaTimers" role="tab">
                                <span class="d-none d-sm-block">Procesos</span>
                            </a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" data-toggle="tab" href="#PestanaComentarios" role="tab">
                                <span class="d-none d-sm-block">Más detalles</span>
                            </a>
                        </li>
                    </ul>
                    <!-- Tab panes -->
                    <div class="tab-content">
                        <div class="tab-pane active p-3" id="PestanaTimers" role="tabpanel">
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
                        <div class="tab-pane p-3" id="PestanaComentarios" role="tabpanel">
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
                                    <label id="lblReprogramado" class="col-form-label"></label>
                                </div>
                            </div>
                            <div class="form-group row" id="divCondicionado" style="display: none;">
                                <label class="col-sm-5 col-form-label"><strong>Razón condicionado:</strong></label>
                                <div class="col-sm-7">
                                    <label id="lblCondicionado" class="col-form-label"></label>
                                </div>
                            </div>
                            <div class="form-group row" id="divDocumentacionComentario" style="display: none;">
                                <label class="col-sm-5 col-form-label"><strong>Observaciones documentación:</strong></label>
                                <div class="col-sm-7">
                                    <label id="lblDocumentacionComentario" class="col-form-label"></label>
                                </div>
                            </div>
                            <div class="form-group row" id="divInfoPersonal" style="display: none;">
                                <label class="col-sm-5 col-form-label"><strong>Observaciones info. personal:</strong></label>
                                <div class="col-sm-7">
                                    <label id="lblInfoPersonalComentario" class="col-form-label"></label>
                                </div>
                            </div>
                            <div class="form-group row" id="divInfoLaboral" style="display: none;">
                                <label class="col-sm-5 col-form-label"><strong>Observaciones info. laboral:</strong></label>
                                <div class="col-sm-7">
                                    <label id="lblInfoLaboralComentario" class="col-form-label"></label>
                                </div>
                            </div>
                            <div class="form-group row" id="divReferencias" style="display: none;">
                                <label class="col-sm-5 col-form-label"><strong>Observaciones referencias:</strong></label>
                                <div class="col-sm-7">
                                    <label id="lblReferenciasComentario" class="col-form-label"></label>
                                </div>
                            </div>
                            <div class="form-group row" id="divCampo" style="display: none;">
                                <label class="col-sm-5 col-form-label"><strong>Observaciones para campo:</strong></label>
                                <div class="col-sm-7">
                                    <label id="lblCampoComentario" class="col-form-label"></label>
                                </div>
                            </div>
                            <div class="form-group row" id="divGestoria" style="display: none;">
                                <label class="col-sm-5 col-form-label"><strong>Observaciones de gestoria:</strong></label>
                                <div class="col-sm-7">
                                    <label id="lblGestoriaComentario" class="col-form-label"></label>
                                </div>
                            </div>
                            <div class="form-group row" id="divComentarioResolucion" style="display: none;">
                                <label class="col-sm-5 col-form-label"><strong>Comentario resolución:</strong></label>
                                <div class="col-sm-7">
                                    <label id="lblResolucionComentario" class="col-form-label"></label>
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

    <!-- modal rechazar una solicitud -->
    <div id="modalRechazar" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="modalRechazarLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title mt-0" id="modalRechazarLabel">Resolución final</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                </div>
                <div class="modal-body">
                    ¿Está seguro de que desea <strong>RECHAZAR</strong> esta solicitud?<br />
                    <br />
                    <div class="form-group">
                        <label class="col-form-label">Observaciones</label>
                        <div>
                            <input id="comentarioRechazar" required="required" class="form-control" type="text" value="" data-parsley-maxlength="150" />
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button id="btnConfirmarRechazar" class="btn btn-danger waves-effect waves-light mr-1">
                        Confirmar
                    </button>
                    <button type="reset" data-dismiss="modal" class="btn btn-secondary waves-effect">
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
                    <h5 class="modal-title mt-0" id="modalAprobarLabel">Resolución final</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                </div>
                <div class="modal-body">
                    ¿Está seguro de que desea <strong>APROBAR</strong> esta solicitud?<br />
                    <br />
                    <div class="form-group">
                        <label class="col-form-label">Observaciones</label>
                        <div>
                            <input id="comentarioAprobar" required="required" class="form-control" type="text" value="" data-parsley-maxlength="150" />
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button id="btnConfirmarAprobar" class="btn btn-success waves-effect waves-light mr-1">
                        Confirmar
                    </button>
                    <button type="reset" data-dismiss="modal" class="btn btn-secondary waves-effect">
                        Cancelar
                    </button>
                </div>
            </div>
        </div>
    </div>

    <!-- modal agregar comentario sobre una referencia personal -->
    <div id="modalComentarioReferencia" class="modal fade bs-example-modal-lg" role="dialog" aria-labelledby="modalComentarioReferenciaLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title mt-0" id="modalComentarioReferenciaLabel">Observaciones de referencia personal</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                </div>
                <form action="#" id="frmObservacionReferencia">
                    <div class="modal-body">
                        ¿Está seguro de actualizar las observaciones de la referencia personal
                        <strong>
                            <label id="lblNombreReferenciaModal"></label>
                        </strong>?<br />
                        <div class="form-group">
                            <label class="col-form-label">Observaciones</label>
                            <textarea id="txtObservacionesReferencia" required="required" class="form-control" data-parsley-maxlength="255" rows="2" data-parsley-group="informacionLaboral"></textarea>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" id="btnEliminarReferencia" data-toggle="modal" data-target="#modalEliminarReferencia" class="btn btn-danger float-left waves-effect waves-light mr-1 validador">
                            Eliminar
                        </button>
                        <button type="button" id="btnReferenciaSinComunicacion" class="btn btn-primary waves-effect waves-light mr-1 validador">
                            Sin comunicación
                        </button>
                        <button type="button" id="btnComentarioReferenciaConfirmar" class="btn btn-primary waves-effect waves-light mr-1 validador">
                            Confirmar
                        </button>
                        <button type="reset" data-dismiss="modal" class="btn btn-secondary waves-effect">
                            Cancelar
                        </button>
                    </div>
                </form>
            </div>
        </div>
    </div>

    <!-- modal aprobar una solicitud -->
    <div id="modalEliminarReferencia" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="modalEliminarReferenciaLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title mt-0" id="modalEliminarReferenciaLabel">Eliminar Referencia Personal</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                </div>
                <div class="modal-body">
                    ¿Está seguro de eliminar esta referencia personal?
                </div>
                <div class="modal-footer">
                    <button id="btnEliminarReferenciaConfirmar" class="btn btn-danger waves-effect waves-light mr-1">
                        Confirmar
                    </button>
                    <button type="reset" data-dismiss="modal" class="btn btn-secondary waves-effect">
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
                        <h5 class="modal-title mt-0" id="modalActualizarIngresosLabel">Editar ingresos</h5>
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
                        <button type="submit" class="btn btn-primary waves-effect waves-light mr-1 validador">
                            Confirmar
                        </button>
                        <button type="reset" data-dismiss="modal" class="btn btn-secondary waves-effect">
                            Cancelar
                        </button>
                    </div>
                </form>
            </div>
        </div>
    </div>

    <!-- modal establecer monto a financiar-->
    <div id="modalMontoFinanciar" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="modalMontoFinanciarLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title mt-0" id="modalMontoFinanciarLabel">Seleccionar Plazo y Monto Financiar</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                </div>
                <div class="modal-body">
                    ¿Está seguro de seleccionar este plazo y monto a financiar?<br />
                    <br />
                </div>
                <div class="modal-footer">
                    <button id="btnConfirmarPrestamoAprobado" class="btn btn-primary waves-effect waves-light mr-1 validador">
                        Confirmar
                    </button>
                    <button type="reset" data-dismiss="modal" class="btn btn-secondary waves-effect">
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
                    <h5 class="modal-title mt-0" id="modalValidarTipoDocsLabel">Terminar validación</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                </div>
                <div class="modal-body">
                    ¿Está seguro de llevar a cabo esta validación?
                </div>
                <div class="modal-footer">
                    <button id="btnValidarTipoDocConfirmar" class="btn btn-primary waves-effect waves-light mr-1 validador">
                        Confirmar
                    </button>
                    <button type="reset" data-dismiss="modal" class="btn btn-secondary waves-effect">
                        Cancelar
                    </button>
                </div>
            </div>
        </div>
    </div>

    <!-- modal detalles del aval -->
    <div id="modalDetallesAval" class="modal fade bs-example-modal-lg" role="dialog" aria-labelledby="modalDetallesAvalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title mt-0" id="modalDetallesAvalLabel">Detalles del aval</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                </div>
                <div class="modal-body">
                    <div class="form-group text-center">
                        <label class="h6">Identidad:</label>
                        <label class="h6" id="lblIdentidadAval"></label>

                        <label class="h6 pl-3">Registrado:</label>
                        <label class="h6" id="lblFechaCreacionAval"></label>

                        <label class="h6 pl-3">Estado:</label>
                        <label class="h6" id="lblEstadoAval"></label>
                    </div>
                    <div class="form-group row">
                        <div class="form-group col-sm-3">
                            <label class="col-form-label">RNT</label>
                            <input id="txtRTNAval" readonly="readonly" class="form-control" type="text" value="" />
                        </div>
                        <div class="form-group col-sm-9">
                            <label class="col-form-label">Nombre completo</label>
                            <input id="txtNombreCompletoAval" readonly="readonly" class="form-control" type="text" value="" />
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="form-group col-sm-3">
                            <label class="col-form-label">Teléfono</label>
                            <input id="txtTelefonoAval" readonly="readonly" class="form-control" type="text" value="" />
                        </div>
                        <div class="form-group col-sm-3">
                            <label class="col-form-label">FechaNacimiento</label>
                            <input id="txtFechaNacimientoAval" readonly="readonly" class="form-control" type="text" value="" />
                        </div>
                        <div class="form-group col-sm-3">
                            <label class="col-form-label">Sexo</label>
                            <input id="txtSexoAval" readonly="readonly" class="form-control" type="text" value="" />
                        </div>
                        <div class="form-group col-sm-3">
                            <label class="col-form-label">Correo</label>
                            <input id="txtCorreoAval" readonly="readonly" class="form-control" type="text" value="" />
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="form-group col-sm-3">
                            <label class="col-form-label">Trabajo</label>
                            <input id="txtLugarTrabajoAval" readonly="readonly" class="form-control" type="text" value="" />
                        </div>
                        <div class="form-group col-sm-3">
                            <label class="col-form-label">Fecha ingreso</label>
                            <input id="txtFechaIngresoAval" readonly="readonly" class="form-control" type="text" value="" />
                        </div>
                        <div class="form-group col-sm-3">
                            <label class="col-form-label">Puesto</label>
                            <input id="txtPuestoAsignadoAval" readonly="readonly" class="form-control" type="text" value="" />
                        </div>
                        <div class="form-group col-sm-3">
                            <label class="col-form-label">Ingresos Mensuales</label>
                            <input id="txtIngresosMensualesAval" readonly="readonly" class="form-control" type="text" value="" />
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="form-group col-sm-3">
                            <label class="col-form-label">Tel. empresa</label>
                            <input id="txtTelEmpresaAval" readonly="readonly" class="form-control" type="text" value="" />
                        </div>
                        <div class="form-group col-sm-3">
                            <label class="col-form-label">Extensión RRHH</label>
                            <input id="txtExtensionRRHHAval" readonly="readonly" class="form-control" type="text" value="" />
                        </div>
                        <div class="form-group col-sm-3">
                            <label class="col-form-label">Extensión Aval</label>
                            <input id="txtExtensionAval" readonly="readonly" class="form-control" type="text" value="" />
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button id="btnMasDetallesAval" data-id="0" class="btn btn-primary waves-effect waves-light mr-1">
                        Más detalles
                    </button>
                    <button type="reset" data-dismiss="modal" class="btn btn-secondary waves-effect">
                        Cerrar
                    </button>
                </div>
            </div>
        </div>
    </div>

    <!-- modal rechazar por incapacidad de pago -->
    <div id="modalRechazarPorIncapcidadPago" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="modalRechazarPorIncapcidadPagoLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title mt-0" id="modalRechazarPorIncapcidadPagoLabel">Rechazar solicitud</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                </div>
                <div class="modal-body">
                    ¿Está seguro de rechazar la solicitud por incapacidad de pago?
                </div>
                <div class="modal-footer">
                    <button id="btnRechazarIncapPagoConfirmar" class="btn btn-primary waves-effect waves-light mr-1 validador">
                        Confirmar
                    </button>
                    <button type="reset" data-dismiss="modal" class="btn btn-secondary waves-effect">
                        Cancelar
                    </button>
                </div>
            </div>
        </div>
    </div>

    <!-- modal digitar monto a financiar manualmente -->
    <div id="ModalDigitarMontosManualmente" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="ModalDigitarMontosManualmenteLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title mt-0" id="ModalDigitarMontosManualmenteLabel">Sugerir Monto</h5>
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
                    <div class="form-group row" id="divCalculandoCuotaManual" style="display: none;">
                        <div class="col-sm-12 text-center">
                            <div class="spinner-border" role="status">
                                <span class="sr-only">Cargando</span>
                            </div>
                            <br />
                            Calculando Cuota...
                        </div>
                    </div>
                    <div class="form-group row" id="divMostrarCalculoCuotaManual" style="display: none;">
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
                    <button id="btnActualizarMontoManualmente" class="btn btn-primary waves-effect waves-light mr-1 validador btn-default" disabled="disabled">
                        Confirmar
                    </button>
                    <button type="reset" data-dismiss="modal" class="btn btn-secondary waves-effect">
                        Cancelar
                    </button>
                </div>
            </div>
        </div>
    </div>

    <script src="/Scripts/js/jquery.min.js"></script>
    <script src="/Scripts/js/bootstrap.bundle.min.js"></script>
    <script src="/Scripts/plugins/mascarasDeEntrada/js/jquery.inputmask.bundle.js"></script>
    <script>
        $(document).ready(function () {
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
            $(".MascaraEnteros").inputmask("integer", {
                alias: 'numeric',
                groupSeparator: ',',
                digits: 0,
                min: 0,
                max: 999,
                digitsOptional: false,
                placeholder: '0',
                autoGroup: true
            });
        });
    </script>
    <!-- ARCHIVOS NECESARIOS PARA LA PANTALLA -->
    <script src="/Scripts/plugins/iziToast/js/iziToast.min.js"></script>
    <script src="/Scripts/plugins/parsleyjs/parsley.js"></script>
    <script src="/Scripts/plugins/magnific-popup/jquery.magnific-popup.min.js"></script>
    <script src="/Scripts/plugins/imgBox/jquery.imgbox.js"></script>
    <script src="/Scripts/plugins/select2/js/select2.full.min.js"></script>
    <script src="/Scripts/plugins/kendo/jszip.min.js"></script>
    <script src="/Scripts/plugins/kendo/kendo.all.min.js"></script>
    <script src="/Scripts/plugins/kendo/PrintHtmlToPDF.js"></script>
    <script src="../../Scripts/app/solicitudes/SolicitudesCredito_Utilitarios.js"></script>
    <script src="../../Scripts/app/solicitudes/SolicitudesCredito_Analisis.js?v=202008211137"></script>
    <script type="x/kendo-template" id="page-template">
        <div class="page-template">
            <div class="header">
                <%--<img src="http://crediflash.prestadito.corp/documentos/base/logo_prestadito.png" style="width:150px;" />--%>
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
