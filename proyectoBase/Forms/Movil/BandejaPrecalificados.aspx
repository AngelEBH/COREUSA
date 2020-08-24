<%@ Page Language="C#" AutoEventWireup="true" Inherits="Clientes_BandejaPrecalificados" CodeBehind="BandejaPrecalificados.aspx.cs" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <title></title>
    <!-- BOOTSTRAP -->
    <link href="/Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/Content/css/style.css" rel="stylesheet" />

    <!-- ARCHIVOS NECESARIOS PARA EL FUNCIONAMIENTO DE LA PAGINA -->
    <link href="/Scripts/plugins/datatables/dataTables.bootstrap4.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/datatables/buttons.bootstrap4.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/datapicker/datepicker3.css" rel="stylesheet" />
    <link href="/Scripts/plugins/iziToast/css/iziToast.min.css" rel="stylesheet" />
    <link href="/CSS/Estilos_CSS.css" rel="stylesheet" />
    <style>
        .opcion {
            cursor: pointer;
        }

        #datatable-recuperacion tbody tr {
            cursor: pointer;
        }

        #datatable-recuperacion tbody td {
            outline: none;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="card">
            <div class="card-body">

                <%--<div class="row mb-2">
                    <div class="col-3 themed-grid-col">
                        <input id="todos" type="radio" name="filtros" value="0" />
                        <li class="mdi mdi-account-group mdi-24px text-success p-0"></li>
                    </div>
                    <div class="col-3 themed-grid-col">

                        <input id="preAprobados" type="radio" name="filtros" value="preAprobados" />
                        <li class="mdi mdi-account-multiple-check mdi-24px text-success p-0"></li>

                    </div>
                    <div class="col-3 themed-grid-col">
                        <input id="analisis" type="radio" name="filtros" value="analisis" />
                        <li class="mdi mdi-account-alert mdi-24px text-warning p-0"></li>
                    </div>

                    <div class="col-3 themed-grid-col">
                        <input id="rechazados" type="radio" name="filtros" value="rechazados" />
                            <li class="mdi mdi-account-remove mdi-24px text-danger p-0"></li>
                    </div>
                </div>--%>

                <div class="btn-group btn-block" role="group" aria-label="Default button group">
                    <button type="button" class="btn btn-sm btn-secondary">Todos</button>
                    <button type="button" class="btn btn-sm btn-success">Pre-Aprobados</button>
                    <button type="button" class="btn btn-sm btn-warning">Analisis</button>
                    <button type="button" class="btn btn-sm btn-danger">Rechazados</button>
                </div>

                <div class="btn-group btn-group-toggle col-lg-12 p-0" data-toggle="buttons">
                    <label class="btn btn-sm btn-secondary active opcion">
                        <input id="todos" type="radio" name="filtros" value="0" />
                        Todos
                    </label>
                    <label class="btn btn-sm btn-success opcion">
                        <input id="preAprobados" type="radio" name="filtros" value="preAprobados" />
                        Pre-Aprobados
                    </label>
                    <label class="btn btn-sm btn-warning opcion">
                        <input id="analisis" type="radio" name="filtros" value="analisis" />
                        Analisis
                    </label>
                    <label class="btn btn-sm btn-danger opcion">
                        <input id="rechazados" type="radio" name="filtros" value="rechazados" />
                        Rechazados
                    </label>
                </div>

                <table id="datatable-recuperacion" class="table-bordered display compact nowrap table-condensed table-hover dataTable" style="width: 100%" role="grid">
                    <thead>
                        <tr>
                            <th>Cliente</th>
                            <th>Identidad</th>
                            <th>Ingresos</th>
                            <th>Telefono</th>
                            <th>Producto</th>
                            <th>Consultado</th>
                            <th>Oficial</th>
                            <th></th>
                        </tr>
                    </thead>
                    <tbody></tbody>
                    <tfoot></tfoot>
                </table>
            </div>
        </div>


        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel runat="server" ID="upLista">
            <ContentTemplate>

                <div class="DivPanelListas" style="width: 1112px; height: calc(100vh - 5px);">
                    <div>
                        <asp:Panel ID="PanelOpciones" runat="server" CssClass="FormatoPanelViewNOAB" Height="32px" Style="margin-left: 5px; margin-top: 5px; background-color: gray; width: fit-content;">
                            <asp:Button ID="btnTodos" runat="server" CssClass="FormatoBotonesIconoFiltros40" Style="background-image: url('/Imagenes/iconoPersonas24.png'); border-bottom-left-radius: 2px; border-top-left-radius: 2px;" Text="Todos" OnClick="btnFiltros_Click" CommandName="0" CommandArgument="0" />
                            <asp:Button ID="btnAprobados" runat="server" CssClass="FormatoBotonesIconoFiltros40" Style="background-image: url('/Imagenes/iconoOk24.png');" Text="Pre-aprobados" OnClick="btnFiltros_Click" CommandName="1" CommandArgument="1" />
                            <asp:Button ID="btnAnalisis" runat="server" CssClass="FormatoBotonesIconoFiltros40" Style="background-image: url('/Imagenes/iconoAlerta24.png');" Text="En analisis" OnClick="btnFiltros_Click" CommandName="3" CommandArgument="3" />
                            <asp:Button ID="btnRechazados" runat="server" CssClass="FormatoBotonesIconoFiltros40" Style="background-image: url('/Imagenes/iconoDetener24.png'); border-bottom-right-radius: 2px; border-top-right-radius: 2px;" Text="Rechazados" OnClick="btnFiltros_Click" CommandName="2" CommandArgument="2" />
                        </asp:Panel>
                    </div>

                    <div class="FormatoPanelSubNOAB" style="width: 1100px; margin: 5px;">
                        <div class="DivTituloGrid">
                            <table class="TableFormatoTituloGrid">
                                <tr>
                                    <td style="width: 10px;">&nbsp</td>
                                    <td style="width: 100px;">Oficial</td>
                                    <td style="width: 230px;">Nombre/Identidad</td>
                                    <td style="width: 200px;">Ingresos/Telefono</td>
                                    <td style="width: 150px;">Producto/Consultado</td>
                                    <td>Detalle</td>
                                    <td style="width: 70px;">Acciones</td>
                                    <td style="width: 20px;">&nbsp</td>
                                </tr>
                            </table>
                        </div>
                        <div class="DivPanelGridScrollFull" style="width: 100%; height: calc(100vh - 119px);">
                            <asp:GridView ID="gvPrecalificado" runat="server" CssClass="GridViewFormatoGeneral" HeaderStyle-CssClass="GridViewCabecera" RowStyle-CssClass="GridViewFilasNeutral" RowStyle-Height="46px" AutoGenerateColumns="False" ShowHeader="False" Style="vertical-align: middle;" OnRowCommand="gvPrecalificado_RowCommand" DataKeyNames="fcIdentidad">
                                <Columns>
                                    <asp:ImageField DataImageUrlField="fcImagen" ItemStyle-Width="24px" ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Middle" />
                                    <asp:BoundField DataField="fcIdentidad" ItemStyle-Width="100px" HtmlEncode="False" />
                                    <asp:BoundField DataField="fcColumna1" ItemStyle-Width="100px" HtmlEncode="False" />
                                    <asp:BoundField DataField="fcColumna2" ItemStyle-Width="230px" HtmlEncode="False" />
                                    <asp:BoundField DataField="fcColumna3" ItemStyle-Width="150px" HtmlEncode="False" />
                                    <asp:BoundField DataField="fcColumna4" ItemStyle-Width="230px" HtmlEncode="False" />
                                    <asp:BoundField DataField="fcColumna5" HtmlEncode="False" />
                                    <asp:ButtonField ButtonType="Image" ImageUrl="/Imagenes/iconoSolicitudesLista30.png" ItemStyle-Width="35px" CommandName="Ver" Text="Ver detalle del precalificado" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" />
                                </Columns>
                                <HeaderStyle CssClass="GridViewCabecera" />
                                <RowStyle CssClass="GridViewFilasBitacoras" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <asp:Label CssClass="Formatolbl" ID="lblMensaje" runat="server" Style="left: 5px; color: red;"></asp:Label>
                <asp:Panel ID="PanelErrores" runat="server" CssClass="FormatoPanelContenedorBordes" Height="50px" Width="200px" Visible="false" Style="left: calc(100vw - 300px); position: absolute; top: 10px; background-color: darkred; border-color: red;">
                    <asp:Label CssClass="Formatolbl" ID="lblMensajeError" runat="server" Style="color: white; margin: 5px;"></asp:Label>
                </asp:Panel>

                <input type="text" id="txtBuscador" name="txtBuscador" class="Formatotxt" maxlength="50" style="top: 51px; width: 150px; left: 780px;" />
                <input type="text" id="txtRegistros" name="txtRegistros" class="Formatotxt" maxlength="20" style="top: 52px; width: 120px; left: 945px; border: hidden; background-color: transparent;" disabled="disabled" />

            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdateProgress ID="prgLoadingStatus" runat="server" AssociatedUpdatePanelID="upLista">
            <ProgressTemplate>
                <asp:Panel ID="PanelCargando" runat="server" CssClass="FormatoPanelView" Height="40px" Width="64px" Style="left: 1030px; top: 42px; background-color: transparent;">
                    <asp:Image ID="imgCargando" runat="server" ImageUrl="/Imagenes/gifCargandoHorizontal30x60.gif" Style="position: absolute; left: 0px; top: 0px;" />
                </asp:Panel>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </form>

    <script src="/Scripts/js/jquery.min.js"></script>
    <script src="/Scripts/js/bootstrap.bundle.min.js"></script>
    <!-- DATATABLES -->
    <script src="/Scripts/plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="/Scripts/plugins/datatables/dataTables.bootstrap4.min.js"></script>
    <script src="/Scripts/plugins/datatables/dataTables.buttons.min.js"></script>
    <script src="/Scripts/plugins/datatables/buttons.bootstrap4.min.js"></script>
    <script src="/Scripts/plugins/datatables/jszip.min.js"></script>
    <script src="/Scripts/plugins/datatables/buttons.html5.min.js"></script>
    <script src="/Scripts/plugins/datatables/dataTables.responsive.min.js"></script>
    <script src="/Scripts/plugins/iziToast/js/iziToast.min.js"></script>
</body>
</html>

