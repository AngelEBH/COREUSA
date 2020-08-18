<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContratoVehiculoClausulaLicencia.aspx.cs" Inherits="proyectoBase.Forms.Otros.ContratoVehiculoClausulaLicencia" %>

<!DOCTYPE html>
<html lang="es" xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="/Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/sweet-alert2/sweetalert2.min.css" rel="stylesheet" />
    <style type="text/css">
        html * {
            font-family: "DejaVu Sans", "Arial", sans-serif !important;
        }

        p {
            font-size: 16px !important;
        }

        label {
            font-size: 16px !important;
        }

        .Negrita {
            font-weight: bold !important;
        }

        @font-face {
            font-family: "DejaVu Sans";
            src: url("/CSS/Content/fonts/DejaVu/DejaVuSans.ttf") format("truetype");
        }
    </style>
</head>
<body>
    <div class="container-fluid" id="GenerarConvenio">
        <div class="row">
            <div class="col-md-12">
                <div class="form-group row">
                    <div class="col-sm-12">
                        <img alt="" src="/Imagenes/logoPrestadito.png" />
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-12 justify-content-center">
                        <h4 class="text-center">CONTRATO DE CRÉDTIO PARA COMPRA DE VEHÍCULO
                        </h4>
                    </div>
                </div>
            </div>
            <div class="col-md-12">
                <div class="form-group row">
                    <div class="col-sm-12">
                        <p>
                            Nosotros, <b>ERICK GEOVANY MOLINA PADILLA,</b> Casado, Ingeniero Industrial, con domicilio en la ciudad de 
                            San Pedro Sula, Departamento de Cortés, quien actúan en su condición de Representante Legal de la Sociedad Mercantil denominada <b>PRESTADITO S.A. de C.V.</b> empresa domiciliada en la ciudad de San Pedro Sula, departamento de Cortes,
                            llamada en adelante <b>PRESTADITO o PRESTAMISTA</b>; y el Sr(a) JOSE RAMIRO  RUBIO CHAVARRIA, mayor de edad, de Nacionalidad Hondureña y de este domicilio, con identidad No. 0301199800931 Con domicilio y
                            dirección en COLONIA FUERZAS ARMADAS, COMAYAGUA, COMAYAGUA, CALLE PRINCIPAL, UNA CUADRA AL NORTE DE LA PULPERIA SOFIA, CASA COLOR MOSTAZA, llamado en adelante <b>EL CLIENTE, PRESTATARIO y/o DEUDOR</b>,
                            convienen celebrar el siguiente <b>CONTRATO DE CREDITO PARA COMPRA DE VEHICULO</b>  y acuerdan lo estipulado en las siguientes clausulas:
                            <b><u>PRIMERO:</u> OBJETO DEL CONTRATO.- EL CLIENTE</b> declara recibir en este acto de  <b>PRESTADITO</b>, un préstamo por la cantidad de  
                            <b>TREINTA Y OCHO MIL, NOVECIENTOS NOVENTA Y NUEVE CON 00/100</b> Lempiras (L.38,999.00) moneda de curso legal en Honduras.<br />
                            <b>-SEGUNDO: CONDICIONES DEL FINANCIAMIENTO.-</b> El préstamo se facilita bajo las siguientes condiciones: <b>A) DESTINO: EL CLIENTE</b> acepta, reconoce y autoriza que la cantidad recibida en préstamo será para compra de:
                        </p>
                        <table class="table table-bordered">
                            <thead>
                                <tr>
                                    <th class="text-center">CANTIDAD</th>
                                    <th class="text-center">DESCRIPCIÓN DEL VEHÍCULO A COMPRAR</th>
                                    <th class="text-center">VALOR</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td class=""></td>
                                    <td class="">
                                        <b>Marca</b>: BAJAJ <b>modelo:</b> PULSAR NS 125 <b>año:</b> 2020 <b>cilindraje:</b> 125 <b>color:</b> NEGRO MULTICOLOR 
                                        <b>tipo: </b>MOTOCICLETA <b>motor:</b> JEYWKC38252
                                    </td>
                                    <td class="text-center">38,999.00
                                    </td>
                                </tr>
                            </tbody>
                            <tfoot>
                                <tr>
                                    <td colspan="2" class="text-right">PRECIO</td>
                                    <td colspan="1" class="text-center">38,999.00</td>
                                </tr>
                                <tr>
                                    <td colspan="2" class="text-right">PRIMA</td>
                                    <td colspan="1" class="text-center">4,000.00</td>
                                </tr>
                            </tfoot>
                        </table>
                        <br />
                        <p>
                            Mismo que será desembolsado por <b>PRESTADITO</b> a la persona que distribuya o sea propietario del vehículo y este ultimo deberá de entregárselo al Cliente
                            cuando sea autorizado por <b>PRESTADITO</b>. 
                            <b>- B) COMISIONES.-TASAS DE INTERES.-COSTO ANUAL TOTAL.- EL CLIENTE</b> se obliga a pagar a <b>PRESTADITO</b>, a partir de esta fecha una tasa de
                            interés fluctuante del <b>3.16% PORCIENTO MENSUAL</b>, pagadero quincenal en moneda de curso legal en Honduras, sobre el saldo total de la deuda. 
                            <b>Por la falta de pago</b> a su vencimiento de cualquiera de los abonos a capital, intereses y/o comisiones, <b>EL CLIENTE</b> pagará intereses moratorios 
                            del <b>4.31% PORCIENTO MENSUAL</b>  sobre el saldo de capital vencido, por razón de daños y perjuicios hasta que se cancele la totalidad de la 
                            mora, sin que deba considerarse prorrogado el plazo.<b>-COSTO ANUAL TOTAL:</b> El costo anual total del préstamo (CAT) es de <u>38%</u>  incluye
                            el cobro y pago del capital, los intereses ordinarios, y bajo la condición que <b>EL CLIENTE</b> cumpla con sus obligaciones en las formas y plazos convenidos.
                            El valor de pago del presente contrato se describe a continuación
                        </p>

                        <table class="table table-bordered">
                            <thead>
                                <tr>
                                    <th class="text-center">VALOR DEL CONTRATO</th>
                                    <th class="text-right">L. 67,057.92</th>
                                    <th class="text-center">PLAZO</th>
                                    <th class="text-right">48 cuotas quincenales</th>
                                </tr>
                                <tr>
                                    <th class="text-center">TOTAL DE GTOS. ADMINISTRADTIVOS Y FINAN.</th>
                                    <th class="text-right">L. 0</th>
                                    <th class="text-center">DIVIDIDO ASÍ</th>
                                    <th class="text-right">47 de L. 1,283.30
                                        <br />
                                        1 de L. 1,283.14</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td colspan="3" class="text-right">SEGURO</td>
                                    <td class="text-right">48  Cuotas de Lps 113.75
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" class="text-right">GPS</td>
                                    <td class="text-right">48  Cuotas de Lps 0.00
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" class="text-right">CUOTA TOTAL</td>
                                    <td class="text-right">Lps 1397.04
                                    </td>
                                </tr>
                            </tbody>
                        </table>

                        <p>
                            <b>C) EL PLAZO DEL FINANCIAMENTO:</b> El plazo de este préstamo es de 48 cuotas (quincenales) dividido así: 47 cuotas de 1,283.30 y una (1) cuota
                            de  1,283.14 (L. 67.057.92) mas el seguro y GPS correspondientes, los cuales se  detallan en el cuadro anterior
                            y en el plan de pago, debiendo hacer efectivo el pago de la primera cuota el 30 de septiembre de 2019 y así sucesivamente de forma <u>quincenal</u>
                            hasta la completa cancelación de la deuda. 
                            <b>D) DE LA FORMA, DE LA MONEDA Y LUGAR DE PAGO:</b> Los contratantes acuerdan que: <b>I)</b>
                            Los abonos se harán primero a gastos, comisiones y cargos que pudieran haberse causado, luego los intereses, y el saldo, si l hubiera, a capital; 
                            <b>- II)</b> El pago del préstamo se hará en la moneda pactada y en efectivo.<b>- III)</b> El pago se realizará conforme a lo 
                            establecido en el plan de pagos, en el caso que la fecha de pago sea día feriado, entonces deberá realizarse el día hábil inmediato
                            anterior, en las oficinas, agencias, sucursales y ventanillas de  <b>PRESTADITO</b>, o en cualquier otra institución tercerizada que
                            se designe oportunamente en virtud de convenios de cobro de cartera. <b>- E) PAGO ANTICIPADO:</b> En caso de pago total de la obligación antes de
                            su vencimiento, <b>EL CLIENTE</b> deberá pagar una comisión de prepago del dos por ciento (2%) sobre el saldo adeudado, y si
                            es un pago parcial a capital superior al diez por ciento (10%) del monto adeudado, también pagará dicha comisión calculada sobre el monto a pagar.
                            Esta condición aplicara únicamente cuando el saldo del capital adeudado exceda cien mil dólares ($100,000.00)
                            o su equivalente en lempiras, o los fondos 
                            sean provenientes de una institución que penalice a  <b>PRESTADITO</b>  por pago anticipado, cualquiera de las dos o ambas conjuntamente.
                            <b>- F) PROPIEDAD DEL VEHICULO.</b> Mientras no se haya cancelado la totalidad del Préstamo, será dueño del vehículo <b>PRESTADITO</b>, el cliente será
                            considerado como poseedor y esta posesión esta condicionada, es decir si el cliente está cumpliendo con las obligaciones contraídas
                            en este contrato, de lo contrario <b>PRESTADITO</b> podrá a su discreción retirarlo. 
                            <b>- G) OTROS GASTOS:</b> Los gastos que se incurra por mantenimiento, reparación
                            y todas las relacionadas para conservación del vehículo en perfecto estado, deberá ser pagadas por el <b>CLIENTE</b>, así como también los gastos que se
                            ocasione en la recuperación del vehículo en caso de mora. 
                            <b>- TERCERO: AUTORIZACIONES ESPECIALES: EL CLIENTE</b> por este acto, en tanto no haya cumplido
                            con el pago total de su obligación, autoriza a  <b>PRESTADITO</b>  expresamente y sin ser necesario la notificación previa para:
                            <b>A)</b> Vender, Ceder o de cualquier otra forma traspasar, o celebrar contratos de participación, de descuentos con relación al crédito y derechos consignados
                            en este documento o títulos valores relacionados a este mismo;
                            <b>B)</b> Autorizar a  <b>PRESTADITO</b>  para que en cualquier tiempo pueda acceder a la información de la Central de Riesgos de la Comisión Nacional
                            de Bancos y Seguros u otra central de riesgo pública o privada, para gestionar
                            y conocer la situación crediticia de <b>EL CLIENTE</b> frente a las demás instituciones del sistema financiero nacional.
                            <b>- C) EL CLIENTE</b> Autoriza de manera Irrevocable, a que <b>PRESTADITO</b> pueda entrar en su domicilio, para solo efecto de retirar el vehículo comprado con este préstamo, o que
                            lo retire de una tercera persona sin necesidad de intervención judicial, esta cláusula solo se ejecutara en caso de mora de 2 o más cuotas vencidas y mientras no haya sido cancelado el total adeudado.
                            <b>- CUARTO: OBLIGACIONES GENERALES.- EL CLIENTE</b> durante la vigencia del presente contrato
                            también se obliga a: <b>A)</b> Permitir que  PRESTADITO  ejerza los controles que juzgue convenientes, para asegurarse que los fondos de este crédito se inviertan en los fines que se han indicado anteriormente
                            y condiciones que se estipulan en este contrato.
                            <b>- B) DE LA GARANTÍA:</b> En calidad
                            de Garantía para el Cumplimiento de la presente obligación <b>El CLIENTE</b> firmara una <b>PAGARE</b> sin protesto, así como también da en propiedad a <b>PRESTADITO</b> el vehículo comprado
                            con el dinero objeto del presente préstamo, quedándose <b>PRESTADITO</b> con la documentación original del vehículo y el
                            <b>CLIENTE</b> en posesión del vehículo del cual será responsable mientras se encuentre en su poder y no se haya cancelado el precio total pactado para la terminación del presente contrato.
                            Sin perjuicio de la <b>designación de garantías fiduciarias como ser Menaje de Hogar y demás bienes
                            pertenecientes AL CLIENTE</b>,  por lo que está terminantemente prohibido para el <b>CLIENTE</b> enajenar, vender, permutar, donar, gravar, prestar o dar en prenda el vehículo dado en propiedad, sin la autorización
                            por escrito otorgada por <b>PRESTADITO</b>, el incumplimiento de las prohibiciones faculta
                            a <b>PRESTADITO</b> a retirar el vehículo.  Para el menaje se formalizará el Inventario de estos, este que pasara a formar parte del presente contrato.
                            <b>C)</b> Suscribir y a mantener un seguro para 
                            vehículos en lempiras moneda de curso legal en Honduras; mientras esté vigente la deuda, por la cuantía
                            y condiciones que señale  <b>PRESTADITO</b>, con una compañía aseguradora; siendo entendido que <b>EL CLIENTE</b> deberá endosar a favor de <b>PRESTADITO</b> la respectiva póliza de seguro, o a favor
                            de la persona natural o jurídica a cuyo nombre se traspase el presente crédito, hasta la total cancelación del
                            saldo pendiente de pago por la deuda.  <b>PRESTADITO</b>  podrá pagar y cargar al préstamo las primas de seguro, si <b>EL CLIENTE</b> no lo renueva y paga a los treinta (30) días previos
                            al vencimiento de la póliza de seguro respectiva, sin que la acción del pago o cargo sea obligatorio para  <b>PRESTADITO</b>,
                            quien no asumirá ni incurrirá en responsabilidad por no hacer el pago de las primas de seguro. <b>D)</b> Mantenerse al día en el pago de los impuestos que graven a 
                            <b>EL CLIENTE</b> o al <b>VEHÍCULO</b> dado en garantía.
                            <b>- E)</b> Cuidar como buen padre de familia el vehículo dado en garantía, mientras se encuentre
                            en su poder y no se haya cancelado el precio total pactado para la terminación del presente contrato, quedando a su cargo los riesgos de dicho bien mueble por lo que será responsable de la perdida, destrucción o deterioro que sufra aun por caso fortuito o fuerza mayor.
                            <b>- F)</b> Mantener la licencia de conducir vigente, mientras no se haya cancelado la totalidad del préstamo, en caso que <b>EL CLIENTE</b> haya solicitado excepción al momento de otorgarse 
                            el Préstamo por no poseer licencia vigente, entonces dispondrá solamente de un máximo de 30 días para presentar
                            la Licencia de conducir a <b>PRESTADITO</b>, caso contrario <b>AUTORIZA</b> anticipadamente a <b>PRESTADITO</b> a que se le retire, en calidad de custodia, la motocicleta hasta
                            que presente la licencia de conducir aun y cuando sus cuotas estén al día. La excepción anterior <b>no faculta al CLIENTE</b> a conducir
                            la motocicleta sin su respectiva Licencia emitida por la Dirección Nacional de Transito, ni a prestarla a quien no tenga dicho documento, ya que <b>PRESTADITO</b> respeta las leyes hondureñas.
                            <b>- QUINTO: DE LOS DEBERES DEL CLIENTE:</b> Se conviene que, desde la fecha de otorgamiento de este contrato, hasta la fecha en que se pague el total de las obligaciones pendientes con  
                            <b>PRESTADITO, EL CLIENTE</b> deberá informar siempre, por vía telefónica o escrita y a la brevedad posible, las siguientes acciones: 
                            <b>1)</b> Contraiga deudas con otras instituciones financieras, no financieras, puestos de bolsa, proveedores, filiales y otros. <b>EL CLIENTE</b> aprueba libre y voluntariamente por ser válidas,
                            todas las condiciones fijadas en este inciso, por entender que de tal manera  <b>PRESTADITO</b>  se asegura de la solvencia de <b>EL CLIENTE</b> y del pago del crédito otorgado.
                            <b>- SEXTO: DE LAS MODIFICACIONES DEL CONTRATO.-  PRESTADITO</b>  comunicará a <b>EL CLIENTE: 1)</b> De manera general y sin necesidad de especificarlo individualmente,
                            las condiciones contractuales pactadas, por cualquier medio impreso de circulación nacional, o el medio de comunicación que las partes hayan designado, en los casos de los efectos de la aplicación
                            de la vigencia de una ley, con 30 días calendario de anticipación a la aplicación de dicho cambio; <b>2)</b> Para el caso que las tasas de intereses y otros cargos sea modificada, se aplicará conforme
                            a un factor variable que considera la tasa de interés que se concede para los depósitos a plazo, más el costo de la intermediación y sumándole un diferencial del veinte por ciento.
                            La tasa de interés se revisará cada 3 meses. En el caso que las tasas de interés sean reguladas por el Banco Central de Honduras conforme al artículo 55 de la Ley del Sistema Financiero,
                            se aplicará la tasa máxima permitida por dicha Institución, o la que fije y aplique <b>PRESTADITO</b>, notificándolo a <b>EL CLIENTE</b>  con 15 días calendario de anticipación por lo menos
                            por cualquiera de los medios de comunicación descritos en este contrato o los otros establecidos por la Ley, siendo entendido que cualquier ajuste resultante de la modificación de la tasa de interés será cubierto
                            por <b>EL CLIENTE</b> quedando  <b>PRESTADITO</b>  autorizado para efectuar y cobrar tales ajustes y modificar la cuota quincenal del financiamiento de acuerdo al plazo que reste
                            para la cancelación del mismo, así mismo las partes acuerdan incorporar como vinculante el principio <b><em>“ceteris paribus”</em></b>, respecto a modificaciones atinentes
                            al contrato o los convenios incorporados.
                            <b>-SEPTIMO: RECLAMOS.</b>- Cuando se presente algún evento por el cual <b>EL CLIENTE</b> desee hacer un reclamo, se dispondrá de un plazo de 10 días hábiles para realizarlo, transcurrido éste,
                            es entendido que caduca su derecho para reclamar y se declara vencido. Cuando sea reclamos por cuestiones de garantía deberá presentarlas al distribuidor autorizado y en caso de ser bienes usados
                            no podrá presentar reclamos después de 30 dias de realizada la compra, es entendido que <b>PRESTADITO</b> no está obligado a resolver cuestiones de garantía puesto que solo es quien financia la compra.
                            <b>- OCTAVO: DEL VENCIMIENTO ANTICIPADO DEL PLAZO DE PAGO.-</b> Además de los casos establecidos por la ley,  <b>PRESTADITO</b> podrá dar por vencido el plazo establecido para el pago del préstamo concedido
                            en este contrato, y en consecuencia exigir el pago inmediato del saldo del capital, intereses, comisiones, recargos y gastos, ya sea por la vía judicial o extra judicial, por cualquiera de los siguientes eventos:
                            <b>a)</b> Por falta de pago de dos o más de las cuotas pactadas, de los intereses, o de cualquier otro cargo pendiente a favor de  <b>PRESTADITO; b)</b> Por el conocimiento de la ejecución judicial
                            iniciada por terceros, o por el mismo  <b>PRESTADITO</b>, en contra de <b>EL CLIENTE</b>, originada por otros créditos; <b>c)</b> Por no destinar el presente préstamo para el fin o fines para los cuales ha sido
                            concedido; <b>d)</b> Por la declaración del estado de suspensión de pagos, de quiebra o de concurso de <b>EL CLIENTE</b>, así como por su inhabilitación para el ejercicio del comercio, o por el ejercicio de acción
                            penal en su contra o de su representante legal que derivare en sentencia de privación de libertad; <b>e)</b> Por el   incumplimiento o negativa por parte de <b>EL CLIENTE</b> a proporcionar la información requerida por
                            <b>PRESTADITO</b>  en forma escrita; <b>f)</b> Por actuación fraudulenta o haber proporcionado a  <b>PRESTADITO</b>  información o datos falsos o incompletos para obtener el préstamo; <b>g)</b> Por ser del conocimiento de
                            <b>PRESTADITO</b>, la existencia de obligaciones de <b>EL CLIENTE</b> pendientes de pago con el Estado, en tal cantidad que a su criterio ponga en peligro la recuperación de los adeudos debido a la preferencia del Estado
                            para obtener el pago a su favor antes que  <b>PRESTADITO; h)</b> El incumplimiento de parte de <b>EL CLIENTE</b> de cualquiera de las obligaciones contraídas en este contrato.
                            <b>- NOVENO: COBROS EXTRAJUDICIALES.-</b> En caso de ser necesarias las gestiones de cobranzas extrajudiciales por la mora en el pago o el vencimiento anticipado del contrato,
                            estas se realizarán de la siguiente manera: <b>1)</b> Para Mora de 1 a 180 días: alternativamente podrán ser llamadas telefónicas, correos electrónicos, mensajes por cualquier medio electrónico,
                            visitas por gestores, cartas de cobro escritas solicitando el pago y dirigidas a las direcciones indicadas. Estas gestiones tendrán un costo de doscientos cincuenta lempiras (L250.00), cargados a estado
                            de cuenta del préstamo otorgado, son acumulables por cada cuota vencida y serán pagados por <b>EL CLIENTE</b> en todos los casos y sin excepción; <b>2)</b> Si su caso fuere trasladado a Profesionales del Derecho,
                            cuyas gestiones iniciales podrán ser: llamadas telefónicas, envió de correos electrónicos, cartas de cobro escritas, y visitas, causaran el cobro de honorarios profesionales según el Arancel del Profesional del
                            Derecho vigente, y se calculará sobre el capital, intereses, comisiones, cargos y seguros en mora, tal como lo establece el artículo 1432 del Código Civil.- En caso de ser perseguida la deuda por proceso Judicial,
                            se cargaran igualmente los gastos ocasionados por costas durante dicho proceso.  <b>PRESTADITO</b>  podrá asignar a una empresa o Agencia de Cobranzas y/o Recuperaciones para  que realice  estas  labores  de  cobro
                            desde  el día uno de  atraso en  el estado de cuenta lo cual es aceptado por el deudor.
                            <b>- DÉCIMO: ACCIONES JUDICIALES.- </b>En caso de mora o vencimiento anticipado del contrato, dará lugar para que <b>PRESTADITO</b> ejerza las acciones judiciales correspondientes, quedando obligado el cliente
                            a pago de gastos y honorarios que ocasione el procedimiento judicial. Así como para determinar el saldo adeudado El estado de cuenta certificado por el contador de <b>PRESTADITO</b> o de quien haya adquirido los derechos,
                            hará fe en juicio para establecer el saldo a cargo de <b>EL CLIENTE</b> y Constituirá junto con el presente contrato título ejecutivo, sin necesidad de reconocimiento de firma ni de otro requisito previo alguno,
                            según lo establecido en la ley del sistema financiero. En caso de ejecución de la presente obligación las partes nos sometemos a la jurisdicción y competencia de los Juzgados de San Pedro Sula, Cortés
                            <b>.- DÉCIMO PRIMERA: <em>MEDIOS PARA COMUNICACIONES</em>.- EL CLIENTE y  PRESTADITO</b>   establecen y a la vez autorizan, que para las distintas notificaciones que se deban hacer conforme a lo estipulado por este contrato
                            o por lo dispuesto por la ley, se harán efectivas a través de uno solo de los siguientes medios: <b>A)</b> Correspondencia ordinaria escrita dirigida a las direcciones indicadas en el preámbulo de este contrato; 
                            <b>B)</b> Notificación por la vía electrónica a su correo electrónico info@alorica.com; <b>C)</b> Notificación mediante cualquier red sociales que pudiese pertenecer al Cliente, <b>D)</b> o a las direcciones indicadas
                            en cualquiera de los documentos suscritos con <b>“ PRESTADITO ”</b>. Cualquier cambio de dirección o número telefónico deberá notificarse fehacientemente, con una anticipación razonable a <b>PRESTADITO</b> y 
                            hasta entonces se considera efectiva.<b>-DÉCIMO SEGUNDA: DE LAS AUTORIZACIONES ESPECIALES. EL CLIENTE</b>  otorga de manera expresa, voluntaria e irrevocable su consentimiento para que en caso de mora,
                            <b>PRESTADITO</b> o sus representantes puedan ingresar a su domicilio a retirar el vehículo, y por lo tanto lo exime de toda responsabilidad que pueda incurrir según el artículo 99 de la Constitución de la Republica.
                            Así como faculta a <b>“ PRESTADITO ”</b>, sus distintas dependencias, así como también a su personal, que mediante visitas a su domicilio se le puedan presentar y ofrecer las diferentes propuestas de negocio,
                            servicios, catálogos de nuevos productos; a su vez, faculta otros canales, sean estos telefónicos o electrónicos, a que se comuniquen y a que le informen en los días de semana, así como también en los días llamados
                            vacaciones, o festivos, en los diferentes horarios abiertos, incluso fin de semana, exonerándole de cualquier perjuicio a la empresa o de ser estas visitas catalogadas como “hostigamiento”.
                            <b>- El CLIENTE:</b> autoriza de manera expresa y voluntaria que en caso  de que PRESTADITO retire el vehículo, pueda ser subastado al mejor postor y el dinero recibido de la misma se abonara a la deuda,
                            si existiera un excedente se le dará al <b>CLIENTE</b> y en caso que no cubriese el total adeudado, <b>PRESTADITO</b> se reserva el derecho de ejercer acciones legales contra el <b>CLIENTE</b> por el pago de saldo total adeudado,
                            que incluye capital, intereses y otros cargos gastos o cargos que incurra.
                            <b>- DÉCIMO TERCERA: COMPROBACION DE HABER RECIBIDO INSTRUCCIÓN Y ORIENTACION DEL PRODUCTO Y ENTREGA DE COPIA DEL CONVENIO Y PLAN DE PAGO.- EL CLIENTE</b> por este acto acepta que previo a la celebración de este contrato,
                            ha recibido toda la orientación y explicación necesaria sobre las condiciones del convenio, las consecuencias legales y judiciales de su incumplimiento, así como que ha recibido una copia íntegra de este documento
                            y del plan de pagos respectivo.- Finalmente las partes declaramos que es cierto todo lo anteriormente expresado, y que por ser ello lo convenido, aceptamos libre y voluntariamente, todas estipulaciones, condiciones
                            y cláusulas contenidas en el presente contrato de préstamo. En fe de lo cual firmamos en la Ciudad de <b>SAN PEDRO SULA, a los 20 días del mes de septiembre del año 2020. </b>
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                        </p>
                    </div>

                </div>
            </div>

            <div class="col-md-12">
                <div class="form-group row justify-content-center">
                    <div class="col-sm-4">
                        <div class="border border-dark"></div>
                    </div>
                    <div class="col-sm-1"></div>
                    <div class="col-sm-4">
                        <div class="border border-dark"></div>
                    </div>
                    <div class="col-sm-4 text-center">
                        <label style="font-size: 18px !important;" class="text-center">JOSE RAMIRO RUBIO CHAVARRIA<br />
                            0301199800931<br />
                            EL CLIENTE</label>
                    </div>
                    <div class="col-sm-1"></div>
                    <div class="col-sm-4 text-center">
                        <label style="font-size: 18px !important;">ERIC GEOVANY MOLINA PADILLA<br />
                            PRESTADITO</label>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script src="/Scripts/js/jquery.min.js"></script>
    <script src="/Scripts/js/bootstrap.bundle.min.js"></script>

    <script>
        $(function () {

            window.print();
        });
    </script>
</body>
</html>
