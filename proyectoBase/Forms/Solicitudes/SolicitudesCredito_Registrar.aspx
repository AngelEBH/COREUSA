<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SolicitudesCredito_Registrar.aspx.cs" Inherits="SolicitudesCredito_Registrar" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <title>Ingresar solicitud de crédito</title>
    <link href="/CSS/Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/CSS/Content/css/style.css?v=202010031033" rel="stylesheet" />
    <link href="/CSS/Content/css/icons.css?v=202010031033" rel="stylesheet" />
    <link href="/Scripts/plugins/iziToast/css/iziToast.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/steps/css/smart_wizard.css" rel="stylesheet" />
    <link href="/CSS/Content/css/font/font-fileuploader.css" rel="stylesheet" />
    <link href="/CSS/Content/css/jquery.fileuploader.min.css" rel="stylesheet" />
    <link href="/CSS/Content/css/jquery.fileuploader-theme-dragdrop.css" rel="stylesheet" />
    <link href="/Scripts/plugins/select2/css/select2.min.css" rel="stylesheet" />
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
                <div class="row justify-content-between">
                    <div class="col-auto">
                        <h5>Nueva solicitud de crédito <small><span runat="server" id="lblMensaje" class="text-danger" visible="false"></span></small></h5>
                    </div>
                    <div class="col-auto">
                        <asp:HyperLink ID="btnLinkAplicacionCredito" runat="server" target="_blank" class="btn btn-primary text-white">Ver Aplicacion de Credito</asp:HyperLink>
                    </div>
                     <div class="col-auto">
                        <%--<asp:HyperLink ID="HyperLink1" runat="server" target="_blank" class="btn btn-primary text-white">Ver Aplicacion de Credito</asp:HyperLink>--%>
                          <%-- <button id="btnNuevaReferencia2" type="button" class="btn btn-info waves-effect waves-light float-right" onclick="ConfirmarAval()">    Agregar aval
                           </button>--%>
                    </div>
                    <div class="col-1 align-self-center">
                        <!-- loader -->
                        <div id="Loader" class="float-right" runat="server" style="display: none;">
                            <div class="spinner-border" role="status">
                                <span class="sr-only"></span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="card-body">
                <div id="smartwizard" class="h-100">
                    <ul>
                        <li><a href="#step-1" class="pt-3 pb-2 font-12">Información del préstamo</a></li>
                        <li><a href="#step_garantia" class="pt-3 pb-2 font-12" runat="server" visible="true" id="step_garantia_titulo">Características garantía</a></li>
                        <li><a href="#step-2" class="pt-3 pb-2 font-12">Información personal</a></li>
                        <li><a href="#step-3" class="pt-3 pb-2 font-12">Información domicilio</a></li>
                        <li><a href="#step-4" class="pt-3 pb-2 font-12">Información laboral</a></li>
                        <li><a href="#step-5" class="pt-3 pb-2 font-12">Información conyugal</a></li>
                        <li><a href="#step-6" class="pt-3 pb-2 font-12">Referencias personales</a></li>
                       <%-- <li><a href="#step-8" class="pt-3 pb-2 font-12">Informacion Aval</a></li>--%>
                        <li><a href="#step-7" class="pt-3 pb-2 font-12">Documentación</a></li>
                    </ul>
                    <div>
                        <!-- Información principal -->
                        <div id="step-1" class="form-section">

                            <div class="form-group row m-0 border-bottom border-gray">
                                <div class="col-4 p-0">
                                    <h6 class="mt-1">Información del préstamo</h6>
                                </div>
                                <div class="col-8 align-self-end text-right">
                                    <label style="visibility: hidden;">Tipo de solicitud: <span class="btn btn-sm btn-info mb-0" id="lblTipodeSolicitud" runat="server">NUEVO</span></label>
                                    <label style="visibility: hidden;">Producto: <span class="btn btn-sm btn-info mb-0" id="lblProducto" runat="server">Prestadito Motos</span></label>
                                    <label style="visibility: hidden;">Score: <span class="btn btn-sm btn-info mb-0" id="lblScoreCliente" runat="server">N/A</span></label>
                                    <button type="button" class="btn btn-md p-0" title="Solicitar cambio de SCORE" data-toggle="modal" data-target="#modalCambiarScore" style="visibility: hidden;"><i class="far fa-edit p-0 m-0"></i></button>
                                </div>
                            </div>
                            <div class="row mb-0">
                                <!-- Información del cliente -->
                                <div class="col-lg-6">
                                    <h6 class="mt-3 mb-1">Datos del cliente</h6>

                                    <div class="form-group row">
                                        <div class="col-sm-3">
                                            <label class="col-form-label">No. Identificacion</label>
                                            <asp:TextBox ID="txtIdentidadCliente" CssClass="form-control form-control-sm mascara-identidad" type="text" Enabled="false" required="required" data-parsley-group="informacionPrestamo" runat="server"></asp:TextBox>
                                        </div>
                                     <%--   <div class="col-6" style="display:none;">
                                            <label class="col-form-label">RTN numérico</label>
                                            <asp:TextBox ID="txtRtnCliente" CssClass="form-control form-control-sm mascara-rtn" type="text" data-parsley-group="informacionPrestamo" runat="server"></asp:TextBox>
                                        </div>--%>
                                        
                                        <div class="col-sm-3">
                                            <label class="col-form-label">Docto Id.Personal</label>
                                            <asp:TextBox ID="txtDocumentoIdPersonal" CssClass="form-control form-control-sm " type="text" Enabled="false"  data-parsley-group="informacionPrestamo" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3" >
                                            <label class="col-form-label">No. Id Fiscal</label>
                                            <asp:TextBox ID="txtNoIdFiscal" CssClass="form-control form-control-sm " type="text"  Enabled="false" data-parsley-group="informacionPrestamo" runat="server"></asp:TextBox>
                                        </div>
                                    
                                        <div class="col-sm-3">
                                            <label class="col-form-label">Docto Id.Personal</label>
                                            <asp:TextBox ID="txtDoctoIdFiscal" CssClass="form-control form-control-sm " type="text" Enabled="false" required="required" data-parsley-group="informacionPrestamo" runat="server"></asp:TextBox>
                                        </div>
                                  
                                     </div>
                                   <%--  desarrollo angel--%>
                                  <%--    <div class="form-group row">
                                     
                                     </div>--%>
                                  <%-- end--%>
                                    <div class="form-group row">
                                        <div class="col-sm-3">
                                            <label class="col-form-label">Primer nombre</label>
                                            <asp:TextBox ID="txtPrimerNombre" CssClass="form-control form-control-sm" type="text" Enabled="false" required="required" data-parsley-group="informacionPrestamo" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label class="col-form-label">Segundo nombre</label>
                                            <asp:TextBox ID="txtSegundoNombre" CssClass="form-control form-control-sm" type="text" Enabled="false" required="required" data-parsley-group="informacionPrestamo" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label class="col-form-label">Primer apellido</label>
                                            <asp:TextBox ID="txtPrimerApellido" CssClass="form-control form-control-sm" type="text" Enabled="false" required="required" data-parsley-group="informacionPrestamo" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label class="col-form-label">Segundo apellido</label>
                                            <asp:TextBox ID="txtSegundoApellido" CssClass="form-control form-control-sm" type="text" Enabled="false" required="required" data-parsley-group="informacionPrestamo" runat="server"></asp:TextBox>
                                        </div>
                                     </div>
                                    <div class="form-group row">
                                        <div class="col-sm-3">
                                            <label class="col-form-label">Ingresos precalificado</label>
                                            <asp:TextBox ID="txtIngresosPrecalificados" CssClass="form-control form-control-sm mascara-cantidad" type="text" Enabled="false" required="required" data-parsley-group="informacionPrestamo" runat="server"></asp:TextBox>
                                        </div>
                                           <div class="col-sm-3">
                                            <label class="col-form-label">Pais de Nacimiento</label>
                                            <asp:TextBox ID="txtPaisNaciemiento" CssClass="form-control form-control-sm " type="text" Enabled="false" required="required" data-parsley-group="informacionPrestamo" runat="server"></asp:TextBox>
                                        </div>                                       
                                        <div class="col-sm-3" >
                                            <label class="col-form-label">Origen Etnico</label>
                                            <asp:TextBox ID="txtOrigenEtnico" CssClass="form-control form-control-sm " type="text"  Enabled="false"  data-parsley-group="informacionPrestamo" runat="server"></asp:TextBox>
                                        </div>

                                    </div>
                                </div>
                                <!-- Información del préstamo máximo -->
                                <div class="col-lg-6 border-left border-gray">
                                    <h6 class="mt-3 mb-1">Información De Cuotas</h6>
                                    <div class="form-group row">
                                     
                                        <div class="col-sm-4">
                                            <asp:Label CssClass="col-form-label" ID="lblTituloPlazoMaximo" runat="server" Text="Cuota del Auto" AssociatedControlID="txtPlazoMaximo" />
                                            <asp:TextBox ID="txtPlazoMaximo" CssClass="form-control form-control-sm" type="text" Enabled="false" required="required" data-parsley-group="informacionPrestamo" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-4">
                                            <asp:Label CssClass="col-form-label" ID="lblTituloCuotaMaxima" runat="server" Text="Cuota Collateral" AssociatedControlID="txtCuotaMaxima" />
                                            <asp:TextBox ID="txtCuotaMaxima" CssClass="form-control form-control-sm mascara-cantidad" type="text" Enabled="false" required="required" data-parsley-group="informacionPrestamo" runat="server"></asp:TextBox>
                                        </div>
                                           <div class="col-sm-4">
                                            <label class="col-form-label">Cuota Final</label>
                                            <asp:TextBox ID="txtPrestamoMaximo" CssClass="form-control form-control-sm mascara-cantidad" type="text" Enabled="false" required="required" data-parsley-group="informacionPrestamo" runat="server"></asp:TextBox>
                                        </div>
                                    </div>

                            
                                </div>
                            </div>
                            <!-- Montos de la solicitud -->
                            <div class="row">
                                <div class="col-12">
                                    <h6 class="mb-1 border-top border-gray pt-3">Producto / Montos de la solicitud</h6>
                                    <div class="form-group row mb-0">
                                        <!-- Producto -->
                                        <div class="col-sm-3" runat="server" id="divProducto">
                                            <label class="col-form-label">Producto</label>
                                            <asp:DropDownList ID="ddlProducto" runat="server" CssClass="form-control form-control-sm" data-parsley-group="informacionPrestamo" required="required" data-parsley-errors-container="#error-ddlProducto"></asp:DropDownList>
                                            <div id="error-ddlProducto"></div>
                                        </div>
                                        <!-- Origen -->
                                        <div class="col-sm-3" runat="server" id="divOrigen">
                                            <label class="col-form-label">Origen</label>
                                            <asp:DropDownList ID="ddlOrigen" runat="server" CssClass="form-control form-control-sm" Enabled="true" data-parsley-group="informacionPrestamo" required="required" data-parsley-errors-container="#error-ddlOrigen"></asp:DropDownList>
                                            <div id="error-ddlOrigen"></div>
                                        </div>
                                        <!-- Frecuencia -->
                                        <div class="col-sm-3" runat="server" id="divFrecuencia">
                                            <label class="col-form-label">Frecuencia</label>
                                            <asp:DropDownList ID="ddlFrecuencia" runat="server" CssClass="form-control form-control-sm" data-parsley-group="informacionPrestamo" required="required" data-parsley-errors-container="#error-ddlFrecuencia"></asp:DropDownList>
                                            <div id="error-ddlFrecuencia"></div>
                                        </div>
                                        <!-- Fecha de inicio de contrato -->
                                        <div class="col-sm-3" id="divContrato" runat="server">
                                            <label class="col-form-label" runat="server" id="lblInicioContrato">Fecha inicio de contrato</label>
                                            <asp:TextBox ID="txtIniciodeContrato" CssClass="form-control form-control-sm mascara-cantidad" Enabled="true" required="required" type="date" data-parsley-group="informacionPrestamo" runat="server"></asp:TextBox>
                                        </div>

                                    </div>
                                    <div class="form-group row mb-0">
                                        <!-- Monto global -->
                                        <div class="col-sm-3">
                                            <asp:Label CssClass="col-form-label" ID="lblTituloMontoPrestmo" runat="server" Text="Valor global" AssociatedControlID="txtValorGlobal" />
                                            <button id="btnSeleccionarPrecioDeMercado" class="btn btn-sm btn-secondary pt-0 pb-0" type="button" visible="false" title="Seleccionar precio de mercado" runat="server">
                                                <i class="fa fa-search"></i>
                                            </button>
                                            <asp:TextBox ID="txtValorGlobal" Enabled="true" CssClass="form-control form-control-sm mascara-cantidad" type="text" required="required" data-parsley-group="informacionPrestamo" runat="server"></asp:TextBox>
                                        </div>
                                        <!-- Valor de la prima -->
                                        <div class="col-sm-3" id="divPrima" runat="server">
                                            <label class="col-form-label" runat="server" id="lblTituloPrima">Valor de la prima</label>
                                            <asp:TextBox ID="txtValorPrima" CssClass="form-control form-control-sm mascara-cantidad" Enabled="false" required="required" type="text" data-parsley-group="informacionPrestamo" runat="server"></asp:TextBox>
                                        </div>

                                        <div class="col-sm-3">
                                            <asp:Label CssClass="col-form-label" ID="lblTituloPlazo" runat="server"  AssociatedControlID="txtPlazosDisponibles" />
                                            <asp:TextBox ID="txtPlazosDisponibles" CssClass="form-control form-control-sm mascara-enteros" type="text" Enabled="true" required="required" data-parsley-group="informacionPrestamo" runat="server"></asp:TextBox>
                                        </div>

                                        <div class="col-sm-3">
                                            <label class="col-form-label">Tasa de interés anual(APR)</label>
                                            <asp:TextBox ID="txtTasaDeInteresAnual" CssClass="form-control form-control-sm font-weight-bold text-right" ReadOnly="true" required="required" type="text" data-parsley-group="informacionPrestamo" runat="server"></asp:TextBox>
                                        </div>

                                        <%--<div class="col-sm-3" id="divPlazo" runat="server">
                                            <asp:Label CssClass="col-form-label" ID="lblTituloPlazo" runat="server" Text="Plazos disponibles" AssociatedControlID="ddlPlazosDisponibles" />
                                            <asp:DropDownList ID="ddlPlazosDisponibles" runat="server" CssClass="form-control form-control-sm" required="required" data-parsley-group="informacionPrestamo" data-parsley-errors-container="#error-ddlPlazosDisponibles"></asp:DropDownList>
                                            <div id="error-ddlPlazosDisponibles"></div>
                                        </div>--%>
                                        <!-- Tipo de moneda -->
                                        <%--<div class="col-sm-3">
                                            <label class="col-form-label">Moneda</label>
                                            <asp:DropDownList ID="ddlMoneda" runat="server" CssClass="form-control form-control-sm" required="required" data-parsley-group="informacionPrestamo" data-parsley-errors-container="#error-ddlMoneda"></asp:DropDownList>
                                            <div id="error-ddlMoneda"></div>
                                        </div>--%>
                                    </div>

                                    <!-- Parámetros de cotizador de autos -->
                                    <div class="form-group row mb-0" runat="server" id="divCotizadorAutos" visible="false">
                                        <div class="col-sm-3">
                                            <asp:Label CssClass="col-form-label" runat="server" Text="Gastos de cierre" AssociatedControlID="ddlTipoGastosDeCierre" />
                                            <asp:DropDownList ID="ddlTipoGastosDeCierre" runat="server" CssClass="form-control form-control-sm" Enabled="false" data-parsley-group="informacionPrestamo" required="required" data-parsley-errors-container="#error-ddlTipoGastosDeCierre"></asp:DropDownList>
                                            <div id="error-ddlTipoGastosDeCierre"></div>
                                        </div>
                                        <div class="col-sm-3">
                                            <label class="col-form-label">Tipo de seguro</label>
                                            <asp:DropDownList ID="ddlTipoDeSeguro" runat="server" CssClass="form-control form-control-sm" Enabled="false" data-parsley-group="informacionPrestamo" required="required" data-parsley-errors-container="#error-ddlTipoDeSeguro"></asp:DropDownList>
                                            <div id="error-ddlTipoDeSeguro"></div>
                                        </div>
                                        <div class="col-sm-3">
                                            <asp:Label CssClass="col-form-label" runat="server" Text="Lleva GPS" AssociatedControlID="ddlGps" />
                                            <asp:DropDownList ID="ddlGps" runat="server" CssClass="form-control form-control-sm" Enabled="false" data-parsley-group="informacionPrestamo" required="required" data-parsley-errors-container="#error-ddlGps"></asp:DropDownList>
                                            <div id="error-ddlGps"></div>
                                        </div>
                                    </div>

                                    <div class="form-group row">
                                        <div class="col-sm-3">
                                            <label class="col-form-label">Valor del préstamo del vehiculo(avalúo)</label>
                                            <asp:TextBox ID="txtValorDePrestamo" CssClass="form-control form-control-sm mascara-cantidad" ReadOnly="true" required="required" type="text" data-parsley-group="informacionPrestamo" runat="server"></asp:TextBox>
                                        </div>
                                             <%--Valor Lienholder--%>
                                          <div class="col-sm-3" id="divLienHolder" runat="server">
                                            <label class="col-form-label">Lien Holder Fee</label>
                                            <asp:TextBox ID="txtLienHolder" CssClass="form-control form-control-sm mascara-cantidad"   type="text" data-parsley-group="informacionPrestamo" runat="server"></asp:TextBox>
                                        </div>
                                        <!-- Valor a Financiar -->
                                        <div class="col-sm-3" id="divValorFinanciar" runat="server">
                                            <label class="col-form-label">Valor total a Financiar <small id="lblTasaAnual" class="font-weight-bold"></small></label>
                                            <asp:TextBox ID="txtValorFinanciar" CssClass="form-control form-control-sm mascara-cantidad" ReadOnly="true" required="required" type="text" data-parsley-group="informacionPrestamo" runat="server"></asp:TextBox>
                                        </div>
                                    
                                        <!-- Valor de la cuota calculada -->
                                        <div class="col-sm-3">
                                            <label class="col-form-label">Valor total de la cuota</label>
                                            <asp:TextBox ID="txtValorCuota" CssClass="form-control form-control-sm mascara-cantidad" ReadOnly="true" Enabled="false" required="required" type="text" data-parsley-group="informacionPrestamo" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Información de la garantía -->
                        <div id="step_garantia" class="form-section" runat="server" visible="true">

                            <div class="row justify-content-between m-0 border-bottom border-gray">
                                <div class="col-auto" style="display:none;">
                                    <div class="form-group row align-items-center mb-1">
                                        <div class="col-sm-auto pr-0 pl-0">
                                            <asp:TextBox ID="txtBuscarVIN" placeholder="Buscar VIN" CssClass="form-control form-control-sm mascara-vin" type="text" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-auto pl-1">
                                            <button type="button" id="btnBuscarVIN" onclick="BuscarVIN()" class="btn btn-sm btn-secondary text-center"><i class="fas fa-search"></i></button>
                                        </div>
                                    </div>
                                </div>
                              <%--  <div class="col-auto pr-0 align-items-center">
                                    <div class="m-0 p-0">
                                        <div class="alert alert-info bg-info text-white mb-0 pt-1 pb-1" role="alert">
                                            <i class="fas fa-exclamation-circle text-white"></i>
                                            <strong>Estimado usuario,</strong> solo los campos con <strong>*</strong> son requeridos.
                                        </div>
                                    </div>
                                </div>--%>
                            </div>
                            <div class="row mb-0">
                                <!-- Información del cliente -->
                                <div class="col-lg-6">
                                    <h6 class="mb-1">Características físicas</h6>

                                    <div class="form-group row">
                                        <div class="col-12">
                                            <label class="col-form-label">VIN</label>
                                            <asp:TextBox ID="txtVIN" placeholder="EJ. JH4TB2H26CC000000" CssClass="form-control form-control-sm mascara-vin" type="text" runat="server" data-parsley-group="informacionGarantia"></asp:TextBox>
                                        </div>
                                        <div class="col-6">
                                            <label class="col-form-label">Tipo de garantía</label>
                                            <asp:TextBox ID="txtTipoDeGarantia" ReadOnly="true" required="required" CssClass="form-control form-control-sm" type="text" runat="server" data-parsley-group="informacionGarantia"></asp:TextBox>
                                        </div>
                                        <div class="col-6">
                                            <label class="col-form-label">Tipo de vehículo</label>
                                            <%--<asp:TextBox ID="txtTipoDeVehiculo" placeholder="EJ. Turismo" CssClass="form-control form-control-sm" type="text" runat="server" data-parsley-group="informacionGarantia"></asp:TextBox>--%>
                                            <asp:DropDownList ID="txtTipoDeVehiculo" runat="server" CssClass="form-control form-control-sm col-form-label" data-parsley-group="informacionGarantia"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <div class="col-sm-3">
                                            <label class="col-form-label">Marca <strong class="text-danger">*</strong></label>
                                            <asp:TextBox ID="txtMarca" placeholder="EJ. Honda" CssClass="form-control form-control-sm" type="text" required="required" runat="server" data-parsley-group="informacionGarantia"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label class="col-form-label">Modelo <strong class="text-danger">*</strong></label>
                                            <asp:TextBox ID="txtModelo" placeholder="EJ. Civic" CssClass="form-control form-control-sm" type="text" required="required" runat="server" data-parsley-group="informacionGarantia"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label class="col-form-label">Año <strong class="text-danger">*</strong></label>
                                            <asp:TextBox ID="txtAnio" CssClass="form-control form-control-sm mascara-enteros" type="text" required="required" runat="server" data-parsley-group="informacionGarantia"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label class="col-form-label">Color</label>
                                            <asp:TextBox ID="txtColor" CssClass="form-control form-control-sm" type="text" runat="server" data-parsley-group="informacionGarantia"></asp:TextBox>
                                        </div>
                                        <div class="col-12" style="display:none;">
                                            <label class="col-form-label">Matrícula</label>
                                            <asp:TextBox ID="txtMatricula" placeholder="EJ. AAA 9999" CssClass="form-control form-control-sm mascara-matricula" type="text" runat="server" data-parsley-group="informacionGarantia"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <!-- Información del préstamo máximo -->
                                <div class="col-lg-6 border-left border-gray">
                                    <h6 class="mb-1">Características mecánicas</h6>
                                    <div class="form-group row mb-4">
                                        <div class="col-sm-4">
                                            <label class="col-form-label">Cilindraje</label>
                                            <asp:TextBox ID="txtCilindraje" placeholder="EJ. 1.8" CssClass="form-control form-control-sm mascara-cilindraje" type="text" runat="server" data-parsley-group="informacionGarantia"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-4">
                                            <label class="col-form-label">Recorrido</label>
                                            <asp:TextBox ID="txtRecorrido" CssClass="form-control form-control-sm mascara-cantidad" type="text" runat="server" data-parsley-group="informacionGarantia"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-4">
                                            <label class="col-form-label">Unidad de medida</label>
                                            <asp:DropDownList ID="ddlUnidadDeMedida" runat="server" CssClass="form-control form-control-sm col-form-label" data-parsley-group="informacionGarantia"></asp:DropDownList>
                                        </div>
                                        <div class="col-sm-4">
                                            <label class="col-form-label">Transmisión</label>
                                            <%--<asp:TextBox ID="txtTransmision" placeholder="" CssClass="form-control form-control-sm" type="text" runat="server" data-parsley-group="informacionGarantia"></asp:TextBox>--%>
                                            <asp:DropDownList ID="txtTransmision" runat="server" CssClass="form-control form-control-sm col-form-label" data-parsley-group="informacionGarantia"></asp:DropDownList>
                                        </div>
                                        <div class="col-sm-4">
                                            <label class="col-form-label">Tipo de combustible</label>
                                            <%--<asp:TextBox ID="txtTipoDeCombustible" placeholder="" CssClass="form-control form-control-sm" type="text" runat="server" data-parsley-group="informacionGarantia"></asp:TextBox>--%>
                                            <asp:DropDownList ID="txtTipoDeCombustible" runat="server" CssClass="form-control form-control-sm col-form-label" data-parsley-group="informacionGarantia"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <h6 class="mb-1">Otros datos de la garantía</h6>
                                    <div class="form-group row">
                                        <div class="col-sm-4" style="display: none">
                                            <label class="col-form-label">Serie 1</label>
                                            <asp:TextBox ID="txtSerieUno" CssClass="form-control form-control-sm" type="text" runat="server" data-parsley-group="informacionGarantia"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-4">
                                            <label class="col-form-label">Serie motor</label>
                                            <asp:TextBox ID="txtSerieMotor" placeholder="" CssClass="form-control form-control-sm" type="text" runat="server" data-parsley-group="informacionGarantia"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-4">
                                            <label class="col-form-label">Serie chasis</label>
                                            <asp:TextBox ID="txtSerieChasis" placeholder="" CssClass="form-control form-control-sm" type="text" runat="server" data-parsley-group="informacionGarantia"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-4">
                                            <label class="col-form-label">Otra serie (Opcional)</label>
                                            <asp:TextBox ID="txtSerieDos" CssClass="form-control form-control-sm" type="text" runat="server" data-parsley-group="informacionGarantia"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-4">
                                            <label class="col-form-label">GPS</label>
                                            <asp:TextBox ID="txtGPS" CssClass="form-control form-control-sm" type="text" runat="server" data-parsley-group="informacionGarantia"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-lg-6">
                                    <h6 class="mb-1">Propietario de la garantía</h6>
                                    <div class="form-group row">
                                        <div class="col-sm-6">
                                            <label class="col-form-label">Identidad</label>
                                            <asp:TextBox ID="txtIdentidadPropietario" CssClass="form-control form-control-sm mascara-identidad" type="text" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-6">
                                            <label class="col-form-label">Nombre completo</label>
                                            <asp:TextBox ID="txtNombrePropietario" CssClass="form-control form-control-sm" type="text" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-6" style="display: none">
                                            <label class="col-form-label">Estado Civil</label>
                                            <asp:DropDownList ID="ddlEstadoCivilPropietario" runat="server" CssClass="form-control form-control-sm col-form-label buscadorddl" data-parsley-errors-container="#error-ddlEstadoCivilPropietario"></asp:DropDownList>
                                            <div id="error-ddlEstadoCivilPropietario"></div>
                                        </div>
                                        <div class="col-sm-6">
                                            <label class="col-form-label">Nacionalidad</label>
                                            <asp:DropDownList ID="ddlNacionalidadPropietario" runat="server" CssClass="form-control form-control-sm col-form-label buscadorddl" data-parsley-errors-container="#error-ddlNacionalidadPropietario"></asp:DropDownList>
                                            <div id="error-ddlNacionalidadPropietario"></div>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-lg-6 border-left border-gray">
                                    <h6 class="mb-1">Vendedor de la garantía</h6>
                                    <div class="form-group row">
                                        <div class="col-sm-6" >
                                            <label class="col-form-label">Identidad</label>
                                            <asp:TextBox ID="txtIdentidadVendedor" CssClass="form-control form-control-sm mascara-identidad" type="text" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-6">
                                            <label class="col-form-label">Nombre completo</label>
                                            <asp:TextBox ID="txtNombreVendedor" CssClass="form-control form-control-sm" type="text" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-6" style="display: none" >
                                            <label class="col-form-label">Estado Civil</label>
                                            <asp:DropDownList ID="ddlEstadoCivilVendedor" runat="server" CssClass="form-control form-control-sm col-form-label buscadorddl" data-parsley-errors-container="#error-ddlEstadoCivilVendedor"></asp:DropDownList>
                                            <div id="error-ddlEstadoCivilVendedor"></div>
                                        </div>
                                        <div class="col-sm-6">
                                            <label class="col-form-label">Nacionalidad</label>
                                            <asp:DropDownList ID="ddlNacionalidadVendedor" runat="server" CssClass="form-control form-control-sm col-form-label buscadorddl" data-parsley-errors-container="#error-ddlNacionalidadVendedor"></asp:DropDownList>
                                            <div id="error-ddlNacionalidadVendedor"></div>
                                        </div>
                                    </div>
                                </div>


                            </div>
                            <div class="row">
                                <div class="col-12">
                                    <h6 class="mb-0 border-top border-gray pt-2"></h6>
                                    <div class="form-group row">
                                        <div class="col-12">
                                            <label class="col-form-label">Comentario</label>
                                            <textarea id="txtComentario" runat="server" class="form-control form-control-sm" data-parsley-maxlength="300" rows="2"></textarea>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Información personal del cliente -->
                        <div id="step-2" class="form-section">

                            <div class="form-group row m-0 border-bottom border-gray">
                                <div class="col-12 p-0">
                                    <h6 class="mt-1">Información personal del cliente</h6>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-8 border-right border-gray">
                                    <h6 class="mt-3 mb-1">Información básica</h6>

                                    <div class="form-group row">
                                        <div class="col-md-4">
                                            <label class="col-form-label">Nacionalidad</label>
                                            <asp:DropDownList ID="ddlNacionalidad" runat="server" CssClass="form-control form-control-sm buscadorddl" required="required" data-parsley-group="informacionPersonal" data-parsley-errors-container="#error-ddlNacionalidad"></asp:DropDownList>
                                            <div id="error-ddlNacionalidad"></div>
                                        </div>
                                        <div class="col-md-4">
                                            <label class="col-form-label">Profesión</label>
                                            <asp:TextBox ID="txtProfesion" CssClass="form-control form-control-sm" type="text" required="required" data-parsley-group="informacionPersonal" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-md-4">
                                            <label class="col-form-label">Estado civil</label>
                                            <asp:DropDownList ID="ddlEstadoCivil" runat="server" CssClass="form-control form-control-sm buscadorddl" required="required" data-parsley-group="informacionPersonal" data-parsley-errors-container="#error-ddlEstadoCivil"></asp:DropDownList>
                                            <div id="error-ddlEstadoCivil"></div>
                                        </div>
                                        <%--<div class="col-md-3">
                                        <label class="col-form-label">Tipo de cliente</label>
                                        <asp:DropDownList ID="ddlTipoDeCliente" runat="server" CssClass="form-control form-control-sm" required="required" data-parsley-group="informacionPersonal" data-parsley-errors-container="#error-ddlTipoDeCliente"></asp:DropDownList>
                                        <div id="error-ddlTipoDeCliente"></div>
                                    </div>--%>
                                    </div>
                                    <div class="form-group row">
                                        <div class="col-md-3">
                                            <label class="col-form-label">Fecha de nacimiento</label>
                                            <asp:TextBox ID="txtFechaDeNacimiento" CssClass="form-control form-control-sm" type="date" Enabled="false" data-parsley-group="informacionPersonal" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3">
                                            <label class="col-form-label">Edad del cliente</label>
                                            <asp:TextBox ID="txtEdadDelCliente" CssClass="form-control form-control-sm" Enabled="false" type="text" required="required" data-parsley-group="informacionPersonal" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-md-6">
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
                                <div class="col-md-4">
                                    <h6 class="mt-3 mb-1">Información de contacto</h6>

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
                                </div>
                            </div>
                        </div>

                        <!-- Información del domicilio del cliente -->
                        <div id="step-3" class="form-section">

                            <div class="row justify-content-between m-0 border-bottom border-gray">
                                <div class="col-auto">
                                    <div class="form-group row align-items-center mb-1">
                                        <h6 class="mt-1">Información del domicilio</h6>
                                    </div>
                                </div>
                                <div class="col-auto pr-0 align-items-center" runat="server" id="divInformacionPreSolicitud_Domicilio" visible="false">
                                    <div class="m-0 p-0">
                                        <div class="alert alert-info bg-info text-white mb-0 pt-1 pb-1" role="alert">
                                            <i class="fas fa-exclamation-circle text-white"></i>
                                            Parte de la información <strong>ha sido extraída de la presolicitud</strong>.
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-8 border-right border-gray">
                                    <div class="form-group row">
                                        <div class="col-sm-8">
                                            <label class="col-form-label">Codigo Postal</label>
                                            <asp:DropDownList ID="ddlBarrioColoniaDomicilio" runat="server" CssClass="form-control form-control-sm buscadorddl"  required="required" data-parsley-group="informacionDomicilio" data-parsley-errors-container="#error-ddlBarrioColoniaDomicilio"></asp:DropDownList>
                                            <div id="error-ddlBarrioColoniaDomicilio"></div>
                                        </div>
                                     <div class="col-sm-3">
                                            <label class="col-form-label">Teléfono casa</label>
                                            <asp:TextBox ID="txtTelefonoCasa" CssClass="form-control form-control-sm mascara-telefono" type="text" data-parsley-group="informacionDomicilio" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-4">
                                    <h6 class="mb-1 pt-0">Información de la vivienda</h6>
                                    <div class="form-group row">
                                        <div class="col-sm-6">
                                            <label class="col-form-label">Tipo de Vivienda</label>
                                            <asp:DropDownList ID="ddlTipoDeVivienda" runat="server" CssClass="form-control form-control-sm buscadorddl" required="required" data-parsley-group="informacionDomicilio" data-parsley-errors-container="#error-ddlTipoDeVivienda"></asp:DropDownList>
                                            <div id="error-ddlTipoDeVivienda"></div>
                                        </div>
                                        <div class="col-sm-6">
                                            <label class="col-form-label">Tiempo de residir</label>
                                            <asp:DropDownList ID="ddlTiempoDeResidir" runat="server" CssClass="form-control form-control-sm buscadorddl" required="required" data-parsley-group="informacionDomicilio" data-parsley-errors-container="#error-ddlTiempoDeResidir"></asp:DropDownList>
                                            <div id="error-ddlTiempoDeResidir"></div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-12">

                                    <h6 class="border-top border-gray"></h6>

                                    <div class="form-group row">
                                     
                                        <div class="col-sm-9" style="display:none">
                                            <label class="col-form-label">Dirección detallada del domicilio</label>
                                            <asp:TextBox ID="txtDireccionDetalladaDomicilio" CssClass="form-control form-control-sm" placeholder="# calle, # avenida, bloque, pasaje, etc" type="text"   runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <div class="col-12">
                                            <label class="col-form-label">Dirección del domicilio </label>
                                            <textarea id="txtReferenciasDelDomicilio" runat="server" required="required" class="form-control form-control-sm" data-parsley-maxlength="255" data-parsley-minlength="5" rows="2" data-parsley-group="informacionDomicilio"></textarea>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        
                        </div>

                        <!-- Información laboral del cliente -->
                        <div id="step-4" class="form-section">

                            <div class="row justify-content-between m-0 border-bottom border-gray">
                                <div class="col-auto">
                                    <div class="form-group row align-items-center mb-0">
                                        <h6 class="mt-1">Información laboral</h6>
                                    </div>
                                </div>
                                <div class="col-auto pr-0 align-items-center" runat="server" id="divInformacionPreSolicitud_Trabajo" visible="false">
                                    <div class="m-0 p-0">
                                        <div class="alert alert-info bg-info text-white mb-0 pt-1 pb-1" role="alert">
                                            <i class="fas fa-exclamation-circle text-white"></i>
                                            Parte de la información <strong>ha sido extraída de la presolicitud</strong>.
                                        </div>
                                    </div>
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
                                        <div class="col-sm-3"  style =" display:none">
                                            <label class="col-form-label">Extensión RRHH</label>
                                            <asp:TextBox ID="txtExtensionRecursosHumanos" CssClass="form-control form-control-sm mascara-extension" type="text" data-parsley-group="informacionLaboral" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3" style =" display:none">
                                            <label class="col-form-label" >Extensión Cliente</label>
                                            <asp:TextBox ID="txtExtensionCliente" CssClass="form-control form-control-sm mascara-extension" type="text" data-parsley-group="informacionLaboral" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-4" style =" display:none">
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
                                    <div class="col-sm-8">
                                            <label class="col-form-label">Codigo Postal</label>
                                            <asp:DropDownList ID="ddlBarrioColoniaEmpresa" runat="server" CssClass="form-control form-control-sm buscadorddl"  required="required" data-parsley-group="informacionLaboral" data-parsley-errors-container="#error-ddlBarrioColoniaEmpresa"></asp:DropDownList>
                                            <div id="error-ddlBarrioColoniaEmpresa"></div>
                                        </div>
                            <%--    <div class="col-sm-3">
                                    <label class="col-form-label">Estado</label>
                                    <asp:DropDownList ID="ddlDepartamentoEmpresa" runat="server" CssClass="form-control form-control-sm buscadorddl" required="required" data-parsley-group="informacionLaboral" data-parsley-errors-container="#error-ddlDepartamentoEmpresa"></asp:DropDownList>
                                    <div id="error-ddlDepartamentoEmpresa"></div>
                                </div>
                                <div class="col-sm-3">
                                    <label class="col-form-label">Condado</label>
                                    <asp:DropDownList ID="ddlMunicipioEmpresa" runat="server" CssClass="form-control form-control-sm buscadorddl" Enabled="true" required="required" data-parsley-group="informacionLaboral" data-parsley-errors-container="#error-ddlMunicipioEmpresa"></asp:DropDownList>
                                    <div id="error-ddlMunicipioEmpresa"></div>
                                </div>
                                <div class="col-sm-3">
                                    <label class="col-form-label">Ciudad</label>
                                    <asp:DropDownList ID="ddlCiudadPobladoEmpresa" runat="server" CssClass="form-control form-control-sm buscadorddl" Enabled="true" required="required" data-parsley-group="informacionLaboral" data-parsley-errors-container="#error-ddlCiudadPobladoEmpresa"></asp:DropDownList>
                                    <div id="error-ddlCiudadPobladoEmpresa"></div>
                                </div>
                                <div class="col-sm-3">
                                    <label class="col-form-label">Codigo Postal</label>
                                    <asp:DropDownList ID="ddlBarrioColoniaEmpresa" runat="server" CssClass="form-control form-control-sm buscadorddl" required="required" data-parsley-group="informacionLaboral" data-parsley-errors-container="#error-ddlBarrioColoniaEmpresa"></asp:DropDownList>
                                    <div id="error-ddlBarrioColoniaEmpresa"></div>
                                </div>--%>
                            </div>

                            <div class="form-group row">
                                <div class="col-12" style =" display:none">
                                    <label class="col-form-label">Dirección detallada del trabajo</label>
                                    <asp:TextBox ID="txtDireccionDetalladaEmpresa" CssClass="form-control form-control-sm" placeholder="# calle, # avenida, bloque, pasaje, etc" type="text"   runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group row">
                                <div class="col-12">
                                    <label class="col-form-label">Observaciones Adicionales del empleo</label>
                                    <textarea id="txtReferenciasEmpresa" runat="server" required="required" class="form-control form-control-sm" data-parsley-maxlength="255" data-parsley-minlength="5" rows="2" data-parsley-group="informacionLaboral"></textarea>
                                </div>
                            </div>
                        </div>

                        <!-- Información del conyugue -->
                        <div id="step-5" class="form-section">

                            <div class="form-group row m-0 border-bottom border-gray">
                                <div class="col-12 p-0">
                                    <h6 class="mt-1">Información del cónyugue</h6>
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

                              <div class="col-sm-6">
                                <div class="col-12 p-0">
                                    <h6 class="mt-1">Referencias ingresadas en la aplicacion</h6>
                                </div>
                                <div class="col- p-0" >
                                      <div class="table-responsive">
                                   <table class="table table-sm table-bordered table-hover cursor-pointer">
                                    <thead>
                                        <tr>
                                            <th>Nombre</th>
                                            <th>Telefono</th>
                                            <th>Parentesco</th>
                                        </tr>
                                    </thead>
                                    <tbody id="BodyReferenciasDeAplicacion">
                                    </tbody>
                                </table>
                               </div>
                                </div>
                            </div>

                            <div class="col-sm-12">
                                  <div class="form-group row m-0 border-bottom border-gray">
                                <div class="col-12 p-0">
                                    <h6 class="mt-1">Referencias personales para la solicitud</h6>
                                </div>
                            </div>
                            <div class="mt-2 header-title">
                                <label class="col-form-label">Mínimo 4 referencias personales.<small class="text-info">(Consejo: Si te equivocas en el dato de una referencia, quítala y vuleve a agregarla.)</small></label>
                                <button id="btnNuevaReferencia" type="button" class="btn btn-info waves-effect waves-light float-right">
                                    Agregar referencia
                                </button>
                            </div>
                            </div>
                          
                          
                            <div class="col-sm-12">
                               <div class="table-responsive">
                                <table class="table table-sm table-bordered table-hover cursor-pointer" id="tblReferenciasPersonales">
                                    <thead>
                                        <tr>
                                            <th>Nombre completo</th>
                                            <th>Telefono</th>
                                            <%--<th>Lugar de trabajo</th>--%>
                                          <%--  <th>Tiempo de conocer</th>--%>
                                            <th>Parentesco</th>
                                            <th>Quitar</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                    </tbody>
                                </table>
                               </div>
                            </div>
                           
                        </div>

                
                        <!-- Documentación de la solicitud -->
                        <div id="step-7" class="form-section">
                            <div class="form-group row m-0 border-bottom border-gray">
                                <div class="col-12 p-0">
                                    <h6 class="mt-1">Documentación de la solicitud <small class="text-info">(Estimado usuario, recuerda subir toda la documentación hasta que ya vayas a guardar la solicitud)</small></h6>
                                </div>
                            </div>
                                <!--<div class="col-12 text-center">
                                <br />
                                <br />
                                <asp:HyperLink ID="btnLinkDocumentos" runat="server" target="_blank" class="text-info text-lg"> <h4>Ir a Verificacion de Documentos</h4></asp:HyperLink>
                            </div>-->
                            <!-- Div donde se generan dinamicamente los inputs para la documentación -->
                        <!-- Div donde se generan dinamicamente los inputs para la documentación -->
                        <div class="row pr-1 pl-1 text-center" id="DivDocumentacion">
                        </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="modalAgregarReferenciaPersonal" tabindex="-1" role="dialog" aria-labelledby="modalAgregarReferenciaPersonalLabel" aria-hidden="true">
            <div class="modal-dialog" role="document">
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
                        <div class="form-group row" style="display:none">
                            <label class="col-sm-4 col-form-label">Tiempo de conocer</label>
                            <div class="col-sm-8">
                                <asp:DropDownList ID="ddlTiempoDeConocerReferencia" runat="server" CssClass="form-control form-control-sm"  data-parsley-errors-container="#error-ddlTiempoDeConocerReferencia"></asp:DropDownList>
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
                        <div class="form-group row" style="display: none;">
                            <label class="col-sm-4 col-form-label">Lugar de trabajo</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtLugarTrabajoReferencia" CssClass="form-control form-control-sm" type="text" Enabled="false" data-parsley-group="referenciasPersonales" runat="server"></asp:TextBox>
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

        <div id="modalCambiarScore" class="modal fade" role="dialog" aria-labelledby="modalCambiarScoreLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h6 class="modal-title mt-0" id="modalCambiarScoreLabel">Solicitar cambio de SCORE</h6>
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    </div>
                    <div class="modal-body">

                        <div class="alert alert-info bg-info text-white" role="alert">
                            <i class="fas fa-exclamation-circle text-white"></i>
                            <strong>Se enviará un correo</strong> al departamento de <strong>crédito</strong> solicitando el cambio de SCORE de este cliente.
                            <br />
                            Al recibir la <strong>respuesta</strong> de dicha solicitud, <strong>actualiza la pantalla</strong>.
                        </div>
                        <div class="form-group row mb-0">
                            <div class="col-sm-3">
                                <label class="col-form-label font-weight-bold">Nuevo score</label>
                            </div>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtNuevoScore" CssClass="form-control form-control-sm mascara-score" type="text" required="required" data-parsley-group="cambiarScore" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-12">
                                <label class="col-form-label">Comentario adicional</label>
                                <textarea id="txtCambiarScoreComentarioAdicional" runat="server" class="form-control form-control-sm" data-parsley-group="cambiarScore" data-parsley-maxlength="150" rows="2"></textarea>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button id="btnSolicitarCambioScore" type="button" class="btn btn-primary waves-effect waves-light">
                            Enviar
                        </button>
                        <button type="reset" data-dismiss="modal" class="btn btn-secondary waves-effect">
                            Cancelar
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <div id="modalSeleccionarPrecioDeMercado" class="modal fade" role="dialog" aria-labelledby="modalSeleccionarPrecioDeMercadoLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h6 class="modal-title mt-0" id="modalSeleccionarPrecioDeMercadoLabel">Seleccionar precio de mercado</h6>
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    </div>
                    <div class="modal-body">

                        <div class="alert alert-info bg-info text-white text-justify" role="alert">
                            <i class="fas fa-exclamation-circle text-white"></i>
                            <strong>Seleccionar precio de mercado</strong> de la garantía.
                            <br />
                            Si la garantía que buscas no está registrada en el sistema o no tiene un precio de mercado asignado, <b>crea la solicitud de precio de mercado </b>para ser <b>validada</b> por el departamento de crédito.
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-6">
                                <label class="col-form-label" for="ddlMarca">Marca</label>
                                <button type="button" id="btnAgregarMarca" class="btn btn-sm btn-secondary pt-1 pb-1 float-right" title="Agregar marca">
                                    <i class="fa fa-plus"></i>
                                </button>
                                <asp:DropDownList ID="ddlMarca" runat="server" CssClass="form-control form-control-sm buscadorddl" data-parsley-group="seleccionarPrecioDeMercado" required="required" data-parsley-errors-container="#error-ddlMarca"></asp:DropDownList>
                                <div id="error-ddlMarca"></div>
                            </div>
                            <div class="col-sm-6">
                                <label class="col-form-label">Modelo - Versión</label>
                                <button type="button" id="btnAgregarModelo" class="btn btn-sm btn-secondary pt-1 pb-1 float-right" title="Agregar modelo" disabled="disabled">
                                    <i class="fa fa-plus"></i>
                                </button>
                                <asp:DropDownList ID="ddlModelo" Enabled="false" runat="server" CssClass="form-control form-control-sm buscadorddl" data-parsley-group="seleccionarPrecioDeMercado" required="required" data-parsley-errors-container="#error-ddlModelo"></asp:DropDownList>
                                <div id="error-ddlModelo"></div>
                            </div>
                            <div class="col-sm-6">
                                <label class="col-form-label">Año</label>
                                <button type="button" id="btnAgregarModeloAnio" class="btn btn-sm btn-secondary pt-1 pb-1 float-right mt-1" title="Agregar año" disabled="disabled">
                                    <i class="fa fa-plus"></i>
                                </button>
                                <asp:DropDownList ID="ddlAnio" Enabled="false" runat="server" CssClass="form-control form-control-sm buscadorddl" data-parsley-group="seleccionarPrecioDeMercado" required="required" data-parsley-errors-container="#error-ddlAnio"></asp:DropDownList>
                                <div id="error-ddlAnio"></div>
                            </div>
                            <div class="col-sm-6">
                                <label class="col-form-label">Precio de mercado actual</label>
                                <asp:TextBox ID="txtPrecioDeMercadoActual" CssClass="form-control form-control-sm mascara-cantidad" type="text" ReadOnly="true" required="required" data-parsley-group="seleccionarPrecioDeMercado" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-6 text-center mt-3">
                                <button type="button" id="btnSeleccionarPrecioDeMercadoConfirmar" class="btn btn-block btn-secondary" title="Seleccionar precio" disabled="disabled">
                                    <i class="far fa-check-circle"></i>
                                    Seleccionar precio
                                </button>
                            </div>
                            <div class="col-6 text-center mt-3">
                                <button type="button" id="btnSolicitarPrecioDeMercado" class="btn btn-block btn-secondary" title="Solicitar reevaluación" disabled="disabled">
                                    <i class="far fa-edit"></i>
                                    Solicitar precio de mercado
                                </button>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="reset" data-dismiss="modal" class="btn btn-secondary waves-effect">
                            Cancelar
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <div id="modalSolicitarPrecioDeMercado" class="modal fade" role="dialog" aria-labelledby="modalSolicitarPrecioDeMercadoLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header pb-2">
                        <div class="form-group row mb-0">
                            <div class="col-12">
                                <h6 class="m-0" id="modalSolicitarPrecioDeMercadoLabel">Solicitar precio de mercado</h6>
                            </div>
                            <div class="col-12 text-muted font-weight-bold">
                                Marca:
                            <span class="lblMarca"></span>
                                / Modelo:
                            <span class="lblModelo"></span>
                                / Año:
                            <span class="lblAnio"></span>
                            </div>
                        </div>
                    </div>
                    <div class="modal-body">

                        <div class="alert alert-info bg-info text-white" role="alert">
                            <i class="fas fa-exclamation-circle text-white"></i>
                            <strong>Solicitar precio de mercado</strong>.
                            <br />
                            La solicitud de precio de mercado será <b>recibida y validad por el departamento de crédito</b>.
                        </div>
                        <div class="form-group row">
                            <div class="col-12">
                                <label class="col-form-label">Precio solicitado</label>
                                <asp:TextBox ID="txtPrecioDeMercadoSolicitado" CssClass="form-control form-control-sm mascara-cantidad" type="text" required="required" data-parsley-group="solicitarPrecioDeMercado" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-12">
                                <label class="col-form-label">Comentarios para el depto. de crédito</label>
                                <textarea id="txtComentarioSolicitarPrecioDeMercado" runat="server" class="form-control form-control-sm" data-parsley-group="solicitarPrecioDeMercado" data-parsley-maxlength="500" rows="4"></textarea>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" id="btnSolicitarPrecioDeMercadoConfirmar" class="btn btn-secondary">
                            Confirmar
                        </button>
                        <button type="reset" data-dismiss="modal" class="btn btn-secondary">
                            Cancelar
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <div id="modalAgregarMarcaPrecioMercado" class="modal fade" role="dialog" aria-labelledby="modalAgregarMarcaPrecioMercadoLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header pb-2">
                        <div class="form-group row mb-0">
                            <div class="col-12">
                                <h6 class="m-0" id="modalAgregarMarcaPrecioMercadoLabel">Agregar nueva marca</h6>
                            </div>
                        </div>
                    </div>
                    <div class="modal-body">
                        <div class="form-group row">
                            <div class="col-12">
                                <label class="col-form-label">Descripción</label>
                                <asp:TextBox ID="txtMarcaAgregar" CssClass="form-control form-control-sm" type="text" required="required" data-parsley-maxlength="150" data-parsley-group="agregarMarca" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" id="btnAgregarMarcaConfirmar" class="btn btn-secondary">
                            Confirmar
                        </button>
                        <button type="button" id="btnAgregarMarcaCancelar" class="btn btn-secondary">
                            Cancelar y volver
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <div id="modalAgregarModelosPrecioMercado" class="modal fade" role="dialog" aria-labelledby="modalAgregarModelosPrecioMercadoLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header pb-2">
                        <div class="form-group row mb-0">
                            <div class="col-12">
                                <h6 class="m-0" id="modalAgregarModelosPrecioMercadoLabel">Agregar nuevo modelo</h6>
                            </div>
                            <div class="col-12 text-muted font-weight-bold">
                                Marca:
                                <span class="lblMarca"></span>
                            </div>
                        </div>
                    </div>
                    <div class="modal-body">
                        <div class="form-group row">
                            <div class="col-6">
                                <label class="col-form-label">Modelo</label>
                                <asp:TextBox ID="txtModeloAgregar" CssClass="form-control form-control-sm" type="text" required="required" data-parsley-maxlength="100" data-parsley-group="agregarModelo" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-6">
                                <label class="col-form-label">Versión</label>
                                <asp:TextBox ID="txtVersionAgregar" CssClass="form-control form-control-sm" type="text" data-parsley-maxlength="20" data-parsley-group="agregarModelo" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" id="btnAgregarModeloConfirmar" class="btn btn-secondary">
                            Confirmar
                        </button>
                        <button type="button" id="btnAgregarModeloCancelar" class="btn btn-secondary">
                            Cancelar y volver
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <div id="modalAgregarAnioModeloPrecioMercado" class="modal fade" role="dialog" aria-labelledby="modalAgregarAnioModeloPrecioMercadoLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header pb-2">
                        <div class="form-group row mb-0">
                            <div class="col-12">
                                <h6 class="m-0" id="modalAgregarAnioModeloPrecioMercadoLabel">Agregar nuevo año del modelo seleccionado</h6>
                            </div>
                            <div class="col-12 text-muted font-weight-bold">
                                Marca:
                                <span class="lblMarca"></span>
                                / Modelo:
                                <span class="lblModelo"></span>
                                <%--/ Año:
                                <span class="lblAnio"></span>--%>
                            </div>
                        </div>
                    </div>
                    <div class="modal-body">
                        <div class="form-group row">
                            <div class="col-12">
                                <label class="col-form-label">Año</label>
                                <asp:DropDownList ID="ddlAnioAgregarAnioModelo" runat="server" CssClass="form-control form-control-sm buscadorddl" data-parsley-group="seleccionarPrecioDeMercado" required="required" data-parsley-errors-container="#error-ddlAnioAgregarAnioModelo"></asp:DropDownList>
                                <div id="error-ddlAnioAgregarAnioModelo"></div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" id="btnAgregarAnioModeloConfirmar" class="btn btn-secondary">
                            Confirmar
                        </button>
                        <button type="button" id="btnAgregarAnioModeloCancelar" class="btn btn-secondary">
                            Cancelar y volver
                        </button>
                    </div>
                </div>
            </div>
        </div>


          <div id="modalConfirmarAval" class="modal fade" role="dialog" aria-labelledby="modalConfirmarAval" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header pb-2">
                        <div class="form-group row mb-0">
                            <div class="col-12">
                                <h6 class="m-0" id="modalConfirmarAval">Desea registrar Aval?</h6>
                            </div>
                       
                        </div>
                    </div>
                
                    <div class="modal-footer">
                        <button type="button" id="btnAgregarAvalModelConfirmar"  class="btn btn-secondary">
                            Si
                        </button>
                        <%--<asp:button   type="text" ID="btnRegistrarAval" runat="server" Text="Si" class="btn btn-secondary" OnClick="btnRegistrarAval_Click" />--%>
                        <button type="button" id="btnAgregarAvalModelCancelar" class="btn btn-secondary">
                            No
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
        const CONSTANTES = <%=this.jsonConstantes%>;
        const PRECALIFICADO = <%=this.jsonPrecalicado%>;
        const numeroPestanaInformacionGarantia = CONSTANTES.RequiereGarantia;
    </script>
    <script>
        $(document).ready(function () {
            //BuscarVIN();
            $("#ddlDepartamentoDomicilio").trigger("change");
            $("#ddlDepartamentoEmpresa").trigger("change");

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
            $(".mascara-telefono").inputmask("999-999-9999");
           // $(".mascara-extension").inputmask("999999");
          //  $(".mascara-identidad").inputmask("999-99-9999");
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
            $(".mascara-score").inputmask("decimal", {
                alias: "numeric",
                groupSeparator: ",",
                digits: 0,
                integerDigits: 3,
                digitsOptional: false,
                placeholder: "0",
                radixPoint: ".",
                autoGroup: true,
                min: 0.0,
            });
        });
    </script>
    <script src="/Scripts/plugins/steps/js/jquery.smartWizard.js"></script>
    <script src="/Scripts/plugins/iziToast/js/iziToast.min.js"></script>
    <script src="/Scripts/plugins/parsleyjs/parsley.js"></script>
    <script src="/Scripts/app/uploader/js/jquery.fileuploader.min.js"></script>
    <script src="/Scripts/plugins/select2/js/select2.full.min.js"></script>
    <script src="/Scripts/plugins/sweet-alert2/sweetalert2.min.js"></script>
    <script src="/Scripts/app/solicitudes/SolicitudesCredito_Registrar.js?v=20210118015352"></script>
    <!-- <script src="/Scripts/app/solicitudes/SolicitudesCredito_Registrar_SeleccionarPrecioDeMercado.js?v=20210118015352"></script> -->
</body>
</html>
