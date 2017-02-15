<%@ Page Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true"
    CodeBehind="Notifiqueme.aspx.cs" Inherits="TCDF.Sinj.Portal.Web.Notifiqueme" %>

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
                <li><a href="#tab_notificar_cadastro_normas">Adicionar Norma</a></li>
                <li><a href="#tab_notificar_edicao_normas">Normas Monitoradas</a></li>
                <li><a href="#tab_atualizar_meus_dados">Minha Conta</a></li>
            </ul>
            <div id="tab_notificar_edicao_normas" class="form">
                <div id="div_notificar_edicao_normas">
                    <fieldset class="w-100-pc">
                        <legend>Monitorar Normas</legend>
                        <div class="mauto table w-90-pc">
                            <div class="line">
                                <div class="column w-100-pc">
                                    <div class="cell w-100-pc text-center">
                                        <div style="height: 50px;">
                                            <div id="div_notificacao_notificar_edicao_normas" class="notify" style="display: none;">
                                            </div>
                                        </div>
                                        <div class="div-light-button">
                                            <a href="javascript:void(0);" onclick="javascript:CriarModalPesquisarNormasMonitorada();"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_add_p.png" >Adicionar</a>
                                        </div>
                                        <div id="div_datatable_notificar_edicao_normas">
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </fieldset>
                </div>
            </div>
            <div id="tab_notificar_cadastro_normas" class="form">
                <div id="div_loading_notificar_cadastro_normas" class="loading" style="display:none;"></div>
                <div id="div_notificar_cadastro_normas">
                    <fieldset class="w-100-pc">
                        <legend>Verificar Novas Normas</legend>
                        <div class="mauto table w-90-pc">
                            <div class="line">
                                <div class="column w-100-pc">
                                    <div class="cell w-100-pc text-center">
                                        <div id="modal_notificar_cadastro_normas" style="display:none;">
                                            <div id="modal_notificar_cadastro_normas_notificacao" class="notify" style="display: none;"></div>
                                            <div class="table w-90-pc">
                                                <div class="line">
                                                    <div class="column w-20-pc">
                                                        <div class="cell fr">
                                                            <label>Tipo de Norma:</label>
                                                        </div>
                                                    </div>
                                                    <div class="column w-60-pc">
                                                        <div id="div_autocomplete_tipo_norma_modal" class="cell w-100-pc">
                                                            <input id="ch_tipo_norma_criacao" name="ch_tipo_norma_criacao" type="hidden" value="" />
                                                            <input id="nm_tipo_norma_criacao" name="nm_tipo_norma_criacao" type="text" value="" class="w-80-pc" /><a title="Listar" id="a_tipo_norma_criacao"></a>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="line">
                                                    <div class="column w-20-pc">
                                                        <div class="cell fr">
                                                            <label>Conector:</label>
                                                        </div>
                                                    </div>
                                                    <div class="column w-60-pc">
                                                        <div class="cell w-60-pc">
                                                            <select id="primeiro_conector_criacao" name="primeiro_conector_criacao">
                                                                <option value=""></option>
                                                                <option value="E">E</option>
                                                                <option value="OU">OU</option>
                                                            </select>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="line">
                                                    <div class="column w-20-pc">
                                                        <div class="cell fr">
                                                            <label>Órgão:</label>
                                                        </div>
                                                    </div>
                                                    <div class="column w-60-pc">
                                                        <div id="div_autocomplete_orgao_modal" class="cell w-100-pc">
                                                            <input id="ch_orgao_criacao" name="ch_orgao_criacao" type="hidden" value="" />
                                                            <input id="nm_orgao_criacao" name="nm_orgao_criacao" type="text" value="" class="w-80-pc" /><a title="Listar" id="a_orgao_criacao"></a>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="line">
                                                    <div class="column w-20-pc">
                                                        <div class="cell fr">
                                                            <label>Conector:</label>
                                                        </div>
                                                    </div>
                                                    <div class="column w-60-pc">
                                                        <div class="cell w-60-pc">
                                                            <select id="segundo_conector_criacao" name="segundo_conector_criacao">
                                                                <option value=""></option>
                                                                <option value="E">E</option>
                                                                <option value="OU">OU</option>
                                                            </select>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="line">
                                                    <div class="column w-20-pc">
                                                        <div class="cell fr">
                                                            <label>Indexação:</label>
                                                        </div>
                                                    </div>
                                                    <div class="column w-60-pc">
                                                        <div id="div_autocomplete_termo_modal" class="cell w-100-pc">
                                                            <input id="ch_termo_criacao" name="ch_termo_criacao" type="hidden" value="" />
                                                            <input id="ch_tipo_termo_criacao" name="ch_tipo_termo_criacao" type="hidden" value="" />
                                                            <input id="nm_termo_criacao" name="nm_termo_criacao" type="text" value="" class="w-80-pc" /><a title="Listar" id="a_termo_criacao"></a>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="line">
                                                    <div class="column w-20-pc">
                                                        <div class="cell fr">
                                                            <label>Ativo:</label>
                                                        </div>
                                                    </div>
                                                    <div class="column w-60-pc">
                                                        <div class="cell w-60-pc">
                                                            <input id="st_criacao" name="st_criacao" type="checkbox" checked="checked" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div style="height: 50px;">
                                            <div id="div_notificacao_notificar_cadastro_normas" class="notify" style="display: none;">
                                            </div>
                                        </div>
                                        <div class="div-light-button">
                                            <a href="javascript:void(0);" onclick="javascript:CriarModalNotificarCadastroNormas();"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_add_p.png" >Adicionar</a>
                                        </div>
                                        <div id="div_datatable_notificar_cadastro_normas">
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
                        <fieldset class="w-100-pc">
                            <legend>Atualizar Meus Dados</legend>
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
