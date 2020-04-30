<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="CadastrarOrgao.aspx.cs" Inherits="TCDF.Sinj.Web.CadastrarOrgao" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/funcoes_orgao.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $('#button_salvar_orgao').click(function () {
                try {
                    Validar('form_orgao');
                    ValidarRegrasEspecificas('form_orgao');
                    var sucesso = function (data) {
                        if (IsNotNullOrEmpty(data, 'results')) {
                            var duplicados = '';
                            for (var i = 0; i < data.results.length; i++) {
                                duplicados += '<br/><a title="Visualizar Órgão" target="_blank" href="./DetalhesDeOrgao.aspx?id_doc=' + data.results[i]._metadata.id_doc + '">' + data.results[i].sg_hierarquia + ' - ' + data.results[i].nm_orgao + '<img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_loupe_p.png" alt="visualizar" /></a>';
                            }
                            $('#form_orgao .notify').modallight({
                                sTitle: "Órgão Duplicado",
                                sContent: 'Foi verificado que o Órgão é duplicado. Órgãos:' + duplicados + '<br/><br/>Deseja salvar mesmo assim?',
                                sWidth: '600',
                                oButtons: [{
                                    html: '<img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_check_p.png" alt="confirmar" /> Sim',
                                    click: function () {
                                        fnSalvar("form_orgao");
                                        $(this).dialog("destroy");
                                    }
                                }, {
                                    html: '<img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_delete_p.png" alt="cancelar" /> Não',
                                    click: function () {
                                        $(this).dialog("destroy");
                                    }
                                }]
                            });
                        }
                        else {
                            return fnSalvar("form_orgao");
                        }
                    }
                    var inicio = function () {
                        $('#div_loading_orgao').show();
                        $('#div_orgao').hide();
                    }
                    var complete = function () {
                        $('#div_loading_orgao').hide();
                        $('#div_orgao').show();
                    }
                    var hiddens_orgaos_anteriores = $('#table_cronologia').find('input[name="orgao_anterior"]');
                    var search = '?nm_orgao=' + $('#nm_orgao').val() + '&sg_orgao=' + $('#sg_orgao').val();
                    for (var i = 0; i < hiddens_orgaos_anteriores.length; i++) {
                        search += '&ch_orgao_anterior=' + $(hiddens_orgaos_anteriores[i]).val().split('#')[0];
                    }
                    $.ajaxlight({
                        sUrl: './ashx/Consulta/OrgaoConsulta.ashx' + search,
                        sType: "POST",
                        fnSuccess: sucesso,
                        fnComplete: complete,
                        fnBeforeSend: inicio,
                        bAsync: true
                    });
                    return false;
                }
                catch (ex) {
                    $('#form_orgao .notify').messagelight({
                        sTitle: "Erro nos dados informados",
                        sContent: ex,
                        sType: "error"
                    }); 
                    return false;
                }
            });
            ConstruirControlesDinamicos();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="divIdentificadorDePagina">
	    <label>.: Cadastrar Órgão</label>
	</div>
    <div id="div_controls" class="control">
    </div>
    <div class="form">
        <div id="modal_cronologia" class="modal" style="display:none;">
            <div class="table w-100-pc">
                <div class="line">
                    <div class="column w-30-pc">
                        <div class="cell fr">
                            <label>Selecionar Órgão:</label>
                        </div>
                    </div>
                    <div class="column w-70-pc">
                        <div id="div_autocomplete_orgao_anterior" class="cell w-100-pc">
                            <input id="ch_orgao_anterior_modal" type="hidden" value="" onchange="javascript:SelecionarOrgaoAnterior();" />
                            <input id="sg_nm_orgao_anterior_modal" type="text" value="" class="w-80-pc" onblur="javascript:SelecionarOrgaoAnterior();"/><a title="Listar" id="a_orgao_anterior_modal"></a>
                        </div>
                    </div>
                </div>
                <div class="line dados_orgao">
                    <div class="column w-30-pc">
                        <div class="cell fr">
                            <label>Nome do Órgão:</label>
                        </div>
                    </div>
                    <div class="column w-70-pc">
                        <div class="cell w-50-pc">
                            <label id="label_nm_orgao_anterior_modal" style="color:#777;"></label>
                        </div>
                    </div>
                </div>
                <div class="line dados_orgao">
                    <div class="column w-30-pc">
                        <div class="cell fr">
                            <label>Sigla do Órgão:</label>
                        </div>
                    </div>
                    <div class="column w-70-pc">
                        <div class="cell w-50-pc">
                            <label id="label_sg_orgao_anterior_modal" style="color:#777;"></label>
                        </div>
                    </div>
                </div>
                <div class="line dados_orgao">
                    <div class="column w-30-pc">
                        <div class="cell fr">
                            <label>Hierarquia:</label>
                        </div>
                    </div>
                    <div class="column w-70-pc">
                        <div class="cell w-50-pc">
                            <label id="label_sg_hierarquia_orgao_anterior_modal" style="color:#777;"></label>
                        </div>
                    </div>
                </div>
                <div class="line dados_orgao">
                    <div class="column w-30-pc">
                        <div class="cell fr">
                            <label>Âmbito:</label>
                        </div>
                    </div>
                    <div class="column w-70-pc">
                        <div class="cell w-50-pc">
                            <label id="label_in_ambito_orgao_anterior_modal" style="color:#777;"></label>
                        </div>
                    </div>
                </div>
                <div class="line dados_orgao">
                    <div class="column w-30-pc">
                        <div class="cell fr">
                            <label>Início de Vigência:</label>
                        </div>
                    </div>
                    <div class="column w-70-pc">
                        <div class="cell w-50-pc">
                            <input type="text" id="input_dt_inicio_vigencia_orgao_anterior_modal" class="w-50-pc date" style="display:none;"/>
                            <label id="label_dt_inicio_vigencia_orgao_anterior_modal" style="color:#777;"></label>
                        </div>
                    </div>
                </div>
                <div class="line dados_orgao">
                    <div class="column w-30-pc">
                        <div class="cell fr">
                            <label>Fim de Vigência:</label>
                        </div>
                    </div>
                    <div class="column w-70-pc">
                        <div class="cell w-50-pc">
                            <input type="text" id="input_dt_fim_vigencia_orgao_anterior_modal" class="w-50-pc date" style="display:none;"/>
                            <label id="label_dt_fim_vigencia_orgao_anterior_modal" style="color:#777;"></label>
                        </div>
                    </div>
                </div>
                <div class="line dados_orgao">
                    <div class="column w-30-pc">
                        <div class="cell fr">
                            <label>Órgão Cadastrador:</label>
                        </div>
                    </div>
                    <div class="column w-70-pc">
                        <div class="cell w-50-pc">
                            <label id="label_in_orgao_cadastrador_orgao_anterior_modal" style="color:#777;"></label>
                        </div>
                    </div>
                </div>
                <div class="notify" style="display:none;"></div>
            </div>
        </div>
        <div id="modal_hierarquia" class="modal" style="display:none;">
            <div class="table w-100-pc">
                <div class="line">
                    <div class="column w-30-pc">
                        <div class="cell fr">
                            <label>Selecionar Órgão:</label>
                        </div>
                    </div>
                    <div class="column w-70-pc">
                        <div id="div_autocomplete_orgao_pai" class="cell w-100-pc">
                            <input id="ch_orgao_pai_modal" type="hidden" value="" onchange="javascript:SelecionarOrgaoPai();" />
                            <input id="sg_nm_orgao_pai_modal" type="text" value="" class="w-80-pc" onblur="javascript:SelecionarOrgaoPai();" /><a title="Listar" id="a_orgao_pai_modal"></a>
                        </div>
                    </div>
                </div>
                <div class="line dados_orgao">
                    <div class="column w-30-pc">
                        <div class="cell fr">
                            <label>Nome do Órgão:</label>
                        </div>
                    </div>
                    <div class="column w-70-pc">
                        <div class="cell w-50-pc">
                            <label id="label_nm_orgao_pai_modal" style="color:#777;"></label>
                        </div>
                    </div>
                </div>
                <div class="line dados_orgao">
                    <div class="column w-30-pc">
                        <div class="cell fr">
                            <label>Sigla do Órgão:</label>
                        </div>
                    </div>
                    <div class="column w-70-pc">
                        <div class="cell w-50-pc">
                            <label id="label_sg_orgao_pai_modal" style="color:#777;"></label>
                        </div>
                    </div>
                </div>
                <div class="line dados_orgao">
                    <div class="column w-30-pc">
                        <div class="cell fr">
                            <label>Hierarquia:</label>
                        </div>
                    </div>
                    <div class="column w-70-pc">
                        <div class="cell w-50-pc">
                            <label id="label_sg_hierarquia_orgao_pai_modal" style="color:#777;"></label>
                        </div>
                    </div>
                </div>
                <div class="line dados_orgao">
                    <div class="column w-30-pc">
                        <div class="cell fr">
                            <label>Âmbito:</label>
                        </div>
                    </div>
                    <div class="column w-70-pc">
                        <div class="cell w-50-pc">
                            <label id="label_in_ambito_orgao_pai_modal" style="color:#777;"></label>
                        </div>
                    </div>
                </div>
                <div class="line dados_orgao">
                    <div class="column w-30-pc">
                        <div class="cell fr">
                            <label>Início de Vigência:</label>
                        </div>
                    </div>
                    <div class="column w-70-pc">
                        <div class="cell w-50-pc">
                            <label id="label_dt_inicio_vigencia_orgao_pai_modal" style="color:#777;"></label>
                        </div>
                    </div>
                </div>
                <div class="line dados_orgao">
                    <div class="column w-30-pc">
                        <div class="cell fr">
                            <label>Fim de Vigência:</label>
                        </div>
                    </div>
                    <div class="column w-70-pc">
                        <div class="cell w-50-pc">
                            <label id="label_dt_fim_vigencia_orgao_pai_modal" style="color:#777;"></label>
                        </div>
                    </div>
                </div>
                <div class="line dados_orgao">
                    <div class="column w-30-pc">
                        <div class="cell fr">
                            <label>Órgão Cadastrador:</label>
                        </div>
                    </div>
                    <div class="column w-70-pc">
                        <div class="cell w-50-pc">
                            <label id="label_in_orgao_cadastrador_orgao_pai_modal" style="color:#777;"></label>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <form id="form_orgao" name="formCadastroOrgao" action="#" method="post">
            <div id="div_orgao">
                <fieldset>
                    <!--<legend>Órgão</legend>-->
                    <div class="mauto table">
                        <div class="line">
                            <div class="column w-100-pc">
                                <div class="cell w-100-pc">
                                    <div class="w-90-pc mauto">
                                        <button type="button" onclick="javascript:CriarModalHierarquia();"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_add_p.png" />Adicionar Órgão Pai</button>
                                    </div>
                                    <table id="table_hierarquia" class="w-90-pc mauto">
                                        <caption>Órgão Pai</caption>
                                        <thead>
                                            <tr>
                                                <th>
                                                    Descrição
                                                </th>
                                                <th>
                                                    Sigla
                                                </th>
                                                <th>
                                                    Hierarquia
                                                </th>
                                                <th>
                                                    Âmbito
                                                </th>
                                                <th>
                                                    Início de Vigência
                                                </th>
                                                <th>
                                                    Fim de Vigência
                                                </th>
                                                <th>
                                                    Órgão Cadastrador
                                                </th>
                                                <th style="width:25px;">
                                                </th>
                                            </tr>
                                        </thead>
                                        <tbody id="tbody_hierarquia">
                                            <tr class="tr_vazia">
                                                <td colspan="8">
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                        <div class="line">
                            <div class="column w-100-pc">
                                <div class="cell w-100-pc">
                                    <div class="w-90-pc mauto">
                                        <button type="button" onclick="javascript:CriarModalCronologia();"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_add_p.png" />Adicionar Órgão Anterior</button>
                                    </div>
                                    <table id="table_cronologia" class="w-90-pc mauto">
                                        <caption>Órgãos Anteriores</caption>
                                        <thead>
                                            <tr>
                                                <th>
                                                    Descrição
                                                </th>
                                                <th>
                                                    Sigla
                                                </th>
                                                <th>
                                                    Hierarquia
                                                </th>
                                                <th>
                                                    Âmbito
                                                </th>
                                                <th>
                                                    Início de Vigência
                                                </th>
                                                <th>
                                                    Fim de Vigência
                                                </th>
                                                <th>
                                                    Órgão Cadastrador
                                                </th>
                                                <th style="width:25px;">
                                                </th>
                                            </tr>
                                        </thead>
                                        <tbody id="tbody_cronologia">
                                            <tr class="tr_vazia">
                                                <td colspan="8">
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
						<div class="line" style="display:none;" id="div_filhos_selecionados">
                            <div class="column w-100-pc">
                                <div class="cell w-100-pc">
                                    <table id="table_filhos" class="w-90-pc mauto">
                                        <caption>Órgãos Filho</caption>
                                        <thead>
                                            <tr>
                                                <th>
                                                    Descrição
                                                </th>
                                                <th>
                                                    Sigla
                                                </th>
                                                <th>
                                                    Hierarquia
                                                </th>
                                                <th>
                                                    Âmbito
                                                </th>
                                                <th>
                                                    Início de Vigência
                                                </th>
                                                <th>
                                                    Fim de Vigência
                                                </th>
                                                <th>
                                                    Órgão Cadastrador
                                                </th>
                                                <th style="width:25px;">
                                                </th>
                                            </tr>
                                        </thead>
                                        <tbody id="tbody_filhos">
                                            <tr class="tr_vazia">
                                                <td colspan="8">
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                        <div id="div_notificacao_orgao" class="notify" style="display:none;"></div>
                        <div class="line">
                            <div class="column w-20-pc">
                                <div class="cell fr">
                                    <label>Nome:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div class="cell w-60-pc">
                                    <input label="Nome" obrigatorio="sim" id="nm_orgao" name="nm_orgao" type="text" value="" class="w-90-pc" />
                                </div>
                            </div>
                        </div>
                        <div class="line">
                            <div class="column w-20-pc">
                                <div class="cell fr">
                                    <label>Sigla:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div class="cell w-80-pc">
                                    <input label="Sigla" obrigatorio="sim" id="sg_orgao" name="sg_orgao" type="text" value="" class="w-30-pc" />
                                </div>
                            </div>
                        </div>
                        <div class="line">
                            <div class="column w-20-pc">
                                <div class="cell fr">
                                    <label>Âmbito:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div class="cell w-100-pc">
                                    <select label="Âmbito" obrigatorio="sim" id="id_ambito" name="id_ambito">
                                    </select>
                                </div>
                            </div>
                        </div>
                        <div class="line">
                            <div class="column w-20-pc">
                                <div class="cell fr">
                                    <label>Órgão Cadastrador:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div id="div_orgaos_cadastradores" class="cell w-100-pc">
                                </div>
                            </div>
                        </div>
                        <div class="line">
                            <div class="column w-20-pc">
                                <div class="cell fr">
                                    <label>Situação:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div id="div_st_orgao" class="cell w-50-pc">
                                    <label style="color:#333333;"><input label="Situação" name="st_orgao" type="radio" value="true" onclick="javascript:SelecionarAtivo();" />Ativo</label>
                                    <label style="color:#333333;"><input label="Situação" name="st_orgao" type="radio" value="false" onclick="javascript:SelecionarInativo();" />Inativo</label>
                                </div>
                            </div>
                        </div>
                        <div class="line">
                            <div class="column w-100-pc">
                                <div class="cell w-100-pc">
                                    <fieldset>
                                        <legend>Início de Vigência</legend>
                                        <div class="w-100-pc table">
                                            <div class="line">
                                                <div class="column w-20-pc">
                                                    <div class="cell fr">
                                                        <label>Data:</label>
                                                    </div>
                                                </div>
                                                <div class="column w-70-pc">
                                                    <div class="cell w-50-pc">
                                                        <input label="Data Início Vigência" obrigatorio="sim" id="dt_inicio_vigencia" name="dt_inicio_vigencia" type="text" value="" class="w-30-pc date" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div id="line_norma_inicio_vigencia" class="line">
                                                <div class="column w-20-pc">
                                                    <div class="cell fr">
                                                        <label>Norma Associada:</label>
                                                    </div>
                                                </div>
                                                <div class="column w-70-pc">
                                                    <div class="cell w-50-pc">
                                                        <input id="norma_inicio_vigencia" name="norma_inicio_vigencia" type="hidden" value="" />
                                                        <input disabled="disabled" id="disabled_norma_inicio_vigencia" type="text" value="" class="w-70-pc" />
                                                        <a id="a_adicionar_norma_inicio_vigencia" href="javascript:void(0);" onclick="javascript:AbrirModalSelecionarNormaAssociada('inicio');" title="Selecionar norma associada à criação"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_add_p.png" alt="select" /></a>
                                                        <a id="a_remover_norma_inicio_vigencia" style="display:none;" title="Remover" href="javascript:void(0);" onclick="javascript:RemoverNormaAssociada('inicio');"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_delete_p.png" alt="remover" /></a>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </fieldset>
                                </div>
                            </div>
                        </div>
                        <div id="line_dt_fim_vigencia" class="line">
                            <div class="column w-100-pc">
                                <div class="cell w-100-pc">
                                    <fieldset>
                                        <legend>Fim de Vigência</legend>
                                        <div class="w-100-pc table">
                                            <div class="line">
                                                <div class="column w-20-pc">
                                                    <div class="cell fr">
                                                        <label>Data:</label>
                                                    </div>
                                                </div>
                                                <div class="column w-70-pc">
                                                    <div class="cell w-50-pc">
                                                        <input id="dt_fim_vigencia" name="dt_fim_vigencia" type="text" value="" class="w-30-pc date" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div id="line_norma_fim_vigencia" class="line">
                                                <div class="column w-20-pc">
                                                    <div class="cell fr">
                                                        <label>Norma Associada:</label>
                                                    </div>
                                                </div>
                                                <div class="column w-70-pc">
                                                    <div class="cell w-50-pc">
                                                        <input id="norma_fim_vigencia" name="norma_fim_vigencia" type="hidden" value="" />
                                                        <input disabled="disabled" id="disabled_norma_fim_vigencia" type="text" value="" class="w-70-pc" />
                                                        <a id="a_adicionar_norma_fim_vigencia" href="javascript:void(0);" onclick="javascript:AbrirModalSelecionarNormaAssociada('fim');" title="Selecionar norma associada à extinção"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_add_p.png" alt="select" /></a>
                                                        <a id="a_remover_norma_fim_vigencia" style="display:none;" title="Remover" href="javascript:void(0);" onclick="javascript:RemoverNormaAssociada('fim');"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_delete_p.png" alt="remover" /></a>                                   
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </fieldset>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="div_buttons_orgao" style="width:220px; margin:auto;" class="loaded">
                        <button id="button_salvar_orgao">
                            <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_disk_p.png" />Salvar
                        </button>
                        <button type="reset">
                            <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_eraser_p.png" />Limpar
                        </button>
                    </div>
                </fieldset>
            </div>
            <div id="div_loading_orgao" class="loading" style="display:none;"></div>
            <br />
        </form>
    </div>
    <div id="modal_norma_associada" style="display:none;">
        <div class="table w-100-pc">
            <div class="line">
                <div class="column w-30-pc">
                    <div class="cell fr">
                        <label>Tipo:</label>
                    </div>
                </div>
                <div class="column w-70-pc">
                    <div id="div_autocomplete_tipo_norma_modal" class="cell w-100-pc">
                        <input id="ch_tipo_norma_modal" type="hidden" value="" />
                        <input id="nm_tipo_norma_modal" type="text" value="" class="w-80-pc" /><a title="Listar" id="a_tipo_norma_modal"></a>
                    </div>
                </div>
            </div>
            <div class="line">
                <div class="column w-30-pc">
                    <div class="cell fr">
                        <label>Número:</label>
                    </div>
                </div>
                <div class="column w-70-pc">
                    <div class="cell w-100-pc">
                        <input id="nr_norma_modal" type="text" value="" class="w-20-pc" />
                    </div>
                </div>
            </div>
            <div class="line">
                <div class="column w-30-pc">
                    <div class="cell fr">
                        <label>Data de Assinatura:</label>
                    </div>
                </div>
                <div class="column w-70-pc">
                    <div class="cell w-100-pc">
                        <input id="dt_assinatura_modal" type="text" value="" class="date" />
                    </div>
                </div>
            </div>
            <div class="line">
                <div class="column w-100-pc">
                    <div class="w-90-pc mauto">
                        <div class="div-light-button fr">
                            <a id="a_pesquisar_norma_associada" href="javascript:void(0);" onclick="javascript:PesquisarNorma();" associacao=""><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_loupe_p.png" alt="consultar" />Pesquisar</a>
                        </div>
                    </div>
                </div>
            </div>
            <div class="line">
                <div class="column w-100-pc">
                    <div class="w-100-pc mauto">
                        <div id="datatable_normas_modal"></div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
