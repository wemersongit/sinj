<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="PesquisarLixeira.aspx.cs" Inherits="TCDF.Sinj.Web.PesquisarLixeira" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('#div_autocomplete_user_erro').autocompletelight({
                sKeyDataName: "nm_login_usuario",
                sValueDataName: "nm_usuario",
                sInputHiddenName: "nm_login_user",
                sInputName: "nm_user",
                sAjaxUrl: './ashx/Autocomplete/UsuarioAutocomplete.ashx',
                bLinkAll: true,
                sLinkName: "a_user"
            });
            selecionarOperador(document.getElementsByName('op_intervalo')[0]);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="divIdentificadorDePagina">
	    <label>.: Pesquisar Lixeira</label>
	</div>
    <div class="form">
        <form id="form_pesquisa_lixeira" name="formPesquisaLixeira" action="./ResultadoDePesquisaLixeira.aspx" method="get">
            <div id="div_pesquisa">
                <fieldset>
                    <!--<legend>Pesquisa</legend>-->
                    <div class="mauto table">
                        <div class="line">
                            <div class="column w-20-pc">
                                <div class="cell fr">
                                    <label>Base:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <select name="nm_base">
                                    <option value=""></option>
                                    <option value="sinj_norma">Norma</option>
                                    <option value="sinj_diario">Diário</option>
                                    <option value="sinj_orgao">Órgão</option>
                                    <option value="sinj_vocabulario">Vocabulário</option>
                                </select>
                            </div>
                        </div>
                        <div class="line">
                            <div class="column w-20-pc">
                                <div class="cell fr">
                                    <label>Data:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div class="cell">
                                    <select name="op_intervalo" onchange="javascript:selecionarOperador(this)" id_target="div_intervalo" class="fl">
                                        <option value="igual">igual</option>
                                        <option value="maior">maior</option>
                                        <option value="maiorouigual">maior ou igual</option>
                                        <option value="menor">menor</option>
                                        <option value="menorouigual">menor ou igual</option>
                                        <option value="diferente">diferente</option>
                                        <option value="intervalo">intervalo</option>
                                    </select>
                                </div>
                                <div class="cell">
                                    <input name="dt_exclusao" type="text" value="" class="date" />
                                </div>
                                <div id="div_intervalo" class="cell" style="display:none">
                                    &nbsp;até&nbsp;
                                    <input name="dt_exclusao_fim" type="text" value="" class="date" />
                                </div>
                            </div>
                        </div>
                        <div class="line">
                            <div class="column w-20-pc">
                                <div class="cell fr">
                                    <label>Nome de Usuario:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div id="div_autocomplete_user_erro" class="cell w-100-pc">
                                    <input id="nm_login_user" name="nm_login_user_erro" type="hidden" value="" />
                                    <input id="nm_user" type="text" value="" class="w-80-pc" /><a title="Listar" id="a_user"></a>
                                </div>
                            </div>
                        </div>
                        <div class="line">
                            <div class="column w-20-pc">
                                <div class="cell fr">
                                    <label>Texto Livre:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div class="cell w-80-pc">
                                    <input name="texto_livre" type="text" value="" />
                                </div>
                            </div>
                        </div>
                        <div id="div_buttons" style="width:220px; margin:auto;" class="loaded">
                            <button id="button_pesquisar">
                                <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_loupe_p.png" />
                                Pesquisar
                            </button>
                            <button type="reset" >
                                <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_eraser_p.png" />
                                Limpar
                            </button>
                        </div>
                    </div>
                </fieldset>
            </div>
            <div id="div_loading" class="loading" style="display:none;"></div>
            <div id="div_notificacao" class="notify" style="display:none;"></div>
        </form>
    </div>
</asp:Content>
    
