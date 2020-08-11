<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SolicitudesCredito_ClientesPrecalificados.aspx.cs" Inherits="proyectoBase.Forms.Solicitudes.SolicitudesCredito_ClientesPrecalificados" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%:Styles.Render("~/Scripts/plugins/datatables.css")%>
    <%:Styles.Render("~/iziToast/css")%>
    <%:Styles.Render("~/Scripts/plugins/select2/css/select2.min.css")%>
    <%:Styles.Render("~/Scripts/plugins/datapicker/datepicker3.css")%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3>Clientes Pre-Aprobados</h3>
    <div class="row">
        <div class="col-12">
            <div class="card m-b-20">
                <div class="card-body">
                    <br />
                    <div class="form-group row">
                        <label class="col-sm-2 col-form-label">Identidad cliente</label>
                        <div class="col-sm-4">
                            <input id="identidadCliente" class="form-control form-control-sm" type="text">
                        </div>
                        <label class="col-sm-2 col-form-label">Nombre cliente</label>
                        <div class="col-sm-4">
                            <input id="nombreCliente" class="form-control form-control-sm" type="text">
                        </div>                        
                    </div>
                    <table id="datatable-bandeja" class="table-condensed table-striped table-bordered dt-responsive display nowrap compact" style="border-collapse: collapse; border-spacing: 0; width: 100%;">
                        <thead>
                            <tr>
                                <th>Identidad</th>
                                <th>Nombre cliente</th>
                                <th>Telefono</th>
                                <th>Ingresos</th>
                                <th>Comentario</th>
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
    <div id="modalActualizarSolicitud" class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="modalActualizarSolicitudLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title mt-0" id="modalActualizarSolicitudLabel">Ingresar solicitud</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                </div>
                <div class="modal-body">
                    ¿Desea ingresar la solicitud de crédito del cliente <label id="lblNombreCliente"></label> con identidad <label id="lblIdentidadCliente"></label>?        
                </div>
                <div class="modal-footer">
                    <button id="btnIngresarSolictudConfirmar" class="btn btn-primary waves-effect waves-light">
                        Confirmar
                    </button>
                    <button type="reset" data-dismiss="modal" class="btn btn-secondary waves-effect">
                        Cancelar
                    </button>
                </div>
            </div>
        </div>
    </div>    
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="server">
    <%:Scripts.Render("~/Scripts/plugins/datatables") %>
    <%:Scripts.Render("~/Scripts/plugins/parsleyjs") %>
    <%:Scripts.Render("~/Scripts/plugins/iziToast") %>
    <%:Scripts.Render("~/Scripts/plugins/select2/js/select2.min.js") %>
    <%:Scripts.Render("~/Scripts/plugins/datapicker/bootstrap-datepicker") %>
    <%:Scripts.Render("~/Scripts/app/solicitudes/SolicitudesCredito_ClientesPrecalificados") %>
</asp:Content>