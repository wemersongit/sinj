<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="GerenciarVocabulario.aspx.cs" Inherits="TCDF.Sinj.Web.GerenciarVocabulario" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/funcoes_vocabulario.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.tabs').tabs();
            ConstruirControlesDinamicos();
        });
    </script>
    <style type="text/css">
        table.relatorio_troca{border:1px solid #AAA;}
        table.relatorio_troca caption{background-color: #CCC; border:1px solid #AAA; padding:3px; font-weight:bold;}
        table.relatorio_troca tr td{border:1px solid #AAA; padding:3px;}
        table.relatorio_troca tr th{background-color: #DDD; border:1px solid #AAA; padding:3px;}
        tr.EEE {background-color: #EEE;}
        td.td_error {color:#990000; font-weight:bold;}
        td.td_success {color:#009900; font-weight:bold;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="divIdentificadorDePagina">
	    <label>.: Gerenciar Vocabulário</label>
	</div>
    <div id="div_controls" class="control">
    </div>
    <div class="w-90-pc mauto tabs">
        <ul>
            <li><a href="#tab_criar_lista">Criar Lista</a></li>
            <li><a href="#tab_trocar_termo" onclick="javascript:ClicarAbaTrocarTermo();">Trocar Termo</a></li>
            <li><a href="#tab_quarentena" onclick="javascript:ClicarAbaQuarentena();">Quarentena</a></li>
            <li><a href="#tab_termos_pendentes" onclick="javascript:ClicarAbaPendentes();">Termos Pendentes</a></li>
            <li><a href="#tab_termos_inativos" onclick="javascript:ClicarAbaInativos();">Termos Inativos</a></li>
            <li><a href="#tab_termos_restaurados" onclick="javascript:ClicarAbaRestaurados();">Termos Restaurados</a></li>
        </ul>
	    <div id="tab_criar_lista" class="form">
            <form id="form_criar_lista" name="formCriarLista" action="#" method="post">
                <input type="hidden" name="in_lista" value="true"/>
                <input type="hidden" name="ch_tipo_termo" value="LA"/>
                <div id="div_criar_lista">
                    <fieldset>
                        <legend>Criar Lista</legend>
                        <div class="mauto table">
                            <div class="line">
                                <div class="column w-30-pc">
                                    <div class="cell fr">
                                        <label>Selecionar Lista Superior:</label>
                                    </div>
                                </div>
                                <div class="column w-70-pc">
                                    <div id="div_autocomplete_lista_superior" class="cell w-100-pc">
                                        <input type="hidden" id="ch_lista_superior" name="ch_lista_superior" value=""/>
                                        <input type="text" id="nm_lista_superior" name="nm_lista_superior" value="" class="w-90-pc" /><a id="a_lista_superior"></a>
                                    </div>
                                </div>
                            </div>
                            <div class="line">
                                <div class="column w-30-pc">
                                    <div class="cell fr">
                                        <label>Nome:</label>
                                    </div>
                                </div>
                                <div class="column w-70-pc">
                                    <div class="cell w-100-pc">
                                        <input label="Nome" obrigatorio="sim" id="nm_termo" name="nm_termo" type="text" value="" class="w-90-pc"/>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="div_buttons_criar_lista" style="width:220px; margin:auto;" class="loaded">
                            <button id="button_criar_lista">
                                <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_disk_p.png" />Salvar
                            </button>
                            <button type="reset">
                                <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_eraser_p.png" />Limpar
                            </button>
                        </div>
                    </fieldset>
                </div>
                <div id="div_notificacao_criar_lista" class="notify" style="display:none;"></div>
                <div id="div_loading_criar_lista" class="loading" style="display:none;"></div>
            </form>
	    </div>
	    <div id="tab_trocar_termo" class="form">
            <div class="mauto table">
                <div class="line">
                    <div class="column w-100-pc">
                        <div class="cell w-100-pc text-center" style="background-color:#CCC;">
	                        <div id="div_datatable_torcar_termos"></div>
                        </div>
                    </div>
                </div>
                <div class="line termo-antigo">
                    <div class="column w-20-pc">
                        <div class="cell fr">
                            <label>Termo Antigo:</label>
                        </div>
                    </div>
                    <div class="column w-60-pc">
                        <div class="cell w-100-pc">
                            <input type="hidden" id="id_doc_termo_antigo_autocomplete" value=""/>
                            <input type="hidden" id="ch_tipo_termo_antigo_autocomplete" value=""/>
                            <input type="text" id="nm_termo_antigo_autocomplete" value="" class="w-90-pc" />
                        </div>
                    </div>
                </div>
                <div class="line termo-novo">
                    <div class="column w-20-pc">
                        <div class="cell fr">
                            <label>Termo Novo:</label>
                        </div>
                    </div>
                    <div class="column w-60-pc">
                        <div id="div_autocomplete_termo_novo" class="cell w-100-pc">
                            <input type="hidden" id="id_doc_termo_novo_autocomplete" value=""/>
                            <input type="text" id="nm_termo_novo_autocomplete" value="" class="w-90-pc" /><a id="a_termo_novo"></a>
                        </div>
                    </div>
                </div>
                <div class="line cancelar">
                    <div class="column w-100-pc">
                        <div class="cell w-100-pc text-center">
                            <button onclick="javascript:ContinuarTrocarTermo();">
                                <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_ok_p.png" />Continuar
                            </button>
                            <button onclick="javascript:CancelarTrocarTermo();">
                                <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_delete_p.png" />Cancelar
                            </button>
                        </div>
                    </div>
                </div>
            </div>
            <form id="form_trocar_termo" name="formTrocarTermo" action="#" method="post">
                <div id="div_trocar_termo">
                    <fieldset>
                        <legend>Trocar Termo</legend>
                        <div id="div_notificacao_trocar_termo" class="notify" style="display:none;"></div>
                        <div class="mauto table">
                            <div class="line">
                                <div class="column w-30-pc">
                                    <div class="cell fr">
                                        <label>Tipo de Termo:</label>
                                    </div>
                                </div>
                                <div class="column w-70-pc">
                                    <div class="cell w-60-pc">
                                        <input type="hidden" id="ch_tipo_termo_novo" name="ch_tipo_termo_novo" value="" />
                                        <div id="div_nm_tipo_termo"></div>
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
                                    <div class="cell w-60-pc">
                                        <input type="hidden" id="id_doc_termo_antigo" name="id_doc_termo_antigo" value="" />
                                        <input type="hidden" id="id_doc_termo_novo" name="id_doc_termo_novo" value="" />
                                        <input type="hidden" id="nm_termo_novo" name="nm_termo_novo" value="" />
                                        <div id="div_nm_termo_novo"></div>
                                    </div>
                                </div>
                            </div>
                            <div class="line DE">
                                <div class="column w-20-pc">
                                    <div class="cell fr">
                                        <label>Termos Gerais:</label>
                                    </div>
                                </div>
                                <div class="column w-70-pc">
                                    <div class="cell w-100-pc">
                                        <div id="termos_gerais_trocar_termo" class="lista w-80-percent simularInputText min-h-20-px" >
	                                    </div>
                                    </div>
                                </div>
                            </div>
                            <div class="line">
                                <div class="column w-20-pc">
                                    <div class="cell fr">
                                        <label>Termos Não Use:</label>
                                    </div>
                                </div>
                                <div class="column w-70-pc">
                                    <div class="cell w-100-pc">
                                        <div id="termos_nao_autorizados_trocar_termo" class="lista w-80-percent simularInputText min-h-20-px" >
	                                    </div>
                                    </div>
                                </div>
                            </div>
                            <div class="line DE">
                                <div class="column w-20-pc">
                                    <div class="cell fr">
                                        <label>Termos Relacionados:</label>
                                    </div>
                                </div>
                                <div class="column w-70-pc">
                                    <div class="cell w-100-pc">
                                        <div id="termos_relacionados_trocar_termo" class="lista w-80-percent simularInputText min-h-20-px" >
	                                    </div>
                                    </div>
                                </div>
                            </div>
                            <div class="line DE">
                                <div class="column w-20-pc">
                                    <div class="cell fr">
                                        <label>Termos Especificos:</label>
                                    </div>
                                </div>
                                <div class="column w-70-pc">
                                    <div class="cell w-100-pc">
                                        <div id="termos_especificos_trocar_termo" class="lista w-80-percent simularInputText min-h-20-px" >
	                                    </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="div_buttons_trocar_termo" style="width:220px; margin:auto;" class="loaded">
                            <button onclick="javascript:return fnTrocarTermo();">
                                <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_disk_p.png" />Salvar
                            </button>
                            <button onclick="javascript:return CancelarTrocarTermo();">
                                <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_delete_p.png" />Cancelar
                            </button>
                        </div>
                    </fieldset>
                </div>
                <div id="div_loading_trocar_termo" class="loading" style="display:none;"></div>
            </form>
	    </div>
	    <div id="tab_quarentena">
            <div class="mauto table w-90-pc">
                <div class="line">
                    <div class="column w-100-pc">
                        <div class="cell w-100-pc text-center">
                            <div style="height:50px;">
                                <div id="div_notificacao_termos_quarentena" class="notify" style="display:none;"></div>
                            </div>
	                        <div id="div_datatable_termos_quarentena"></div>
                        </div>
                    </div>
                </div>
            </div>
	    </div>
	    <div id="tab_termos_pendentes">
            <div class="mauto table w-90-pc">
                <div class="line">
                    <div class="column w-100-pc">
                        <div class="cell w-100-pc text-center">
                            <div style="height:50px;">
                                <div id="div_notificacao_termos_pendentes" class="notify" style="display:none;"></div>
                            </div>
	                        <div id="div_datatable_termos_pendentes"></div>
                        </div>
                    </div>
                </div>
            </div>
	    </div>
	    <div id="tab_termos_inativos">
            <div class="mauto table w-90-pc">
                <div class="line">
                    <div class="column w-100-pc">
                        <div class="cell w-100-pc text-center">
                            <div style="height:50px;">
                                <div id="div_notificacao_termos_inativos" class="notify" style="display:none;"></div>
                            </div>
	                        <div id="div_datatable_termos_inativos"></div>
                        </div>
                    </div>
                </div>
            </div>
	    </div>
	    <div id="tab_termos_restaurados">
            <div class="mauto table w-90-pc">
                <div class="line">
                    <div class="column w-100-pc">
                        <div class="cell w-100-pc text-center">
                            <div style="height:50px;">
                                <div id="div_notificacao_termos_restaurados" class="notify" style="display:none;"></div>
                            </div>
                            <div id="div_selecionar_termos_restaurados">
                                <a class="fr button" href="javascript:void(0);" onclick="javascript:trocarTermosRestaurados();">Trocar</a>
	                            <div id="div_datatable_termos_restaurados"></div>
                            </div>
                            <div id="div_selecionados_trocar_termos_restaurados" class="mauto table w-100-pc" style="display:none;">
                                <div class="line">
                                    <div class="column w-20-pc">
                                        <div class="cell fr">
                                            <label>Termos Antigos:</label>
                                        </div>
                                    </div>
                                    <div class="column w-60-pc">
                                        <div class="cell w-100-pc">
                                            <div id="div_termos_antigos_restaurados">
                                                
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="line">
                                    <div class="column w-20-pc">
                                        <div class="cell fr">
                                            <label>Termo Novo:</label>
                                        </div>
                                    </div>
                                    <div class="column w-60-pc">
                                        <div id="div_autocomplete_termo_novo_restaurados" class="cell w-100-pc">
                                            <input type="hidden" id="id_doc_termo_novo_restaurados_autocomplete" value=""/>
                                            <input type="text" id="nm_termo_novo_restaurados_autocomplete" value="" class="w-90-pc" /><a id="a_termo_novo_restaurados"></a>
                                        </div>
                                    </div>
                                </div>
                                <div class="line">
                                    <div class="column w-100-pc">
                                        <div class="cell w-100-pc text-center">
                                            <button onclick="javascript:continuarTrocarTermosRestaurados();">
                                                <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_ok_p.png" />Continuar
                                            </button>
                                            <button onclick="javascript:cancelarTrocarTermosRestaurados();">
                                                <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_delete_p.png" />Cancelar
                                            </button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <form id="form_trocar_termos_restaurados" name="formTrocarTermosRestaurados" action="#" method="post" style="display:none;">
                                <div id="div_trocar_termos_restaurados">
                                    <fieldset>
                                        <legend>Trocar Termo</legend>
                                        <div id="div_notificacao_trocar_termo_restaurados" class="notify" style="display:none;"></div>
                                        <div class="mauto table w-90-pc">
                                            <div class="line">
                                                <div class="column w-30-pc">
                                                    <div class="cell fr">
                                                        <label>Tipo de Termo:</label>
                                                    </div>
                                                </div>
                                                <div class="column w-70-pc">
                                                    <div class="cell w-60-pc">
                                                        <input type="hidden" id="ch_tipo_termo_novo_restaurados" name="ch_tipo_termo_novo" value="" />
                                                        <div id="div_nm_tipo_termo_restaurados"></div>
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
                                                    <div class="cell w-60-pc">
                                                        <div id="div_termos_antigos_restaurados_trocar"></div>
                                                        <input type="hidden" id="id_doc_termo_novo_restaurados" name="id_doc_termo_novo" value="" />
                                                        <input type="hidden" id="nm_termo_novo_restaurados" name="nm_termo_novo" value="" />
                                                        <div id="div_nm_termo_novo_restaurados"></div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="line DE">
                                                <div class="column w-20-pc">
                                                    <div class="cell fr">
                                                        <label>Termos Gerais:</label>
                                                    </div>
                                                </div>
                                                <div class="column w-70-pc">
                                                    <div class="cell w-100-pc">
                                                        <div id="termos_gerais_trocar_termo_restaurados" class="lista w-80-percent simularInputText min-h-20-px" >
	                                                    </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="line">
                                                <div class="column w-20-pc">
                                                    <div class="cell fr">
                                                        <label>Termos Não Use:</label>
                                                    </div>
                                                </div>
                                                <div class="column w-70-pc">
                                                    <div class="cell w-100-pc">
                                                        <div id="termos_nao_autorizados_trocar_termo_restaurados" class="lista w-80-percent simularInputText min-h-20-px" >
	                                                    </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="line DE">
                                                <div class="column w-20-pc">
                                                    <div class="cell fr">
                                                        <label>Termos Relacionados:</label>
                                                    </div>
                                                </div>
                                                <div class="column w-70-pc">
                                                    <div class="cell w-100-pc">
                                                        <div id="termos_relacionados_trocar_termo_restaurados" class="lista w-80-percent simularInputText min-h-20-px" >
	                                                    </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="line DE">
                                                <div class="column w-20-pc">
                                                    <div class="cell fr">
                                                        <label>Termos Especificos:</label>
                                                    </div>
                                                </div>
                                                <div class="column w-70-pc">
                                                    <div class="cell w-100-pc">
                                                        <div id="termos_especificos_trocar_termo_restaurados" class="lista w-80-percent simularInputText min-h-20-px" >
	                                                    </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div id="div_buttons_trocar_termo_restaurados" style="width:220px; margin:auto;" class="loaded">
                                            <button onclick="javascript:return fnTrocarTermosRestaurados();">
                                                <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_disk_p.png" />Salvar
                                            </button>
                                            <button onclick="javascript:return cancelarTrocarTermosRestaurados();">
                                                <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_delete_p.png" />Cancelar
                                            </button>
                                        </div>
                                    </fieldset>
                                </div>
                            </form>
                        </div>
                    </div>
                </div>
            </div>
	    </div>
	</div>
    <div id="div_modal_filtro" style="display:none;">
        <label><input type="checkbox" name="filtro_ch_tipo_termo" value="AU" />Autoridade</label>
        <label><input type="checkbox" name="filtro_ch_tipo_termo" value="DE" />Descritor</label>
        <label><input type="checkbox" name="filtro_ch_tipo_termo" value="ES" />Especificador</label>
        <label><input type="checkbox" name="filtro_ch_tipo_termo" value="LA" />Lista</label>
    </div>
    <div id="div_modal_termo" style="display:none;">
    </div>
    <div id="div_modal_trocar_tipo_termo_novo" style="display:none;">
        <div class="mauto table w-100-pc">
            <div class="line">
                <div class="column w-20-pc">
                    <div class="cell fr">
                        <label>Tipo de Termo:</label>
                    </div>
                </div>
                <div class="column w-70-pc">
                    <div class="cell w-60-pc">
                        <select id="select_trocar_tipo_termo_novo">
                            <option value=""></option>
                            <option value="DE">Descritor</option>
                            <option value="ES">Especificador</option>
                            <option value="AU">Autoridade</option>
                            <option value="LA">Lista Auxiliar</option>
                        </select>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
