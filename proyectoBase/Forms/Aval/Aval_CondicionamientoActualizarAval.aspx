<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Aval_CondicionamientoActualizarAval.aspx.cs" Inherits="proyectoBase.Forms.Aval.Aval_CondicionamientoActualizarAval" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%: Styles.Render("~/iziToast/css") %>
    <script>
        function resizeIframe(obj) {
            obj.style.height = obj.contentWindow.document.documentElement.scrollHeight + 'px';
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-12">
            <div class="card m-b-20">
                <div class="card-body">
                    <iframe src="../Aval/Aval_Actualizar.aspx" frameborder="0" width="100%" scrolling="no" onload="resizeIframe(this)"></iframe>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="server">
    <%:Scripts.Render("~/Scripts/plugins/iziToast") %>
</asp:Content>
