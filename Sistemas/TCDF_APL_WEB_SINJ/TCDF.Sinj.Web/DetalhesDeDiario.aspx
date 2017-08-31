<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="DetalhesDeDiario.aspx.cs" Inherits="TCDF.Sinj.Web.DetalhesDeDiario" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/jquery.highlight.js"></script>
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/funcoes_diario.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript" language="javascript">
        <%
            var highlight = HttpUtility.UrlDecode(Request.Form["highlight"]);
        %>
        var highlight = <%= !string.IsNullOrEmpty(highlight) ? highlight : "\"\"" %>;
        $(document).ready(function () {
            cookies_aux = ['back_history_aux_datatable', 'back_history_aux_filtros', 'back_history_aux_tab'];
            DetalhesDeDiario(highlight);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="divIdentificadorDePagina">
	    <label>.: Detalhes de Diário</label>
	</div>
    <div id="div_controls" class="control">
    </div>
    <div class="form">
        <div id="modal_texto" style="display:none;">
            <div class="loading" style="display:none;"></div>
            <div class="notify" style="display:none;"></div>
            <div class="text"></div>
        </div>
        <div id="div_diario" class="loaded">
            <div class="mauto table w-60-pc">
                <div class="line">
                    <div class="column w-100-pc" style="padding-top:0px !important;">
                        <div id="div_controls_detalhes" class="cell fr">
                        </div>
                    </div>
                </div>
            </div>
            <fieldset class="w-60-pc">
                <legend>Diário</legend>
                <div class="mauto table">
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>Nome do Tipo de Diário:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div id="div_nm_tipo_fonte" class="cell w-60-pc nm_tipo_fonte">
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>Nome do Tipo de Edição:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div id="div_nm_tipo_edicao" class="cell w-60-pc nm_tipo_edicao">
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>Diferencial Edição:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div id="div_nm_diferencial_edicao" class="cell w-60-pc nm_diferencial_edicao">
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
                            <div id="div_nr_diario" class="cell w-60-pc nr_diario">
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>Letra:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div id="div_cr_diario" class="cell w-60-pc">
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>Seção:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div id="div_secao_diario" class="cell w-60-pc secao_diario">
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>Data da Assinatura:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div id="div_dt_assinatura" class="cell w-60-pc dt_assinatura_text">
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>Pendente de Revisão:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div id="div_st_pendente" class="cell w-60-pc">
                            </div>
                        </div>
                    </div>
                    <div id="line_ds_pendencia" class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>Observação sobre pendência:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div id="div_ds_pendencia" class="cell w-60-pc">
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>Suplemento:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div id="div_st_suplemento" class="cell w-60-pc">
                            </div>
                        </div>
                    </div>
                    <div id="line_nm_diferencial_suplemento" class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>Diferencial Suplemento:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div id="div_nm_diferencial_suplemento" class="cell w-60-pc nm_diferencial_suplemento">
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>Arquivos:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div id="div_ar_diario" class="cell w-60-pc">
                            </div>
                        </div>
                    </div>
                   
					<div class="line no-hide">
	                    <div class="column w-100-pc">
	                        <div class="cell w-100-pc text-center" style="background-color:#CCC;">
	                            <a href="javascript:void(0);" onclick="javascript:ExpandirDadosDeCadastro();" class="expansible closed" >Cadastro <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_arrow_down.png"/></a>
	                        	<div id="div_detalhes_cadastro" style="display:none;"> 
									<span id="span_nome_usuario_cadastro"></span>
									<span id="span_data_cadastro"> -  </span>
								</div>
	                        </div>
	                    </div>
	                </div>
					<div class="line no-hide">
	                    <div class="column w-100-pc">
	                        <div class="cell w-100-pc text-center" style="background-color:#CCC;">
	                            <a href="javascript:void(0);" onclick="javascript:ExpandirAlteracoes();" class="expansible closed" >Alterações <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_arrow_down.png"/></a>
	                        	<div id="div_alteracoes" style="display:none;"> 
								</div>
	                        </div>
	                    </div>
	                </div>
                </div>
            </fieldset>
        </div>
        <div id="div_notificacao_diario" class="notify" style="display:none;"></div>
        <div id="div_loading_diario" class="loading" style="display:none;"></div>
    </div>
</asp:Content>
