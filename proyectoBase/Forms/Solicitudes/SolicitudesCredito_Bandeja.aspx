<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SolicitudesCredito_Bandeja.aspx.cs" Inherits="SolicitudesCredito_Bandeja" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <meta name="description" content="Bandeja de solicitudes de crédito" />
    <title>Bandeja de solicitudes</title>
    <link href="/CSS/Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/CSS/Content/css/icons.css" rel="stylesheet" />
    <link href="/CSS/Content/css/style.css?v=202010031105" rel="stylesheet" />
    <link href="/Scripts/plugins/datatables/dataTables.bootstrap4.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/datatables/buttons.bootstrap4.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/datatables/responsive.bootstrap4.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/iziToast/css/iziToast.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/select2/css/select2.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/datapicker/datepicker3.css" rel="stylesheet" />
    <link href="/Scripts/plugins/unitegallery/css/unitegallery.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/unitegallery/themes/default/ug-theme-default.css" rel="stylesheet" />
    <link href="/CSS/Estilos_CSS.css" rel="stylesheet" />
    <style>


        .animacion {
       /*position: absolute;*/

                  animation-name: parpadeo;
                  animation-duration: 1s;
                  animation-timing-function: linear;
                  animation-iteration-count: infinite;

                  -webkit-animation-name:parpadeo;
                  -webkit-animation-duration: 1s;
                  -webkit-animation-timing-function: linear;
                  -webkit-animation-iteration-count: infinite;
                }

                @-moz-keyframes parpadeo{  
                  0% { opacity: 1.0; }
                  50% { opacity: 0.0; }
                  100% { opacity: 1.0; }
                }

                @-webkit-keyframes parpadeo {  
                  0% { opacity: 1.0; }
                  50% { opacity: 0.0; }
                   100% { opacity: 1.0; }
                }

                @keyframes parpadeo {  
                  0% { opacity: 1.0; }
                   50% { opacity: 0.0; }
                  100% { opacity: 1.0; }
                }



        .cursor-pointer {
            cursor: pointer;
        }


        #datatable-bandeja tbody tr {
            cursor: pointer;
        }

        #datatable-bandeja tbody td {
            outline: none;
            padding-top: 0 !important;
            padding-bottom: 0 !important;
        }

        div.dt-buttons {
            position: relative;
            float: left;
        }

        #divContenedor_datatableButtons div .dropdown-menu {
            overflow-y: auto;
            max-height: 300px !important;
        }

        table.datatable-cells-responsive tbody td.td-responsive {
            max-width: 200px;
            overflow: hidden;
            text-overflow: ellipsis;
            white-space: nowrap;
        }

            table.datatable-cells-responsive tbody td.td-responsive:hover {
                overflow: visible;
                white-space: inherit;
            }
    </style>
</head>
<body class="EstiloBody-Listado">
    <div class="card">
        <div class="card-header">
            <div class="row justify-content-between">
                <div class="col-auto">
                    <h6>Bandeja general de solicitudes</h6>
                </div>
                <div class="col-4">
                    <input id="txtDatatableFilter" class="float-right form-control" type="text" placeholder="Buscar" aria-label="Buscar" />
                </div>
            </div>
        </div>
        <div class="card-body">
            <div class="table-responsive p-0 mb-1">
                <div class="btn-group btn-group-toggle col-lg-12 p-0" data-toggle="buttons">
                    <label class="btn btn-secondary active cursor-pointer">
                        <input id="general" type="radio" name="filtros" value="0" />
                        General
                    </label>
                    <label class="btn btn-secondary cursor-pointer">
                        <input id="recepcion" type="radio" name="filtros" value="6" />
                        En Recepción
                    </label>
                    <label class="btn btn-secondary cursor-pointer">
                        <input id="analisis" type="radio" name="filtros" value="7" />
                        En Análisis
                    </label>
                    <label class="btn btn-secondary cursor-pointer">
                        <input id="campo" type="radio" name="filtros" value="8" />
                        En Campo
                    </label>
                    <label class="btn btn-secondary cursor-pointer">
                        <input id="condicionadas" type="radio" name="filtros" value="9" />
                        Condicionadas
                    </label>
                    <label class="btn btn-secondary cursor-pointer">
                        <input id="reprogramadas" type="radio" name="filtros" value="10" />
                        Incompleta
                    </label>
                    <label class="btn btn-secondary cursor-pointer">
                        <input id="validacion" type="radio" name="filtros" value="11" />
                        Validación
                    </label>
                    <label class="btn btn-warning cursor-pointer">
                        <input id="pendientes" type="radio" name="filtros" value="12" />
                        Pendientes
                    </label>
                    <label class="btn btn-success cursor-pointer">
                        <input id="aprobadas" type="radio" name="filtros" value="13" />
                        Aprobadas
                    </label>
                    <label class="btn btn-danger cursor-pointer">
                        <input id="rechazadas" type="radio" name="filtros" value="14" />
                        Rechazadas
                    </label>
                </div>
            </div>

            <div class="row mb-1 justify-content-center">
                <div class="col-lg-3 col-md-3 col-sm-6 col-6 align-self-end">
                    <div id="divContenedor_datatableButtons"></div>
                </div>
                <div class="col-lg-3 col-md-3 col-sm-6 col-6 align-self-end">
                    <label class="col-form-label">Búsqueda por Mes</label>
                    <select id="ddlMesIngreso" class="form-control form-control-sm">
                        <option value="">Seleccionar</option>
                        <option value="01">Enero</option>
                        <option value="02">Febrero</option>
                        <option value="03">Marzo</option>
                        <option value="04">Abril</option>
                        <option value="05">Mayo</option>
                        <option value="06">Junio</option>
                        <option value="07">Julio</option>
                        <option value="08">Agosto</option>
                        <option value="09">Septiembre</option>
                        <option value="10">Octubre</option>
                        <option value="11">Noviembre</option>
                        <option value="12">Diciembre</option>
                    </select>
                </div>
                <div class="col-lg-3 col-md-3 col-sm-6 col-6">
                    <label class="col-form-label">Búsqueda por Fecha</label>
                    <div class="input-daterange input-group" id="date-range">
                        <input type="text" class="form-control form-control-sm" name="fecha-minima" id="fecha-minima" />
                        <input type="text" class="form-control form-control-sm" name="fecha-maxima" id="fecha-maxima" />
                    </div>
                </div>
                <div class="col-lg-3 col-md-3 col-sm-6 col-6 align-self-end">
                    <label class="col-form-label">Búsqueda por Año</label>
                    <input id="ddlAnioIngreso" class="form-control form-control-sm" type="text" />
                </div>
            </div>

            <div class="table-responsive">
                <table id="datatable-bandeja" class="table table-bordered table-sm table-hover dataTable datatable-cells-responsive" style="width: 100%" role="grid">
                    <thead>
                        <tr>
                            <th><span class="pt-2 pb-2">Acciones</span></th>
                            <th>Solicitud de crédito</th>
                            <th>Producto / CC / Vendedor</th>
                            <th>Cliente / Identidad</th>
                            <th>Marca / Modelo / Año / VIN</th>
                            <th>Ingreso</th>
                            <th>Recepc.</th>
                            <th>Anális.</th>
                            <th>Campo</th>
                            <th>Condic.</th>
                            <th>Incom.</th>
                            <th>Valida.</th>
                            <th>Resolución</th>
                        </tr>
                    </thead>
                    <tbody></tbody>
                    <tfoot></tfoot>
                </table>
            </div>
        </div>
    </div>

    <!-- modal de documentos de la garantía -->
    <div id="modalDocumentosDeLaGarantia" class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="modalDocumentosDeLaGarantiaLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header pb-2">
                    <div class="form-group row mb-0">
                        <div class="col-12">
                            <h6 class="m-0" id="modalDocumentosDeLaGarantiaLabel">Fotografías de la garantía | Solicitud de crédito N° <b class="lblNoSolicitudCredito"></b></h6>
                        </div>
                        <div class="col-12 text-muted">
                            Marca:
                            <span class="lblMarca"></span>
                            / Modelo:
                            <span class="lblModelo"></span>
                            / Año:
                            <span class="lblAnio"></span>
                            / Color:
                            <span class="lblColor"></span>
                            / VIN:
                            <span class="lblVIN"></span>
                        </div>
                    </div>
                </div>
                <div class="modal-body">
                    <div class="form-group row">
                        <div class="col-12">
                            <!-- Div donde se muestran las fotografías de la garantía -->
                            <div class="align-self-center" id="divGaleriaGarantia" runat="server" style="display: none;"></div>
                        </div>
                    </div>
                    <div class="row mt-3 mb-0">
                        <div class="col-12">
                            <div class="table-responsive">
                                <table class="table table-sm" style="margin-bottom: 0px;">
                                    <tbody>
                                        <tr>
                                            <td><span class="font-weight-bold text-muted">Valor del mercado</span></td>
                                            <td><span class="label label-table label-success" id="lblValorMercadoGarantia"></span></td>
                                            <td></td>
                                            <td><span class="font-weight-bold text-muted">Valor de la prima</span></td>
                                            <td id="lblValorPrima"></td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td><span class="font-weight-bold text-muted">Valor a Prestar</span></td>
                                            <td id="lblValorAPrestar"></td>
                                            <td></td>
                                            <td><span class="font-weight-bold text-muted">Valor a Financiar</span></td>
                                            <td id="lblValorAFinanciar"></td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td colspan="6"></td>
                                        </tr>
                                    </tbody>
                                </table>
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

    <div id="modalDocumentosGarantiaSolicitud" class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="modalDocumentosGarantiaSolicitudLabel" aria-hidden="true">
        <div class="modal-dialog modal-xl">
            <div class="modal-content">
                <div class="modal-header pb-2">
                    <div class="form-group row mb-0">
                        <div class="col-12">
                            <h6 class="m-0" id="modalDocumentosGarantiaSolicitudLabel">Expediente Solicitud - Garantía | Solicitud de crédito N° <b class="lblNoSolicitudCredito"></b></h6>
                        </div>
                        <div class="col-12 text-muted">
                            Marca:
                            <span class="lblMarca"></span>
                            / Modelo:
                            <span class="lblModelo"></span>
                            / Año:
                            <span class="lblAnio"></span>
                            / VIN:
                            <span class="lblVIN"></span>
                        </div>
                    </div>
                </div>
                <div class="modal-body w-100 h-100 pt-0">
                    <div class="row mt-3">
                        <div class="col-6">
                            <div class="table-responsive">
                                <table class="table table-sm" style="margin-bottom: 0px;">
                                    <tbody>
                                        <tr>
                                            <td><span class="font-weight-bold">Información del cliente</span></td>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td><span class="font-weight-bold text-muted">Cliente</span></td>
                                            <td><span class="label label-table label-success lblNombreCliente"></span></td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td><span class="font-weight-bold text-muted">Identidad</span></td>
                                            <td class="lblIdentidadCliente"></td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                        <div class="col-6">
                            <div class="table-responsive">
                                <table class="table table-sm" style="margin-bottom: 0px;">
                                    <tbody>
                                        <tr>
                                            <td colspan="3"><span class="font-weight-bold">Información de la solicitud</span></td>
                                        </tr>
                                        <tr>
                                            <td><span class="font-weight-bold text-muted">Producto</span></td>
                                            <td class="lblProducto"></td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td><span class="font-weight-bold text-muted">CC / Vendedor</span></td>
                                            <td class="lblAgenciaYVendedorAsignado"></td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                    <div class="row mt-3">
                        <div class="col-sm-4 border-right border-top pt-3">
                            <div class="table-responsive border-right border p-2" style="max-height: 50vh; overflow: auto;">
                                <table id="tblExpedienteSolicitudGarantia" class="table table-sm table-hover cursor-pointer" style="width: 100%">
                                    <thead>
                                        <tr>
                                            <th>Documento</th>
                                            <th class="text-center no-sort"></th>
                                            <th class="text-center no-sort"></th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                        <div class="col-sm-8 border-top pt-3">
                            <h6 class="font-weight-bold mt-0">Previsualización</h6>

                            <div class="align-self-center" id="divPrevisualizacionDocumento" runat="server" style="display: none;"></div>
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

    
    <!-- modal detall condicionamiento -->
    <div id="modalCrearExpedientePrestamo" class="modal fade"  width ="100%" tabindex="-1" role="dialog" aria-labelledby="modalCrearExpedientePrestamoLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h6 class="modal-title mt-0" id="modalCrearExpedientePrestamoLabel">Detalle Condicion <span class="lblNoSolicitudCredito"></span></h6>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                          <table id="tblListaSolicitudCondiciones" class="table table-condensed table-striped" width ="100%">
                                <thead>
                                    <tr>
                                        <th>Tipo Condición</th>
                                        <th>Descripción</th>
                                        <th style="width:80%!important;">Comentario Adicional</th>
                                        <th>Estado</th>
                                    </tr>
                                </thead>
                                <tbody>
                                </tbody>
                            </table>
                    </div>
                </div>
                <div class="modal-footer pt-2 pb-2">
                 
                    <button type="reset" data-dismiss="modal" class="btn btn-secondary waves-effect">
                        Cancelar
                    </button>
                </div>
            </div>
        </div>
    </div>

     <div id="modalValidarToken" class="modal fade"  width ="100%" tabindex="-1" role="dialog" aria-labelledby="modalCrearExpedientePrestamoLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h6 class="modal-title mt-0" id="modalValidacionTokenDocumentoEliminacion">Eliminar Documento(Token) <span class="lblNoSolicitudCredito"></span></h6>
                </div>
                <div class="modal-body">
                    <div class="col-lg-8 col-md-8 col-sm-8 col-8 align-self-end">
                    <label class="col-form-label">Ingrese Token</label>
                    <input id="ddlToken" class="form-control form-control-sm" type="text" />
                </div>
                </div>
                <div class="modal-footer pt-2 pb-2">
                 <button  id="btnValidoTokenConfirmar" class="btn btn-primary waves-effect waves-light mr-1 validador">
                        Confirmar
                    </button>
                    <button type="reset" data-dismiss="modal" class="btn btn-secondary waves-effect">
                        Cancelar
                    </button>
                </div>
            </div>
        </div>
    </div>

        <!-- modal detalle referencia  -->
    <div id="modalReferenciaPersonal" class="modal fade bs-example-modal-lg"  tabindex="-1" role="dialog" aria-labelledby="modalReferenciaPersonalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header pb-2">
                    <h6 class="modal-title mt-0" id="modalReferenciaPersonalLabel">Referencias personales </h6>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                          <table id="tblListaSolicitudReferencia" class="table table-condensed table-striped">
                                <thead>
                                    <tr>
                                       <th>Nombre referencia</th>
                                         <th>Lugar de trabajo</th>
                                         <th>Tiempo de conocer ref</th>
                                         <th>Telefono ref</th>
                                         <th>Parentesco ref</th>
                                         <th></th>
                                    </tr>
                                </thead>
                                <tbody>
                                </tbody>
                            </table>
                    </div>
                </div>
                <div class="modal-footer pt-2 pb-2">
                 
                    <button type="reset" data-dismiss="modal" class="btn btn-secondary waves-effect">
                        Cancelar
                    </button>
                </div>
            </div>
        </div>
    </div>


    <!-- modal agregar comentario sobre una referencia personal -->
    <div id="modalComentarioReferencia" class="modal fade bs-example-modal-lg" role="dialog" aria-labelledby="modalComentarioReferenciaLabel" aria-hidden="true" >
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title mt-0" id="modalComentarioReferenciaLabel">Observaciones de referencia personal</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                </div>
                <form action="#" id="frmObservacionReferencia">

                    <div class="panel-body">
                      <div class="row mb-0">
                        
                          <div class="col-lg-5 col-md-5 col-10 border-left border-gray" id="div1" runat="server">
                              <%--<h6 class="font-weight-bold">&nbsp;&nbsp;Garantia</h6>--%>
                                   <div class="row" >
                                    <%--<div class="form-group row">--%>
                                     <div class="col-12">
                                                <!-- Div donde se muestran las fotografías de la garantía -->
                                                <div class="align-self-center" id="div2" runat="server" style="display: none;"></div>
                                          <%--  </div>--%>
                                     </div>
                                 </div>   
                         <div class="row mt-3 mb-0">
                    <div class="col-sm-12">
                        <div class="table-responsive">
                            <table class="table table-sm" style="margin-bottom: 0px;">                              
                                <tbody>
                                    <tr>
                                        <td><span class="font-weight-bold text-muted">Marca</span></td>
                                        <td>
                                            <asp:Label class="label label-table label-success" ID="lblMarcaReferencia" runat="server"></asp:Label>
                                        </td>
                                        <td></td>
                                    </tr>
                                     <tr>
                                        <td><span class="font-weight-bold text-muted">Modelo</span></td>
                                        <td>
                                            <asp:Label class="label label-table label-success" ID="lblModeloReferencia" runat="server"></asp:Label>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td><span class="font-weight-bold text-muted">Vin</span></td>
                                        <td>
                                            <asp:Label ID="lblVinReferencia" runat="server"></asp:Label>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td><span class="font-weight-bold text-muted">Añio</span></td>
                                        <td>
                                            <asp:Label ID="lblAnioReferencia" runat="server"></asp:Label>
                                        </td>
                                        <td></td>
                                    </tr>
                                
                                    <tr>
                                        <td colspan="3"></td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                  
                </div>


                             </div>

                          <!-- -->
                              <div class="col-lg-6 col-md-6 col-12 border-left border-gray" id="divPrestamoSolicitado" runat="server">
                                        <h6 class="font-weight-bold"> <asp:Label ID="spanNombreClienteReferencia" CssClass="col-sm-6" runat="server">prueba</asp:Label></h6>
                                      <div class="row">
                                        <div class="col-md-12">
                                        <div class="form-group row">                                
                                            <label class="col-sm-7">Edad</label>
                                            <asp:label ID="lblEdadCliente_Referencia" CssClass="col-sm-5"  runat="server"></asp:label>
                                            <label class="col-sm-7">Dirección Detallada Domicilio </label>
                                            <asp:label ID="lblDireccionDomicilio_Referencia" CssClass="col-sm-5"  runat="server"></asp:label>
                                            <label class="col-sm-7">Lugar de Trabajo</label>
                                            <asp:label ID="lblLugarTrabajo_Referencia" CssClass="col-sm-5"  runat="server"></asp:label>
                                            <label class="col-sm-7">Puesto Asignado</label>
                                            <asp:label ID="lblPuestoAsignado_Referencia" CssClass="col-sm-5"  runat="server"></asp:label>
                                            <label class="col-sm-7">Dirección Detallada de Trabajo</label>
                                            <asp:label ID="lblDireccionEmpresa_Referecia" CssClass="col-sm-5"  runat="server"></asp:label>
                                            <label class="col-sm-7">Ingresos Mensuales</label>
                                            <asp:label ID="lblIngresosMensualesReferencia" CssClass="col-sm-5"  runat="server"></asp:label>                                                                          
                                          
                                        </div>  
                                       </div>                              
                                          
                                     </div>
                                  
                                   <div class="row mt-3 mb-0">
                    <div class="col-sm-12">
                        <div class="table-responsive">
                            <table class="table table-sm" style="margin-bottom: 0px;">                              
                                <tbody>
                                    <tr>
                                        <td><span class="font-weight-bold text-muted">Valor Mercado </span></td>
                                        <td>
                                            <asp:Label class="label label-table label-success" ID="txtValorMercadoReferencia" runat="server"></asp:Label>
                                        </td>
                                        <td><span class="font-weight-bold text-muted">Plazo </span></td>
                                         <td>
                                            <asp:Label class="label label-table label-success" ID="txtPlazoReferencia" runat="server"></asp:Label>
                                        </td>
                                    </tr>

                                     <tr>
                                        <td><span class="font-weight-bold text-muted">Valor Prima</span></td>
                                        <td>
                                            <asp:Label class="label label-table label-success" ID="txtValorPrimaReferencia" runat="server"></asp:Label>
                                        </td>
                                          <td><span class="font-weight-bold text-muted">Frecuencia</span></td>
                                         <td>
                                            <asp:Label class="label label-table label-success" ID="txtFrecuensiaReferencia" runat="server"></asp:Label>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td><span class="font-weight-bold text-muted">Valor Prestar</span></td>
                                        <td>
                                            <asp:Label ID="txtValorPrestarReferencia" runat="server"></asp:Label>
                                        </td>
                                          <td><span class="font-weight-bold text-muted">Cuota Auto </span></td>
                                         <td>
                                            <asp:Label class="label label-table label-success" ID="txtCuotaAutoReferencia" runat="server"></asp:Label>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td><span class="font-weight-bold text-muted">Valor Financiar</span></td>
                                        <td>
                                            <asp:Label ID="txtValorFinanciarReferencia" runat="server"></asp:Label>
                                        </td>
                                          <td><span class="font-weight-bold text-muted">Cuota Collateral </span></td>
                                         <td>
                                            <asp:Label class="label label-table label-success" ID="txtCuotaCollateralReferencia" runat="server"></asp:Label>
                                        </td>
                                    </tr>

                                     <tr>
                                        <td></td>
                                        <td>                                       
                                        </td>
                                          <td><span class="font-weight-bold text-muted">Cuota Total </span></td>
                                         <td>
                                            <asp:Label class="label label-table label-success" ID="txtCuotaTotalReferencia" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                
                                    <tr>
                                        <td colspan="4"></td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                  
                </div>


          </div>       
                          
          </div>
             </div>


                    <div class="modal-body">
                          <div class="form-group">
                           
                            <textarea id="txtDetalleReferencia" required="required" class="form-control" rows="8" readonly  style="overflow:hidden!important">
                             
                            </textarea>
                        </div>
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
                            Sin Confirmar
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

    <script src="/Scripts/js/jquery.min.js"></script>
    <script src="/Scripts/js/bootstrap.bundle.min.js"></script>
    <script src="/Scripts/plugins/iziToast/js/iziToast.min.js"></script>
    <script src="/Scripts/plugins/parsleyjs/parsley.js"></script>
    <!-- datatable js -->
    <script src="/Scripts/plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="/Scripts/plugins/datatables/dataTables.bootstrap4.min.js"></script>
    <!-- Buttons -->
    <script src="/Scripts/plugins/datatables/dataTables.buttons.min.js"></script>
    <script src="/Scripts/plugins/datatables/buttons.bootstrap4.min.js"></script>
    <script src="/Scripts/plugins/datatables/jszip.min.js"></script>
    <script src="/Scripts/plugins/datatables/pdfmake.min.js"></script>
    <script src="/Scripts/plugins/datatables/vfs_fonts.js"></script>
    <script src="/Scripts/plugins/datatables/buttons.html5.min.js"></script>
    <script src="/Scripts/plugins/datatables/buttons.print.min.js"></script>
    <script src="/Scripts/plugins/datatables/buttons.colVis.min.js"></script>
    <!-- Responsive -->
    <script src="/Scripts/plugins/datatables/dataTables.responsive.min.js"></script>
    <script src="/Scripts/plugins/datatables/responsive.bootstrap4.min.js"></script>
    <script src="/Scripts/plugins/select2/js/select2.full.min.js"></script>
    <script src="/Scripts/plugins/datapicker/bootstrap-datepicker.js"></script>
    <script src="/Scripts/plugins/moment/moment.js"></script>
    <script src="/Scripts/plugins/moment/moment-with-locales.min.js"></script>
    <script src="/Scripts/plugins/unitegallery/js/unitegallery.min.js"></script>
    <script src="/Scripts/plugins/unitegallery/themes/default/ug-theme-default.js"></script>
    <script src="/Scripts/app/solicitudes/SolicitudesCredito_Bandeja.js?v=20210310035985"></script>
</body>
</html>
