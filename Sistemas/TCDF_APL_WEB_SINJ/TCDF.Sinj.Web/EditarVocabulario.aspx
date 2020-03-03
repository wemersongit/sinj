<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="EditarVocabulario.aspx.cs" Inherits="TCDF.Sinj.Web.EditarVocabulario" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/funcoes_vocabulario.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript" >
        $(document).ready(function () {
            $('#button_salvar_vocabulario').click(function () {
                return fnSalvar("form_usuario");
            });
            ConstruirControlesDinamicos();
            PreencherVocabularioEdicao();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="divIdentificadorDePagina">
	    <label>.: Editar Vocabulário</label>
	</div>
    <div id="div_controls" class="control">
    </div>
    <div class="form">
        <form id="form_vocabulario" name="formCadastroVocabulario" action="#" method="post">
            <div id="div_vocabulario">
                <fieldset>
                    <!--<legend>Vocabulário</legend>-->
                    <div id="div_notificacao_vocabulario" class="notify" style="display:none;"></div>
                    <div class="mauto table">
                        <div class="line">
                            <div class="column w-20-pc">
                                <div class="cell fr">
                                    <label>Tipo de Termo:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div class="cell w-60-pc">
                                    <select id="ch_tipo_termo" name="ch_tipo_termo" label="Tipo de Termo" obrigatorio="sim" onchange="javascript:SelecionarTipoDeTermo();" disabled="disabled">
                                        <option value=""></option>
                                        <option value="DE">Descritor</option>
                                        <option value="ES">Especificador</option>
                                        <option value="AU">Autoridade</option>
                                        <option value="LA">Lista Auxiliar</option>
                                    </select>
                                </div>
                            </div>
                        </div>
                        <div class="line LA">
                            <div class="column w-20-pc">
                                <div class="cell fr">
                                    <label>Lista:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div class="cell w-100-pc">
                                    <input type="text" id="nm_lista_superior" name="nm_lista_superior" value="" class="w-90-pc" disabled="disabled" />
                                </div>
                            </div>
                        </div>
                        <div class="line no-hide">
                            <div class="column w-20-pc">
                                <div class="cell fr">
                                    <label>Termo:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div class="cell w-100-pc">
                                    <input label="Termo" obrigatorio="sim" id="nm_termo" name="nm_termo" type="text" value="" class="w-90-pc" disabled="disabled"/>
                                </div>
                            </div>
                        </div>
                        <div class="line no-hide">
                            <div class="column w-100-pc">
                                <div class="cell w-100-pc text-center" style="background-color:#CCC;">
	                                <a id="a_selecionar_termos" href="javascript:void(0);" onclick="javascript:ExpandirTabelaDeTermos();" class="expansible closed" >Selecionar Termos <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_arrow_down.png"/></a>
	                                <div id="div_datatable_termos" style="display:none;" ></div>
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
                                    <div id="termos_gerais" class="lista w-80-percent simularInputText" style="display:none;">
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
                                    <div id="termos_nao_autorizados" class="lista w-80-percent simularInputText" style="display:none;">
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
                                    <div id="termos_relacionados" class="lista w-80-percent simularInputText" style="display:none;">
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
                                    <div id="termos_especificos" class="lista w-80-percent simularInputText" style="display:none;">
	                                </div>
                                </div>
                            </div>
                        </div>
                        <div class="line no-hide">
                            <div class="column w-20-pc">
                                <div class="cell fr">
                                    <label>Nota Explicativa:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div class="cell w-100-pc">
	                                <textarea rows="5" id="ds_nota_explicativa" name="ds_nota_explicativa" class="w-80-pc" ></textarea>
                                </div>
                            </div>
                        </div>
                        <div class="line no-hide">
                            <div class="column w-20-pc">
                                <div class="cell fr">
                                    <label>Fontes Pesquisadas:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div class="cell w-100-pc">
	                                <textarea rows="5" id="ds_fontes_pesquisadas" name="ds_fontes_pesquisadas" class="w-80-pc" ></textarea>
                                </div>
                            </div>
                        </div>
                        <div class="line no-hide">
                            <div class="column w-20-pc">
                                <div class="cell fr">
                                    <label>Norma de Origem:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div class="cell w-100-pc">
	                                <textarea rows="5" id="ds_texto_fonte" name="ds_texto_fonte" class="w-80-pc" ></textarea>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="div_buttons_vocabulario" style="width:220px; margin:auto;" class="loaded">
                        <button id="button_salvar_vocabulario">
                            <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_disk_p.png" />Salvar
                        </button>
                        <button type="button" onclick="javascript:window.history.back();">
                            <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_delete_p.png" />Cancelar
                        </button>
                    </div>
                </fieldset>
            </div>
            <div id="div_loading_vocabulario" class="loading" style="display:none;"></div>
        </form>
    </div>
</asp:Content>
