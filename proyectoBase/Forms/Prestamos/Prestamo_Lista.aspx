﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Prestamo_Lista.aspx.cs" Inherits="Prestamos_Prestamo_Lista" %>


<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <title>Mis prestamos</title>
    <!-- BOOTSTRAP -->
    <link href="/CSS/Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/CSS/Content/css/icons.css" rel="stylesheet" />
    <link href="/CSS/Content/css/style.css" rel="stylesheet" />
    <!-- ARCHIVOS NECESARIOS PARA EL FUNCIONAMIENTO DE LA PAGINA -->
    
    <link href="/Scripts/plugins/datatables/dataTables.bootstrap4.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/datatables/buttons.bootstrap4.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/datatables/responsive.bootstrap4.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/iziToast/css/iziToast.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/datapicker/datepicker3.css" rel="stylesheet" />
    <link href="/CSS/Estilos_CSS.css" rel="stylesheet" />
    <style>
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
                    <h6>Cartera de prestamos</h6>
                </div>
                <div class="col-4">
                    <input id="txtDatatableFilter" class="float-right form-control w-75" type="text" placeholder="Buscar"
                        aria-label="Buscar" />
                </div>
            </div>
        </div>
        <div class="card-body">

            <div class="form-inline justify-content-center">
                <div class="form-group mb-2">
                    <label class="form-control-plaintext">Estado</label>
                </div>
                <div class="form-group mx-sm-3 mb-2">
                    <select id="estadoFiltro" class="form-control form-control-sm">
                        <option value="" selected="selected">Todos</option>
                        <option value="Pendiente">Pendiente</option>
                        <option value="Vigente">Vigente</option>
                        
                       
                    </select>
                </div>

                <div class="form-group mb-2">
                    <label class="form-control-plaintext">Búsqueda por Mes</label>
                </div>
                <div class="form-group mx-sm-3 mb-2">
                    <select id="mesIngreso" class="form-control form-control-sm">
                        <option value="" selected="selected">Seleccionar</option>
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

                <div class="form-group mb-2">
                    <label class="form-control-plaintext">Búsqueda por Año</label>
                </div>
                <div class="form-group mx-sm-3 mb-2">
                    <input id="añoIngreso" class="form-control form-control-sm" type="text" />
                </div>

                <div class="form-group mb-2">
                    <label class="form-control-plaintext">Búsqueda por Fecha</label>
                </div>
                <div class="form-group mx-sm-3 mb-2 col-sm-2">
                    <div class="input-daterange input-group" id="date-range">
                        <input type="text" class="form-control form-control-sm" name="min" id="min" />
                        <input type="text" class="form-control form-control-sm" name="max" id="max" />
                    </div>
                </div>
            </div>

            <table id="datatable-bandeja" class="table-bordered display compact nowrap table-condensed table-hover dataTable" style="width: 100%" role="grid">
                <thead>
                    <tr>
                        <th>Cliente</th>
                        <th>Sol.</th>
                        <th>Prestamo</th>
                        <th>ID.Prod.</th>
                        <th>Producto</th>
                        <th>Fec.Prestamo</th>
                        <th>Capital</th>
                        <th>Saldo</th>
                        <th>Estado</th>
                    </tr>
                </thead>
                <tbody>
                </tbody>
            </table>
        </div>
    </div>

    <!--modal de confirmacion de ver la Prestamo condicionada -->
    <div id="modalActualizarPrestamo" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="modalActualizarPrestamoLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title mt-0" id="modalActualizarPrestamoLabel">Actualizar Prestamo de crédito</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                </div>
                <div class="modal-body">
                    ¿Desea revisar la información de este prestamo?
                </div>
                <div class="modal-footer">
                    <button id="btnActualizar" class="btn btn-primary waves-effect waves-light">
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
    <script src="/Scripts/plugins/moment/moment.js"></script>
    <script src="/Scripts/plugins/moment/moment-with-locales.min.js"></script>
    <script src="/Scripts/plugins/datapicker/bootstrap-datepicker.js"></script>
    <script src="/Scripts/plugins/accounting/accounting.min.js"></script>

    <script src="/Scripts/app/Prestamos/Prestamos_Lista.js?V=20201103144650"></script>

   
</body>
</html>
