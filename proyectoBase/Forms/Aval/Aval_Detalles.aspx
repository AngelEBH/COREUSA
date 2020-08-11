<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Aval_Detalles.aspx.cs" Inherits="proyectoBase.Forms.Aval.Aval_Detalles" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <title>Detalles del Aval</title>
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
                    <div class="row">
                        <div class="col-12">
                            <div class="table-responsive">
                                <table class="table table-condensed">
                                    <thead class="thead-light">
                                        <tr>
                                            <th>
                                                <i class="mdi mdi-account mdi-24px"></i>
                                                <label class="h4 noSpace">Aval del cliente:&nbsp;</label>
                                                <asp:Label CssClass="h4 noSpace" ID="lblNombreCliente" runat="server"></asp:Label>
                                            </th>
                                            <th>
                                                <label class="h4 noSpace">Solicitud:&nbsp;</label>
                                                <asp:Label CssClass="h4 noSpace" ID="lblIDSolicitud" runat="server"></asp:Label>
                                            </th>
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
                        <!--COMIENZAN TABS-->
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
                                    <div id="collapseInformacionPersonal" class="panel-collapse" role="tabpanel" aria-labelledby="headingOne">
                                        <div class="panel-body">
                                            <div class="card-body">
                                                <div class="container-fluid">
                                                    <div class="row">
                                                        <div class="col-md-8">
                                                            <div class="form-group row">

                                                                <label class="col-sm-6">Nombre completo</label>
                                                                <asp:Label ID="lblNombreAval" CssClass="col-sm-6" runat="server"></asp:Label>

                                                                <label class="col-sm-6">Identidad</label>
                                                                <asp:Label ID="lblIdentidadAval" CssClass="col-sm-6" runat="server"></asp:Label>

                                                                <label class="col-sm-6">RTN</label>
                                                                <asp:Label ID="lblRtnAval" CssClass="col-sm-6" runat="server"></asp:Label>

                                                                <label class="col-sm-6">Numero de teléfono</label>
                                                                <asp:HyperLink ID="lblNumeroTelefono" NavigateUrl="tel:+55599999999" CssClass="col-sm-6" runat="server"></asp:HyperLink>

                                                                <label class="col-sm-6">Nacionalidad</label>
                                                                <asp:Label ID="lblNacionalidad" CssClass="col-sm-6" runat="server"></asp:Label>

                                                                <label class="col-sm-6">Fecha de nacimiento</label>
                                                                <asp:Label ID="lblFechaNacimientoAval" CssClass="col-sm-6" runat="server"></asp:Label>

                                                                <label class="col-sm-6">Edad</label>
                                                                <asp:Label ID="lblEdadAval" CssClass="col-sm-6" runat="server"></asp:Label>

                                                                <label class="col-sm-6">Correo electrónico</label>
                                                                <asp:HyperLink ID="lblCorreoAval" CssClass="col-sm-6" NavigateUrl="mailto:correo@gmail.com" runat="server"></asp:HyperLink>

                                                                <label class="col-sm-6">Profesion u oficio</label>
                                                                <asp:Label ID="lblProfesionAval" CssClass="col-sm-6" runat="server"></asp:Label>

                                                                <label class="col-sm-6">Sexo</label>
                                                                <asp:Label ID="lblSexoAval" CssClass="col-sm-6" runat="server"></asp:Label>

                                                                <label class="col-sm-6">Estado civil</label>
                                                                <asp:Label ID="lblEstadoCivilAval" CssClass="col-sm-6" runat="server"></asp:Label>

                                                                <label class="col-sm-6">Vivienda</label>
                                                                <asp:Label ID="lblViviendaAval" CssClass="col-sm-6" runat="server"></asp:Label>

                                                                <label class="col-sm-6">Tiempo de residir</label>
                                                                <asp:Label ID="lblTiempoResidirAval" CssClass="col-sm-6" runat="server"></asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-3">
                                                            <label class="header-title TituloDivDocumentacion">Documentación</label>
                                                            <div class="container" id="divDocumentacionCedulaAval">
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
                                                    <a href="#collapseInformacionDomicilio" class="text-dark collapsed h5 collapsed" data-toggle="collapse"
                                                        aria-expanded="false"
                                                        aria-controls="collapseTwo">
                                                        <i class="mdi mdi-home-variant mdi-24px"></i>
                                                        Informacion Domicilio
                                                    </a>
                                                </h6>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="collapseInformacionDomicilio" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingTwo">
                                        <div class="panel-body">
                                            <div class="card-body">
                                                <div class="container-fluid">
                                                    <div class="row">
                                                        <div class="col-md-8">
                                                            <div class="form-group row">

                                                                <label class="col-sm-6">Departamento</label>
                                                                <asp:Label ID="lblDeptoAval" CssClass="col-sm-6" runat="server"></asp:Label>

                                                                <label class="col-sm-6">Municipio</label>
                                                                <asp:Label ID="lblMunicipioAval" CssClass="col-sm-6" runat="server"></asp:Label>

                                                                <label class="col-sm-6">Ciudad</label>
                                                                <asp:Label ID="lblCiudadAval" CssClass="col-sm-6" runat="server"></asp:Label>

                                                                <label class="col-sm-6">Barrio/colonia</label>
                                                                <asp:Label ID="lblBarrioColoniaAval" CssClass="col-sm-6" runat="server"></asp:Label>

                                                                <label class="col-sm-6">Dirección detallada</label>
                                                                <asp:Label ID="lblDireccionDetalladaAval" CssClass="col-sm-6" runat="server"></asp:Label>

                                                                <label class="col-sm-6">Referencias del domicilio</label>
                                                                <asp:Label ID="lblReferenciaDomicilioAval" CssClass="col-sm-6" runat="server"></asp:Label>
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
                                <div class="panel panel-default" id="divConyugueAval">
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
                                                                <asp:HyperLink ID="lblTelefonoConyugue" NavigateUrl="tel:+55599999999" CssClass="col-sm-8" runat="server"></asp:HyperLink>

                                                                <label class="col-sm-4">Lugar de trabajo conyugue</label>
                                                                <asp:Label ID="lblLugarTrabajoConyugue" CssClass="col-sm-8" runat="server"></asp:Label>

                                                                <label class="col-sm-4">Telefono trabajo conyugue</label>
                                                                <asp:HyperLink ID="lblTelefonoTrabajoConyugue" NavigateUrl="tel:+55599999999" CssClass="col-sm-8" runat="server"></asp:HyperLink>

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
                                                                <asp:Label ID="lblNombreTrabajoAval" CssClass="col-sm-6" runat="server"></asp:Label>

                                                                <label class="col-sm-6">Ingresos mensuales</label>
                                                                <asp:Label ID="lblIngresosMensualesAval" CssClass="col-sm-6" runat="server"></asp:Label>

                                                                <label class="col-sm-6">Puesto asigando</label>
                                                                <asp:Label ID="lblPuestoAsignadoAval" CssClass="col-sm-6" runat="server"></asp:Label>

                                                                <label class="col-sm-6">Fecha de ingreso</label>
                                                                <asp:Label ID="lblFechaIngresoAval" CssClass="col-sm-6" runat="server"></asp:Label>

                                                                <label class="col-sm-6">Arraigo laboral</label>
                                                                <asp:Label ID="lblArraigoLaboral" CssClass="col-sm-6" runat="server"></asp:Label>

                                                                <label class="col-sm-6">Teléfono empresa</label>
                                                                <asp:HyperLink ID="lblTelefonoEmpresaAval" NavigateUrl="tel:+55599999999" CssClass="col-sm-6" runat="server"></asp:HyperLink>

                                                                <label class="col-sm-6">Extensión RRHH</label>
                                                                <asp:Label ID="lblExtensionRecursosHumanos" CssClass="col-sm-6" runat="server"></asp:Label>

                                                                <label class="col-sm-6">Extensión Aval</label>
                                                                <asp:Label ID="lblExtensionAval" CssClass="col-sm-6" runat="server"></asp:Label>

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
                                                            <label class="header-title text-center">Documentación</label>
                                                            <div class="container" id="divDocumentacionLaboral">
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
        </div>
    </div>

    <!-- MODAL VISUALIZAR DOCUMENTACION DE ESTE AVAL-->
    <div id="modalFinalizarValidarDocumentacion" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="modalFinalizarValidarDocumentacionLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title mt-0" id="modalFinalizarValidarDocumentacionLabel">Documentación del Aval</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                </div>
                <div class="modal-body">
                    <div class="container-fluid">
                        <div class="row">
                            <div class="col-md-12">
                                <label class="mt-0 header-title text-center">Documentación identidad</label>
                                <div class="popup-gallery" id="divDocumentacionCedulaModal">
                                </div>
                            </div>
                            <div class="col-md-12">
                                <label class="mt-0 header-title text-center">Documentación Domicilio</label>
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
    <script src="../../Scripts/plugins/iziToast/js/iziToast.js"></script>
    <script src="../../Scripts/plugins/iziToast/js/iziToast.min.js"></script>
    <script src="../../Scripts/plugins/magnific-popup/jquery.magnific-popup.min.js"></script>
    <script src="../../Scripts/plugins/imgBox/jquery.imgbox.js"></script>
    <script src="../../Scripts/app/Aval/Aval_Detalles.js"></script>
</body>
</html>