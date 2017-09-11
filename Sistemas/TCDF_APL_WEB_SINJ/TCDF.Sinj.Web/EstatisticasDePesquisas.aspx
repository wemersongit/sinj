<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="EstatisticasDePesquisas.aspx.cs" Inherits="TCDF.Sinj.Web.EstatisticasDePesquisas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <style type="text/css">
        table th, table td{padding:5px 0px;}
        table caption{background-color: #CCC; font-weight:bold; padding:5px}
        table thead tr{background-color: #DDD;}
        table tr.EEE{background-color: #EEE;}
        .estatisticas .filtro {
            width: 47%;
            float: left;
            padding-right: 10px;
        }
    </style>
    
    <script type="text/javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/jquery.printElement.js"></script>
    <script type="text/javascript" src="<%= TCDF.Sinj.Util._urlPadrao%>/Scripts/html2canvas.js"></script>
    <script type="text/javascript" src="<%= TCDF.Sinj.Util._urlPadrao%>/Scripts/jspdf.min.js"></script>
    <script type="text/javascript" src="<%= TCDF.Sinj.Util._urlPadrao%>/Scripts/jspdf.plugin.autotable.js"></script>
    <script type="text/javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/chart.js"></script>
    <script type="text/javascript">
        var charts = {};

        var jOptionsDatatable = {
            "aLengthMenu": [[10, 25, 50, 100, -1], [10, 25, 50, 100, "Todos"]],
            "ordering": false,
            "oLanguage": {
                "oPaginate": { "sFirst": "<<", "sLast": ">>", "sNext": ">", "sPrevious": "<" },
                "sEmptyTable": "Não foram encontrados registros",
                "sInfo": "<span>Exibindo de <b>_START_</b> até <b>_END_</b> de <b>_TOTAL_</b> registros encontrados.</span>",
                "sInfoEmpty": " ",
                "sInfoFiltered": "",
                "sInfoThousands": ".",
                "sLengthMenu": "Exibir _MENU_ registros",
                "sLoadingRecords": "Carregando...",
                "sProcessing": "<span><b>Processando...</b></span>",
                "sSearch": "Filtrar",
                "sZeroRecords": "Não foram encontrados registros"
            }
        }

        function selectRange(el) {
            if ($(el).val() == 'intervalo') {
                $('div.interval', $(el).closest('div.line')).show();
            }
            else {
                $('div.interval', $(el).closest('div.line')).hide();
            }
        }

        function clickAba(el) {
            var target = el.href.split('#')[1];
            if(!$('#'+target).hasClass('loaded')){
                listarAggs($('#' + target + ' form').attr('id'));
                $('#' + target).addClass('loaded')
            }
        }

        function printPDFChart(nm_grafico, div_chart){
            var pdf = new jsPDF('landscape', 'pt');
            var measureDefault = { width: pdf.internal.pageSize.width - 30, height: pdf.internal.pageSize.height - 80 }

            html2canvas($('#' + div_chart), {
                onrendered: function (canvas) {
                    var imgData = canvas.toDataURL('image/png');
                    pdf.text(20, 40, nm_grafico);
                  
                    pdf.addImage(imgData, 'PNG', 15, 70, measureDefault.width, 0);
                    pdf.save(nm_grafico + '.pdf');
                }
            });
        }


        function printPDFTable(nm_grafico, div_table) {

            var pdf = new jsPDF('landscape', 'pt');

            var res = pdf.autoTableHtmlToJson(document.getElementById(div_table));
            var options = {
                styles: {
                    textColor: 20                     
                },
                headerStyles: {
                    fillColor: [200,200,200]
                },
                bodyStyles: {},
                alternateRowStyles: {
                    columnWidth: 'auto',
                    lineWidth: 'auto'
                },
                columnStyles: {
                    columnWidth: 'auto',
                    lineWidth: 'auto'
                },

                pageBreak: 'auto', // 'auto', 'avoid' or 'always'
                tableWidth: 'auto',
                startY: 70,
            };
            pdf.autoTable(res.columns, res.data, options);
            pdf.save(nm_grafico + '.pdf');
        }

        function listarAggs(id_form) {
            var nm_agg = "";
            if (IsNotNullOrEmpty(id_form)) {
                nm_agg = document.forms[id_form].nm_agg.value;
            }
            try {
                var sucesso = function (data) {
                    if (IsNotNullOrEmpty(data, 'aggregations')) {
                        var aggs = ["agg_termos", "agg_tipos", "agg_dt_historico"];
                        if (IsNotNullOrEmpty(nm_agg)) {
                            aggs = [nm_agg];
                        }

                        for (var a = 0; a < aggs.length; a++) {
                            //$('#super_loading').hide();
                            var agg = aggs[a];
                            var tbody = '';
                            var oAgg = eval('data.aggregations.' + agg);

                            $('.' + agg).removeClass('scroll');
                            $('#tfoot_' + agg).html('');

                            $("#table_" + agg).DataTable().destroy();

                            if (IsNotNullOrEmpty(oAgg, 'buckets') && oAgg.buckets.length > 0) {
                                var labels_chart = [];
                                var data_chart_sum_value = [];
                                var data_chart_doc_count = [];
                                if (oAgg.buckets.length > 10) {
                                    $('.' + agg).addClass('scroll');
                                }
                                var iTotal = 0;
                                var i = 0;
                                var maior = 0;
                                var searchLink = '';
                                var sum_value = 0;
                                var doc_count = 0;
                                for (; i < oAgg.buckets.length; i++) {
                                    var link = '';
                                    var key = IsNotNullOrEmpty(oAgg.buckets[i].key_as_string) ? oAgg.buckets[i].key_as_string : oAgg.buckets[i].key;
                                    doc_count = oAgg.buckets[i].doc_count;
                                    sum_value = oAgg.buckets[i].agg_sum.value;

                                    labels_chart.push(key);

                                    data_chart_sum_value.push(sum_value);
                                    data_chart_doc_count.push(doc_count);

                                    if (agg == 'agg_termos') {
                                        searchLink = '?termo=' + key;
                                    }
                                    else if (agg == 'agg_tipos') {
                                        searchLink = '?nm_tipo_pesquisa=' + key;
                                    }
                                    else if (agg == 'agg_dt_historico') {
                                        searchLink = '?dt_historico=' + key;
                                    }
                                    link = '<a title="visualizar pesquisas" href="./ResultadoDePesquisaPesquisas.aspx' + searchLink + '" ><img src="' + _urlPadrao + '/Imagens/ico_loupe_p.png" /></a>';

                                    //$('#tbody_resultado').append('<tr class="' + (i % 2 == 0 ? "EEE" : "") + ' termo"><td>' + data.aggregations.agg_termos.buckets[i].key + '</td><td style="text-align:center;">' + data.aggregations.agg_termos.buckets[i].doc_count + '</td><td>' + link + '</td></tr>');

                                    tbody += '<tr class="' + (i % 2 == 0 ? "EEE" : "") + ' ' + agg + '"><td>' + key + '</td><td class="text-center">' + sum_value + '</td><td class="text-center">' + doc_count + '</td><td>' + link + '</td></tr>';

                                    iTotal += sum_value;
                                    if (sum_value > maior) {
                                        maior = sum_value;
                                    }
                                    if (doc_count > maior) {
                                        maior = doc_count;
                                    }

                                }
                                //$('#tfoot_' + agg).html('<tr class="' + (i % 2 == 0 ? "EEE" : "") + ' ' + agg + '"><td class="bold">TOTAL</td><td colspan="2" class="text-center">' + iTotal + '</td></tr>');
                                $('#tbody_' + agg).html(tbody);

                                $("#table_" + agg).dataTable(jOptionsDatatable);

                                $("#chart_" + agg).show();
                                if (IsNotNullOrEmpty(charts['chart_' + agg])) {
                                    charts['chart_' + agg].data.datasets[0].data = data_chart_doc_count;
                                    charts['chart_' + agg].data.datasets[1].data = data_chart_sum_value;
                                    charts['chart_' + agg].data.labels = labels_chart;
                                    charts['chart_' + agg].options.scales.yAxes[0].ticks.stepSize = maior.toString().length >= 3 ? 10 : (maior > 60 ? parseInt(maior / 10, 10) : maior > 30 ? 5 : 1);

                                    charts['chart_' + agg].update();
                                }
                                else {
                                    charts['chart_' + agg] = new Chart(document.getElementById("chart_" + agg), {
                                        responsive: true,
                                        type: 'line',
                                        data: {
                                            labels: labels_chart,
                                            datasets: [{
                                                label: "Quantidade/Usuário",
                                                data: data_chart_doc_count,
                                                borderColor: 'rgb(255, 159, 64)',
                                                borderWidth: 1
                                            }, {
                                                label: "Quantidade",
                                                data: data_chart_sum_value,
                                                borderColor: 'rgb(54, 162, 235)',
                                                borderWidth: 1
                                            }]
                                        },
                                        options: {
                                            scales: {
                                                yAxes: [{
                                                    ticks: {
                                                        beginAtZero: true,
                                                        stepSize: maior.toString().length >= 3 ? 10 : (maior > 60 ? parseInt(maior / 10, 10) : maior > 30 ? 5 : 1)
                                                    }
                                                }],
                                                xAxes: [{
                                                    ticks: {
                                                        minRotation: 89
                                                    }
                                                }]
                                            },
                                            legend: {
                                                display: false
                                            }
                                        }
                                    });
                                }
                            }
                            else {
                                if (IsNotNullOrEmpty(charts['chart_' + agg])) {
                                    charts['chart_' + agg].destroy();
                                }
                                $("#chart_" + agg).hide();
                                $('#tbody_' + agg).html('<tr><td colspan="2">Nenhum registro encontrado.</td></tr>');
                            }
                        }
                    }
                    $('#super_loading').hide();
                }

                var jOptions = {
                    sUrl: "./ashx/Consulta/HistoricoDePesquisaConsulta.ashx?tipo_pesquisa=estatistica",
                    sType: "GET",
                    fnSuccess: sucesso,
                    fnBeforeSend: gInicio,
                    bAsync: true
                };

                if (IsNotNullOrEmpty(id_form)) {
                    jOptions.sType= "POST";
                    jOptions.sFormId= id_form;
                }
                $.ajaxlight(jOptions);
            } catch (e) {
                console.log(e);
            }
            return false;
        }

        $(document).ready(function () {
            $('#tabs_estatistica').tabs();
            $('#tabs_estatistica a[first]').click();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div>
        <div class="divIdentificadorDePagina">
	        <label>.: Estatísticas de Pesquisa</label>
	    </div>
        <div id="div_controls" class="control">
            <div class="div-light-button">
                <a href="./PesquisarPesquisas.aspx"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_loupe_p.png">Pesquisar</a>
            </div>
            <div class="div-light-button">
                <a href="./EstatisticasDePesquisas.aspx"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_loupe_p.png">Estatísticas</a>
            </div>
        </div>

        <div id="tabs_estatistica" class="tabs_estatistica tabs_datatable w-90-pc mauto">
            <ul>
                <li>
                    <a href="#termos_pesquisados" onclick="clickAba(this);" first="first">Termos Pesquisados</a>
                </li>
                <li>
                    <a href="#tipos_de_pesquisa" onclick="clickAba(this);">Tipos de Pesquisa</a>
                </li>
                <li>
                    <a href="#por_periodo" onclick="clickAba(this);">Por Período</a>
                </li>
            </ul>
            <div id="termos_pesquisados">
                <div class="table w-100-pc">
                    <div class="line">
                        <div class="column w-100-pc">
                            <div class="table w-100-pc">
                                <h1>
                                    TERMOS USADOS NAS PESQUISAS
                                </h1>
                                <div class="line">
                                    <div class="column w-100-pc">
                                        <div class="filtro">
                                            <form id="form_configuracoes_termos" name="form_configuracoes_termos" method="POST" action="#" onsubmit="javascript:return listarAggs('form_configuracoes_termos');">
                                                <input type="hidden" name="nm_agg" value="agg_termos" />
                                                <div class="table mleft">
                                                    <div class="line">
                                                        <div class="column">
                                                            <div class="cell fr">
                                                                <label>Filtrar por data:</label>
                                                            </div>
                                                        </div>
                                                        <div class="column">
                                                            <select name="op_intervalo" onchange="selectRange(this)">
                                                                <option value=""></option>
                                                                <option value="igual">Igual</option>
                                                                <option value="maior">Maior que</option>
                                                                <option value="maiorouigual">Maior ou igual</option>
                                                                <option value="menor">Menor que</option>
                                                                <option value="menorouigual">Menor ou igual</option>
                                                                <option value="diferente">Diferente</option>
                                                                <option value="intervalo">Intervalo</option>
                                                            </select>
                                                        </div>
                                                        <div class="column">
                                                            <input name="dt_historico" type="text" class="date" />
                                                        </div>
                                                        <div class="column interval" style="display:none">
                                                            &nbsp;até&nbsp;
                                                            <input label="Data" name="dt_historico_fim" type="text" class="date"/>
                                                        </div>
                                                    </div>
                                                    <div class="line">
                                                        <div class="column w-100-pc">
                                                            <div class="text-right">
                                                                <button type="submit" title="Aplicar">
                                                                    <img src="<%=TCDF.Sinj.Util._urlPadrao%>/Imagens/ico_check.png?1" width="20" height="20" />Aplicar
                                                                </button>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </form>
                                        </div>
                                        <div class="text-right">
                                            <button type="button" title="Gerar PDF" onclick="javascript:printPDFTable('Tabela dos termos usados nas pesquisas', 'table_agg_termos')">
                                                <img src="<%=TCDF.Sinj.Util._urlPadrao%>/Imagens/ico_pdf.png?1" width="20" height="20" /> Salvar Tabela
                                            </button>
                                            <button type="button" title="Gerar PDF" onclick="javascript:printPDFChart('Gráfico dos termos usados nas pesquisas', 'chart_agg_termos')">
                                                <img src="<%=TCDF.Sinj.Util._urlPadrao%>/Imagens/ico_pdf.png?1" width="20" height="20" /> Salvar Gráfico
                                            </button>
                                        </div>
                                        <br />
                                        <div class="tabela agg_termos">
                                            <table id="table_agg_termos" cellspacing="0">
                                                <thead>
                                                    <tr>
                                                        <th>Termos Pesquisados</th>
                                                        <th style="text-align:center;">Quantidade</th>
                                                        <th style="text-align:center;">Quantidade/Usuário</th>
                                                        <th> </th>
                                                    </tr>
                                                </thead>
                                                <tbody id="tbody_agg_termos">
                                
                                                </tbody>
                                                <tfoot id="tfoot_agg_termos">
                                                </tfoot>
                                            </table>
                                        </div>
                                        <br />
                                        <div class="grafico">
                                            <canvas id="chart_agg_termos"></canvas>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="tipos_de_pesquisa">
                <div class="table w-100-pc">
                    <div class="line">
                        <div class="column w-100-pc">
                            <div class="table w-100-pc">            
                                <h1>
                                        TIPOS DE PESQUISA
                                </h1>
                                <div class="line">
                                    <div class="column w-100-pc">
                                        <div class="filtro">
                                            <form id="form_configuracoes_tipos" name="form_configuracoes_tipos" method="POST" action="#" onsubmit="javascript:return listarAggs('form_configuracoes_tipos');">
                                                <input type="hidden" name="nm_agg" value="agg_tipos" />
                                                <div class="table mleft">
                                                    <div class="line">
                                                        <div class="column">
                                                            <div class="cell fr">
                                                                <label>Filtrar por data:</label>
                                                            </div>
                                                        </div>
                                                        <div class="column">
                                                            <select name="op_intervalo" onchange="selectRange(this)">
                                                                <option value=""></option>
                                                                <option value="igual">Igual</option>
                                                                <option value="maior">Maior que</option>
                                                                <option value="maiorouigual">Maior ou igual</option>
                                                                <option value="menor">Menor que</option>
                                                                <option value="menorouigual">Menor ou igual</option>
                                                                <option value="diferente">Diferente</option>
                                                                <option value="intervalo">Intervalo</option>
                                                            </select>
                                                        </div>
                                                        <div class="column">
                                                            <input name="dt_historico" type="text" class="date" />
                                                        </div>
                                                        <div class="column interval" style="display:none">
                                                            &nbsp;até&nbsp;
                                                            <input label="Data" name="dt_historico_fim" type="text" class="date"/>
                                                        </div>
                                                    </div>
                                                    <div class="line">
                                                        <div class="column w-100-pc">
                                                            <div class="text-right">
                                                                <button type="submit" title="Aplicar">
                                                                    <img src="<%=TCDF.Sinj.Util._urlPadrao%>/Imagens/ico_check.png?1" width="20" height="20" />Aplicar
                                                                </button>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </form>
                                        </div>
                                        <div class="text-right">
                                            <button type="button" title="Gerar PDF" onclick="javascript:printPDFTable('Tabela das pesquisas usadas', 'table_agg_tipos')">
                                                <img src="<%=TCDF.Sinj.Util._urlPadrao%>/Imagens/ico_pdf.png?1" width="20" height="20" /> Salvar Tabela
                                            </button>
                                            <button type="button" title="Gerar PDF" onclick="javascript:printPDFChart('Gráfico das pesquisas usadas', 'chart_agg_tipos')">
                                                <img src="<%=TCDF.Sinj.Util._urlPadrao%>/Imagens/ico_pdf.png?1" width="20" height="20" /> Salvar Gráfico
                                            </button>
                                        </div>
                                        <br />
                                        <div class="tabela agg_tipos">
                                            <table id="table_agg_tipos" cellspacing="0">
                                                <thead>
                                                    <tr>
                                                        <th>Tipo de Pesquisa</th>
                                                        <th style="text-align:center;">Quantidade</th>
                                                        <th style="text-align:center;">Quantidade/Usuário</th>
                                                        <th> </th>
                                                    </tr>
                                                </thead>
                                                <tbody id="tbody_agg_tipos">
                                                </tbody>
                                                <tfoot id="tfoot_agg_tipos">
                                                </tfoot>
                                            </table>
                                        </div>
                                        <br />
                                        <div class="grafico">
                                            <canvas id="chart_agg_tipos"></canvas>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="por_periodo">
                <div class="table w-100-pc">
                    <div class="line">
                        <div class="column w-100-pc">
                            <div class="table w-100-pc">            
                                <h1>
                                    PESQUISAS POR PERÍODO
                                </h1>
                                <div class="line">
                                    <div class="column w-100-pc">
                                        <div class="filtro">
                                            <form id="form_configuracoes_dt_historico" name="form_configuracoes_dt_historico" method="POST" action="#" onsubmit="javascript:return listarAggs('form_configuracoes_dt_historico');">
                                                <input type="hidden" name="nm_agg" value="agg_dt_historico" />
                                                <div class="table mleft">
                                                    <div class="line">
                                                        <div class="column">
                                                            <div class="cell fr">
                                                                <label>Filtrar por data:</label>
                                                            </div>
                                                        </div>
                                                        <div class="column">
                                                            <select name="op_intervalo" onchange="selectRange(this)">
                                                                <option value=""></option>
                                                                <option value="igual">Igual</option>
                                                                <option value="maior">Maior que</option>
                                                                <option value="maiorouigual">Maior ou igual</option>
                                                                <option value="menor">Menor que</option>
                                                                <option value="menorouigual">Menor ou igual</option>
                                                                <option value="diferente">Diferente</option>
                                                                <option value="intervalo">Intervalo</option>
                                                            </select>
                                                        </div>
                                                        <div class="column">
                                                            <input name="dt_historico" type="text" class="date" />
                                                        </div>
                                                        <div class="column interval" style="display:none">
                                                            &nbsp;até&nbsp;
                                                            <input label="Data" name="dt_historico_fim" type="text" class="date"/>
                                                        </div>
                                                    </div>
                                                    <div class="line">
                                                        <div class="column">
                                                            <div class="cell fr">
                                                                <label>Agrupar por:</label>
                                                            </div>
                                                        </div>
                                                        <div class="column">
                                                            <select name="agrupar_dt">
                                                                <option value="dia">Dia</option>
                                                                <option value="mes">Mês</option>
                                                                <option value="ano">Ano</option>
                                                            </select>
                                                        </div>
                                                    </div>
                                                    <div class="line">
                                                        <div class="column w-100-pc">
                                                            <div class="text-right">
                                                                <button type="submit" title="Aplicar">
                                                                    <img src="<%=TCDF.Sinj.Util._urlPadrao%>/Imagens/ico_check.png?1" width="20" height="20" />Aplicar
                                                                </button>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </form>
                                        </div>
                                        <div class="text-right">
                                            <button type="button" title="Gerar PDF" onclick="javascript:printPDFTable('Tabela das pesquisas por data', 'table_agg_dt_historico')">
                                                <img src="<%=TCDF.Sinj.Util._urlPadrao%>/Imagens/ico_pdf.png?1" width="20" height="20" /> Salvar Tabela
                                            </button>
                                            <button type="button" title="Gerar PDF" onclick="javascript:printPDFChart('Gráfico das pesquisas por data', 'chart_agg_dt_historico')">
                                                <img src="<%=TCDF.Sinj.Util._urlPadrao%>/Imagens/ico_pdf.png?1" width="20" height="20" /> Salvar Gráfico
                                            </button>
                                        </div>
                                        <br />
                                        <div class="tabela agg_dt_historico">
                                            <table id="table_agg_dt_historico" cellspacing="0">
                                                <thead>
                                                    <tr>
                                                        <th>Período</th>
                                                        <th style="text-align:center;">Quantidade</th>
                                                        <th style="text-align:center;">Quantidade/Usuário</th>
                                                        <th> </th>
                                                    </tr>
                                                </thead>
                                                <tbody id="tbody_agg_dt_historico">
                                                </tbody>
                                                <tfoot id="tfoot_agg_dt_historico">
                                                </tfoot>
                                            </table>
                                        </div>
                                        <br />
                                        <div class="grafico">
                                            <canvas id="chart_agg_dt_historico"></canvas>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </div>
    </div>
</asp:Content>
