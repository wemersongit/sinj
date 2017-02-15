<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="EditarOrgao.aspx.cs" Inherits="TCDF.Sinj.Web.EditarOrgao" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">

    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/funcoes_orgao.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
//            $('#button_salvar_orgao').click(function () {
//                return fnSalvar("form_orgao");
//            });
            ConstruirControlesDinamicos();
            PreencherOrgaoEdicao();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="divIdentificadorDePagina">
	    <label>.: Editar Órgão</label>
	</div>
    <div id="div_controls" class="control">
    </div>
    <div class="form">
        <form id="form_orgao" name="formCadastroOrgao" action="#" method="post">
            <input id="id_doc" name="id_doc" type="hidden" value="" class="w-90-pc" />
            <div id="div_orgao_loading" class="loading" style="display:none;"></div>
            <div id="div_orgao" class="loaded">
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
                                    <input type="text" id="input_dt_inicio_vigencia_orgao_anterior_modal" class="w-50-pc date" style="display:none;"></label>
                                    <label id="label_dt_inicio_vigencia_orgao_anterior_modal" style="color:#777";></label>
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
                                    <input type="text" id="input_dt_fim_vigencia_orgao_anterior_modal" class="w-50-pc date" style="display:none;"></label>
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
                <fieldset>
                    <legend>Órgão</legend>
                    <div id="div_notificacao_orgao" class="notify" style="display:none;"></div>
                    <div class="mauto table">
                        <div class="line">
                            <div class="column w-100-pc">
                                <div class="cell w-100-pc">
                                    <div class="w-90-pc mauto">
                                        <button type="button" onclick="javascript:CriarModalHierarquia();"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_add_p.png" >Adicionar Órgão Pai</button>
                                    </div>
                                    <table id="table_hierarquia" class="w-90-pc mauto word-no-break">
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
                                        <button type="button" onclick="javascript:CriarModalCronologia();"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_add_p.png" >Adicionar Órgão Anterior</button>
                                    </div>
                                    <table id="table_cronologia" class="w-90-pc mauto word-no-break">
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
	                                    <caption>Órgãos Herdados</caption>
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
                                        <option value="0"></option>
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
                                    <label id="label_inativar_filhos" style="display:none;color:#333333;" ch_orgao=""><input id="checkbox_inativar_filhos" name="inativar_filhos" type="checkbox" value="false" /> Inativar Hierarquia Inferior </label>
                                </div>
                            </div>
                        </div>
                        <%--<div class="line">
                            <div class="column w-20-pc">
                                <div class="cell fr">
                                    <label>Data Início Vigência:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div class="cell w-50-pc">
                                    <input label="Data Início Vigência" obrigatorio="sim" id="dt_inicio_vigencia" name="dt_inicio_vigencia" type="text" value="" class="w-30-pc date" />
                                </div>
                            </div>
                        </div>
                        <div id="line_dt_fim_vigencia" class="line">
                            <div class="column w-20-pc">
                                <div class="cell fr">
                                    <label>Data Fim Vigência:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div class="cell w-50-pc">
                                    <input id="dt_fim_vigencia" name="dt_fim_vigencia" type="text" value="" class="w-30-pc date" />
                                </div>
                            </div>
                        </div>
                        <div id="line_norma_associada" class="line">
                            <div class="column w-20-pc">
                                <div class="cell fr">
                                    <label>Norma Associada:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div class="cell w-50-pc">
                                    <input disabled="" id="disabled_norma_associada" type="text" value="" class="w-70-pc" />
                                    <input id="norma_associada" name="norma_associada" type="hidden" value="" />
                                    <a id="a_removerNormaAssociada" style="display:none;" title="Remover" href="javascript:void(0);" onclick="javascript:RemoverNormaAssociada();"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_delete_p.png" alt="remover" /></a>                                   
                                </div>
                            </div>
                        </div>
                        <div class="line AU">
                            <div class="column w-100-pc">
                                <div class="cell w-100-pc text-center" style="background-color:#CCC;">
	                                <a id="a_selecionar_normas" href="javascript:void(0);" onclick="javascript:ExpandirTabelaDeNormas();" class="expansible closed" >Selecionar Normas <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_arrow_down.png"/></a>
	                                <div id="div_datatable_normas" style="display:none;" ></div>
                                </div>
                            </div>
                        </div>--%>
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
                        <button id="button_salvar_orgao" onclick="javascript: return SalvarComInativarFilhos()">
                            <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_disk_p.png" />Salvar
                        </button>
                        <button onclick="javascript:window.history.back();">
                            <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_delete_p.png" />Cancelar
                        </button>
                    </div>
                </fieldset>
            </div>
            <div id="div_notificacao_atualizar_normas" style="display:none;"></div>
            <br />
        </form>
    </div>
    <div id="modal_inativar_filhos_sucesso" style="display:none;"></div>
    <div id="modal_inativar_filhos" style="display:none;">
        <p> Os seguintes órgãos não serão inativados porque não puderam ser validados.
        <br/>Ajuste-os para que sua situação seja alterada.<br/>
        </p>
        <div id="div_validacao_filhos" class="mauto table">
            <div class="column w-100-pc">
                <div class="cell w-100-pc">
                    <table id="table_validacao_filhos" class="w-100-pc mauto word-no-break">
                        <caption> Órgãos Inferiores Invalidados </caption>
                        <thead>
                            <tr>
                                <th>
                                    Descrição
                                </th>
                                <th>
                                    Motivo
                                </th>
                            </tr>
                        </thead>
                        <tbody id="tbody_validacao_filhos">
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
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