<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="Notifiqueme.aspx.cs" Inherits="TCDF.Sinj.Web.Notifiqueme" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/funcoes_notifiqueme.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/funcoes_norma.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/sessao_notifiqueme.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="divIdentificadorDePagina">
        <label>
            .: Notifique-me</label>
    </div>
    <div style="height: 50px;">
        <div id="div_notificacao_notifiqueme" class="notify" style="display: none;">
        </div>
    </div>
    <div id="div_loading_notifiqueme" class="loading" style="display: none;">
    </div>
    <div id="div_notifiqueme">
        <div class="w-90-pc mauto tabs">
            <ul>
                <li><a href="#tab_notificar_acompanhar_diario">Acompanhar inclusão de Diários</a></li>
                <li><a href="#tab_notificar_cadastro_normas">Acompanhar inclusão de Normas</a></li>
                <li><a href="#tab_notificar_edicao_normas">Acompanhar alteração de Normas</a></li>
                <li><a href="#tab_atualizar_meus_dados">Alterar meus dados</a></li>
            </ul>
            <div id="tab_notificar_acompanhar_diario" class="form">
                <div id="div_notificar_acompanhar_diario">
                    <fieldset class="w-90-pc">
                        <div class="mauto table w-90-pc">
                            <form id="form_termo_acompanhar_diario" url-ajax="./ashx/Push/NotifiquemeTermoDiarioIncluir.ashx" name="formTermoAcompanharDiario" action="#" method="post" onsubmit="return adicionarTermoDiario('form_termo_acompanhar_diario');">
                                <div id="div_notificacao_notificar_acompanhar_diario" class="notify" style="display: none;"></div>
                                <p><br />Receba uma notificação no seu e-mail caso seja incluído algum diário no SINJ contendo um texto monitorado por você.</p>
                                <input id="ch_termo_diario_monitorado" name="ch_termo_diario_monitorado" type="hidden" value="" />
                                <div class="line">
                                    <div class="column w-75-px">
                                        <div class="cell">
                                            <label>Diário:</label>
                                        </div>
                                    </div>
                                    <div id="div_autocomplete_tipo_fonte" class="column">
                                        <input id="ch_tipo_fonte_diario_monitorado" name="ch_tipo_fonte_diario_monitorado" type="hidden" value="" />
                                        <input id="nm_tipo_fonte_diario_monitorado" name="nm_tipo_fonte_diario_monitorado" type="text" value="" class="w-80-pc" /><a title="Listar" id="a_tipo_fonte"></a><a href="javascript:void(0);" data-toggle="tooltip" title='Informe em qual diário deseja monitorar o texto informado. Ex: DODF, DOCL, DJe. Se deixado em branco o texto será pesquisado em todos os diários.'>&nbsp;<img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_help.png" alt="info" width="12px" height="12px" /></a>
                                    </div>
                                </div>
                                <div class="line">
                                    <div class="column w-75-px">
                                        <div class="cell">
                                            <label>Texto*:</label>
                                        </div>
                                    </div>
                                    <div class="column w-350-px">
                                        <div class="cell w-100-pc">
                                            <input id="ds_termo_diario_monitorado" name="ds_termo_diario_monitorado" type="text" class="w-90-pc" obrigatorio="sim" label="Texto a ser monitorado" /><a href="javascript:void(0);" data-toggle="tooltip" title='Informe o texto que deseja monitorar nos diários. Este campo é obrigatório.'>&nbsp;<img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_help.png" alt="info" width="12px" height="12px" /></a>
                                        </div>
                                    </div>
                                    <div class="column">
                                        <div class="cell w-100-pc">
                                            <label><input title="A busca exata irá inserir, automaticamente, aspas no texto monitorado para garantir maior relevância à sua pesquisa." id="in_exata_diario_monitorado" name="in_exata_diario_monitorado" type="checkbox" checked="checked" style="vertical-align:middle;" value="1" />Buscar pela expressão exata.</label>
                                        </div>
                                    </div>
                                </div>
                                <div class="line">
                                    <div class="column w-400-px">
                                        <div class="cell fr">
                                            <button class="add" title="Adicionar um texto ou termo a ser monitoridado nos diários oficiais." type="submit"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_add_p.png" >Adicionar</button>
                                            <button class="edit" style="display:none;" title="Alterar os critérios monitorados." type="submit"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_disk_p.png" >Salvar</button>
                                            <button title="Limpar o formulário e não salvar." type="reset" onclick="fnCancelar('form_termo_acompanhar_diario');"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_delete_p.png" >Cancelar</button>
                                        </div>
                                    </div>
                                </div>
                            </form>
                        </div>
                        <div class="mauto table w-90-pc">
                            <div class="line">
                                <div class="column w-100-pc">
                                    <div class="cell w-100-pc">
                                        <div id="div_datatable_notificar_acompanhar_diario">
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </fieldset>
                </div>
            </div>
            <div id="tab_notificar_cadastro_normas" class="form">
                <div id="div_notificar_cadastro_normas">
                    <fieldset class="w-90-pc">
                        <form id="form_notificar_cadastro_normas" url-ajax="./ashx/Push/NotifiquemeCriacaoNormaIncluir.ashx" name="formNotificarCadastroNormas" action="#" method="post" onsubmit="return adicionarMonitoramentoDeCadastroDeNorma('form_notificar_cadastro_normas');">
                            <input id="ch_criacao_norma_monitorada" name="ch_criacao_norma_monitorada" type="hidden" value="" />
                            <div class="mauto table w-90-pc">
                                <div id="div_notificacao_notificar_cadastro_normas" class="notify" style="display: none;"></div>
                                <p><br />Receba uma notificação no seu e-mail caso ocorra a inclusão de alguma norma no SINJ dentro dos critérios monitorados por você.</p>
                                <div class="line">
                                    <div class="column w-110-px">
                                        <div class="cell">
                                            <label>Tipo de Norma:</label>
                                        </div>
                                    </div>
                                    <div class="column w-350-px">
                                        <div id="div_autocomplete_tipo_norma_modal" class="cell w-100-pc">
                                            <input id="ch_tipo_norma_criacao" name="ch_tipo_norma_criacao" type="hidden" value="" />
                                            <input id="nm_tipo_norma_criacao" name="nm_tipo_norma_criacao" type="text" value="" class="w-80-pc" /><a title="Listar" id="a_tipo_norma_criacao"></a><a href="javascript:void(0);" data-toggle="tooltip" title='O Tipo da Norma a ser monitorado. Ex.: Decreto, Lei, Portaria, etc.'>&nbsp;<img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_help.png" alt="info" width="12px" height="12px" /></a>
                                        </div>
                                    </div>
                                </div>
                                <div class="line">
                                    <div class="column w-110-px">
                                        <div class="cell">
                                            <label>Conector:</label>
                                        </div>
                                    </div>
                                    <div class="column w-350-px">
                                        <div class="cell w-60-pc">
                                            <select id="primeiro_conector_criacao" name="primeiro_conector_criacao">
                                                <option value=""></option>
                                                <option value="E">E</option>
                                                <option value="OU">OU</option>
                                            </select>
                                            <a href="javascript:void(0);" data-toggle="tooltip" title='Selecionar um conector para combinar os critérios a serem monitorados. Conecta o Tipo da Norma com o Órgão ou a Indexação.'>&nbsp;<img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_help.png" alt="info" width="12px" height="12px" /></a>
                                        </div>
                                    </div>
                                </div>
                                <div class="line">
                                    <div class="column w-110-px">
                                        <div class="cell">
                                            <label>Órgão:</label>
                                        </div>
                                    </div>
                                    <div class="column w-500-px">
                                        <div id="div_autocomplete_orgao_modal" class="cell w-100-pc">
                                            <input id="ch_orgao_criacao" name="ch_orgao_criacao" type="hidden" value="" />
                                            <input id="nm_orgao_criacao" name="nm_orgao_criacao" type="text" value="" class="w-80-pc" /><a title="Listar" id="a_orgao_criacao"></a>
                                            <a href="javascript:void(0);" data-toggle="tooltip" title='O Órgão que publíca a norma. Ex.: CLDF, PGDF, TCDF, etc.'>&nbsp;<img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_help.png" alt="info" width="12px" height="12px" /></a>
                                        </div>
                                    </div>
                                </div>
                                <div class="line">
                                    <div class="column w-110-px">
                                        <div class="cell">
                                            <label>Conector:</label>
                                        </div>
                                    </div>
                                    <div class="column w-350-px">
                                        <div class="cell w-60-pc">
                                            <select id="segundo_conector_criacao" name="segundo_conector_criacao">
                                                <option value=""></option>
                                                <option value="E">E</option>
                                                <option value="OU">OU</option>
                                            </select>
                                            <a href="javascript:void(0);" data-toggle="tooltip" title='Selecionar um conector para combinar os critérios a serem monitorados. Conecta o Tipo da Norma ou o Órgão com a Indexação.'>&nbsp;<img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_help.png" alt="info" width="12px" height="12px" /></a>
                                        </div>
                                    </div>
                                </div>
                                <div class="line">
                                    <div class="column w-110-px">
                                        <div class="cell">
                                            <label>Indexação:</label>
                                        </div>
                                    </div>
                                    <div class="column w-350-px">
                                        <div id="div_autocomplete_termo_modal" class="cell w-100-pc">
                                            <input id="ch_termo_criacao" name="ch_termo_criacao" type="hidden" value="" />
                                            <input id="ch_tipo_termo_criacao" name="ch_tipo_termo_criacao" type="hidden" value="" />
                                            <input id="nm_termo_criacao" name="nm_termo_criacao" type="text" value="" class="w-80-pc" /><a title="Listar" id="a_termo_criacao"></a>
                                            <a href="javascript:void(0);" data-toggle="tooltip" title='A Indexação compreende o vocabulário controlado atribuindo assuntos relacionados às normas publicadas. Ex.: Saneamento, Educação Infantil, Equipamento de Segurança, etc.'>&nbsp;<img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_help.png" alt="info" width="12px" height="12px" /></a>
                                        </div>
                                    </div>
                                </div>
                                <div class="line">
                                    <div class="column w-500-px">
                                        <div class="cell fr">
                                            <button class="add" title="Adicionar critérios de monitoração para quando uma norma for incluída no sistema." type="submit"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_add_p.png" >Adicionar</button>
                                            <button class="edit" style="display:none;" title="Alterar os critérios monitorados." type="submit"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_disk_p.png" >Salvar</button>
                                            <button title="Limpar o formulário e não salvar." type="reset" onclick="fnCancelar('form_notificar_cadastro_normas');"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_delete_p.png" >Cancelar</button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </form>
                        <div class="mauto table w-90-pc">
                            <div class="line">
                                <div class="column w-100-pc">
                                    <div class="cell w-100-pc">
                                        <div id="div_datatable_notificar_cadastro_normas"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </fieldset>
                </div>
            </div>
            <div id="tab_notificar_edicao_normas" class="form">
                <div id="div_notificar_edicao_normas">
                    <fieldset class="w-90-pc">
                        <div class="mauto table w-90-pc">
                            <div id="div_notificacao_notificar_edicao_normas" class="notify" style="display: none;"></div>
                            <p><br />Receba uma notificação no seu e-mail caso ocorra alguma alteração em uma norma monitorada por você.</p>
                            <div class="line">
                                <div class="column w-500-px">
                                    <div class="cell fl">
                                        <button title="Adicionar uma norma a ser monitoridada. Caso ela sofra alguma alteração, de cadastro ou por outra norma." type="button" onclick="CriarModalPesquisarNormasMonitorada();"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_add_p.png" >Adicionar</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="mauto table w-90-pc">
                            <div class="line">
                                <div class="column w-100-pc">
                                    <div class="cell w-100-pc">
                                        <div id="div_datatable_notificar_edicao_normas">
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </fieldset>
                </div>
            </div>

            <div id="tab_atualizar_meus_dados" class="form">
                <form id="form_notifiqueme_atualizar" name="formEdicaoNotifiqueme" post="post" action="#">
                    <div id="div_atualizar_meus_dados">
                        <fieldset class="w-90-pc">
                            <div class="mauto table w-90-pc">
                                <div class="line">
                                    <div class="column w-30-pc">
                                        <div class="cell fr">
                                            <label>Nome Completo:</label>
                                        </div>
                                    </div>
                                    <div class="column w-70-pc">
                                        <div class="cell w-60-pc">
                                            <input label="Nome Completo" obrigatorio="sim" id="nm_usuario_push" name="nm_usuario_push"
                                                type="text" value="" class="w-90-pc" />
                                        </div>
                                    </div>
                                </div>
                                <div class="line">
                                    <div class="column w-30-pc">
                                        <div class="cell fr">
                                            <label>E-mail:</label>
                                        </div>
                                    </div>
                                    <div class="column w-70-pc">
                                        <div class="cell w-60-pc">
                                            <input id="email_usuario_push" type="text" value="" class="w-90-pc" disabled="disabled" />
                                        </div>
                                    </div>
                                </div>
                                <div class="line">
                                    <div class="column w-30-pc">
                                        <div class="cell fr">
                                            <label>Senha Antiga:</label>
                                        </div>
                                    </div>
                                    <div class="column w-70-pc">
                                        <div class="cell w-60-pc">
                                            <input id="senha_usuario_push_antiga" name="senha_usuario_push_antiga" type="password" value="" class="w-30-pc" />
                                        </div>
                                    </div>
                                </div>
                                <div class="line">
                                    <div class="column w-30-pc">
                                        <div class="cell fr">
                                            <label>Senha Nova:</label>
                                        </div>
                                    </div>
                                    <div class="column w-70-pc">
                                        <div class="cell w-60-pc">
                                            <input id="senha_usuario_push" name="senha_usuario_push" type="password" value="" class="w-30-pc" />
                                        </div>
                                    </div>
                                </div>
                                <div class="line">
                                    <div class="column w-30-pc">
                                        <div class="cell fr">
                                            <label>Confirme a Senha:</label>
                                        </div>
                                    </div>
                                    <div class="column w-70-pc">
                                        <div class="cell w-60-pc">
                                            <input id="senha_usuario_push_confirme" name="senha_usuario_push" type="password" value="" class="w-30-pc" />
                                        </div>
                                    </div>
                                </div>
                                <div id="line_ativo" class="line">
                                    <div class="column w-30-pc">
                                        <div class="cell fr">
                                            <label>Ativo:</label>
                                        </div>
                                    </div>
                                    <div class="column w-30-pc">
                                        <div class="cell w-100-pc">
                                            <input id="st_push" name="st_push" type="checkbox" value="1" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="loaded text-center">
                                <button id="button_salvar_notifiqueme">
                                    <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_disk_p.png" />Salvar
                                </button>
                            </div>
                        </fieldset>
                    </div>
                    <div class="notify" style="display: none;">
                    </div>
                    <div class="loading" style="display: none;">
                    </div>
                </form>
            </div>
        </div>
    </div>
    <div id="modal_confirmacao_excluir" style="display:none;"></div>
    <div id="modal_pesquisar_normas_monitoradas" style="display:none;">
        <div class="table w-100-pc">
            <div class="line">
                <div class="column w-30-pc">
                    <div class="cell fr">
                        <label>Tipo:</label>
                    </div>
                </div>
                <div class="column w-70-pc">
                    <div id="div1" class="cell w-100-pc">
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
                            <a href="javascript:void(0);" onclick="javascript:PesquisarNorma();"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_loupe_p.png" alt="consultar" />Pesquisar</a>
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
