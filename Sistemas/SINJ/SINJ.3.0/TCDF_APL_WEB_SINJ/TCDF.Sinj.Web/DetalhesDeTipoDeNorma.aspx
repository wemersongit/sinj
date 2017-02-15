<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="DetalhesDeTipoDeNorma.aspx.cs" Inherits="TCDF.Sinj.Web.DetalhesDeTipoDeNorma" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">

    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/funcoes_tipo_de_norma.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            DetalhesTipoDeNorma();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="divIdentificadorDePagina">
	    <label>.: Detalhes Tipo de Norma</label>
	</div>
    <div id="div_controls" class="control">
    </div>
    <div class="form">
        <div id="div_tipo_de_norma" class="loaded">
            <div class="mauto table w-60-pc">
                <div class="line">
                    <div class="column w-100-pc" style="padding-top:0px !important;">
                        <div id="div1" class="cell fr">
                        </div>
                    </div>
                </div>
            </div>
            <fieldset class="w-60-pc">
                <legend>Tipo de Norma</legend>
                <div class="mauto table">
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>Nome do Tipo de Norma:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div id="div_nm_tipo_norma" class="cell w-60-pc">
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>Descrição:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div id="div_ds_tipo_norma" class="cell w-60-pc">
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-30-pc">
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
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>Grupos:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div id="div_grupos" class="cell w-100-pc">
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>Conjunta:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div id="div_in_conjunta" class="cell w-100-pc">
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>Questionável:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div id="div_in_questionavel" class="cell w-100-pc">
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>Numeração por Órgão:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div id="div_in_numeracao_por_orgao" class="cell w-100-pc">
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>Apelidável:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div class="cell w-100-pc">
                                <div id="div_in_apelidavel" class="cell w-100-pc"></div>
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
        <div id="div_notificacao_tipo_de_norma" class="notify" style="display:none;"></div>
        <div id="div_loading_tipo_de_norma" class="loading" style="display:none;"></div>
    </div>
</asp:Content>
