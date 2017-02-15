<%@ Page Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="PesquisarTextoDiario.aspx.cs" Inherits="TCDF.Sinj.Portal.Web.PesquisarTextoDiario" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('#div_autocomplete_tipo_fonte').autocompletelight({
                sKeyDataName: "ch_tipo_fonte",
                sValueDataName: "nm_tipo_fonte",
                sInputHiddenName: "ch_tipo_fonte",
                sInputName: "nm_tipo_fonte",
                sAjaxUrl: './ashx/Autocomplete/TipoDeFonteAutocomplete.ashx',
                bLinkAll: true,
                sLinkName: "a_tipo_fonte"
            });
        });
        function changeCheckboxIntervalo(input) {
            if ($(input).is(':checked')) {
                $('#div_data_inicio label').text('Início:');
                $('#div_data_termino').html('<label>Término:</label><br />' + '<input name="dt_assinatura_termino" type="text" value="" class="date w-80-pc" />');
            }
            else {
                $('#div_data_inicio label').text('Data:');
                $('#div_data_termino').html('');
            }
        }
        function pesquisarTextoDiario() {
            try {
                removerFiltro();
                $("#div_resultado").dataTablesLight({
                    sAjaxUrl: './ashx/Datatable/TextoDiarioDatatable.ashx?' + $("#form_pesquisa_diario").serialize(),
                    aoColumns: _columns_texto_diario,
                    fnServerDataAggregation: function (data) {
                        if (IsNotNullOrEmpty(data, 'aggregations.ano_assinatura.buckets')) {
                            $('#div_buttons_ano').html('');
                            for (var i = 0; i < data.aggregations.ano_assinatura.buckets.length; i++) {
                                $('#div_buttons_ano').append('<button title="Filtrar o resultado da pesquisa." class="clean block" onclick="javascript:filtar(\'' + data.aggregations.ano_assinatura.buckets[i].key_as_string + '\')">' + data.aggregations.ano_assinatura.buckets[i].key_as_string + ' (' + data.aggregations.ano_assinatura.buckets[i].doc_count + ')</button>');
                            }
                            $('#div_ano').show();
                        }
                        else {
                            $('#div_ano').hide();
                        }
                    }
                });
            }
            catch (ex) {
                console.log(ex);
            }
            return false;
        }
        function filtar(ano) {
            $("#div_resultado").dataTablesLight({
                sAjaxUrl: './ashx/Datatable/TextoDiarioDatatable.ashx?ano=' + ano + '&' + $("#form_pesquisa_diario").serialize(),
                aoColumns: _columns_texto_diario
            });
            $('#div_buttons_remover_filtro').html('<button title="Remover o filtro." class="clean block w-100-pc text-left" onclick="javascript:pesquisarTextoDiario();">' + ano + '<span class="fr">X</span></button>');
            $('#div_filtro_atual').show();
        }
        function removerFiltro() {
            $('#div_filtro_atual').hide();
            $('#div_buttons_remover_filtro').html('');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="divIdentificadorDePagina">
	    <label>.: Pesquisar Diário</label>
	</div>
    <div class="w-80-pc mauto" style="background-color:#EEE;">
        <form id="form_pesquisa_diario" name="formPesquisaDiario" action="#" method="post" onsubmit="javascript:return pesquisarTextoDiario();">
            <input type="hidden" name="tipo_pesquisa" value="texto_diario" />
            <div class="mauto table w-90-pc">
                <div class="line">
                    <div class="column w-100-pc">
                        <div class="mauto table w-100-pc">
                            <div class="line">
                                <div class="column w-100-pc">
                                    <div class="cell w-100-pc" style="position:relative;">
                                        <input autofocus="autofocus" id="input_all" name="filetext" type="text" value="" class="w-100-pc" placeholder="Pesquisa..." /><button type="submit" style="background: none;border: none;padding: 0; margin: 0; position: absolute;top: 0;right: 0;"><img alt="ok" src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico-lupa-p_green.png" /></button>
                                    </div>
                                </div>
                            </div>
                            <div class="line">
                                <div class="column w-100-pc">
                                    <div class="cell w-40-pc">
                                        <label>Tipo de Diário:</label><br />
                                        <div id="div_autocomplete_tipo_fonte">
                                            <input id="ch_tipo_fonte" name="ch_tipo_fonte" type="hidden" value="1" />
                                            <input id="nm_tipo_fonte" name="nm_tipo_fonte" type="text" value="DODF" class="w-80-pc" /><a title="Listar" id="a_tipo_fonte"></a>
                                        </div>
                                    </div>
                                    <div class="cell w-10-pc">
                                        <br />
                                    </div>
                                    <div id="div_data_inicio" class="cell w-20-pc">
                                        <label>Início:</label><br />
                                        <input name="dt_assinatura_inicio" type="text" value="" class="date w-80-pc" />
                                    </div>
                                    <div class="cell w-10-pc" style="margin-top:10px;">
                                        <label><input type="checkbox" name="intervalo" onchange="javascript:changeCheckboxIntervalo(this)" value="1" checked="checked" style="vertical-align:middle;" /> Intervalo</label>
                                    </div>
                                    <div id="div_data_termino" class="cell w-20-pc">
                                        <label>Término:</label><br />
                                        <input name="dt_assinatura_termino" type="text" value="" class="date w-80-pc" />
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
        <div class="mauto table w-100-pc">
            <div class="line">
                <div class="column w-10-pc">
                    <div id="div_filtro_atual" style="display:none; margin-bottom:10px; padding:10px;">
                        <div style="padding:3px; background:#DDD; font-weight:bold;">
                            Filtro atual
                        </div>
                        <div id="div_buttons_remover_filtro" style="padding:3px; background:#EEE;">
                        
                        </div>
                    </div>
                    <div id="div_ano" style="display:none; padding:10px;">
                        <div style="padding:3px; background:#DDD; font-weight:bold;">
                            Filtrar por ano
                        </div>
                        <div id="div_buttons_ano" style="padding:3px; background:#EEE;">
                        
                        </div>
                    </div>
                    <br />
                </div>
                <div class="column w-80-pc">
                    <div id="div_resultado" class="datatable-clean">

                    </div>
                </div>
                <div class="column w-10-pc">
                    <br />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
