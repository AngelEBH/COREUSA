<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SolicitudesCredito_Cliente_ActualizarBarrioColonia.aspx.cs" Inherits="SolicitudesCredito_Cliente_ActualizarBarrioColonia" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <title>Actualizar barrio/colonia</title>
    <!-- BOOTSTRAP -->
    <link href="/Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/Content/css/icons.css" rel="stylesheet" />
    <link href="/Content/css/style.css?v=202010031105" rel="stylesheet" />
    <!-- ARCHIVOS NECESARIOS PARA EL FUNCIONAMIENTO DE LA PAGINA -->
    <link href="/Scripts/plugins/datatables/dataTables.bootstrap4.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/datatables/buttons.bootstrap4.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/datatables/responsive.bootstrap4.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/iziToast/css/iziToast.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/select2/css/select2.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/datapicker/datepicker3.css" rel="stylesheet" />
    <link href="/Content/css/bandejaSolicitudes.css" rel="stylesheet" />
    <link href="/CSS/Estilos_CSS.css" rel="stylesheet" />
    <style>
        .opcion {
            cursor: pointer;
        }

        .dataTable tbody tr {
            cursor: pointer;
        }

        .dataTable tbody td {
            outline: none;
            padding-top: 0 !important;
            padding-bottom: 0 !important;
        }
    </style>
</head>
<body runat="server" class="EstiloBody-Listado">
    <div class="card">
        <div class="card-header">
            <div class="row">
                <div class="col-8">
                    <h6>Actualizar direcciones....</h6>
                </div>
                <div class="col-4">
                    <input id="txtDatatableFilter" class="float-right form-control w-75" type="text" placeholder="Buscar"
                        aria-label="Buscar" />
                </div>
            </div>
        </div>
        <div class="card-body">

            <div class="table-responsive">
                <table id="datatable-listado" class="table-bordered display compact nowrap table-condensed table-hover dataTable" style="width: 100%" role="grid">
                    <thead>
                        <tr>
                            <th>No</th>
                            <th>Identidad</th>
                            <th>Nombre cliente</th>
                            <th>Acciones</th>
                        </tr>
                    </thead>
                    <tbody></tbody>
                    <tfoot></tfoot>
                </table>
            </div>
        </div>
    </div>


    <form runat="server">
        <div id="modalActualizarInformacion" class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="modalActualizarInformacionLabel" aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <h6 class="modal-title m-0" id="modalActualizarInformacionLabel">Actualizar información</h6>
                    </div>
                    <div class="modal-body">
                        Cliente: <strong><span id="lblCliente"></span></strong>
                        <br />
                        Identidad <strong><span id="lblIdentidadCliente"></span></strong>?
                    <br />

                        <div class="row">
                            <div class="col-12">
                                <h6>Información domicilio </h6>

                                <div class="form-group row">
                                    <div class="col-sm-3">
                                        <label class="col-form-label">Departamento</label>
                                        <asp:DropDownList ID="ddlDepartamentoDomicilio" runat="server" CssClass="form-control form-control-sm buscadorddl" required="required" data-parsley-group="informacionDomicilio" data-parsley-errors-container="#error-ddlDepartamentoDomicilio"></asp:DropDownList>
                                        <div id="error-ddlDepartamentoDomicilio"></div>
                                    </div>
                                    <div class="col-sm-3">
                                        <label class="col-form-label">Municipio</label>
                                        <asp:DropDownList ID="ddlMunicipioDomicilio" runat="server" CssClass="form-control form-control-sm buscadorddl" Enabled="false" required="required" data-parsley-group="informacionDomicilio" data-parsley-errors-container="#error-ddlMunicipioDomicilio"></asp:DropDownList>
                                        <div id="error-ddlMunicipioDomicilio"></div>
                                    </div>
                                    <div class="col-sm-3">
                                        <label class="col-form-label">Ciudad/Poblado</label>
                                        <asp:DropDownList ID="ddlCiudadPobladoDomicilio" runat="server" CssClass="form-control form-control-sm buscadorddl" Enabled="false" required="required" data-parsley-group="informacionDomicilio" data-parsley-errors-container="#error-ddlCiudadPobladoDomicilio"></asp:DropDownList>
                                        <div id="error-ddlCiudadPobladoDomicilio"></div>
                                    </div>
                                    <div class="col-sm-3">
                                        <label class="col-form-label">Barrio/Colonia</label>
                                        <asp:DropDownList ID="ddlBarrioColoniaDomicilio" runat="server" CssClass="form-control form-control-sm buscadorddl" Enabled="false" required="required" data-parsley-group="informacionDomicilio" data-parsley-errors-container="#error-ddlBarrioColoniaDomicilio"></asp:DropDownList>
                                        <div id="error-ddlBarrioColoniaDomicilio"></div>
                                    </div>
                                </div>
                                <div class="form-group row">
                                    <div class="col-sm-12">
                                        <label class="col-form-label">Dirección detallada del domicilio</label>
                                        <textarea id="txtDireccionDetalladaDomicilio" runat="server" readonly="readonly" class="form-control form-control-sm" data-parsley-group="informacionDomicilio"></textarea>
                                    </div>
                                    <div class="col-12">
                                        <label class="col-form-label">Referencias del domicilio</label>
                                        <textarea id="txtReferenciasDelDomicilio" runat="server" readonly="readonly" class="form-control form-control-sm" data-parsley-group="informacionDomicilio"></textarea>
                                    </div>
                                </div>
                            </div>
                            <div class="col-12 border-gray">
                                <h6>Información laboral </h6>

                                <div class="form-group row">
                                    <div class="col-sm-3">
                                        <label class="col-form-label">Departamento</label>
                                        <asp:DropDownList ID="ddlDepartamentoEmpresa" runat="server" CssClass="form-control form-control-sm buscadorddl" required="required" data-parsley-group="informacionLaboral" data-parsley-errors-container="#error-ddlDepartamentoEmpresa"></asp:DropDownList>
                                        <div id="error-ddlDepartamentoEmpresa"></div>
                                    </div>
                                    <div class="col-sm-3">
                                        <label class="col-form-label">Municipio</label>
                                        <asp:DropDownList ID="ddlMunicipioEmpresa" runat="server" CssClass="form-control form-control-sm buscadorddl" Enabled="false" required="required" data-parsley-group="informacionLaboral" data-parsley-errors-container="#error-ddlMunicipioEmpresa"></asp:DropDownList>
                                        <div id="error-ddlMunicipioEmpresa"></div>
                                    </div>
                                    <div class="col-sm-3">
                                        <label class="col-form-label">Ciudad/Poblado</label>
                                        <asp:DropDownList ID="ddlCiudadPobladoEmpresa" runat="server" CssClass="form-control form-control-sm buscadorddl" Enabled="false" required="required" data-parsley-group="informacionLaboral" data-parsley-errors-container="#error-ddlCiudadPobladoEmpresa"></asp:DropDownList>
                                        <div id="error-ddlCiudadPobladoEmpresa"></div>
                                    </div>
                                    <div class="col-sm-3">
                                        <label class="col-form-label">Barrio/Colonia</label>
                                        <asp:DropDownList ID="ddlBarrioColoniaEmpresa" runat="server" CssClass="form-control form-control-sm buscadorddl" Enabled="false" required="required" data-parsley-group="informacionLaboral" data-parsley-errors-container="#error-ddlBarrioColoniaEmpresa"></asp:DropDownList>
                                        <div id="error-ddlBarrioColoniaEmpresa"></div>
                                    </div>
                                </div>
                                <div class="form-group row">
                                    <div class="col-12">
                                        <label class="col-form-label">Dirección detallada del trabajo</label>
                                        <textarea id="txtDireccionDetalladaEmpresa" runat="server" readonly="readonly" class="form-control form-control-sm" data-parsley-group="informacionLaboral"></textarea>
                                    </div>
                                    <div class="col-12">
                                        <label class="col-form-label">Referencias de la ubicación del trabajo</label>
                                        <textarea id="txtReferenciasEmpresa" runat="server" readonly="readonly" class="form-control form-control-sm" data-parsley-group="informacionLaboral"></textarea>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" data-dismiss="modal" class="btn btn-secondary waves-effect">
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
    <script src="/Scripts/plugins/parsleyjs/parsley.js"></script>
    <script src="/Scripts/plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="/Scripts/plugins/datatables/dataTables.bootstrap4.min.js"></script>
    <script src="/Scripts/plugins/datatables/dataTables.buttons.min.js"></script>
    <script src="/Scripts/plugins/datatables/buttons.bootstrap4.min.js"></script>
    <script src="/Scripts/plugins/datatables/jszip.min.js"></script>
    <script src="/Scripts/plugins/datatables/pdfmake.min.js"></script>
    <script src="/Scripts/plugins/datatables/vfs_fonts.js"></script>
    <script src="/Scripts/plugins/datatables/buttons.html5.min.js"></script>
    <script src="/Scripts/plugins/datatables/buttons.print.min.js"></script>
    <script src="/Scripts/plugins/datatables/buttons.colVis.min.js"></script>
    <script src="/Scripts/plugins/datatables/dataTables.responsive.min.js"></script>
    <script src="/Scripts/plugins/datatables/responsive.bootstrap4.min.js"></script>
    <script src="/Scripts/plugins/select2/js/select2.full.min.js"></script>
    <script src="/Scripts/plugins/datapicker/bootstrap-datepicker.js"></script>
    <script src="/Scripts/plugins/moment/moment.js"></script>
    <script src="/Scripts/plugins/moment/moment-with-locales.min.js"></script>
    <script src="/Scripts/app/solicitudes/SolicitudesCredito_Cliente_ActualizarBarrioColonia.js"></script>
</body>
</html>
