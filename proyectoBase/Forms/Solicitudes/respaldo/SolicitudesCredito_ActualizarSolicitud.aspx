<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SolicitudesCredito_ActualizarSolicitud.aspx.cs" Inherits="SolicitudesCredito_ActualizarSolicitud" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <title>Actualizar solicitud de crédito</title>
    <link href="/CSS/Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/CSS/Content/css/style.css?v=202010031033" rel="stylesheet" />
    <link href="/CSS/Content/css/icons.css?v=202010031033" rel="stylesheet" />
    <link href="/Scripts/plugins/iziToast/css/iziToast.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/steps/css/smart_wizard.css" rel="stylesheet" />
    <link href="/CSS/Content/css/font/font-fileuploader.css" rel="stylesheet" />
    <link href="/CSS/Content/css/jquery.fileuploader.min.css" rel="stylesheet" />
    <link href="/CSS/Content/css/jquery.fileuploader-theme-dragdrop.css" rel="stylesheet" />
    <link href="/Scripts/plugins/select2/css/select2.min.css" rel="stylesheet" />
    <link href="/CSS/Estilos_CSS.css" rel="stylesheet" />
    <link href="/Scripts/plugins/sweet-alert2/sweetalert2.min.css" rel="stylesheet" />
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

                <!-- loader -->
                <div class="float-right" id="divLoader" runat="server" visible="false">
                    <div class="spinner-border" role="status">
                        <span class="sr-only"></span>
                    </div>
                </div>

                <h5>Actualizar solicitud de crédito <small><span runat="server" id="lblMensaje" class="text-danger" visible="false"></span></small></h5>
            </div>
            <div class="card-body">
                <div id="smartwizard" class="h-100">
                    <ul>
                        <li runat="server" id="liInformacionPrestamo"><a href="#step-1" class="pt-3 pb-2 font-12">Condicionamientos</a></li>
                        <li runat="server" visible="false" id="liInformacionPrestamoSolicitado"><a href="#step-informacionPrestamoSolicitado" class="pt-3 pb-2 font-12">Información préstamo solicitado</a></li>
                        <li runat="server" visible="false" id="liInformacionPersonal"><a href="#step-2" class="pt-3 pb-2 font-12">Información personal</a></li>
                        <li runat="server" visible="false" id="liInformacionDomicilio"><a href="#step-3" class="pt-3 pb-2 font-12">Información domicilio</a></li>
                        <li runat="server" visible="false" id="liInformacionLaboral"><a href="#step-4" class="pt-3 pb-2 font-12">Información laboral</a></li>
                        <li runat="server" visible="false" id="liInformacionConyugal"><a href="#step-5" class="pt-3 pb-2 font-12">Información conyugal</a></li>
                        <li runat="server" visible="false" id="liReferenciasPersonales"><a href="#step-6" class="pt-3 pb-2 font-12">Referencias personales</a></li>
                        <li runat="server" visible="false" id="liDocumentacion"><a href="#step-7" class="pt-3 pb-2 font-12">Documentación</a></li>
                       <%-- <li runat="server" visible="false" id="liInformacionAval"><a href="#step-8" class="pt-3 pb-2 font-12">Informacion Aval</a></li>--%>
                    </ul>
                    <div>
                        <!-- Listado de condiciones de la solicitud -->
                        <div id="step-1" class="form-section">

                            <div class="row justify-content-between m-0 border-bottom border-gray">
                                <div class="col-auto pl-0">
                                    <h6 class="mt-1">Condiciones de la solcitud</h6>
                                </div>
                                <div class="col-auto pr-0 align-items-center">
                                    <div class="m-0 p-0">
                                        <div class="alert alert-info bg-info text-white mb-0 pt-1 pb-1" role="alert">
                                            <i class="fas fa-download text-white"></i>
                                            <a href="/Documentos/Recursos/MANUAL_DE ACTUALIZACION_DE_SOLICITUD_PDF.pdf" download class="text-white"><strong>Descargar manual de usuario</strong></a>
                                        </div>
                                    </div>
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


                        <!-- Información principal -->
                        <div id="step-informacionPrestamoSolicitado" class="form-section">

                            <div class="form-group row m-0 border-bottom border-gray justify-content-between">
                                <div class="col-auto p-0">
                                    <h6 class="mt-1">Actualizar información de la solicitud</h6>
                                </div>
                                <div class="col-auto p-0 mb-1">
                                    <button type="button" class="btn btn-warning pt-1 pb-1" id="btnFinalizarCondiciones_InfoSolicitud" data-toggle="modal" data-target="#modalFinalizarCondicion_InfoSolicitud">
                                        <i class="far fa-check-square"></i>
                                        Finalizar condiciones
                                    </button>
                                </div>
                            </div>

                            <!-- Montos de la solicitud -->
                            <div class="row">
                                <div class="col-12">
                                    <h6 class="mb-1 pt-3">Montos de la solicitud</h6>

                                    <div class="form-group row mb-0">
                                        <!-- Monto global -->
                                        <div class="col-sm-3">
                                            <asp:Label CssClass="col-form-label" ID="lblTituloMontoPrestmo" runat="server" Text="Valor global" AssociatedControlID="txtValorGlobal" />
                                            <asp:TextBox ID="txtValorGlobal" CssClass="form-control form-control-sm mascara-cantidad" Enabled="true" type="text" required="required" data-parsley-group="informacionPrestamo" runat="server"></asp:TextBox>
                                        </div>
                                        <!-- Valor de la prima -->
                                        <div class="col-sm-3" id="divPrima" runat="server">
                                            <label class="col-form-label" runat="server" id="lblTituloPrima">Valor de la prima</label>
                                            <asp:TextBox ID="txtValorPrima" CssClass="form-control form-control-sm mascara-cantidad" Enabled="true" required="required" type="text" data-parsley-group="informacionPrestamo" runat="server"></asp:TextBox>
                                        </div>
                                        <!-- Valor a Financiar -->
                                        <div class="col-sm-3" id="divPlazo" runat="server">
                                            <label class="col-form-label">Plazo <span class="font-weight-bold" id="lblTipoDePlazo" runat="server" Visible="false"></span></label>
                                            <asp:TextBox ID="txtPlazoSeleccionado" CssClass="form-control form-control-sm mascara-plazos" Enabled="true" required="required" type="text" data-parsley-group="informacionPrestamo" runat="server"></asp:TextBox>
                                        </div>
                                        <!-- frecuencia -->
                                          <div class="col-sm-3" runat="server" id="divFrecuencia">
                                            <label class="col-form-label">Frecuencia</label>
                                            <asp:DropDownList ID="ddlFrecuencia" runat="server" CssClass="form-control form-control-sm" data-parsley-group="informacionPrestamo" required="required" data-parsley-errors-container="#error-ddlFrecuencia"></asp:DropDownList>
                                            <div id="error-ddlFrecuencia"></div>
                                        </div>
                                    </div>
                                    <div class="form-group row mb-0" runat="server" id="divCotizadorAutos" visible="false">
                                        <div class="col-sm-4">
                                            <asp:Label CssClass="col-form-label" runat="server" Text="Gastos de cierre" AssociatedControlID="ddlTipoGastosDeCierre" />
                                            <asp:DropDownList ID="ddlTipoGastosDeCierre" runat="server" CssClass="form-control form-control-sm" Enabled="false" data-parsley-group="informacionPrestamo" required="required" data-parsley-errors-container="#error-ddlTipoGastosDeCierre"></asp:DropDownList>
                                            <div id="error-ddlTipoGastosDeCierre"></div>
                                        </div>
                                        <div class="col-sm-4">
                                            <label class="col-form-label">Tipo de seguro</label>
                                            <asp:DropDownList ID="ddlTipoDeSeguro" runat="server" CssClass="form-control form-control-sm" Enabled="false" data-parsley-group="informacionPrestamo" required="required" data-parsley-errors-container="#error-ddlTipoDeSeguro"></asp:DropDownList>
                                            <div id="error-ddlTipoDeSeguro"></div>
                                        </div>
                                        <div class="col-sm-4">
                                            <asp:Label CssClass="col-form-label" runat="server" Text="Lleva GPS" AssociatedControlID="ddlGps" />
                                            <asp:DropDownList ID="ddlGps" runat="server" CssClass="form-control form-control-sm" Enabled="false" data-parsley-group="informacionPrestamo" required="required" data-parsley-errors-container="#error-ddlGps"></asp:DropDownList>
                                            <div id="error-ddlGps"></div>
                                        </div>
                                        <div class="col-sm-4">
                                            <asp:Label CssClass="col-form-label" runat="server" Text="Aparato GPS Financiado" AssociatedControlID="ddlPagoGPS" />
                                            <asp:DropDownList ID="ddlPagoGPS" runat="server" CssClass="form-control form-control-sm" Enabled="false" data-parsley-group="informacionPrestamo" required="required" data-parsley-errors-container="#error-ddlPagoGPS"></asp:DropDownList>
                                            <div id="error-ddlPagoGPS"></div>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                    

                                        <div class="col-sm-3">
                                            <label class="col-form-label">Valor del préstamo</label>
                                            <asp:TextBox ID="txtValorDePrestamo" CssClass="form-control form-control-sm mascara-cantidad" ReadOnly="false" required="required" type="text" data-parsley-group="informacionPrestamo" runat="server"></asp:TextBox>
                                        </div>
                                        <!-- Valor a Financiar -->
                                        <div class="col-sm-3" id="divValorFinanciar" runat="server">
                                            <label class="col-form-label">Valor total a Financiar <small id="lblTasaAnual" class="font-weight-bold"></small></label>
                                            <asp:TextBox ID="txtValorFinanciar" CssClass="form-control form-control-sm mascara-cantidad" ReadOnly="false" required="required" type="text" data-parsley-group="informacionPrestamo" runat="server"></asp:TextBox>
                                        </div>
                                          <div class="col-sm-3">
                                            <label class="col-form-label">Tasa de interés anual(APR)</label>
                                            <asp:TextBox ID="txtTasaDeInteresAnual" CssClass="form-control form-control-sm font-weight-bold text-right" ReadOnly="true" required="required" type="text" data-parsley-group="informacionPrestamo" runat="server"></asp:TextBox>
                                        </div>
                                        <!-- Valor de la cuota calculada -->
                                        <div class="col-sm-3">
                                            <label class="col-form-label">Valor total de la cuota</label>
                                            <asp:TextBox ID="txtValorCuota" CssClass="form-control form-control-sm mascara-cantidad" ReadOnly="true" required="required" type="text" data-parsley-group="informacionPrestamo" runat="server"></asp:TextBox>
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
                                    <button type="button" class="btn btn-warning pt-1 pb-1" id="btnFinalizarCondiciones_InfoPersonal" data-toggle="modal" data-target="#modalFinalizarCondicion_InfoPersonal">
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
                                            <asp:TextBox ID="txtIdentidadCliente" CssClass="form-control form-control-sm " type="text"  required="required" data-parsley-group="informacionPersonal" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-6"  style="display:none;">
                                            <label class="col-form-label">RTN numérico</label>
                                            <asp:TextBox ID="txtRtnCliente" CssClass="form-control form-control-sm mascara-rtn" type="text" data-parsley-group="informacionPersonal" runat="server"></asp:TextBox>
                                        </div>
                                       <div class="col-sm-6">
                                            <label class="col-form-label">Documento Id Personal</label>
                                            <asp:DropDownList ID="dllDoctoIdPersonal" runat="server" CssClass="form-control form-control-sm buscadorddl" required="required" data-parsley-group="informacionPersonal" data-parsley-errors-container="#error-ddlNacionalidad"></asp:DropDownList>
                                           <%-- <div id="error-ddlNacionalidad"></div>--%>
                                        </div>

                                          <div class="col-6"  >
                                            <label class="col-form-label">No Id Fiscal</label>
                                            <asp:TextBox ID="txtNoIdFiscal" CssClass="form-control form-control-sm " type="text" data-parsley-group="informacionPersonal" runat="server"></asp:TextBox>
                                        </div>
                                       <div class="col-sm-6">
                                            <label class="col-form-label">Documento Id Fiscal</label>
                                            <asp:DropDownList ID="dllDocumentoIDFiscal" runat="server" CssClass="form-control form-control-sm buscadorddl" required="required" data-parsley-group="informacionPersonal" data-parsley-errors-container="#error-ddlDocumentoIdFiscal"></asp:DropDownList>
                                           <%-- <div id="error-ddlNacionalidad"></div>--%>
                                        </div>
                                        <div class="col-sm-6">
                                            <label class="col-form-label">Primer nombre</label>
                                            <asp:TextBox ID="txtPrimerNombre" CssClass="form-control form-control-sm" type="text"  required="required" data-parsley-group="informacionPersonal" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-6">
                                            <label class="col-form-label">Segundo nombre</label>
                                            <asp:TextBox ID="txtSegundoNombre" CssClass="form-control form-control-sm" type="text" data-parsley-group="informacionPersonal" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-6">
                                            <label class="col-form-label">Primer apellido</label>
                                            <asp:TextBox ID="txtPrimerApellido" CssClass="form-control form-control-sm" type="text"  required="required" data-parsley-group="informacionPersonal" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-6">
                                            <label class="col-form-label">Segundo apellido</label>
                                            <asp:TextBox ID="txtSegundoApellido" CssClass="form-control form-control-sm" type="text" data-parsley-group="informacionPersonal" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-6">
                                            <label class="col-form-label">Nacionalidad</label>
                                            <asp:DropDownList ID="ddlNacionalidad" runat="server" CssClass="form-control form-control-sm buscadorddl" required="required" data-parsley-group="informacionPersonal" data-parsley-errors-container="#error-ddlNacionalidad"></asp:DropDownList>
                                            <div id="error-ddlNacionalidad"></div>
                                        </div>
                                           <div class="col-sm-6">
                                            <label class="col-form-label">Origen Etnico</label>
                                            <asp:DropDownList ID="dllOrigenEtnico" runat="server" CssClass="form-control form-control-sm" required="required" data-parsley-group="informacionPersonal" data-parsley-errors-container="#error-ddlTipoDeCliente"></asp:DropDownList>
                                           <%-- <div id="error-ddlTipoDeCliente"></div>--%>
                                        </div>
                                        <div class="col-sm-6">
                                            <label class="col-form-label">Tipo de cliente</label>
                                            <asp:DropDownList ID="ddlTipoDeCliente" runat="server" CssClass="form-control form-control-sm" required="required" data-parsley-group="informacionPersonal" data-parsley-errors-container="#error-ddlTipoDeCliente"></asp:DropDownList>
                                            <div id="error-ddlTipoDeCliente"></div>
                                        </div>
                                            <div class="col-sm-6">
                                            <label class="col-form-label">Pais Nacimiento</label>
                                            <asp:DropDownList ID="dllPaisNacimiento" runat="server" CssClass="form-control form-control-sm" required="required"  data-parsley-errors-container="#error-ddlPaisNacimiento"></asp:DropDownList>
                                            <div id="error-ddlPaisNacimiento"></div>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <div class="col-md-4">
                                            <label class="col-form-label">Fecha de nacimiento</label>
                                            <asp:TextBox ID="txtFechaDeNacimiento" CssClass="form-control form-control-sm" type="date" data-parsley-group="informacionPersonal" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-md-4">
                                            <label class="col-form-label">Edad del cliente</label>
                                            <asp:TextBox ID="txtEdadDelCliente" CssClass="form-control form-control-sm"  type="text" required="required" data-parsley-group="informacionPersonal" runat="server"></asp:TextBox>
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
                                            <asp:TextBox ID="txtCorreoElectronico" CssClass="form-control form-control-sm" type="email" data-parsley-group="informacionPersonal" runat="server"></asp:TextBox>
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
                                    <button type="button" class="btn btn-warning pt-1 pb-1" id="btnFinalizarCondiciones_InfoDomicilio" data-toggle="modal" data-target="#modalFinalizarCondicion_InfoDomicilio">
                                        <i class="far fa-check-square"></i>
                                        Finalizar condiciones
                                    </button>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-12">
                                    <div class="form-group row">
                                      <%--  <div class="col-sm-3">
                                            <label class="col-form-label">Departamento</label>
                                            <asp:DropDownList ID="ddlDepartamentoDomicilio" runat="server" CssClass="form-control form-control-sm buscadorddl" required="required" data-parsley-group="informacionDomicilio" data-parsley-errors-container="#error-ddlDepartamentoDomicilio"></asp:DropDownList>
                                            <div id="error-ddlDepartamentoDomicilio"></div>
                                        </div>--%>
                                       <%-- <div class="col-sm-3">
                                            <label class="col-form-label">Municipio</label>
                                            <asp:DropDownList ID="ddlMunicipioDomicilio" runat="server" CssClass="form-control form-control-sm buscadorddl" Enabled="false" required="required" data-parsley-group="informacionDomicilio" data-parsley-errors-container="#error-ddlMunicipioDomicilio"></asp:DropDownList>
                                            <div id="error-ddlMunicipioDomicilio"></div>
                                        </div>--%>
                                       <%-- <div class="col-sm-3">
                                            <label class="col-form-label">Ciudad/Poblado</label>
                                            <asp:DropDownList ID="ddlCiudadPobladoDomicilio" runat="server" CssClass="form-control form-control-sm buscadorddl" Enabled="false" required="required" data-parsley-group="informacionDomicilio" data-parsley-errors-container="#error-ddlCiudadPobladoDomicilio"></asp:DropDownList>
                                            <div id="error-ddlCiudadPobladoDomicilio"></div>
                                        </div>--%>
                                        <div class="col-sm-8">
                                            <label class="col-form-label">Codigo Postal</label>
                                            <asp:DropDownList ID="ddlBarrioColoniaDomicilio" runat="server" CssClass="form-control form-control-sm buscadorddl"  required="required" data-parsley-group="informacionDomicilio" data-parsley-errors-container="#error-ddlBarrioColoniaDomicilio"></asp:DropDownList>
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
                                    <button type="button" class="btn btn-warning pt-1 pb-1" id="btnFinalizarCondiciones_InfoLaboral" data-toggle="modal" data-target="#modalFinalizarCondicion_InfoLaboral">
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
                             <%--   <div class="col-sm-3">
                                    <label class="col-form-label">Departamento</label>
                                    <asp:DropDownList ID="ddlDepartamentoEmpresa" runat="server" CssClass="form-control form-control-sm buscadorddl" required="required" data-parsley-group="informacionLaboral" data-parsley-errors-container="#error-ddlDepartamentoEmpresa"></asp:DropDownList>
                                    <div id="error-ddlDepartamentoEmpresa"></div>
                                </div>--%>
                             <%--   <div class="col-sm-3">
                                    <label class="col-form-label">Municipio</label>
                                    <asp:DropDownList ID="ddlMunicipioEmpresa" runat="server" CssClass="form-control form-control-sm buscadorddl" Enabled="false" required="required" data-parsley-group="informacionLaboral" data-parsley-errors-container="#error-ddlMunicipioEmpresa"></asp:DropDownList>
                                    <div id="error-ddlMunicipioEmpresa"></div>
                                </div>--%>
                              <%--  <div class="col-sm-3">
                                    <label class="col-form-label">Ciudad/Poblado</label>
                                    <asp:DropDownList ID="ddlCiudadPobladoEmpresa" runat="server" CssClass="form-control form-control-sm buscadorddl" Enabled="false" required="required" data-parsley-group="informacionLaboral" data-parsley-errors-container="#error-ddlCiudadPobladoEmpresa"></asp:DropDownList>
                                    <div id="error-ddlCiudadPobladoEmpresa"></div>
                                </div>--%>
                                <div class="col-sm-8">
                                    <label class="col-form-label">Codigo Postal</label>
                                    <asp:DropDownList ID="ddlBarrioColoniaEmpresa" runat="server" CssClass="form-control form-control-sm buscadorddl"  required="required" data-parsley-group="informacionLaboral" data-parsley-errors-container="#error-ddlBarrioColoniaEmpresa"></asp:DropDownList>
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
                                    <button type="button" class="btn btn-warning pt-1 pb-1" id="btnFinalizarCondiciones_InfoConyugue" data-toggle="modal" data-target="#modalFinalizarCondicion_InfoConyugal">
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
                                    <button type="button" class="btn btn-warning pt-1 pb-1" id="btnFinalizarCondiciones_Referencias" data-toggle="modal" data-target="#modalFinalizarCondicion_ReferenciasPersonales">
                                        <i class="far fa-check-square"></i>
                                        Finalizar condiciones
                                    </button>
                                </div>
                            </div>

                            <button type="button" id="btnAgregarReferencia" class="btn btn-info mb-1 mt-1">Nuevo</button>

                            <div class="table-responsive">
                                <table class="table tabla-compacta table-striped table-bordered table-hover" id="tblReferenciasPersonales" runat="server">
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
                                    <button type="button" class="btn btn-warning pt-1 pb-1" id="btnFinalizarCondiciones_Documentacion" data-toggle="modal" data-target="#modalFinalizarCondicion_Documentacion">
                                        <i class="far fa-check-square"></i>
                                        Finalizar condiciones
                                    </button>
                                </div>
                            </div>
                            <!-- Div donde se generan dinamicamente los inputs para la documentación -->
                            <div class="row pr-1 pl-1 text-center" id="DivDocumentacion">
                            </div>
                        </div>

                  <%--      <div id="step-8" class="form-section">

                            <div class="form-group row m-0 border-bottom border-gray justify-content-between">
                                <div class="col-auto p-0">
                                    <h6 class="mt-1">Actualizar información Aval</h6>
                                </div>
                                <div class="col-auto p-0 mb-1">
                                    <button type="button" class="btn btn-warning pt-1 pb-1" id="btnmodalFinalizarCondicion_Aval" data-toggle="modal" data-target="#modalFinalizarCondicion_InfoAval">
                                        <i class="far fa-check-square"></i>
                                        Finalizar condiciones
                                    </button>
                                </div>
                            </div>
                            </div>--%>



                    </div>
                </div>
            </div>
        </div>


        <!--modal de agregar nueva referencia personal -->
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
                                <asp:TextBox ID="txtTelefonoReferencia" CssClass="form-control form-control-sm " type="text" required="required" data-parsley-group="referenciasPersonales" runat="server"></asp:TextBox>
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
                            Guardar
                        </button>
                        <button type="reset" data-dismiss="modal" class="btn btn-secondary waves-effect">
                            Cancelar
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <!--modal de eliminar referencia personal -->
        <div id="modalEliminarReferenciaPersonal" class="modal fade" role="dialog" aria-labelledby="modalEliminarReferenciaPersonalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h6 class="modal-title mt-0" id="modalEliminarReferenciaPersonalLabel">Eliminar referencia personal</h6>
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    </div>
                    <div class="modal-body">
                        <label class="col-form-label font-weight-bold">¿Está seguro de eliminar esta referencia personal?</label>
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

        <!--modal de editar referencia personal-->
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
                                <asp:TextBox ID="txtTelefonoReferenciaPersonal_Editar" CssClass="form-control form-control-sm " type="text" required="required" data-parsley-group="referenciasPersonalesEditar" runat="server"></asp:TextBox>
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
                                <asp:DropDownList ID="ddlParentescos_Editar" runat="server" CssClass="form-control form-control-sm" required="required" data-parsley-group="referenciasPersonalesEditar" data-parsley-errors-container="#error-ddlParentesco_Editar"></asp:DropDownList>
                                <div id="error-ddlParentescos_Editar"></div>
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

        <!--modal de finalizar condiciones de informacion personal -->
        <div id="modalFinalizarCondicion_InfoSolicitud" class="modal fade bs-example-modal-lg" role="dialog" aria-labelledby="modalFinalizarCondicion_InfoSolicitudLabel" aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header pt-2 pb-2">
                        <h6 class="modal-title" id="modalFinalizarCondicion_InfoSolicitudLabel">Finalizar condicionamientos (<b>Información de la solicitud</b>)</h6>
                    </div>
                    <div class="modal-body">
                        <div class="form-group row">
                            <div class="col-12">
                                <div class="table-responsive">
                                    <table id="tblCondicionesInformacionSolicitud" class="table tabla-compacta table-striped table-bordered" runat="server">
                                        <thead>
                                            <tr>
                                                <th>Tipo de condición</th>
                                                <th>Descripción</th>
                                                <th>Comentario adicional</th>
                                                <th>Estado</th>
                                                <th>Acciones</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button data-dismiss="modal" class="btn btn-secondary waves-effect">
                            Cerrar
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <!--modal de finalizar condiciones de informacion personal -->
        <div id="modalFinalizarCondicion_InfoPersonal" class="modal fade bs-example-modal-lg" role="dialog" aria-labelledby="modalFinalizarCondicionListaLabel" aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header pt-2 pb-2">
                        <h6 class="modal-title" id="modalFinalizarCondicionListaLabel">Finalizar condicionamientos (<b>Información personal</b>)</h6>
                    </div>
                    <div class="modal-body">
                        <div class="form-group row">
                            <div class="col-12">
                                <div class="table-responsive">
                                    <table id="tblCondicionesInformacionPersonal" class="table tabla-compacta table-striped table-bordered" runat="server">
                                        <thead>
                                            <tr>
                                                <th>Tipo de condición</th>
                                                <th>Descripción</th>
                                                <th>Comentario adicional</th>
                                                <th>Estado</th>
                                                <th>Acciones</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button data-dismiss="modal" class="btn btn-secondary waves-effect">
                            Cerrar
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <!--modal de finalizar condiciones de informacion del domicilio -->
        <div id="modalFinalizarCondicion_InfoDomicilio" class="modal fade bs-example-modal-lg" role="dialog" aria-labelledby="modalFinalizarCondicion_InfoDomicilioLabel" aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header pt-2 pb-2">
                        <h6 class="modal-title" id="modalFinalizarCondicion_InfoDomicilioLabel">Finalizar condicionamientos (<b>Información del domicilio</b>)</h6>
                    </div>
                    <div class="modal-body">
                        <div class="form-group row">
                            <div class="col-12">
                                <div class="table-responsive">
                                    <table id="tblCondicionesDomicilio" class="table tabla-compacta table-striped table-bordered" runat="server">
                                        <thead>
                                            <tr>
                                                <th>Tipo de condición</th>
                                                <th>Descripción</th>
                                                <th>Comentario adicional</th>
                                                <th>Estado</th>
                                                <th>Acciones</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button data-dismiss="modal" class="btn btn-secondary waves-effect">
                            Cerrar
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <!--modal de finalizar condiciones de informacion laboral -->
        <div id="modalFinalizarCondicion_InfoLaboral" class="modal fade bs-example-modal-lg" role="dialog" aria-labelledby="modalFinalizarCondicion_InfoLaboralLabel" aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header pt-2 pb-2">
                        <h6 class="modal-title" id="modalFinalizarCondicion_InfoLaboralLabel">Finalizar condicionamientos (<b>Información laboral</b>)</h6>
                    </div>
                    <div class="modal-body">
                        <div class="form-group row">
                            <div class="col-12">
                                <div class="table-responsive">
                                    <table id="tblCondicionesLaboral" class="table tabla-compacta table-striped table-bordered" runat="server">
                                        <thead>
                                            <tr>
                                                <th>Tipo de condición</th>
                                                <th>Descripción</th>
                                                <th>Comentario adicional</th>
                                                <th>Estado</th>
                                                <th>Acciones</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button data-dismiss="modal" class="btn btn-secondary waves-effect">
                            Cerrar
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <!--modal de finalizar condiciones de referencias personales -->
        <div id="modalFinalizarCondicion_ReferenciasPersonales" class="modal fade bs-example-modal-lg" role="dialog" aria-labelledby="modalFinalizarCondicion_ReferenciasPersonalesLabel" aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header pt-2 pb-2">
                        <h6 class="modal-title" id="modalFinalizarCondicion_ReferenciasPersonalesLabel">Finalizar condicionamientos (<b>Referencias personales</b>)</h6>
                    </div>
                    <div class="modal-body">
                        <div class="form-group row">
                            <div class="col-12">
                                <div class="table-responsive">
                                    <table id="tblCondicionesReferenciasPersonales" class="table tabla-compacta table-striped table-bordered" runat="server">
                                        <thead>
                                            <tr>
                                                <th>Tipo de condición</th>
                                                <th>Descripción</th>
                                                <th>Comentario adicional</th>
                                                <th>Estado</th>
                                                <th>Acciones</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button data-dismiss="modal" class="btn btn-secondary waves-effect">
                            Cerrar
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <!--modal de finalizar condiciones de documentación -->
        <div id="modalFinalizarCondicion_Documentacion" class="modal fade bs-example-modal-lg" role="dialog" aria-labelledby="modalFinalizarCondicion_DocumentacionLabel" aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header pt-2 pb-2">
                        <h6 class="modal-title" id="modalFinalizarCondicion_DocumentacionLabel">Finalizar condicionamientos (<b>Documentación</b>)</h6>
                    </div>
                    <div class="modal-body">
                        <div class="form-group row">
                            <div class="col-12">
                                <div class="table-responsive">
                                    <table id="tblCondicionesDocumentacion" class="table tabla-compacta table-striped table-bordered" runat="server">
                                        <thead>
                                            <tr>
                                                <th>Tipo de condición</th>
                                                <th>Descripción</th>
                                                <th>Comentario adicional</th>
                                                <th>Estado</th>
                                                <th>Acciones</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button data-dismiss="modal" class="btn btn-secondary waves-effect">
                            Cerrar
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <!--modal de finalizar condiciones de informacion conyugal -->
        <div id="modalFinalizarCondicion_InfoConyugal" class="modal fade bs-example-modal-lg" role="dialog" aria-labelledby="modalFinalizarCondicion_InfoConyugalLabel" aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header pt-2 pb-2">
                        <h6 class="modal-title" id="modalFinalizarCondicion_InfoConyugalLabel">Finalizar condicionamientos (<b>Información conyugal</b>)</h6>
                    </div>
                    <div class="modal-body">
                        <div class="form-group row">
                            <div class="col-12">
                                <div class="table-responsive">
                                    <table id="tblCondicionesInformacionConyugal" class="table tabla-compacta table-striped table-bordered" runat="server">
                                        <thead>
                                            <tr>
                                                <th>Tipo de condición</th>
                                                <th>Descripción</th>
                                                <th>Comentario adicional</th>
                                                <th>Estado</th>
                                                <th>Acciones</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button data-dismiss="modal" class="btn btn-secondary waves-effect">
                            Cerrar
                        </button>
                    </div>
                </div>
            </div>
        </div>

         <!--modal de finalizar condiciones de informacion Aval -->
<%--        <div id="modalFinalizarCondicion_InfoAval" class="modal fade bs-example-modal-lg" role="dialog" aria-labelledby="modalFinalizarCondicion_InfoAvalLabel" aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header pt-2 pb-2">
                        <h6 class="modal-title" id="modalFinalizarCondicion_InfoAvalLabel">Finalizar condicionamientos (<b>Información Aval</b>)</h6>
                    </div>
                    <div class="modal-body">
                        <div class="form-group row">
                            <div class="col-12">
                                <div class="table-responsive">
                                    <table id="tblCondicionesInformacionAval" class="table tabla-compacta table-striped table-bordered" runat="server">
                                        <thead>
                                            <tr>
                                                <th>Tipo de condición</th>
                                                <th>Descripción</th>
                                                <th>Comentario adicional</th>
                                                <th>Estado</th>
                                                <th>Acciones</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button data-dismiss="modal" class="btn btn-secondary waves-effect">
                            Cerrar
                        </button>
                    </div>
                </div>
            </div>
        </div>--%>

        <!--modal de confirmacion de finalizacion de condiciones  -->
        <div id="modalFinalizarCondicion_Confirmar" class="modal fade" role="dialog" aria-labelledby="modalFinalizarCondicion_ConfirmarLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h6 class="modal-title mt-0" id="modalFinalizarCondicion_ConfirmarLabel">Finalizar condición</h6>
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    </div>
                    <div class="modal-body">
                        <label class="col-form-label font-weight-bold">¿Está seguro de finalizar esta condición?</label>
                    </div>
                    <div class="modal-footer">
                        <button id="btnFinalizarCondicion_Confirmar" type="button" class="btn btn-danger waves-effect waves-light">
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
        const ID_CLIENTE = <%=this.IdCliente%>;
        const ID_PRODUCTO = <%=this.IdProducto%>;
        const ID_SOLICITUD = <%=this.IdSolicitud%>;
        const TASA_ANUAL =  <%=this.TasaAnual%>;
        const PRECALIFICADO = <%=this.jsonPrecalicado%>;
    </script>
    <script src="/Scripts/plugins/steps/js/jquery.smartWizard.js"></script>
    <script src="/Scripts/plugins/iziToast/js/iziToast.min.js"></script>
    <script src="/Scripts/plugins/parsleyjs/parsley.js"></script>
    <script src="/Scripts/app/uploader/js/jquery.fileuploader.min.js"></script>
    <script src="/Scripts/plugins/select2/js/select2.full.min.js"></script>
    <script src="/Scripts/plugins/sweet-alert2/sweetalert2.min.js"></script>
    <script src="/Scripts/app/solicitudes/SolicitudesCredito_ActualizarSolicitud.js?v=20210222044785"></script>
</body>
</html>
