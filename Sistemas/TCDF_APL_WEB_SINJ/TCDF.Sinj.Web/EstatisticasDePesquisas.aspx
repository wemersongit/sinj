<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="EstatisticasDePesquisas.aspx.cs" Inherits="TCDF.Sinj.Web.EstatisticasDePesquisas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <style type="text/css">
        table {width:300px; margin-left: 50px; float:left;}
        table th, table td{padding:5px 0px;}
        table caption{background-color: #CCC; font-weight:bold; padding:5px}
        table thead tr{background-color: #DDD;}
        table tr.EEE{background-color: #EEE;}
    </style>
    <script type="text/javascript">
        function mostrarMais(){
            var count = $('#tbody_resultado tr.termo').length;
            listarTermosPesquisados(count + 20);
        }
        function listarTermosPesquisados(size){
            //$('#tbody_resultado').html('');
            var success = function (data) {
                //$('#super_loading').hide();
                var tbody_resultado = '';
                var tbody_dt_historico = '';
                if (IsNotNullOrEmpty(data, 'aggregations.agg_terms.buckets')) {
                    //$('#tbody_resultado').append('<tr><td colspan="3">' + (data.aggregations.agg_terms.sum_other_doc_count + data.aggregations.agg_terms.buckets.length) + ' termos' + (data.hits.hits.length > 1 ? "s" : "") + ' encontrado' + (data.hits.hits.length > 1 ? "s" : "") + '.</td></tr>');
                    for (var i = 0; i < data.aggregations.agg_terms.buckets.length; i++) {
                        var link = '<a title="visualizar pesquisas" href="./ResultadoDePesquisaPesquisas.aspx?termo=' + data.aggregations.agg_terms.buckets[i].key + '" ><img src="' + _urlPadrao + '/Imagens/ico_loupe_p.png" /></a>';
                        //$('#tbody_resultado').append('<tr class="' + (i % 2 == 0 ? "EEE" : "") + ' termo"><td>' + data.aggregations.agg_terms.buckets[i].key + '</td><td style="text-align:center;">' + data.aggregations.agg_terms.buckets[i].doc_count + '</td><td>' + link + '</td></tr>');
                        tbody_resultado += '<tr class="' + (i % 2 == 0 ? "EEE" : "") + ' termo"><td>' + data.aggregations.agg_terms.buckets[i].key + '</td><td style="text-align:center;">' + data.aggregations.agg_terms.buckets[i].doc_count + '</td><td>' + link + '</td></tr>';
                    }
                    if (data.aggregations.agg_terms.sum_other_doc_count > 0) {
                        $('#tfoot_resultado').html('<tr><td colspan="3" style="text-align:center;"><a href="javascript:void(0);" onclick="javascript:mostrarMais(\'terms\');" >Mostrar Mais...</a></td></tr>')
                    }
                    $('#tbody_resultado').html(tbody_resultado);
                    $('#td_total').text(data.hits.total);
                    $('#td_geral').text(data.aggregations.agg_tipo_pesquisa.buckets.geral.doc_count);
                    $('#td_norma').text(data.aggregations.agg_tipo_pesquisa.buckets.norma.doc_count);
                    $('#td_diario').text(data.aggregations.agg_tipo_pesquisa.buckets.diario.doc_count);
                    $('#td_avancada').text(data.aggregations.agg_tipo_pesquisa.buckets.avancada.doc_count);
                    $('#td_dt_inicio').text(data.hits.hits[0]._source._metadata.dt_doc);
                }
                else {
                    $('#tbody_resultado').append('<tr><td colspan="2">Nenhum registro encontrado.</td></tr>');
                }
                if (IsNotNullOrEmpty(data, 'aggregations.agg_dt_historico.buckets')) {
                    for (var i = 0; i < data.aggregations.agg_dt_historico.buckets.length; i++) {
                        var link = '<a title="visualizar pesquisas desta data" href="./ResultadoDePesquisaPesquisas.aspx?op_intervalo=igual&dt_historico=' + data.aggregations.agg_dt_historico.buckets[i].key_as_string + '" ><img src="' + _urlPadrao + '/Imagens/ico_loupe_p.png" /></a>';
                        tbody_dt_historico += '<tr class="' + (i % 2 == 0 ? "EEE" : "") + ' termo"><td>' + data.aggregations.agg_dt_historico.buckets[i].key_as_string + '</td><td style="text-align:center;">' + data.aggregations.agg_dt_historico.buckets[i].doc_count + '</td><td>' + link + '</td></tr>';
                    }
                    $('#tbody_dt_historico').html(tbody_dt_historico);
                }
                else {
                    $('#tbody_dt_historico').append('<tr><td colspan="2">Nenhum registro encontrado.</td></tr>');
                }
                $('#super_loading').hide();
            }
            var beforeSend = function (Data) {
                $('#super_loading').show();
            }
            $.ajaxlight({
                sUrl: "./ashx/Consulta/HistoricoDePesquisaConsulta.ashx?tipo_pesquisa=estatistica&size="+size,
                sType: "GET",
                bAsync: true,
                fnSuccess: success,
                fnBeforeSend: beforeSend
            });
        }
        $(document).ready(function () {
            listarTermosPesquisados(20);
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
        <div class="table w-100-pc">
            <div class="line">
                <div class="column w-100-pc">
                    <div id="div_resultado" style="font-size:16px;">
                        <table id="table_termos" cellspacing="0">
                            <caption>
                                TERMOS USADOS NAS PESQUISAS
                            </caption>
                            <thead>
                                <tr>
                                    <th id="th_termos">Termos</th>
                                    <th style="text-align:center;">Quantidade</th>
                                    <th> </th>
                                </tr>
                            </thead>
                            <tbody id="tbody_resultado">
                                
                            </tbody>
                            <tfoot id="tfoot_resultado">
                            </tfoot>
                        </table>
                        <table id="table_dt_historico" cellspacing="0">
                            <caption>
                                TOTAL POR DATA
                            </caption>
                            <thead>
                                <tr>
                                    <th>Data</th>
                                    <th style="text-align:center;">Quantidade</th>
                                    <th> </th>
                                </tr>
                            </thead>
                            <tbody id="tbody_dt_historico">
                            </tbody>
                        </table>
                        <table id="table_total" cellspacing="0">
                            <caption>
                                TIPOS DE PESQUISAS
                            </caption>
                            <thead>
                                
                            </thead>
                            <tbody id="tbody_total">
                                <tr>
                                    <th>Geral</th>
                                    <td id="td_geral"></td>
                                </tr>
                                <tr class="EEE">
                                    <th>Norma</th>
                                    <td id="td_norma"></td>
                                </tr>
                                <tr>
                                    <th>Diário</th>
                                    <td id="td_diario"></td>
                                </tr>
                                <tr class="EEE">
                                    <th>Avançada</th>
                                    <td id="td_avancada"></td>
                                </tr>
                                <tr>
                                    <th>Todas</th>
                                    <td id="td_total"></td>
                                </tr>
                                <tr class="EEE">
                                    <th>Início da Contagem</th>
                                    <td id="td_dt_inicio"></td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
