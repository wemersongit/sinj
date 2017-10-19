<%@ Page Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="PesquisarDiretorioDiario.aspx.cs" Inherits="TCDF.Sinj.Portal.Web.PesquisarDiretorioDiario" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <style type="text/css">
        #table_diarios td{padding:5px 0px;}
        #table_diarios tr{border:}
        #table_diarios tr.DDD{background-color: #DDD;}
        #table_diarios tr.EEE{background-color: #EEE;}
        a.close{float: right;padding-right: 5px; text-decoration: none;}
    </style>
    <script type="text/javascript">
        <%
            var json_tipos_de_fonte = new TCDF.Sinj.Portal.Web.ashx.Autocomplete.TipoDeFonteAutocomplete().AutocompleteTipoDeFonte("...", "", "", "", "usados");
        %>

        $(document).ready(function () {

            var tiposDeFonte = <%= json_tipos_de_fonte != "" ? json_tipos_de_fonte : "\"\"" %>;
            for(var i = 0; i < tiposDeFonte.results.length; i++){
                if(tiposDeFonte.results[i].nm_tipo_fonte == "Não tem definido."){
                    continue;
                }
                $('#select_tipo_fonte').append('<option value="' + tiposDeFonte.results[i].ch_tipo_fonte + '" '+(tiposDeFonte.results[i].nm_tipo_fonte == "DODF" ? 'selected="selected"' : '')+'>' + tiposDeFonte.results[i].nm_tipo_fonte + '</option');
            }
            //$('#select_tipo_fonte option[value="DODF"]').attr('selected', 'selected');
            for (var i = 0; i < anosDeAssinatura.length; i++) {
                $('#select_ano').append('<option value="' + anosDeAssinatura[i].ano + '" >' + anosDeAssinatura[i].ano + '</option');
            }
            $('#select_ano option[value=' + new Date().getFullYear() + ']').attr('selected', 'selected');
            var meses = getMeses();
            for (var i = 0; i < meses.length; i++) {
                $('#select_mes').append('<option value="' + meses[i].nr_mes + '" nm_mes="' + meses[i].nm_mes + '" >' + meses[i].nm_mes + '</option');
            }
            $('#select_mes option[value=' + (new Date().getMonth() + 1) + ']').attr('selected', 'selected');

            pesquisarDiretorioDiario();
        });
        function pesquisarDiretorioDiario() {
            var ch_tipo_fonte = $('#select_tipo_fonte').val();
            var ano = $('#select_ano').val();
            var mes = $('#select_mes').val();
            mes = $('#select_mes option[value="' + mes + '"]').attr('nm_mes');

            $('#tbody_resultado').html('');

            var nm_tipo_fonte = $('#select_tipo_fonte option[value="'+ch_tipo_fonte+'"]').text();



            $('#th_diarios').text((nm_tipo_fonte != "" ? nm_tipo_fonte + 's' : 'Diários ') + ' de ' + mes + ' de ' + ano);
            var success = function (data) {
                $('#super_loading').hide();
                var paramCountsToHistory = '';
                if (IsNotNullOrEmpty(data, 'hits.hits')) {
                    paramCountsToHistory = "total=" + JSON.stringify({ nm_base: 'sinj_diario', ds_base: 'Diários', nr_total: data.hits.total });
                    $('#tbody_resultado').append('<tr><td colspan="2">' + data.hits.hits.length + ' registro' + (data.hits.hits.length > 1 ? "s" : "") + ' encontrado' + (data.hits.hits.length > 1 ? "s" : "") + '.</td></tr>');
                    var dt_assinatura = "";
                    var guid = "";
                    var id_file = "";
                    var nm_file = "";
                    var url_file = "";
                    for (var i = 0; i < data.hits.hits.length; i++) {
                        if (data.hits.hits[i].fields.partial[0].dt_assinatura != dt_assinatura) {
                            dt_assinatura = data.hits.hits[i].fields.partial[0].dt_assinatura;
                            $('#tbody_resultado').append('<tr class="DDD bold"><td colspan="2">' + dt_assinatura + '</td></tr>');
                        }
                        var links = '';
                        if(IsNotNullOrEmpty(data.hits.hits[i].fields.partial[0].arquivos)){
                            for(var j = 0; j < data.hits.hits[i].fields.partial[0].arquivos.length; j++){
                                id_file = data.hits.hits[i].fields.partial[0].arquivos[j].arquivo_diario.id_file;
                                nm_file = data.hits.hits[i].fields.partial[0].arquivos[j].arquivo_diario.filename;
                                url_file = './Diario/' + data.hits.hits[i].fields.partial[0].ch_diario + '/' + id_file + '/arq/' + j + '/' + nm_file;
                                links = '<a title="visualizar texto" target="_blank" href="./TextoArquivoDiario.aspx?id_file=' + id_file + '" ><img src="' + _urlPadrao + '/Imagens/ico_doc_m.png" alt="texto" width="15px" /></a>' +
                                '&nbsp;&nbsp;<a title="baixar arquivo" target="_blank" href="'+url_file+'"><img src="' + _urlPadrao + '/Imagens/ico_pdf.png" alt="download" width="15px" />' +
                                '<b>' + (data.hits.hits[i].fields.partial[0].arquivos[j].arquivo_diario.filesize / 1024).toFixed(0) + ' KB</b></a><br/>';
                            }
                        }
                        else if(IsNotNullOrEmpty(data.hits.hits[i].fields.partial[0].ar_diario, 'id_file')){
                            id_file = data.hits.hits[i].fields.partial[0].ar_diario.id_file;
                            nm_file = data.hits.hits[i].fields.partial[0].ar_diario.filename;
                            url_file = './Diario/' + data.hits.hits[i].fields.partial[0].ch_diario + '/' + id_file + '/arq/' + j + '/' + nm_file;
                            links = '<a title="visualizar texto" target="_blank" href="./TextoArquivoDiario.aspx?id_file=' + id_file + '" ><img src="' + _urlPadrao + '/Imagens/ico_doc_m.png" alt="texto" width="15px" /></a>' +
                            '&nbsp;&nbsp;<a title="baixar arquivo" target="_blank" href="'+url_file+'"><img src="' + _urlPadrao + '/Imagens/ico_pdf.png" alt="download" width="15px" />' +
                            '<b>' + (data.hits.hits[i].fields.partial[0].ar_diario.filesize / 1024).toFixed(0) + ' KB</b></a>';
                        }
                        var nm_diario = montarDescricaoDiario(data.hits.hits[i].fields.partial[0]);
                        //var nm_diario = data.hits.hits[i].fields.partial[0].nm_tipo_fonte + ' ' + data.hits.hits[i].fields.partial[0].nr_diario + ' ' + ' seção ' + data.hits.hits[i].fields.partial[0].secao_diario + ' de ' + data.hits.hits[i].fields.partial[0].dt_assinatura;
                        //                        $('#div_resultado').append('<a target="_blank" title="Baixar Arquivo do Diário" href="./BaixarArquivoDiario.aspx?id_file=' + data.hits.hits[i].fields.partial[0].ar_diario.id_file + '" >' + nm_file + ' (' + medida + ')' + '<img src="' + _urlPadrao + '/Imagens/ico_link.png" width="20px" height="20px"/></a><br/>');
                        $('#tbody_resultado').append('<tr class="' + guid + '"><td>&nbsp;&nbsp;&nbsp;&nbsp;' + nm_diario + '</td><td>' + links + '</td></tr>');
                    }
                }
                else {
                    paramCountsToHistory = "total=" + JSON.stringify({ nm_base: 'sinj_diario', ds_base: 'Diários', nr_total: 0 });
                    $('#tbody_resultado').append('<tr><td colspan="2">Nenhum registro encontrado.</td></tr>');
                }
                SalvarConsultaNoHistorico(paramCountsToHistory, 'tipo_pesquisa=diretorio_diario&nm_tipo_fonte=' + nm_tipo_fonte + '&ano=' + ano + '&mes='+mes);
            }
            var beforeSubmit = function (Data) {
                $('#super_loading').show();
            }

            $.ajaxlight({
                sUrl: "./ashx/Consulta/DiarioConsulta.ashx?iDisplayStart=0&iDisplayLength=300",
                sType: "POST",
                bAsync: true,
                fnSuccess: success,
                sFormId: "form_pesquisa_diario",
                fnBeforeSubmit: beforeSubmit
            });
        }
//        function closeTr(tr_class) {
//            if ($('tr.' + tr_class).css('display') == 'none') {
//                
//            }
//        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="divIdentificadorDePagina">
	    <label>.: Pesquisar Diário</label>
	</div>
    <div class="w-60-pc" style="background-color:#EEE; margin-left:50px;">
        <form id="form_pesquisa_diario" name="formPesquisaDiario" action="#" method="post" onsubmit="javascript:return pesquisarTextoDiario();">
            <input type="hidden" name="tipo_pesquisa" value="diretorio_diario" />
            <div class="mauto table w-90-pc">
                <div class="line">
                    <div class="column w-100-pc">
                        <div class="mauto table w-100-pc">
                            <div class="line">
                                <div class="column w-100-pc">
                                    <div class="cell w-30-pc">
                                        <label>Tipo de Diário:</label><br />
                                        <select id="select_tipo_fonte" name="ch_tipo_fonte" onchange="javascript:pesquisarDiretorioDiario();">
                                        </select>
                                    </div>
                                    <div class="cell w-10-pc">
                                        <br />
                                    </div>
                                    <div class="cell w-20-pc">
                                        <label>Ano:</label><br />
                                        <select id="select_ano" name="ano" onchange="javascript:pesquisarDiretorioDiario();">
                                        </select>
                                    </div>
                                    <div class="cell w-10-pc">
                                        <br />
                                    </div>
                                    <div class="cell w-20-pc">
                                        <label>Mês:</label><br />
                                        <select id="select_mes" name="mes" onchange="javascript:pesquisarDiretorioDiario();">
                                        </select>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </form>
        <br />
    </div>
    <div id="div_pesquisa">
        <div class="table w-60-pc" style="margin-left:50px;">
            <div class="line">
                <div class="column w-100-pc">
                    <div id="div_resultado" style="font-size:16px;">
                        <table id="table_diarios" cellspacing="0">
                            <thead>
                                <tr>
                                    <th id="th_diarios">Diários</th>
                                    <th> </th>
                                </tr>
                            </thead>
                            <tbody id="tbody_resultado">
                                
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
    
</asp:Content>
