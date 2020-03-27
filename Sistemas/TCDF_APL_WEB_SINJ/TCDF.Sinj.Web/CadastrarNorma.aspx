<%@ Page Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="CadastrarNorma.aspx.cs" Inherits="TCDF.Sinj.Web.CadastrarNorma" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/funcoes_norma.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/ckeditor/ckeditor.js"></script>
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/ckeditor/adapters/jquery.js"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $('#button_salvar_norma').click(function () {
                return fnSalvar("form_norma");
            });
            ConstruirControlesDinamicos();
            configureCkeditor();
            <% if(isAdmin && !string.IsNullOrEmpty(situacoes)){ %>
                fnAutocompleteSituacao(<%= situacoes%>);
            <%} %>
        });
        
    </script>
    <style type="text/css">
        #sg_hierarquia_nm_vigencia{z-index:10005;}
        #div_modal_importar_arquivo table thead th.nm_arquivo {width:250px;}
        #ds_ementa, #ds_observacao{text-align:justify;}
        #checkbox_st_habilita_pesquisa {}

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div id="slave" class="on-show-top">
        <div class="divIdentificadorDePagina">
	        <label>.: Cadastrar Norma</label>
	    </div>
        <div id="div_controls" class="control">
        </div>
        <div id="modal_fonte" style="display:none;">
            <div class="consultar">
                <form id="form_consultar_diario" name="form_consultar_diario" action="#" method="post"  onsubmit="javascript:return consultarDiario('form_consultar_diario');">
                    <div class="table w-100-pc">
                        <div class="line">
                            <div class="column w-30-pc">
                                <div class="cell fr">
                                    <label>Tipo de Fonte:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div id="div_autocomplete_tipo_fonte_modal" class="cell w-60-pc">
                                    <input id="ch_tipo_fonte_modal" name="ch_tipo_fonte" obrigatorio="sim" label="Tipo de Fonte" type="hidden" value="" />
                                    <input id="nm_tipo_fonte_modal" type="text" value="" class="w-80-pc" onblur="onblurTipoDeFonte()"/><a title="Listar" id="a_tipo_fonte"></a>
                                </div>
                            </div>
                        </div>
                        <div class="line">
                            <div class="column w-30-pc">
                                <div class="cell fr">
                                    <label>Data da Publicação:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div class="cell w-50-pc">
                                    <input id="dt_assinatura_fonte_modal" name="dt_assinatura" type="text" value="" class="w-50-pc date" obrigatorio="sim" label="Data da Publicação" />
                                </div>
                            </div>
                        </div>
                        <div class="line">
                            <div class="column w-100-pc text-right">
                                <button type="submit" title="Consulta os diários com tipo e data informados">
                                    <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_loupe_p.png" alt="ok" /> Consultar Diários
                                </button>
                                <button id="buttonProsseguirSemDiario" title="Prosseguir a inclusão da fonte sem selecionar um diário" onclick="prosseguirSemDiario()" style="display:none;">
                                    <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_alert_p.png" alt="alerta" /> Prosseguir sem Diário
                                </button>
                            </div>
                        </div>
                    </div>
                </form>
            </div>
            <div class="table w-100-pc resultado">
                <div class="line">
                    <div class="column w-100-pc">
                        <div class="cell w-100-pc">
                            <table id="table_diarios" class="w-100-pc mauto">
                                <caption>Diários</caption>
                                <thead>
                                    <tr>
                                        <th>
                                            Descrição
                                        </th>
                                        <th style="width:200px;">
                                            Arquivo
                                        </th>
                                    </tr> 
                                </thead>
                                <tbody id="tbody_diarios">
                                    <tr class="tr_vazia">
                                        <td colspan="2">
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
            <div class="table w-100-pc diario">
                <div class="line">
                    <div class="column w-30-pc">
                        <div class="cell fr">
                            <label>Diário*:</label>
                        </div>
                    </div>
                    <div class="column w-70-pc">
                        <div class="cell w-100-pc">
                            <input id="json_diario_fonte" type="hidden" value="" />
                            <input id="json_arquivo_diario_fonte" type="hidden" value="" />
                            <input id="ds_diario" type="text" value="" class="w-100-pc" disabled="disabled" />
                        </div>
                    </div>
                </div>
                <div class="line">
                    <div class="column w-30-pc">
                        <div class="cell fr">
                            <label>Tipo de Publicação*:</label>
                        </div>
                    </div>
                    <div class="column w-70-pc">
                        <div id="div_autocomplete_tipo_publicacao_modal" class="cell w-60-pc">
                            <input id="ch_tipo_publicacao_modal" type="hidden" value="" obrigatorio="sim" label="Tipo de Publicação" />
                            <input id="nm_tipo_publicacao_modal" type="text" value="" class="w-80-pc" /><a title="Listar" id="a_tipo_publicacao"></a>
                        </div>
                    </div>
                </div>
                <div class="line">
                    <div class="column w-30-pc">
                        <div class="cell fr">
                            <label>Página:</label>
                        </div>
                    </div>
                    <div class="column w-70-pc">
                        <div class="cell w-50-pc">
                            <input id="nr_pagina_modal" type="text" value="" class="w-30-pc" />
                        </div>
                    </div>
                </div>
                <div class="line">
                    <div class="column w-30-pc">
                        <div class="cell fr">
                            <label>Coluna:</label>
                        </div>
                    </div>
                    <div class="column w-70-pc">
                        <div class="cell w-50-pc">
                            <input id="nr_coluna_modal" type="text" value="" class="w-30-pc" />
                        </div>
                    </div>
                </div>
                <div class="line">
                    <div class="column w-30-pc">
                        <div class="cell fr">
                            <label>Observação:</label>
                        </div>
                    </div>
                    <div class="column w-70-pc">
                        <div class="cell w-100-pc">
                            <textarea id="ds_observacao_fonte_modal" cols="100" rows="10" style="width:80%; max-width:100%;"></textarea>
                        </div>
                    </div>
                </div>
                <div class="line">
                    <div class="column w-30-pc">
                        <div class="cell fr">
                            <label>Motivo da Republicação:</label>
                        </div>
                    </div>
                    <div class="column w-70-pc">
                        <div class="cell w-100-pc">
                            <input id="ds_republicacao_modal" type="text" value="" class="w-70-pc" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="notify" style="display:none;"></div>
        </div>
        <div id="modal_origem" class="modal" style="display:none;">
            <div class="table w-100-pc">
                <div class="line">
                    <div class="column w-30-pc">
                        <div class="cell fr">
                            <label>Retroativo:</label>
                        </div>
                    </div>
                    <div class="column w-70-pc">
                    <div id="div1" class="cell w-100-pc">
                        <input type="checkbox" id="checkbox_retroativo" onchange="javascript:fnAutocompleteOrigem()" />
                    </div>
                    </div>
                </div>
                <div class="line">
                    <div class="column w-30-pc">
                        <div class="cell fr">
                            <label>Selecionar Origem:</label>
                        </div>
                    </div>
                    <div class="column w-70-pc">
                        <div id="div_autocomplete_origem_modal" class="cell w-100-pc">
                            <input id="ch_orgao" type="hidden" value="" />
                            <input id="sg_hierarquia_nm_vigencia" type="text" value="" class="w-80-pc" onblur="javascript:MostrarDadosOrigem();" /><a title="Listar" id="a_origem"></a>
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
	                    <div class="cell w-100-pc">
	                        <label id="label_nm_orgao_modal" style="color:#888;"></label>
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
	                    <div class="cell w-100-pc">
	                        <label id="label_sg_orgao_modal" style="color:#888;"></label>
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
	                    <div class="cell w-100-pc">
	                        <label id="label_sg_hierarquia_modal" style="color:#888;"></label>
	                    </div>
	                </div>
	            </div>
	            <div class="line dados_orgao">
	                <div class="column w-30-pc">
	                    <div class="cell fr">
	                        <label>Descrição Hierarquia:</label>
	                    </div>
	                </div>
	                <div class="column w-70-pc">
	                    <div class="cell w-100-pc">
	                        <label id="label_nm_hierarquia_modal" style="color:#888;"></label>
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
	                        <label id="label_in_ambito_modal" style="color:#888;"></label>
	                    </div>
	                </div>
	            </div>
	            <div class="line dados_orgao">
	                <div class="column w-30-pc">
	                    <div class="cell fr">
	                        <label>Vigência:</label>
	                    </div>
	                </div>
	                <div class="column w-70-pc">
	                    <div class="cell w-50-pc">
	                        <label id="label_dt_inicio_vigencia_modal" style="color:#888;"></label>
	                    </div>
	                </div>
	            </div>
	            <div class="line dados_orgao">
	                <div class="column w-30-pc">
	                    <div class="cell fr">
	                        <label>Vigência:</label>
	                    </div>
	                </div>
	                <div class="column w-70-pc">
	                    <div class="cell w-50-pc">
	                        <label id="label_dt_fim_vigencia_modal" style="color:#888;"></label>
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
	                    <div class="cell w-100-pc">
	                        <label id="label_orgao_cadastrador_modal" style="color:#888;"></label>
	                    </div>
	                </div>
	            </div>
            </div>
        </div>
        <div id="modal_autoria" class="modal" style="display:none;">
            <div class="table w-100-pc">
                <div class="line">
                    <div class="column w-30-pc">
                        <div class="cell fr">
                            <label>Autoria:</label>
                        </div>
                    </div>
                    <div class="column w-70-pc">
                        <div id="div_autocomplete_autoria_modal" class="cell w-100-pc">
                            <input id="ch_autoria" type="hidden" value="" />
                            <input id="nm_autoria" type="text" value="" class="w-80-pc" /><a title="Listar" id="a_autoria"></a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="modal_interessado" class="modal" style="display:none;">
            <div class="table w-100-pc">
                <div class="line">
                    <div class="column w-30-pc">
                        <div class="cell fr">
                            <label>Interessado:</label>
                        </div>
                    </div>
                    <div class="column w-70-pc">
                        <div id="div_autocomplete_interessado_modal" class="cell w-100-pc">
                            <input id="ch_interessado" type="hidden" value="" />
                            <input id="nm_interessado" type="text" value="" class="w-80-pc" /><a title="Listar" id="a_interessado"></a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="modal_nome" class="modal" style="display:none;">
            <div class="table w-100-pc">
                <div class="line">
                    <div class="column w-30-pc">
                        <div class="cell fr">
                            <label>Nome:</label>
                        </div>
                    </div>
                    <div class="column w-70-pc">
                        <div class="cell w-100-pc">
                            <input id="nm_nome" type="text" value="" class="w-80-pc" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="modal_indexacao" style="display:none;">
            <div class="table w-100-pc">
                <div class="line">
                    <div class="column w-30-pc">
                        <div class="cell fr">
                            <label>Pesquisa exata:</label>
                        </div>
                    </div>
                    <div class="column w-70-pc">
                        <div class="cell w-100-pc">
                            <input type="checkbox" onclick="javascript:fnAutocompleteVocabulario(this);" />
                        </div>
                    </div>
                </div>
                <div class="line">
                    <div class="column w-30-pc">
                        <div class="cell fr">
                            <label>Termo:</label>
                        </div>
                    </div>
                    <div class="column w-70-pc">
                        <div id="div_autocomplete_termo_modal" class="cell w-100-pc">
                            <input id="ch_termo_modal" type="hidden" value="" />
                            <input id="ch_tipo_termo_modal" type="hidden" value="" />
                            <input autofocus id="nm_termo_modal" type="text" value="" class="w-80-pc" /><a title="Listar" id="a_termo"></a>
                            <a href="javascript:void(0);" onclick="javascript:AdicionarTermo();"><img valign="absmiddle" alt="Adicionar" src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_add_p.png"  /></a>
                        </div>
                    </div>
                </div>
                <div class="line">
                    <div class="column w-100-pc">
                        <div class="cell w-100-pc">
                            <table class="mauto w-90-pc">
                                <caption>Indexação</caption>
                                <thead>
                                    <tr>
                                        <th>
                                            Termos
                                        </th>
                                        <th style="width:40px">
                                        </th>
                                    </tr>
                                </thead>
                                <tbody id="tbody_termos_modal">
                                    <tr class="tr_vazia">
                                        <td colspan="2">
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
            <div class="notify" style="display:none;"></div>
        </div>
        <div id="modal_decisao" style="display:none;">
            <div class="table w-100-pc">
                <div class="line">
                    <div class="column w-30-pc">
                        <div class="cell fr">
                            <label>Decisão:</label>
                        </div>
                    </div>
                    <div class="column w-70-pc">
                        <div class="cell">
                            <select id="in_tipo_decisao_modal">
                                <option value="0" >Liminar Deferida</option>
                                <option value="1" >Liminar Indeferida</option>
                                <option value="2" >Mérito Procedente</option>
                                <option value="6" >Mérito Parcialmente Procedente</option>
                                <option value="3" >Mérito Improcedente</option>
                                <option value="4" >Extinta</option>
                                <option value="5" >Ajuizado</option>
                                <option value="7" >Prejudicada</option>
                            </select>
                        </div>
                    </div>
                </div>
                <div class="line">
                    <div class="column w-30-pc">
                        <div class="cell fr">
                            <label>Complemento da Decisão:</label>
                        </div>
                    </div>
                    <div class="column w-70-pc">
                        <div class="cell w-100-pc">
                            <input id="ds_complemento_modal" type="text" value="" class="w-70-pc" />
                        </div>
                    </div>
                </div>
                <div class="line">
                    <div class="column w-30-pc">
                        <div class="cell fr">
                            <label>Data de Publicação:</label>
                        </div>
                    </div>
                    <div class="column w-70-pc">
                        <div class="cell w-100-pc">
                            <input id="dt_decisao_modal" class="date" type="text" value="" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="notify" style="display:none;"></div>
        </div>
        <div id="modal_requerente" style="display:none;">
            <div class="table w-100-pc">
                <div class="line">
                    <div class="column w-30-pc">
                        <div class="cell fr">
                            <label>Requerente:</label>
                        </div>
                    </div>
                    <div class="column w-70-pc">
                        <div id="div_autocomplete_requerente_modal" class="cell w-100-pc">
                            <input id="ch_requerente_modal" type="hidden" value="" />
                            <input id="nm_requerente_modal" type="text" value="" class="w-80-pc" /><a title="Listar" id="a_requerente"></a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="modal_requerido" style="display:none;">
            <div class="table w-100-pc">
                <div class="line">
                    <div class="column w-30-pc">
                        <div class="cell fr">
                            <label>Requerido:</label>
                        </div>
                    </div>
                    <div class="column w-70-pc">
                        <div id="div_autocomplete_requerido_modal" class="cell w-100-pc">
                            <input id="ch_requerido_modal" type="hidden" value="" />
                            <input id="nm_requerido_modal" type="text" value="" class="w-80-pc" /><a title="Listar" id="a_requerido"></a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="modal_procurador" style="display:none;">
            <div class="table w-100-pc">
                <div class="line">
                    <div class="column w-30-pc">
                        <div class="cell fr">
                            <label>Procurador:</label>
                        </div>
                    </div>
                    <div class="column w-70-pc">
                        <div id="div_autocomplete_procurador_modal" class="cell w-100-pc">
                            <input id="ch_procurador_modal" type="hidden" value="" />
                            <input id="nm_procurador_modal" type="text" value="" class="w-80-pc" /><a title="Listar" id="a_procurador"></a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="modal_relator" style="display:none;">
            <div class="table w-100-pc">
                <div class="line">
                    <div class="column w-30-pc">
                        <div class="cell fr">
                            <label>Relator:</label>
                        </div>
                    </div>
                    <div class="column w-70-pc">
                        <div id="div_autocomplete_relator_modal" class="cell w-100-pc">
                            <input id="ch_relator_modal" type="hidden" value="" />
                            <input id="nm_relator_modal" type="text" value="" class="w-80-pc" /><a title="Listar" id="a_relator"></a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="div_arquivo" style="display:none;" class="mauto w-90-pc">
            <div id="editar_arquivo_notificacao" class="notify" style="display:none;"></div>
            <div id="div_conteudo_arquivo"></div>
            <form id="form_editar_arquivo" name="form_editar_arquivo" action="#" method="post">
                <div class="text-right">
                    <button type="reset" onclick="javascript:closeEditFile();" class="clean">
                        <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_fechar.png" alt="cancela" width="18px" height="18px"/>
                    </button>
                </div>
                <input type="hidden" name="id_file" class="id_file" value=""/>
                <label>Nome do Arquivo:</label><input type="text" name="filename" class="filename" value=""/>
                <textarea name="arquivo" id="arquivo" rows="10" cols="80" style="display:none"></textarea>
                <div style="width:210px; margin:auto; display:none;" class="loaded buttons">
                    <button type="submit">
                        <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_save.png" alt="add" width="18px" height="18px"/> Salvar
                    </button>
                    <button type="reset" onclick="javascript:closeEditFile();">
                        <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_fechar.png" alt="cancela" width="18px" height="18px"/> Cancelar
                    </button>
                </div>
            </form>
        </div>
        <div class="form">
            <form id="form_norma" name="formCadastroNorma" action="#" method="post">
                <div class="loaded" id="div_norma">
                    <div id="div_identificacao">
                        <br />
                        <div id="div_notificacao_norma" class="notify w-80-pc mauto" style="display:none;"></div>
                        <fieldset>
                            <legend>Identificação</legend>
                            <div class="mauto table">
                                <div class="line">
                                    <div class="column w-20-pc">
                                        <div class="cell fr">
                                            <label>Tipo*:</label>
                                        </div>
                                    </div>
                                    <div class="column w-70-pc">
                                        <div id="div_autocomplete_tipo_norma" class="cell w-60-pc">
                                            <input id="ch_tipo_norma" name="ch_tipo_norma" type="hidden" value="" obrigatorio="sim" label="Tipo de Norma" />
                                            <input id="in_g1" type="hidden" value="" />
                                            <input id="in_g2" name="in_g2" type="hidden" value="" />
                                            <input id="in_g3" type="hidden" value="" />
                                            <input id="in_g4" type="hidden" value="" />
                                            <input id="in_g5" type="hidden" value="" />
                                            <input id="EhLei" type="hidden" value="" />
                                            <input id="EhDecreto" type="hidden" value="" />
                                            <input id="EhResolucao" type="hidden" value="" />
                                            <input id="EhPortaria" type="hidden" value="" />
                                            <input id="in_conjunta" type="hidden" value="" />
                                            <input id="in_numeracao_por_orgao" type="hidden" value="" />
                                            <input id="in_apelidavel" type="hidden" value="" />
                                            <input id="nm_tipo_norma" name="nm_tipo_norma" type="text" value="" class="w-80-pc" onblur="javascript:SelecionarTipoDeNorma();"/><a title="Listar" id="a_tipo_norma"></a>
                                        </div>
                                        <label style="font-size:large">Habilitar Pesquisa:</label>      
                                        <input id="st_habilita_pesquisa" name="st_habilita_pesquisa" value="true" type="checkbox" title="Habilita a visualizacao no Sinj pesquisa."/>
                                    </div>
                                </div>
                                <div class="line">
                                    <div class="column w-20-pc">
                                        <div class="cell fr">
                                            <label>Número:</label>
                                        </div>
                                    </div>
                                    <div class="column w-70-pc">
                                        <div class="cell w-80-pc">
                                            <input id="nr_norma" name="nr_norma" obrigatorio="sim" type="text" value="" class="w-20-pc" label="Número" />
                                        	<input type="checkbox" id="norma_sem_numero" onchange="javascript:VerificarNormaSemNumero()"/> <label>*Norma sem número</label>
                                        </div>
                                    </div>
                                </div>
                                <div class="line">
                                    <div class="column w-20-pc">
                                        <div class="cell fr">
                                            <label>Letra:</label>
                                        </div>
                                    </div>
                                    <div class="column w-70-pc">
                                        <div class="cell w-80-pc">
                                            <input id="cr_norma" name="cr_norma" type="text" value="" class="w-20-pc" />
                                        </div>
                                    </div>
                                </div>
                                <div class="line">
                                    <div class="column w-20-pc">
                                        <div class="cell fr">
                                            <label>Sequencial:</label>
                                        </div>
                                    </div>
                                    <div class="column w-70-pc">
                                        <div class="cell w-80-pc">
                                            <input id="nr_sequencial" name="nr_sequencial" type="text" value="" class="w-20-pc"/>
                                        </div>
                                    </div>
                                </div>
                                <div class="line">
                                    <div class="column w-20-pc">
                                        <div class="cell fr">
                                            <label>Data da Assinatura*:</label>
                                        </div>
                                    </div>
                                    <div class="column w-70-pc">
                                        <div class="cell w-80-pc">
                                            <input id="dt_assinatura" name="dt_assinatura" type="text" value="" class="w-20-pc date" />
                                        </div>
                                    </div>
                                </div>
                                <div class="line">
                                    <div class="column w-20-pc">
                                        <div class="cell fr">
                                            <label>Vacatio Legis:</label>
                                        </div>
                                    </div>
                                    <div class="column w-70-pc">
                                        <div class="cell w-80-pc">
                                            <input id="st_vacatio_legis" name="st_vacatio_legis" type="checkbox" value="1" onclick="selecionarVacatioLegis();" />
                                        </div>
                                    </div>
                                </div>
                                <div class="line line_vacatio_legis" style="display:none;">
                                    <div class="column w-20-pc">
                                        <div class="cell fr">
                                            <label>Data Início Vigência*:</label>
                                        </div>
                                    </div>
                                    <div class="column w-70-pc">
                                        <div class="cell w-80-pc">
                                            <input id="dt_inicio_vigencia" name="dt_inicio_vigencia" type="text" value="" class="w-20-pc date" obrigatorio="não" label="Data Início Vigência" />
                                        </div>
                                    </div>
                                </div>
                                <div class="line line_vacatio_legis" style="display:none;">
                                    <div class="column w-20-pc">
                                        <div class="cell fr">
                                            <label>Descrição Vacatio Legis:</label>
                                        </div>
                                    </div>
                                    <div class="column w-70-pc">
                                        <div class="cell w-80-pc">
                                            <textarea id="ds_vacatio_legis" name="ds_vacatio_legis" value="" cols="100" rows="10" style="width:80%; max-width:100%;"></textarea>
                                        </div>
                                    </div>
                                </div>
                                <div class="line">
                                    <div class="column w-20-pc">
                                        <div class="cell fr">
                                            <label>Âmbito*:</label>
                                        </div>
                                    </div>
                                    <div class="column w-70-pc">
                                        <div class="cell w-80-pc">
                                            <select label="Âmbito" obrigatorio="sim" id="id_ambito" name="id_ambito">
                                            </select>
                                        </div>
                                    </div>
                                </div>
                                
                                
                                
                               
                                
                                
                                <div class="line">
                                    <div class="column w-100-pc">
                                        <div class="cell w-100-pc">
                                            <div class="w-90-pc mauto">
                                                <button type="button" onclick="javascript:CriarModalOrigem();"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_add_p.png" alt="add"/> Adicionar Origem</button>
                                            </div>
                                            <table id="table_origens" class="w-90-pc mauto word-no-break">
                                                <caption>Origens*</caption>
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
                                            <tbody id="tbody_origens">
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
                                        <div class="cell w-100-pc text-right">
                                            <button type="button" onclick="javascript:ValidarDuplicidadeDeNorma();" title="Verifica junto ao banco se não há duplicidade."><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_check_p.png" alt="ok" > Validar Duplicidade</button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id="div_notificacao_norma_duplicadade" class="w-80-pc mauto" style="display:none;"></div>
                        </fieldset>
                    </div>

                    <div id="div_dados_gerais" class="pos_cad">
                        <fieldset>
                            <legend>Dados Gerais</legend>
                            <div class="mauto table">
                            <% if (TCDF.Sinj.Util.UsuarioTemPermissao(TCDF.Sinj.Web.Sinj.oSessaoUsuario, TCDF.Sinj.AcoesDoUsuario.nor_fst))
                               { %>
                                <div class="line">
                                    <div class="column w-22-pc">
                                        <div class="cell fr">
                                            <label>Forçar Situação: <input id="st_situacao_forcada" name="st_situacao_forcada" type="checkbox" value="1" onchange="changeStSiuacaoForcada(this);" /></label>
                                        </div>
                                    </div>
                                </div>
                                <div class="line">
                                    <div class="column w-20-pc">
                                        &nbsp;
                                    </div>
                                    <div class="column w-80-pc">
                                        <div class="cell w-100-pc">
                                            <span class="attention">Atenção: Ao selecionar uma situação para a norma, a mesma não será alterada por vides e/ou decisões (ação).</span>
                                        </div>
                                    </div>
                                </div>
                                <div class="line line_situacao" style="display:none;">
                                    <div class="column w-20-pc">
                                        <div class="cell fr">
                                            <label>Situação da Norma*:</label>
                                        </div>
                                    </div>
                                    <div class="column w-70-pc">
                                        <div id="div_autocomplete_situacao" class="cell w-60-pc">
                                            <input id="ch_situacao" name="ch_situacao" type="hidden" value="" label="Situação da Norma" obrigatorio="nao" />
                                            <input id="nm_situacao" name="nm_situacao" type="text" value="" class="w-80-pc" /><a title="Listar" id="a_situacao"></a>
                                        </div>
                                    </div>
                                </div>
                                <%} %>
                                <div id="line_apelido" class="line">
                                    <div class="column w-20-pc">
                                        <div class="cell fr">
                                            <label>Apelido:</label>
                                        </div>
                                    </div>
                                    <div class="column w-70-pc">
                                        <div class="cell w-50-pc">
                                            <input id="nm_apelido" name="nm_apelido" type="text" value="" class="w-100-pc" />
                                        </div>
                                    </div>
                                </div>
                                <div class="line">
                                    <div class="column w-20-pc">
                                        <div class="cell fr">
                                            <label>Ementa*:</label>
                                        </div>
                                    </div>
                                    <div class="column w-70-pc">
                                        <div class="cell w-100-pc">
                                            <textarea id="ds_ementa" name="ds_ementa" obrigatorio="sim" label="Ementa" cols="100" rows="10" style="width:80%; max-width:100%;"></textarea>
                                        </div>
                                    </div>
                                </div>

                                <!-- Adicionando  projetoDeLei-->
                                <div class="line projetoDeLei">
                                    <div class="column w-20-pc">
                                        <div class="cell fr">
                                            <label>Número e Ano da Proposição de Origem:</label>
                                        </div>
                                    </div>
                                    <div class="column w-70-pc">
                                        <div class="cell w-80-pc">
                                            <input id="nr_projeto_lei" name="nr_projeto_lei" type="text" value="" class="w-50-pc"/>
                                        </div>
                                    </div>
                                </div>
                                
                                <div class="line projetoDeLei">
                                    <div class="column w-20-pc">
                                        <div class="cell fr">
                                            <label>Url da Proposição de Origem:</label>
                                        </div>
                                    </div>
                                    <div class="column w-70-pc">
                                        <div class="cell w-80-pc">
                                            <input id="url_projeto_lei" name="url_projeto_lei" type="text" value="" class="w-80-pc"/>
                                        </div>
                                    </div>
                                </div>



                                <div class="line">
                                    <div class="column w-20-pc">
                                        <div class="cell fr">
                                            <label>Observação:</label>
                                        </div>
                                    </div>
                                    <div class="column w-70-pc">
                                        <div class="cell w-100-pc">
                                            <textarea id="ds_observacao" name="ds_observacao" cols="100" rows="3" style="width:80%; max-width:100%;"></textarea>
                                        </div>
                                    </div>
                                </div>
                                <div class="line">
                                    <div class="column w-22-pc">
                                        <div class="cell fr">
                                            <label>Pendente de revisão:<input id="st_pendencia" name="st_pendencia" value="true" type="checkbox" title="Marca a norma como pendente." onclick="javascript:MarcarPendente();" /></label>
                                        </div>
                                    </div>
                                </div>
                                <div id="line_ds_pendencia" class="line">
                                    <div class="column w-20-pc">
                                        <div class="cell fr">
                                            <label>Comentário da Pendência:</label>
                                        </div>
                                    </div>
                                    <div class="column w-70-pc">
                                        <div class="cell w-100-pc">
                                            <textarea id="ds_pendencia" name="ds_pendencia" cols="100" rows="3" style="width:80%; max-width:100%;"></textarea>
                                        </div>
                                    </div>
                                </div>
                                <div class="line">
                                    <div class="column w-22-pc">
                                        <div class="cell fr">
                                            <label>Destacada:<input id="st_destaque" name="st_destaque" value="true" type="checkbox" title="Marca a norma como destacada."/></label>
                                        </div>
                                    </div>
                                </div>
                                <div id="line_autorias" class="line">
                                    <div class="column w-100-pc">
                                        <div class="cell w-100-pc">
                                            <div class="w-90-pc mauto">
                                                <button type="button" onclick="javascript:CriarModalAutoria();"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_add_p.png" alt="add"/> Adicionar Autoria</button>
                                            </div>
                                            <table id="table_autorias" class="w-90-pc mauto">
                                                <caption>Autorias</caption>
                                                <thead>
                                                    <tr>
                                                        <th>
                                                            Nome
                                                        </th>
                                                        <th style="width:25px;">
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <tbody id="tbody_autorias">
                                                    <tr class="tr_vazia">
                                                        <td colspan="2">
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                                <div id="line_interessados" class="line">
                                    <div class="column w-100-pc">
                                        <div class="cell w-100-pc">
                                            <div class="w-90-pc mauto">
                                                <button type="button" onclick="javascript:CriarModalInteressado();"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_add_p.png" alt="add"/> Adicionar Interessado</button>
                                            </div>
                                            <table id="table_interessados" class="w-90-pc mauto">
                                                <caption>Interessados</caption>
                                                <thead>
                                                    <tr>
                                                        <th>
                                                            Nome
                                                        </th>
                                                        <th style="width:25px;">
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <tbody id="tbody_interessados">
                                                    <tr class="tr_vazia">
                                                        <td colspan="2">
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
                                                <button type="button" onclick="javascript:CriarModalNome();"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_add_p.png" alt="add"/> Adicionar Nome</button>
                                            </div>
                                            <table id="table_nomes" class="w-90-pc mauto">
                                                <caption>Lista de Nomes</caption>
                                                <thead>
                                                    <tr>
                                                        <th>
                                                            Nome
                                                        </th>
                                                        <th style="width:25px;">
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <tbody id="tbody_nomes">
                                                    <tr class="tr_vazia">
                                                        <td colspan="2">
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </fieldset>
                    </div>
                    <div id="div_publicacoes" class="pos_cad">
                        <fieldset>
                            <legend>Publicações</legend>
                            <div class="mauto table">
                                <div class="line">
                                    <div class="column w-100-pc">
                                        <div class="cell w-100-pc">
                                            <div class="w-90-pc mauto">
                                                <button type="button" onclick="javascript:CriarModalFonte();"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_add_p.png" alt="add"/> Adicionar Fonte</button>
                                            </div>
                                            <table id="table_fontes" class="w-90-pc mauto word-no-break">
                                                <caption>Fontes</caption>
                                                <thead>
                                                    <tr>
                                                        <th>
                                                            Fonte
                                                        </th>
                                                        <th>
                                                            Publicação
                                                        </th>
                                                        <th>
                                                            Página
                                                        </th>
                                                        <th>
                                                            Coluna
                                                        </th>
                                                        <th>
                                                            Observação
                                                        </th>
                                                        <th>
                                                            Motivo
                                                        </th>
                                                        <th>
                                                            Arquivo
                                                        </th>
                                                        <th style="width:60px;">
                                                        </th>
                                                    </tr> 
                                                </thead>
                                                <tbody id="tbody_fontes">
                                                    <tr class="tr_vazia">
                                                        <td colspan="8">
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </fieldset>
                    </div>
                    <div id="div_indexacao" class="pos_cad">
                        <fieldset>
                            <legend>Indexação</legend>
                            <div class="mauto table">
                                <div class="line">
                                    <div class="column w-100-pc">
                                        <div class="cell w-100-pc">
                                            <div class="w-90-pc mauto">
                                                <button type="button" onclick="javascript:CriarModalIndexacao();"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_add_p.png" alt="add"/> Adicionar Indexação</button>
                                            </div>
                                            <table id="table_indexacoes" class="w-90-pc mauto">
                                                <caption>Termos de Indexação</caption>
                                                <thead>
                                                    <tr>
                                                        <th>
                                                            Termos
                                                        </th>
                                                        <th style="width:60px;">
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <tbody id="tbody_indexacoes">
                                                    <tr class="tr_vazia">
                                                        <td colspan="2">
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </fieldset>
                    </div>
                    <div id="div_dados_de_acoes">
                        <fieldset>
                            <legend>Dados de Ações</legend>
                            <div class="mauto table">
                                <div class="line">
                                    <div class="column w-100-pc">
                                        <div class="cell w-100-pc">
                                            <div class="w-90-pc mauto">
                                                <button type="button" onclick="javascript:CriarModalDecisao();"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_add_p.png" alt="add"/> Adicionar Decisão</button>
                                            </div>
                                            <table id="table_decisoes" class="w-90-pc mauto">
                                                <caption>Decisões</caption>
                                                <thead>
                                                    <tr>
                                                        <th>
                                                            Tipo*
                                                        </th>
                                                        <th>
                                                            Complemento
                                                        </th>
                                                        <th>
                                                            Data de Publicação*
                                                        </th>
                                                        <th style="width:60">
                                                        </th>
                                                    </tr> 
                                                </thead>
                                                <tbody id="tbody_decisoes">
                                                    <tr class="tr_vazia">
                                                        <td colspan="4"></td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                                <div class="line">
                                    <div class="column w-20-pc">
                                        <div class="cell fr">
                                            <label>Procedência:</label>
                                        </div>
                                    </div>
                                    <div class="column w-70-pc">
                                        <input name="ds_procedencia" type="text" value="" class="w-50-pc" />
                                    </div>
                                </div>
                                <div class="line">
                                    <div class="column w-20-pc">
                                        <div class="cell fr">
                                            <label>Parâmetro Constitucional:</label>
                                        </div>
                                    </div>
                                    <div class="column w-70-pc">
                                        <div class="cell w-100-pc">
                                            <input name="ds_paramentro_constitucional" type="text" value="" class="w-50-pc" />
                                        </div>
                                    </div>
                                </div>
                                <div class="line">
                                    <div class="column w-100-pc">
                                        <div class="cell w-100-pc">
                                            <div class="w-90-pc mauto">
                                                <button type="button" onclick="javascript:CriarModalRequerente();"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_add_p.png" alt="add"/> Adicionar Requerente</button>
                                            </div>
                                            <table id="table_requerentes" class="w-90-pc mauto">
                                                <caption>Requerentes</caption>
                                                <thead>
                                                    <tr>
                                                        <th>
                                                            Nome
                                                        </th>
                                                        <th style="width:60px;">
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <tbody id="tbody_requerentes">
                                                    <tr class="tr_vazia">
                                                        <td colspan="2">
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
                                                <button type="button" onclick="javascript:CriarModalRequerido();"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_add_p.png" alt="add"/> Adicionar Requerido</button>
                                            </div>
                                            <table id="table_requeridos" class="w-90-pc mauto">
                                                <caption>Requeridos</caption>
                                                <thead>
                                                    <tr>
                                                        <th>
                                                            Nome
                                                        </th>
                                                        <th style="width:60px;">
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <tbody id="tbody_requeridos">
                                                    <tr class="tr_vazia">
                                                        <td colspan="2">
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
                                                <button type="button" onclick="javascript:CriarModalProcurador();"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_add_p.png" alt="add"/> Adicionar Procurador</button>
                                            </div>
                                            <table id="table_procuradores" class="w-90-pc mauto">
                                                <caption>Procuradores Responsáveis</caption>
                                                <thead>
                                                    <tr>
                                                        <th>
                                                            Nome
                                                        </th>
                                                        <th style="width:60px;">
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <tbody id="tbody_procuradores">
                                                    <tr class="tr_vazia">
                                                        <td colspan="2">
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
                                                <button type="button" onclick="javascript:CriarModalRelator();"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_add_p.png" alt="add"/> Adicionar Relator</button>
                                            </div>
                                            <table id="table_relatores" class="w-90-pc mauto">
                                                <caption>Relatores</caption>
                                                <thead>
                                                    <tr>
                                                        <th>
                                                            Nome
                                                        </th>
                                                        <th style="width:60px;">
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <tbody id="tbody_relatores">
                                                    <tr class="tr_vazia">
                                                        <td colspan="2">
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
                                            <label>Texto da Ação:</label>
                                        </div>
                                    </div>
                                    <div class="column w-60-pc">
                                        <div id="arquivo_acao" class="cell w-100-pc">
                                            <input type="hidden" id="hidden_json_arquivo_texto_acao" name="json_arquivo_texto_acao" class="json_arquivo" value="" />
                                            <label id="label_arquivo_texto_acao" class="name" style="color:#000;"></label>
                                            <a href="javascript:void(0);" onclick="javascript:anexarInputFile(this);" class="attach" ><img valign="absmiddle" alt="Anexar" src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_attach_p.png"  /></a>
                                            <a href="javascript:void(0);" onclick="javascript:deletarInputFile(this);" class="delete" style="display:none;"><img valign="absmiddle" alt="Remover" src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_delete_p.png"  /></a>
                                            <a path="ar_acao" href="javascript:void(0);" onclick="javascript:editarInputFile(this);" class="create" title="Editar ou criar um arquivo"><img width="16" valign="absmiddle" alt="Editar" src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_edit_file.png" /></a>
                                            <a path="ar_acao" div_arquivo="arquivo_acao" href="javascript:void(0);" onclick="javascript:abrirModalImportarArquivo(this);" class="import" title="Importar arquivo do módulo de arquivos"><img width="16" valign="absmiddle" alt="importar" src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_import.png" /></a>
                                            <span class="attention">(Arquivo deve ser no formato .pdf ou .html)</span>
                                        </div>
                                    </div>
                                </div>
                                <div class="line">
                                    <div class="column w-20-pc">
                                        <div class="cell fr">
                                            <label>Efeito da Decisão:</label>
                                        </div>
                                    </div>
                                    <div class="column w-60-pc">
                                        <div class="cell w-100-pc">
                                            <select name="ds_efeito_decisao">
                                                <option value=""></option>
                                                <option value="Ex Nunc">Ex Nunc</option>
                                                <option value="Ex Tunc">Ex Tunc</option>
                                            </select>
                                        </div>
                                    </div>
                                </div>
                                <div class="line">
                                    <div class="column w-20-pc">
                                        <div class="cell fr">
                                            <label>Url Acompanhamento:</label>
                                        </div>
                                    </div>
                                    <div class="column w-80-pc">
                                        <div class="cell w-100-pc">
                                            <input name="url_referencia" type="text" value="" class="w-90-pc" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </fieldset>
                    </div>
                    <div style="width:210px; margin:auto; display:none;" class="loaded buttons">
                        <button id="button_salvar_norma">
                            <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_save.png" alt="add" width="18px" height="18px"/>  Salvar
                        </button>
                        <button type="reset">
                            <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_eraser_p.png" alt="cancela" width="18px" height="18px"/> Limpar
                        </button>
                    </div>
                </div>
            </form>
        </div>
    </div>
    <!-- FORMULÁRIO PARA ANEXAR ARQUIVO -->
    <div id="div_upload_file" style="display:none;">
        <form id="form_upload_file" name="form_upload_file" action="#" url-ajax="./ashx/Arquivo/UploadFile.ashx" method="post">
            <input type="hidden" name="nm_base" value="sinj_norma"/>
            <input type="file" name="file" onchange="javascript:salvarArquivoSelecionado(this)" id_div_file="" />
        </form>
    </div>

    <!-- Modal Importar Documentos Produzidos -->
    <div id="div_modal_importar_arquivo" style="display:none; max-width:850px;">
        <form id="form_importar_arquivo" name="form_importar_arquivo" action="#" url-ajax="./ashx/Arquivo/ImportFile.ashx" method="get" onsubmit="javascript:return importarArquivo('arquivo_fonte','form_importar_arquivo');">
            <input type="hidden" name="id_file" value="" />
            <input type="hidden" name="filename" value="" />
            <input type="hidden" name="nm_base_origem" value="sinj_arquivo" />
            <input type="hidden" name="nm_base_destino" value="sinj_norma" />
            <input type="hidden" name="ds_diario" value="" />
            <div class="notify" style="display:none;"></div>
            <div id="div_arquivos" class="div_arquivos">
                <div class="list_dir">
                    <div id="div_list_dir" class="div_list_dir">
                    </div>
                </div>
                <div class="list_arq">
                    <div id="div_list_arq">
                    </div>
                </div>
            </div>
            <div class="table w-100-pc" style="display:none;">
                <div class="line">
                    <div class="column w-10-pc">
                        <label>Arquivo:</label>
                    </div>
                    <div class="column w-90-pc">
                        <input class="input_chave_importar w-90-pc" disabled="disabled" id="input_chave_importar" label="Arquivo" obrigatorio="sim" type="text" value="" />
                    </div>
                </div>
            </div>
        </form>
    </div>
</asp:Content>
