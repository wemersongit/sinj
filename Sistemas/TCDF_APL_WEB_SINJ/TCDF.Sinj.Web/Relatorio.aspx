<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="Relatorio.aspx.cs" Inherits="TCDF.Sinj.Web.Relatorio" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/funcoes_vocabulario.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/funcoes_relatorio.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.tabs').tabs();
            ConstruirControlesDinamicos();
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="divIdentificadorDePagina">
	    <label>.: Relatórios</label>
	</div>
    <div class="w-90-pc mauto tabs">
        <ul>
            <li><a href="#tab_norma" >Norma</a></li>
            <li><a href="#tab_termo" >Termos</a></li>
        </ul>
	    <div id="tab_norma" class="form">
            <form id="form_relatorio_norma" name="formRelatorioNorma" action="#" method="get">
                <div id="div_norma" class="loaded">
                    <fieldset style="width:auto !important;">
                        <legend>Relatório de Normas</legend>
                        <div class="mauto table">
                            <div class="line">
                                <div class="column w-20-pc">
                                    <div class="cell fr">
                                        <label>Tipo de Norma:</label>
                                    </div>
                                </div>
                                <div class="column w-70-pc">
                                    <div class="cell w-60-pc">
                                        <div id="div_autocomplete_tipo_norma" class="cell w-60-pc">
                                            <input id="ch_tipo_norma" name="ch_tipo_norma" type="hidden" value="" />
                                            <input id="nm_tipo_norma" name="nm_tipo_norma" type="text" value="" class="w-80-pc" /><a title="Listar" id="a_tipo_norma"></a>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="line">
                                <div class="column w-20-pc">
                                    <div class="cell fr">
                                        <label>Data do Cadastro:</label>
                                    </div>
                                </div>
                                <div class="column w-70-pc">
                                    <div class="cell w-80-pc">
                                        <input id="dt_cadastro" name="dt_cadastro" type="text" value="" class="w-20-pc date" />&nbsp;até&nbsp;<input id="ate_dt_cadastro" name="dt_cadastro" type="text" value="" class="w-20-pc date" />
                                    </div>
                                </div>
                            </div>
                            <div class="line">
                                <div class="column w-20-pc">
                                    <div class="cell fr">
                                        <label>Data da Alteração:</label>
                                    </div>
                                </div>
                                <div class="column w-70-pc">
                                    <div class="cell w-80-pc">
                                        <input id="dt_alteracao" name="dt_alteracao" type="text" value="" class="w-20-pc date" />&nbsp;até&nbsp;<input id="Text2" name="dt_alteracao" type="text" value="" class="w-20-pc date" />
                                    </div>
                                </div>
                            </div>
                            <div class="line">
                                <div class="column w-20-pc">
                                    <div class="cell fr">
                                        <label>Origem:</label>
                                    </div>
                                </div>
                                <div class="column w-70-pc">
                                    <div id="div_autocomplete_origem_modal" class="cell w-100-pc">
                                        <input id="ch_orgao" name="ch_orgao" type="hidden" value="" />
                                        <input id="sg_hierarquia_nm_vigencia" type="text" value="" class="w-80-pc" /><a title="Listar" id="a_origem"></a>
                                    </div>
                                </div>
                            </div>
                            <div class="line">
                                <div class="column w-20-pc">
                                    <div class="cell fr">
                                        <label>Cadastrado por:</label>
                                    </div>
                                </div>
                                <div class="column w-70-pc">
                                    <div id="div_autocomplete_usuario_cadastrador_modal" class="cell w-100-pc">
                                        <input id="nm_login_usuario" name="nm_login_usuario_cadastro" type="hidden" value="" />
                                        <input id="nm_usuario" type="text" value="" class="w-80-pc" /><a title="Listar" id="a_usuario"></a>
                                        <input id="checkbox_cadastro_inativo" type="checkbox" value="true" name="cadastrado_por_inativo" onclick="javascript:BuscarUsuarioInativo()"/>  Inativos
                                    </div>
                                </div>
                            </div>
                            <div class="line">
                                <div class="column w-20-pc">
                                    <div class="cell fr">
                                        <label>Alterado por:</label>
                                    </div>
                                </div>
                                <div class="column w-70-pc">
                                    <div id="div_autocomplete_usuario_alterador_modal" class="cell w-100-pc">
                                        <input id="nm_login_usuario_alterador" name="nm_login_usuario_alteracao" type="hidden" value="" />
                                        <input id="nm_usuario_alterador" type="text" value="" class="w-80-pc" /><a title="Listar" id="a_usuario_alterador"></a>
                                        <input id="checkbox_alteracao_inativo" type="checkbox" value="true" name="alterado_por_inativo" onclick="javascript:BuscarUsuarioInativo()"/>  Inativos
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
                                        <label>Normas Pendentes:</label>
                                    </div>
                                </div>
                                <div class="column w-70-pc">
                                    <div id="div_normas_pendentes" class="cell w-100-pc">
                                        <input id="checkbox_st_pendencia" type="checkbox" value="true" name="st_pendencia">
                                    </div>
                                </div>
                            </div>
                            <div style="text-align:center; margin:auto;" class="loaded">
                                <button id="button_pesquisar_norma">
                                    <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_loupe_p.png" />
                                    Pesquisar
                                </button>
                                <button type="reset" >
                                    <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_eraser_p.png" />
                                    Limpar
                                </button>
                            </div>
                            <div id="div_relatorio_norma"></div>
                        </div>
                    </fieldset>
                </div>
                <div id="div_loading_norma" class="loading" style="display:none;"></div>
                <div id="div_notificacao_norma" class="notify" style="display:none;"></div>
            </form>
        </div>
	    <div id="tab_termo" class="form">
            <div id="div_termos_por_tipo">
                <fieldset style="width:auto !important;">
                    <legend>Quantidade de Termos por Tipo</legend>
                    <div class="loaded">
                        <div class="mauto table">
                            <div style="text-align:center; margin:auto;" class="loaded">
                                <button id="button_termos_por_tipo">
                                    <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_loupe_p.png" />
                                    Consultar Total de Termos
                                </button>
                            </div>
                            <div id="div_total_termos_por_tipo"></div>
                        </div>
                    </div>
                    <div class="loading" style="display:none;"></div>
                    <div class="notify" style="display:none;"></div>
                </fieldset>
            </div>
            <div id="div_termos_homonimos">
                <fieldset style="width:auto !important;">
                    <legend>Termos Homônimos</legend>
                    <div class="loaded">
                        <div class="mauto table">
                            <div style="text-align:center; margin:auto;" class="loaded">
                                <button id="button_termos_homonimos">
                                    <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_loupe_p.png" />
                                    Consultar Termos Homônimos
                                </button>
                            </div>
                            <div id="div_tabela_termos_homonimos"></div>
                        </div>
                    </div>
                    <div class="loading" style="display:none;"></div>
                    <div class="notify" style="display:none;"></div>
                </fieldset>
            </div>
            <div id="div_termos_nao_autorizados">
                    <fieldset style="width:auto !important;">
                        <legend>Termos Não Autorizados</legend>
                        <div class="loaded">
                            <div class="mauto table">
                                <div style="text-align:center; margin:auto;" class="loaded">
                                    <button id="button_termos_nao_autorizados">
                                        <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_loupe_p.png" />
                                        Consultar Termos Não Autorizados
                                    </button>
                                </div>
                                <div id="div_tabela_termos_nao_autorizados"></div>
                            </div>
                        </div>
                    </fieldset>
            </div>
        </div>
    </div>
    
    <div id="div_modal_filtro" style="display:none;">
        <label><input type="checkbox" name="filtro_ch_tipo_termo" value="AU" />Autoridade</label>
        <label><input type="checkbox" name="filtro_ch_tipo_termo" value="DE" />Descritor</label>
        <label><input type="checkbox" name="filtro_ch_tipo_termo" value="ES" />Especificador</label>
        <label><input type="checkbox" name="filtro_ch_tipo_termo" value="LA" />Lista</label>
    </div>
</asp:Content>
