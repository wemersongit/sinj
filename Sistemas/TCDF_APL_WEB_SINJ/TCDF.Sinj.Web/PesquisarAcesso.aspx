<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="PesquisarAcesso.aspx.cs" Inherits="TCDF.Sinj.Web.PesquisarAcesso" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {
            var _app = GetParameterValue('app');
            if (_app == "push") {
                $('#div_autocomplete_user').autocompletelight({
                    sKeyDataName: "email_usuario_push",
                    sValueDataName: "nm_usuario_push",
                    sInputHiddenName: "nm_login_user",
                    sInputName: "nm_user",
                    sAjaxUrl: './ashx/Autocomplete/NotifiquemeAutocomplete.ashx',
                    bLinkAll: true,
                    sLinkName: "a_user",
                    jPagination: { bPaginate: true,
                        iDisplayLength: 20,
                        bButtonsPaginate: true
                    }
                });
            }
            else {
                $('#div_autocomplete_user').autocompletelight({
                    sKeyDataName: "nm_login_usuario",
                    sValueDataName: "nm_usuario",
                    sInputHiddenName: "nm_login_user",
                    sInputName: "nm_user",
                    sAjaxUrl: './ashx/Autocomplete/UsuarioAutocomplete.ashx',
                    bLinkAll: true,
                    sLinkName: "a_user",
                    jPagination: { bPaginate: true,
                        iDisplayLength: 20,
                        bButtonsPaginate: true
                    }
                });
            }

            selecionarOperador(document.getElementsByName('op_intervalo')[0]);

        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <%
        var _app = Request["app"];
        if (string.IsNullOrEmpty(_app))
        {
            _app = "sistema";
        }
    %>
    <div class="divIdentificadorDePagina">
	    <label>.: Pesquisar Acessos do <%= _app == "push" ? "Push" : "Sistema"%></label>
	</div>
    <div id="div_controls" class="control">
        <div class="div-light-button">
            <a href="./PesquisarAcesso.aspx?app=sistema"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_loupe_p.png">Acessos do Sistema</a>
        </div>
        <div class="div-light-button">
            <a href="./PesquisarAcesso.aspx?app=push"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_loupe_p.png">Acessos do Push</a>
        </div>
    </div>
    <div class="form">
        <form id="form_pesquisa_acesso_sistema" name="formPesquisaAcessoSistema" action="./ResultadoDePesquisaAcesso.aspx" method="get">
            <input type="hidden" name="app" value="<%= _app %>" />
            <div id="div_notificacao" class="notify" style="display:none;"></div>
            <div id="div_pesquisa">
                <fieldset>
                    <!--<legend>Pesquisa</legend>-->
                    <div class="mauto table">
                        <div class="line">
                            <div class="column w-20-pc">
                                <div class="cell fr">
                                    <label>Usuário:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div id="div_autocomplete_user" class="cell w-100-pc">
                                    <input id="nm_user" type="text" value="" class="w-80-pc" /><a title="Listar" id="a_user"></a>
                                    <input id="nm_login_user" name="nm_login_user_acesso" type="hidden" value="" />
                                </div>
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
                                    <input name="dt_acesso" type="text" value="" class="date" />
                                </div>
                                <div id="div_intervalo" class="cell" style="display:none">
                                    &nbsp;até&nbsp;
                                    <input name="dt_acesso_fim" type="text" value="" class="date" />
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
        </form>
    </div>
</asp:Content>
