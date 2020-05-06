<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="DetalhesDeUsuario.aspx.cs" Inherits="TCDF.Sinj.Web.DetalhesDeUsuario" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">

    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/funcoes_usuario.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            DetalhesUsuario();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="divIdentificadorDePagina">
	    <label>.: Detalhes de Usuário</label>
	</div>
    <div id="div_controls" class="control">
    </div>
    <div class="form">
        <div id="div_usuario" class="loaded">
            <div class="mauto table w-60-pc">
                <div class="line">
                    <div class="column w-100-pc" style="padding-top:0px !important;">
                        <div id="div_controls_detalhes" class="cell fr">
                        </div>
                    </div>
                </div>
            </div>
            <fieldset>
                <!--<legend>Usuário</legend>-->
                <div class="mauto table">
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>Login do Usuário:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div id="div_nm_login_usuario" class="cell w-60-pc">
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
                            <div id="div_nm_usuario" class="cell w-60-pc">
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
                            <div id="div_email_usuario" class="cell w-60-pc">
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>Página Inicial:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div id="div_pagina_inicial" class="cell w-60-pc">
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>Tema:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div id="div_tema" class="cell w-60-pc">
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>Último Login:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div id="div_dt_ultimo_login" class="cell w-60-pc">
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>Forçar Mudança de Senha:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div id="div_in_alterar_senha" class="cell w-60-pc">
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>Tentativas de Login:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div id="div_nr_tentativa_login" class="cell w-60-pc">
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
                            <div id="div_orgao_cadastrador" class="cell w-60-pc">
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>Ativo:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div class="cell w-60-pc">
                                <input id="checkbox_st_usuario" type="checkbox" disabled="disabled" />
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>Perfil:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div id="div_nm_perfil" class="cell w-60-pc">
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-100-pc">
                            <div class="cell w-100-pc">
                                <table class="w-100-pc mauto grupos" style="text-align:center;">
                                    <thead>
                                        <tr>
                                            <th colspan="6">Permissões de Cadastro</th>
                                        </tr>
                                        <tr>
                                            <th>Formulários / Ações</th>
                                            <th>Cadastrar</th>
                                            <th>Editar</th>
                                            <th>Pesquisar</th>
                                            <th>Visualizar</th>
                                            <th>Excluir</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <th>Normas</th>
                                            <td id="nor_inc" ></td>
                                            <td id="nor_edt" ></td>
                                            <td id="nor_pes" ></td>
                                            <td id="nor_vis" ></td>
                                            <td id="nor_exc" ></td>
                                        </tr>
                                        <tr>
                                            <th>Diário Oficial</th>
                                            <td id="dio_inc" ></td>
                                            <td id="dio_edt" ></td>
                                            <td id="dio_pes" ></td>
                                            <td id="dio_vis" ></td>
                                            <td id="dio_exc" ></td>
                                        </tr>
                                        <tr>
                                            <th>Tipo de Fonte</th>
                                            <td id="tdf_inc" ></td>
                                            <td id="tdf_edt" ></td>
                                            <td id="tdf_pes" ></td>
                                            <td id="tdf_vis" ></td>
                                            <td id="tdf_exc" ></td>
                                        </tr>
                                        <tr>
                                            <th>Tipo de Norma</th>
                                            <td id="tdn_inc" ></td>
                                            <td id="tdn_edt" ></td>
                                            <td id="tdn_pes" ></td>
                                            <td id="tdn_vis" ></td>
                                            <td id="tdn_exc" ></td>
                                        </tr>
                                        <tr>
                                            <th>Tipo de Publicação</th>
                                            <td id="tdp_inc" ></td>
                                            <td id="tdp_edt" ></td>
                                            <td id="tdp_pes" ></td>
                                            <td id="tdp_vis" ></td>
                                            <td id="tdp_exc" ></td>
                                        </tr>
                                        <tr>
                                            <th>Tipo de Diário</th>
                                            <td id="tdd_inc" ></td>
                                            <td id="tdd_edt" ></td>
                                            <td id="tdd_pes" ></td>
                                            <td id="tdd_vis" ></td>
                                            <td id="tdd_exc" ></td>
                                        </tr>
                                        <tr>
                                            <th>Interessado</th>
                                            <td id="int_inc" ></td>
                                            <td id="int_edt" ></td>
                                            <td id="int_pes" ></td>
                                            <td id="int_vis" ></td>
                                            <td id="int_exc" ></td>
                                        </tr>
                                        <tr>
                                            <th>Situação</th>
                                            <td id="sit_inc" ></td>
                                            <td id="sit_edt" ></td>
                                            <td id="sit_pes" ></td>
                                            <td id="sit_vis" ></td>
                                            <td id="sit_exc" ></td>
                                        </tr>
                                        <tr>
                                            <th>Tipo de Relação</th>
                                            <td id="tdr_inc" ></td>
                                            <td id="tdr_edt" ></td>
                                            <td id="tdr_pes" ></td>
                                            <td id="tdr_vis" ></td>
                                            <td id="tdr_exc" ></td>
                                        </tr>
                                        <tr>
                                            <th>Autoria</th>
                                            <td id="aut_inc" ></td>
                                            <td id="aut_edt" ></td>
                                            <td id="aut_pes" ></td>
                                            <td id="aut_vis" ></td>
                                            <td id="aut_exc" ></td>
                                        </tr>
                                        <tr>
                                            <th>Requerente</th>
                                            <td id="rqe_inc" ></td>
                                            <td id="rqe_edt" ></td>
                                            <td id="rqe_pes" ></td>
                                            <td id="rqe_vis" ></td>
                                            <td id="rqe_exc" ></td>
                                        </tr>
                                        <tr>
                                            <th>Requerido</th>
                                            <td id="rqi_inc" ></td>
                                            <td id="rqi_edt" ></td>
                                            <td id="rqi_pes" ></td>
                                            <td id="rqi_vis" ></td>
                                            <td id="rqi_exc" ></td>
                                        </tr>
                                        <tr>
                                            <th>Relator</th>
                                            <td id="rel_inc" ></td>
                                            <td id="rel_edt" ></td>
                                            <td id="rel_pes" ></td>
                                            <td id="rel_vis" ></td>
                                            <td id="rel_exc" ></td>
                                        </tr>
                                        <tr>
                                            <th>Procurador</th>
                                            <td id="pro_inc" ></td>
                                            <td id="pro_edt" ></td>
                                            <td id="pro_pes" ></td>
                                            <td id="pro_vis" ></td>
                                            <td id="pro_exc" ></td>
                                        </tr>
                                        <tr>
                                            <th>Órgão</th>
                                            <td id="org_inc" ></td>
                                            <td id="org_edt" ></td>
                                            <td id="org_pes" ></td>
                                            <td id="org_vis" ></td>
                                            <td id="org_exc" ></td>
                                        </tr>
                                        <tr>
                                            <th>Vocabulário</th>
                                            <td id="voc_inc" ></td>
                                            <td id="voc_edt" ></td>
                                            <td id="voc_pes" ></td>
                                            <td id="voc_vis" ></td>
                                            <td id="voc_exc" ></td>
                                        </tr>
                                        <tr>
                                            <th>Usuário</th>
                                            <td id="usr_inc" ></td>
                                            <td id="usr_edt" ></td>
                                            <td id="usr_pes" ></td>
                                            <td id="usr_vis" ></td>
                                            <td id="usr_exc" ></td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-100-pc">
                            <div class="cell w-100-pc">
                                <table class="grupos mauto w-100-pc text-center">
                                    <thead>
                                        <tr>
                                            <th>Permissões Especiais</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                            
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>

                    <div class="line">
                        <div class="column w-25-pc">
                            <div class="cell w-100-pc">
                                <table class="grupos mauto grupos w-200-px text-center">
                                    <thead>
                                        <tr>
                                            <th colspan="2">Auditoria</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <th>Erros</th>
                                            <td id="aud_err"></td>
                                        </tr>
                                        <tr>
                                            <th>Acessos</th>
                                            <td id="aud_ace"></td>
                                        </tr>
                                        <tr>
                                            <th>Operações</th>
                                            <td id="aud_ope"></td>
                                        </tr>
                                        <tr>
                                            <th>Pesquisas</th>
                                            <td id="aud_pes"></td>
                                        </tr>
                                        <tr>
                                            <th>Sessões</th>
                                            <td id="aud_ses"></td>
                                        </tr>
                                        <tr>
                                            <th>Lixeira</th>
                                            <td id="aud_lix"></td>
                                        </tr>
                                        <tr>
                                            <th>Push</th>
                                            <td id="aud_pus"></td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                        <div class="column w-25-pc">
                            <div class="cell w-100-pc">
                                <div class="line">
                                    <div class="column w-100-pc">
                                        <div class="cell w-100-pc">
                                            <table class="grupos w-200-px mauto text-center">
                                                <thead>
                                                    <tr>
                                                        <th colspan="2">Configurações</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <tr>
                                                        <th>Editar</th>
                                                        <td id="cfg_edt"></td>
                                                    </tr>
                                                    <tr>
                                                        <th>Visualizar</th>
                                                        <td id="cfg_vis"></td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                                <div class="line">
                                    <div class="column w-100-pc">
                                        <div class="cell w-100-pc">
                                            <table class="grupos w-200-px mauto text-center">
                                                <thead>
                                                    <tr>
                                                        <th colspan="2">Vocabulário</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <tr>
                                                        <th>Gerenciar</th>
                                                        <td id="voc_ger"></td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                                    
                            </div>
                        </div>
                            
                        <div class="column w-25-pc">
                            <div class="cell w-100-pc">
                                <div class="line">
                                    <div class="column w-100-pc">
                                        <div class="cell w-100-pc">
                                            <table class="grupos w-200-px mauto text-center">
                                                <thead>
                                                    <tr>
                                                        <th colspan="2">Relatórios</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <tr>
                                                        <th>Pesquisar</th>
                                                        <td id="rio_pes"></td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                                <div class="line">
                                    <div class="column w-100-pc">
                                        <div class="cell w-100-pc">
                                            <table class="grupos w-200-px mauto text-center">
                                                <thead>
                                                    <tr>
                                                        <th colspan="2">Normas</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <tr>
                                                        <th>Forçar Situação</th>
                                                        <td id="nor_fst"></td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>
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
        <div id="div_notificacao_usuario" class="notify" style="display:none;"></div>
        <div id="div_loading_usuario" class="loading" style="display:none;"></div>
    </div>
</asp:Content>
