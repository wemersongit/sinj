<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="EditarUsuario.aspx.cs" Inherits="TCDF.Sinj.Web.EditarUsuario" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">

    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/funcoes_usuario.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $('#button_salvar_usuario').click(function () {
                return fnSalvar("form_usuario");
            });
            ConstruirControlesDinamicos();
            PreencherUsuarioEdicao();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="divIdentificadorDePagina">
	    <label>.: Editar Usuário</label>
	</div>
    <div id="div_controls" class="control">
    </div>
    <div class="form">
        <form id="form_usuario" name="formEdicaoUsuario" action="#" method="post">
            <input id="id_doc" name="id_doc" type="hidden" value="" class="w-90-pc" />
            <div id="div_usuario">
                <fieldset>
                    <legend>Usuário</legend>
                    <div id="div_notificacao_usuario" class="notify" style="display:none;"></div>
                    <div class="mauto table">
                        <div class="line">
                            <div class="column w-30-pc">
                                <div class="cell fr">
                                    <label>Nome do Usuário:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div class="cell w-60-pc">
                                    <input label="Nome" obrigatorio="sim" id="nm_usuario" name="nm_usuario" type="text" value="" class="w-90-pc" />
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
                                    <input id="email_usuario" name="email_usuario" type="text" value="" class="w-90-pc" />
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
                                    <input id="pagina_inicial" name="pagina_inicial" type="hidden" value="" />
                                    <input id="ds_pagina_inicial" name="ds_pagina_inicial" type="text" value="" class="w-80-pc" /><a title="Listar" id="a_pagina_inicial"></a>
                                </div>
                            </div>
                        </div>
                        <div class="line">
                            <div class="column w-30-pc">
                                <div class="cell fr">
                                    <label>Tema de Cores:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div class="cell w-60-pc">
                                    <select id="ch_tema" name="ch_tema"></select>
                                </div>
                            </div>
                        </div>
                        <div class="line">
                            <div class="column w-30-pc">
                                <div class="cell fr">
                                    <label>Senha:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div class="cell w-100-pc">
                                    <input type="password" id="senha_usuario" name="senha_usuario" value="" />
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
                                <div class="cell w-100-pc">
                                    <input type="password" id="redigite_senha_usuario" value="" />
                                </div>
                            </div>
                        </div>
                        <div class="line">
                            <div class="column w-30-pc">
                                <div class="cell fr">
                                    <label>Forçar mudança de senha:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div class="cell w-100-pc">
                                    <input type="checkbox" id="in_alterar_senha" name="in_alterar_senha" value="true" />
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
                                <div class="cell w-60-pc">
                                    <select label="Órgão Cadastrador" obrigatorio="sim" id="orgao_cadastrador" name="orgao_cadastrador"></select>
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
                                    <input type="checkbox" id="st_usuario" name="st_usuario" value="true" />
                                </div>
                            </div>
                        </div>
                        <div class="line">
                            <div class="column w-30-pc">
                                <div class="cell fr">
                                    <label>Selecionar Perfil:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div id="div_perfil" class="cell w-60-pc">
                                    <input id="ch_perfil" name="ch_perfil" type="hidden" value="" />
                                    <input id="autocomplete_grupos" type="hidden" value="" />
                                    <input id="nm_perfil" name="nm_perfil" type="text" value="" class="w-80-pc" onblur="javascript:SelecionarPerfil();" /><a title="Listar" id="a_perfil"></a>
                                </div>
                            </div>
                        </div>
                        <div class="line">
                            <div class="column w-90-pc">
                                <div class="cell w-100-pc">
                                    <table id="grupos" class="grupos" class="w-90-pc mauto" style="text-align:center; margin:auto;">
                                        <thead>
                                            <tr>
                                                <th>Formulários / Ações</th>
                                                <th>Cadastrar</th>
                                                <th>Editar</th>
                                                <th>Pesquisar</th>
                                                <th>Visualizar</th>
                                                <th>Excluir</th>
                                                <th>Gerenciar</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr>
                                                <th>Normas</th>
                                                <td><input type="checkbox" id="nor_inc" name="grupos" title="Cadastrar Normas" value="NOR.INC" /></td>
                                                <td><input type="checkbox" id="nor_edt" name="grupos" title="Editar Normas" value="NOR.EDT" /></td>
                                                <td><input type="checkbox" id="nor_pes" name="grupos" title="Pesquisar Normas" value="NOR.PES" /></td>
                                                <td><input type="checkbox" id="nor_vis" name="grupos" title="Visualizar Normas" value="NOR.VIS" /></td>
                                                <td><input type="checkbox" id="nor_exc" name="grupos" title="Excluir Normas" value="NOR.EXC" /></td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <th>Diário Oficial</th>
                                                <td><input type="checkbox" id="dio_inc" name="grupos" title="Cadastrar Diário Oficial" value="DIO.INC" /></td>
                                                <td><input type="checkbox" id="dio_edt" name="grupos" title="Editar Diário Oficial" value="DIO.EDT" /></td>
                                                <td><input type="checkbox" id="dio_pes" name="grupos" title="Pesquisar Diário Oficial" value="DIO.PES" /></td>
                                                <td><input type="checkbox" id="dio_vis" name="grupos" title="Visualizar Diário Oficial" value="DIO.VIS" /></td>
                                                <td><input type="checkbox" id="dio_exc" name="grupos" title="Excluir Diário Oficial" value="DIO.EXC" /></td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <th>Tipo de Fonte</th>
                                                <td><input type="checkbox" id="tdf_inc" name="grupos" title="Cadastrar Tipo de Fonte" value="TDF.INC" /></td>
                                                <td><input type="checkbox" id="tdf_edt" name="grupos" title="Editar Tipo de Fonte" value="TDF.EDT" /></td>
                                                <td><input type="checkbox" id="tdf_pes" name="grupos" title="Pesquisar Tipo de Fonte" value="TDF.PES" /></td>
                                                <td><input type="checkbox" id="tdf_vis" name="grupos" title="Visualizar Tipo de Fonte" value="TDF.VIS" /></td>
                                                <td><input type="checkbox" id="tdf_exc" name="grupos" title="Excluir Tipo de Fonte" value="TDF.EXC" /></td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <th>Tipo de Norma</th>
                                                <td><input type="checkbox" id="tdn_inc" name="grupos" title="Cadastrar Tipo de Norma" value="TDN.INC" /></td>
                                                <td><input type="checkbox" id="tdn_edt" name="grupos" title="Editar Tipo de Norma" value="TDN.EDT" /></td>
                                                <td><input type="checkbox" id="tdn_pes" name="grupos" title="Pesquisar Tipo de Norma" value="TDN.PES" /></td>
                                                <td><input type="checkbox" id="tdn_vis" name="grupos" title="Visualizar Tipo de Norma" value="TDN.VIS" /></td>
                                                <td><input type="checkbox" id="tdn_exc" name="grupos" title="Excluir Tipo de Norma" value="TDN.EXC" /></td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <th>Tipo de Diário</th>
                                                <td><input type="checkbox" id="tdd_inc" name="grupos" title="Cadastrar Tipo de Diário" value="TDD.INC" /></td>
                                                <td><input type="checkbox" id="tdd_edt" name="grupos" title="Editar Tipo de Diário" value="TDD.EDT" /></td>
                                                <td><input type="checkbox" id="tdd_pes" name="grupos" title="Pesquisar Tipo de Diário" value="TDD.PES" /></td>
                                                <td><input type="checkbox" id="tdd_vis" name="grupos" title="Visualizar Tipo de Diário" value="TDD.VIS" /></td>
                                                <td><input type="checkbox" id="tdd_exc" name="grupos" title="Excluir Tipo de Diário" value="TDD.EXC" /></td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <th>Tipo de Publicação</th>
                                                <td><input type="checkbox" id="tdp_inc" name="grupos" title="Cadastrar Tipo de Publicação" value="TDP.INC" /></td>
                                                <td><input type="checkbox" id="tdp_edt" name="grupos" title="Editar Tipo de Publicação" value="TDP.EDT" /></td>
                                                <td><input type="checkbox" id="tdp_pes" name="grupos" title="Pesquisar Tipo de Publicação" value="TDP.PES" /></td>
                                                <td><input type="checkbox" id="tdp_vis" name="grupos" title="Visualizar Tipo de Publicação" value="TDP.VIS" /></td>
                                                <td><input type="checkbox" id="tdp_exc" name="grupos" title="Excluir Tipo de Publicação" value="TDP.EXC" /></td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <th>Interessado</th>
                                                <td><input type="checkbox" id="int_inc" name="grupos" title="Cadastrar Interessado" value="INT.INC" /></td>
                                                <td><input type="checkbox" id="int_edt" name="grupos" title="Editar Interessado" value="INT.EDT" /></td>
                                                <td><input type="checkbox" id="int_pes" name="grupos" title="Pesquisar Interessado" value="INT.PES" /></td>
                                                <td><input type="checkbox" id="int_vis" name="grupos" title="Visualizar Interessado" value="INT.VIS" /></td>
                                                <td><input type="checkbox" id="int_exc" name="grupos" title="Excluir Interessado" value="INT.EXC" /></td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <th>Situação</th>
                                                <td><input type="checkbox" id="sit_inc" name="grupos" title="Cadastrar Situação" value="SIT.INC" /></td>
                                                <td><input type="checkbox" id="sit_edt" name="grupos" title="Editar Situação" value="SIT.EDT" /></td>
                                                <td><input type="checkbox" id="sit_pes" name="grupos" title="Pesquisar Situação" value="SIT.PES" /></td>
                                                <td><input type="checkbox" id="sit_vis" name="grupos" title="Visualizar Situação" value="SIT.VIS" /></td>
                                                <td><input type="checkbox" id="sit_exc" name="grupos" title="Excluir Situação" value="SIT.EXC" /></td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <th>Tipo de Relação</th>
                                                <td><input type="checkbox" id="tdr_inc" name="grupos" title="Cadastrar Tipo de Relação" value="TDR.INC" /></td>
                                                <td><input type="checkbox" id="tdr_edt" name="grupos" title="Editar Tipo de Relação" value="TDR.EDT" /></td>
                                                <td><input type="checkbox" id="tdr_pes" name="grupos" title="Pesquisar Tipo de Relação" value="TDR.PES" /></td>
                                                <td><input type="checkbox" id="tdr_vis" name="grupos" title="Visualizar Tipo de Relação" value="TDR.VIS" /></td>
                                                <td><input type="checkbox" id="tdr_exc" name="grupos" title="Excluir Tipo de Relação" value="TDR.EXC" /></td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <th>Autoria</th>
                                                <td><input type="checkbox" id="aut_inc" name="grupos" title="Cadastrar Autoria" value="AUT.INC" /></td>
                                                <td><input type="checkbox" id="aut_edt" name="grupos" title="Editar Autoria" value="AUT.EDT" /></td>
                                                <td><input type="checkbox" id="aut_pes" name="grupos" title="Pesquisar Autoria" value="AUT.PES" /></td>
                                                <td><input type="checkbox" id="aut_vis" name="grupos" title="Visualizar Autoria" value="AUT.VIS" /></td>
                                                <td><input type="checkbox" id="aut_exc" name="grupos" title="Excluir Autoria" value="AUT.EXC" /></td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <th>Requerente</th>
                                                <td><input type="checkbox" id="rqe_inc" name="grupos" title="Cadastrar Requerente" value="RQE.INC" /></td>
                                                <td><input type="checkbox" id="rqe_edt" name="grupos" title="Editar Requerente" value="RQE.EDT" /></td>
                                                <td><input type="checkbox" id="rqe_pes" name="grupos" title="Pesquisar Requerente" value="RQE.PES" /></td>
                                                <td><input type="checkbox" id="rqe_vis" name="grupos" title="Visualizar Requerente" value="RQE.VIS" /></td>
                                                <td><input type="checkbox" id="rqe_exc" name="grupos" title="Excluir Requerente" value="RQE.EXC" /></td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <th>Requerido</th>
                                                <td><input type="checkbox" id="rqi_inc" name="grupos" title="Cadastrar Requerido" value="RQI.INC" /></td>
                                                <td><input type="checkbox" id="rqi_edt" name="grupos" title="Editar Requerido" value="RQI.EDT" /></td>
                                                <td><input type="checkbox" id="rqi_pes" name="grupos" title="Pesquisar Requerido" value="RQI.PES" /></td>
                                                <td><input type="checkbox" id="rqi_vis" name="grupos" title="Visualizar Requerido" value="RQI.VIS" /></td>
                                                <td><input type="checkbox" id="rqi_exc" name="grupos" title="Excluir Requerido" value="RQI.EXC" /></td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <th>Relator</th>
                                                <td><input type="checkbox" id="rel_inc" name="grupos" title="Cadastrar Relator" value="REL.INC" /></td>
                                                <td><input type="checkbox" id="rel_edt" name="grupos" title="Editar Relator" value="REL.EDT" /></td>
                                                <td><input type="checkbox" id="rel_pes" name="grupos" title="Pesquisar Relator" value="REL.PES" /></td>
                                                <td><input type="checkbox" id="rel_vis" name="grupos" title="Visualizar Relator" value="REL.VIS" /></td>
                                                <td><input type="checkbox" id="rel_exc" name="grupos" title="Excluir Relator" value="REL.EXC" /></td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <th>Procurador</th>
                                                <td><input type="checkbox" id="pro_inc" name="grupos" title="Cadastrar Procurador" value="PRO.INC" /></td>
                                                <td><input type="checkbox" id="pro_edt" name="grupos" title="Editar Procurador" value="PRO.EDT" /></td>
                                                <td><input type="checkbox" id="pro_pes" name="grupos" title="Pesquisar Procurador" value="PRO.PES" /></td>
                                                <td><input type="checkbox" id="pro_vis" name="grupos" title="Visualizar Procurador" value="PRO.VIS" /></td>
                                                <td><input type="checkbox" id="pro_exc" name="grupos" title="Excluir Procurador" value="PRO.EXC" /></td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <th>Órgão</th>
                                                <td><input type="checkbox" id="org_inc" name="grupos" title="Cadastrar Órgão" value="ORG.INC" /></td>
                                                <td><input type="checkbox" id="org_edt" name="grupos" title="Editar Órgão" value="ORG.EDT" /></td>
                                                <td><input type="checkbox" id="org_pes" name="grupos" title="Pesquisar Órgão" value="ORG.PES" /></td>
                                                <td><input type="checkbox" id="org_vis" name="grupos" title="Visualizar Órgão" value="ORG.VIS" /></td>
                                                <td><input type="checkbox" id="org_exc" name="grupos" title="Excluir Órgão" value="ORG.EXC" /></td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <th>Vocabulário</th>
                                                <td><input type="checkbox" id="voc_inc" name="grupos" title="Cadastrar Vocabulário" value="VOC.INC" /></td>
                                                <td><input type="checkbox" id="voc_edt" name="grupos" title="Editar Vocabulário" value="VOC.EDT" /></td>
                                                <td><input type="checkbox" id="voc_pes" name="grupos" title="Pesquisar Vocabulário" value="VOC.PES" /></td>
                                                <td><input type="checkbox" id="voc_vis" name="grupos" title="Visualizar Vocabulário" value="VOC.VIS" /></td>
                                                <td><input type="checkbox" id="voc_exc" name="grupos" title="Excluir Vocabulário" value="VOC.EXC" /></td>
                                                <td><input type="checkbox" id="voc_ger" name="grupos" title="Gerenciar Vocabulário" value="VOC.GER" /></td>
                                            </tr>
                                            <tr>
                                                <th>Usuário</th>
                                                <td><input type="checkbox" id="usr_inc" name="grupos" title="Cadastrar Usuário" value="USR.INC" /></td>
                                                <td><input type="checkbox" id="usr_edt" name="grupos" title="Editar Usuário" value="USR.EDT" /></td>
                                                <td><input type="checkbox" id="usr_pes" name="grupos" title="Pesquisar Usuário" value="USR.PES" /></td>
                                                <td><input type="checkbox" id="usr_vis" name="grupos" title="Visualizar Usuário" value="USR.VIS" /></td>
                                                <td><input type="checkbox" id="usr_exc" name="grupos" title="Excluir Usuário" value="USR.EXC" /></td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <th>Configuração</th>
                                                <td></td>
                                                <td><input type="checkbox" id="cfg_edt" name="grupos" title="Editar Configuração" value="CFG.EDT" /></td>
                                                <td></td>
                                                <td><input type="checkbox" id="cfg_vis" name="grupos" title="Visualizar Configuração" value="CFG.VIS" /></td>
                                                <td></td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <th>Relatório</th>
                                                <td></td>
                                                <td></td>
                                                <td><input type="checkbox" id="rio_pes" name="grupos" title="Pesquisar Relatório" value="RIO.PES" /></td>
                                                <td></td>
                                                <td></td>
                                                <td></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    <table id="grupos_auditoria" class="grupos" class="w-60-pc mauto" style="text-align:center; margin:auto;">
                                        <thead>
                                            <tr>
                                                <th>Formulários / Ações</th>
                                                <th>Erros</th>
                                                <th>Acessos</th>
                                                <th>Operações</th>
                                                <th>Pesquisa</th>
                                                <th>Sessões</th>
                                                <th>Lixeira</th>
                                                <th>Push</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr>
                                                <th>Auditoria</th>
                                                <td><input type="checkbox" id="aud_err" name="grupos" title="Erros" value="AUD.ERR" /></td>
                                                <td><input type="checkbox" id="aud_ace" name="grupos" title="Acessos" value="AUD.ACE" /></td>
                                                <td><input type="checkbox" id="aud_ope" name="grupos" title="Operações" value="AUD.OPE" /></td>
                                                <td><input type="checkbox" id="aud_pes" name="grupos" title="Pesquisas" value="AUD.PES" /></td>
                                                <td><input type="checkbox" id="aud_ses" name="grupos" title="Sessões" value="AUD.SES" /></td>
                                                <td><input type="checkbox" id="aud_lix" name="grupos" title="Lixeira" value="AUD.LIX" /></td>
                                                <td><input type="checkbox" id="aud_pus" name="grupos" title="Push" value="AUD.PUS" /></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="div_buttons_usuario" style="width:220px; margin:auto;" class="loaded">
                        <button id="button_salvar_usuario">
                            <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_disk_p.png" />Salvar
                        </button>
                        <button onclick="javascript:window.history.back();">
                            <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_delete_p.png" />Cancelar
                        </button>
                    </div>
                </fieldset>
            </div>
            <div id="div_loading_usuario" class="loading" style="display:none;"></div>
        </form>
    </div>
</asp:Content>