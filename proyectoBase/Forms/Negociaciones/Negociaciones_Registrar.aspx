<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Negociaciones_Registrar.aspx.cs" Inherits="proyectoBase.Forms.Negociaciones.Negociaciones_Registrar" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <title>Registrar negociacion</title>
    <!-- BOOTSTRAP -->
    <link href="/Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/Content/css/style.css" rel="stylesheet" />
    <!-- ARCHIVOS NECESARIOS PARA EL FUNCIONAMIENTO DE LA PAGINA -->
    <link href="/Scripts/plugins/iziToast/css/iziToast.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/datapicker/datepicker3.css" rel="stylesheet" />
    <link href="/CSS/Estilos_CSS.css" rel="stylesheet" />
    <link href="/Content/css/font/font-fileuploader.css" rel="stylesheet" />
    <link href="/Content/css/jquery.fileuploader.min.css" rel="stylesheet" />
    <link href="/Content/css/jquery.fileuploader-theme-dragdrop.css" rel="stylesheet" />

</head>
<body class="EstiloBody-Listado-W1100px">
    <form runat="server" id="FrmGuardarNegociacion">
        <asp:ScriptManager runat="server" ID="smMultiview"></asp:ScriptManager>
        <asp:UpdatePanel runat="server" ID="upMultiView">
            <ContentTemplate>
                <div class="card m-0">
                    <div class="card-header">
                        <h6>Guardar Negociación</h6>
                    </div>
                    <div class="card-body pt-0">
                        <div runat="server" style="padding-top: 1.25rem;" id="divParametros" visible="true">
                            <div class="form-group form-row">
                                <div class="col">
                                    <label class="col-form-label">Cliente</label>
                                    <asp:TextBox ID="txtNombreCliente" Enabled="false" CssClass="form-control form-control-sm col-form-label" required="required" Text="Mayra Julissa Ventura Pineda" runat="server"></asp:TextBox>

                                    <label class="col-form-label">Teléfono</label>
                                    <asp:TextBox ID="txtTelefonoCliente" type="tel" Enabled="false" CssClass="form-control form-control-sm col-form-label telefono" required="required" Text="96286135" runat="server"></asp:TextBox>

                                </div>
                                <div class="col">
                                    <label class="col-form-label">Identidad</label>
                                    <asp:TextBox ID="txtIdentidadCliente" Enabled="false" CssClass="form-control form-control-sm col-form-label" required="required" Text="0501199506533" runat="server"></asp:TextBox>

                                    <label class="col-form-label">Fecha</label>
                                    <asp:TextBox ID="txtFecha" Enabled="false" CssClass="form-control form-control-sm col-form-label" required="required" Text="" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group form-row">
                                <div class="col">
                                    <label class="col-form-label">Valor del automovil</label>
                                    <asp:TextBox ID="txtValorVehiculo" type="tel" CssClass="form-control form-control-sm col-form-label MascaraCantidad" required="required" Text="" runat="server"></asp:TextBox>
                                </div>
                                <div class="col" id="divPrima" runat="server" visible="true">
                                    <label class="col-form-label">Valor de la Prima</label>
                                    <asp:Label CssClass="" ID="lblPorcenajedePrima" runat="server" Text="" Visible="false" />
                                    <asp:TextBox ID="txtValorPrima" type="tel" CssClass="form-control form-control-sm col-form-label MascaraCantidad" required="required" Text="" runat="server"></asp:TextBox>
                                </div>

                                <div class="col-sm-4" id="divMontoFinanciarVehiculo" runat="server" visible="false">
                                    <label class="col-form-label">Valor del vehiculo a financiar</label>
                                    <asp:TextBox ID="txtMonto" Enabled="false" type="tel" CssClass="form-control form-control-sm col-form-label MascaraCantidad" required="required" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-form-label">Score Promedio</label>
                                <asp:TextBox ID="txtScorePromedio" type="tel" CssClass="form-control form-control-sm col-form-label MascaraNumerica" required="required" Text="" runat="server"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <div>
                                    <div class="form-check form-check-inline">
                                        <asp:RadioButton CssClass="form-check-input" ID="rbFinanciamiento" runat="server" GroupName="rbTipoFinanciamiento" OnCheckedChanged="rbFinanciamiento_CheckedChanged" AutoPostBack="true" />
                                        <label class="form-check-label">Financiamiento</label>
                                    </div>
                                    <div class="form-check form-check-inline">
                                        <asp:RadioButton CssClass="form-check-input" ID="rbEmpeno" runat="server" GroupName="rbTipoFinanciamiento" OnCheckedChanged="rbEmpeno_CheckedChanged" AutoPostBack="true" />
                                        <label class="form-check-label">Financiamiento con garantía</label>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-form-label">Plazos disponibles</label>
                                <asp:DropDownList ID="ddlPlazos" runat="server" CssClass="form-control form-control-sm col-form-label"></asp:DropDownList>
                            </div>

                            <h6 class="border-top pt-2">Información de la garantía</h6>

                            <div class="form-group form-row">
                                <div class="col">
                                    <label class="col-form-label">Marca</label>
                                    <asp:DropDownList ID="ddlMarca" runat="server" CssClass="form-control form-control-sm col-form-label" OnSelectedIndexChanged="ddlMarca_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                </div>
                                <div class="col" id="div2" runat="server" visible="true">
                                    <label class="col-form-label">Modelo</label>
                                    <asp:DropDownList ID="ddlModelo" Enabled="false" runat="server" CssClass="form-control form-control-sm col-form-label"></asp:DropDownList>
                                </div>

                                <div class="col-sm-4" id="div3" runat="server" visible="true">
                                    <label class="col-form-label">Año</label>
                                    <asp:TextBox ID="txtAnio" Enabled="false" type="tel" CssClass="form-control form-control-sm col-form-label MascaraCantidad" required="required" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group form-row">
                                <div class="col">
                                    <label class="col-form-label">Matricula</label>
                                    <asp:TextBox ID="txtMatricula" type="tel" CssClass="form-control form-control-sm col-form-label MascaraCantidad" required="required" Text="" runat="server"></asp:TextBox>
                                </div>
                                <div class="col" id="div4" runat="server" visible="true">
                                    <label class="col-form-label">Color</label>
                                    <asp:TextBox ID="txtColor" type="tel" CssClass="form-control form-control-sm col-form-label MascaraCantidad" required="required" Text="" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group form-row">
                                <div class="col">
                                    <label class="col-form-label">Cilindraje</label>
                                    <asp:TextBox ID="txtCilindraje" type="tel" CssClass="form-control form-control-sm col-form-label MascaraCantidad" required="required" Text="" runat="server"></asp:TextBox>
                                </div>
                                <div class="col" id="div5" runat="server" visible="true">
                                    <label class="col-form-label">Recorrido</label>
                                    <asp:TextBox ID="txtRecorrido" type="tel" CssClass="form-control form-control-sm col-form-label MascaraCantidad" required="required" Text="" runat="server"></asp:TextBox>
                                </div>
                                <div class="col" id="div8" runat="server" visible="true">
                                    <label class="col-form-label">Unidad de medida</label>
                                    <asp:DropDownList ID="ddlUnidadDeMedida" runat="server" CssClass="form-control form-control-sm col-form-label"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group form-row">
                                <div class="col">
                                    <label class="col-form-label">Origen</label>
                                    <asp:DropDownList ID="ddlOrigen" runat="server" CssClass="form-control form-control-sm col-form-label"></asp:DropDownList>
                                </div>
                                <div class="col" id="div6" runat="server" visible="true">
                                    <label class="col-form-label">Vendedor</label>
                                    <asp:TextBox ID="txtVendedor" type="tel" CssClass="form-control form-control-sm col-form-label MascaraCantidad" required="required" Text="" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-sm-4" id="div7" runat="server" visible="true">
                                    <label class="col-form-label">Autolote</label>
                                    <asp:DropDownList ID="ddlAutolote" runat="server" CssClass="form-control form-control-sm col-form-label"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group form-row">
                                <div class="col-12">
                                    <label class="col-form-label">Fotografía</label>
                                    <input type="file" id="fotografiaGarantia" name="files">
                                </div>
                            </div>
                            <div class="form-group row mb-0">
                                <div class="button-items col-sm-12 justify-content-center">
                                    <asp:Button ID="btnGuardarNegociacion" Text="Guardar" CssClass="btn btn-primary btn-lg waves-effect waves-light" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdateProgress ID="updateProgress" runat="server">
            <ProgressTemplate>
                <div class="loading">
                    <div class="loader"></div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </form>

    <script src="/Scripts/js/jquery.min.js"></script>
    <script src="/Scripts/js/bootstrap.bundle.min.js"></script>
    <!-- ARCHIVOS NECESARIOS PARA LA PANTALLA -->
    <script src="/Scripts/plugins/iziToast/js/iziToast.min.js"></script>
    <script src="/Scripts/plugins/parsleyjs/parsley.js"></script>
    <script src="/Scripts/plugins/datapicker/bootstrap-datepicker.js"></script>
    <script src="/Scripts/app/uploader/js/jquery.fileuploader.min.js"></script>
    <script>
        $(document).ready(function () {
            $('#fotografiaGarantia').fileuploader({
                inputNameBrackets: false,
                theme: 'dragdrop',
                limit: 1, //limite de archivos a subir
                maxSize: 10, //peso máximo de todos los archivos seleccionado en megas (MB)
                fileMaxSize: 2, //peso máximo de un archivo
                extensions: ['jpg', 'png'],// extensiones/formatos permitidos

                upload: {
                    url: 'SolicitudesCredito_Registrar.aspx?type=upload&doc=1',
                    data: null,
                    type: 'POST',
                    enctype: 'multipart/form-data',
                    start: true,
                    synchron: true,
                    beforeSend: null,
                    onSuccess: function (result, item) {
                        var data = {};
                        try {
                            data = JSON.parse(result);
                        } catch (e) {
                            data.hasWarnings = true;
                        }

                        // validar exito
                        if (data.isSuccess && data.files[0]) {
                            item.name = data.files[0].name;
                            item.html.find('.column-title > div:first-child').text(data.files[0].name).attr('title', data.files[0].name);
                        }

                        // validar si se produjo un error
                        if (data.hasWarnings) {
                            for (var warning in data.warnings) {
                                alert(data.warnings);
                            }

                            item.html.removeClass('upload-successful').addClass('upload-failed');
                            return this.onError ? this.onError(item) : null;
                        }

                        item.html.find('.fileuploader-action-remove').addClass('fileuploader-action-success');
                        setTimeout(function () {
                            item.html.find('.progress-bar2').fadeOut(400);
                        }, 400);
                    },
                    onError: function (item) {
                        var progressBar = item.html.find('.progress-bar2');

                        if (progressBar.length) {
                            progressBar.find('span').html(0 + "%");
                            progressBar.find('.fileuploader-progressbar .bar').width(0 + "%");
                            item.html.find('.progress-bar2').fadeOut(400);
                        }

                        item.upload.status != 'cancelled' && item.html.find('.fileuploader-action-retry').length == 0 ? item.html.find('.column-actions').prepend(
                            '<button type="button" class="fileuploader-action fileuploader-action-retry" title="Retry"><i class="fileuploader-icon-retry"></i></button>'
                        ) : null;
                    },
                    onProgress: function (data, item) {
                        var progressBar = item.html.find('.progress-bar2');

                        if (progressBar.length > 0) {
                            progressBar.show();
                            progressBar.find('span').html(data.percentage + "%");
                            progressBar.find('.fileuploader-progressbar .bar').width(data.percentage + "%");
                        }
                    },
                    onComplete: null,
                },
                onRemove: function (item) {
                    $.post('SolicitudesCredito_Registrar.aspx?type=remove', { file: item.name });
                },
                dialogs: {
                    /*Mensajes que se muestran al usuario, ya sean alertas o confirmaciones, se pueden adecuar a cualquier requerimiento*/

                    //personalizar alertas
                    alert: function (text) {
                        return iziToast.warning({
                            title: 'Atencion',
                            message: text
                        });
                    },

                    //personalizar confimaciones
                    confirm: function (text, callback) {
                        confirm(text) ? callback() : null;
                    }
                },
                captions: $.extend(true, {}, $.fn.fileuploader.languages['es'], {
                    feedback: 'Arrastra y suelta los archivos aqui',
                    feedback2: 'Arrastra y suelta los archivos aqui',
                    drop: 'Arrastra y suelta los archivos aqui',
                    button: 'Buscar archivos',
                    confirm: 'Confirmar',
                    cancel: 'Cancelar'
                })
            });
        });
    </script>
</body>
</html>

