<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="DetalhesDeVocabulario.aspx.cs" Inherits="TCDF.Sinj.Web.DetalhesDeVocabulario" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/funcoes_vocabulario.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            DetalhesVocabulario();
        });
    </script>
    <style type="text/css">
        #label_nm_termo{font-size:16px; margin-left:50px; color:#333;}
        .relacoes{margin-left:50px;}
        .relacoes ul{margin-left:15px;}
        .relacoes ul li a{color:#333 !important;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="divIdentificadorDePagina">
	    <label>.: Detalhes de Vocabulário</label>
	</div>
    <div id="div_controls" class="control">
    </div>
    <div class="form">
        <div id="div_vocabulario" class="loaded">
            <div class="mauto table w-80-pc">
                <div class="line">
                    <div class="column w-100-pc" style="padding-top:0px !important;">
                        <div id="div_controls_detalhes" class="cell fr">
                        </div>
                    </div>
                </div>
            </div>
            <fieldset>
                <legend id="label_tipo_termo">Vocabulário</legend>
                <div class="mauto table">
                    <div class="line">
                        <div class="column w-100-pc">
                            <div class="cell">
                                <label id="label_nm_termo"></label>
                                <img id="img_status" title="Termo Pendente" alt="Pendente" src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_alert_p.png" style="margin-bottom:-3px;" />
                            </div>
                        </div>
                    </div>
                    <div class="line div_termos_gerais" style="display:none;">
                        <div class="column w-100-pc">
                            <div class="cell relacoes">
                                <label>TG</label><br />
                                <ul id="ul_termos_gerais">
                                </ul>
                            </div>
                        </div>
                    </div>
                    <div class="line div_termos_especificos" style="display:none;">
                        <div class="column w-100-pc">
                            <div class="cell relacoes">
                                <label>TE</label><br />
                                <ul id="ul_termos_especificos">
                                </ul>
                            </div>
                        </div>
                    </div>
                    <div class="line div_termos_relacionados" style="display:none;">
                        <div class="column w-100-pc">
                            <div class="cell relacoes">
                                <label>TR</label><br />
                                <ul id="ul_termos_relacionados">
                                </ul>
                            </div>
                        </div>
                    </div>
                    <div class="line div_termos_itens" style="display:none;">
                        <div class="column w-100-pc">
                            <div class="cell relacoes">
                                <label>ITENS</label><br />
                                <ul id="ul_termos_itens">
                                </ul>
                            </div>
                        </div>
                    </div>
                    <div class="line div_termos_lista" style="display:none;">
                        <div class="column w-100-pc">
                            <div class="cell relacoes">
                                <label>LISTA AUXILIAR</label><br />
                                <ul id="ul_termos_lista">
                                </ul>
                            </div>
                        </div>
                    </div>
                    <div class="line div_termos_sublista" style="display:none;">
                        <div class="column w-100-pc">
                            <div class="cell relacoes">
                                <label>SUBLISTA</label><br />
                                <ul id="ul_termos_sublista">
                                </ul>
                            </div>
                        </div>
                    </div>
                    <div class="line div_termos_use" style="display:none;">
                        <div class="column w-100-pc">
                            <div class="cell relacoes">
                                <label>USE</label><br />
                                <ul id="ul_termos_use">
                                </ul>
                            </div>
                        </div>
                    </div>
                    <div class="line div_termos_nao_use" style="display:none;">
                        <div class="column w-100-pc">
                            <div class="cell relacoes">
                                <label>NÃO USE</label><br />
                                <ul id="ul_termos_nao_use">
                                </ul>
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>Nota Explicativa:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div id="div_nota_explicativa" class="cell w-60-pc">
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>Fontes Pesquisadas:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div id="div_fontes_pesquisadas" class="cell w-60-pc">
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>Norma de Origem:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div id="div_texto_fonte" class="cell w-60-pc">
                            </div>
                        </div>
                    </div>
					<div class="line no-hide">
	                    <div class="column w-100-pc">
	                        <div class="cell w-100-pc text-center" style="background-color:#CCC;">
	                            <a id="a_selecionar_normas" href="javascript:void(0);" onclick="javascript:ExpandirTabelaDeNormas();" class="expansible closed" >Exibir Normas <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_arrow_down.png"/></a>
	                            <div id="div_datatable_normas" style="display:none;" ></div>
	                        </div>
	                    </div>
	                </div>
                </div>
            </fieldset>
        </div>
        <div id="modal_descricao_origem" style="display:none;"></div>
        <div id="div_notificacao_vocabulario" class="notify" style="display:none;"></div>
        <div id="div_loading_vocabulario" class="loading" style="display:none;"></div>
    </div>
</asp:Content>
