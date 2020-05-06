<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="DetalhesDeOrgao.aspx.cs" Inherits="TCDF.Sinj.Web.DetalhesDeOrgao" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/funcoes_orgao.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            DetalhesOrgao();
        });
		
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="divIdentificadorDePagina">
	    <label>.: Detalhes do Órgão</label>
	</div>
    <div id="div_controls" class="control">
        <div class="div-light-button" id="button_atualizar_normas" style="display:none;">
            <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_manage_p.png" >Atualizar Normas
        </div>
    </div>
    <div id="modal_confirmacao_atualizar_normas" style="display:none;">
    </div>
    <div class="form">
        <div id="modal_descricao_origem" style="display:none;"></div>
        <div id="div_orgao_loading" class="loading" style="display:none;"></div>
        <div id="div_orgao_notificacao" style="display:none;"></div>
        <div id="div_orgao">
            <br />
            <div class="mauto table w-80-pc">
                <div class="line">
                    <div class="column w-100-pc" style="padding-top:0px !important;">
                        <div id="div_controls_detalhes" class="cell fr">
                        </div>
                    </div>
                </div>
            </div>
            <fieldset>
                <!--<legend>Órgão</legend>-->
                <div class="mauto table">
                    <div class="line">
                        <div class="column w-20-pc">
                            <div class="cell fr">
                                <label>Nome:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div id="div_nm_orgao" class="cell w-100-pc word-no-break">
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
                            <div id="div_sg_orgao" class="cell w-80-pc">
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
                            <div id="div_ambito" class="cell w-100-pc">
                                    
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
                                                <div id="div_dt_inicio_vigencia" class="cell w-50-pc">
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
                                                <div id="div_norma_inicio_vigencia" class="cell w-50-pc">
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
                                                <div id="div_dt_fim_vigencia" class="cell w-50-pc">
                                                </div>
                                            </div>
                                        </div>
                                        <div id="Div2" class="line">
                                            <div class="column w-20-pc">
                                                <div class="cell fr">
                                                    <label>Norma Associada:</label>
                                                </div>
                                            </div>
                                            <div class="column w-70-pc">
                                                <div id="div_norma_fim_vigencia" class="cell w-50-pc">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </fieldset>
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-100-pc">
                            <div class="cell w-100-pc">
                                <table id="table_orgao_superior" class="w-90-pc mauto">
                                    <caption>Órgão Superior</caption>
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
                                            <th>
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody id="tbody_orgao_superior" class="word-no-break">
                                        <tr>
                                            <td colspan="8">
                                                Nenhum órgão superior
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
                                <table id="table_orgaos_inferiores" class="w-90-pc mauto">
                                    <caption>Órgãos Inferiores</caption>
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
                                            <th>
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody id="tbody_orgaos_inferiores" class="word-no-break">
                                        <tr>
                                            <td colspan="8">
                                                Nenhum órgão inferior
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
                                <table id="table_orgaos_anteriores" class="w-90-pc mauto">
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
                                            <th>
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody id="tbody_orgaos_anteriores">
                                        <tr>
                                            <td colspan="8">
                                                Nenhum órgão anterior
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
                                <table id="table_orgaos_posteriores" class="w-90-pc mauto">
                                    <caption>Órgãos Posteriores</caption>
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
                                            <th>
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody id="tbody_orgaos_posteriores">
                                        <tr>
                                            <td colspan="8">
                                                Nenhum órgão posterior
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
					<div class="line no-hide">
	                    <div class="column w-100-pc">
	                        <div class="cell w-100-pc text-center" style="background-color:#CCC;">
	                            <a id="a_selecionar_normas" href="javascript:void(0);" class="expansible closed" >Exibir Normas <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_arrow_down.png"/></a>
	                            <%--<a id="a1" href="javascript:void(0);" onclick="javascript:ExpandirTabelaDeNormas('');" class="expansible closed" >Exibir Normas <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_arrow_down.png"/></a>--%>
	                            <div id="div_datatable_normas" style="display:none;" ></div>
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
        <br />
    </div>
</asp:Content>
