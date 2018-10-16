<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="PesquisarErroSistema.aspx.cs" Inherits="TCDF.Sinj.Web.PesquisarErroSistema" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/funcoes_auditoria.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            for (var key in operacoes_parse) {
                $('#select_ch_operacao').append('<option value="' + key + '">' + operacoes_parse[key] + '</option>');
            }
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
	    <label>.: Pesquisar Erro do Sistema</label>
	</div>
    <div id="div_controls" class="control">
        <div class="div-light-button">
            <a href="./PesquisarErroSistema.aspx"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_loupe_p.png">Erros de Sistema</a>
        </div>
        <div class="div-light-button">
            <a href="./PesquisarErroExtracao.aspx"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_loupe_p.png">Erros de Extracao</a>
        </div>
        <div class="div-light-button">
            <a href="./PesquisarErroIndexacao.aspx"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_loupe_p.png">Erros de Indexação</a>
        </div>
    </div>
    <div class="form">
        <form id="form_pesquisa_erro_sistema" name="formPesquisaErroSistema" action="./ResultadoDePesquisaErroSistema.aspx" method="get">
            <div id="div_pesquisa">
                <fieldset>
                    <!--<legend>Pesquisa</legend>-->
                    <div class="mauto table">
                        <div class="line">
                            <div class="column w-20-pc">
                                <div class="cell fr">
                                    <label>Operação:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <select id="select_ch_operacao" name="ch_operacao">
                                    <option value=""></option>
                                </select>
                            </div>
                        </div>
                        <div class="line">
                            <div class="column w-20-pc">
                                <div class="cell fr">
                                    <label>Tipo de Erro:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <select name="nm_tipo">
                                    <option value=""></option>
                                    <option value="request">Requisição</option>
                                    <option value="push">Push</option>
                                    <option value="erro_net">.NET</option>
                                    <option value="erro_js">Javascript</option>
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
                                    <input name="dt_log_erro" type="text" value="" class="date" />
                                </div>
                                <div id="div_intervalo" class="cell" style="display:none">
                                    &nbsp;até&nbsp;
                                    <input name="dt_log_erro_fim" type="text" value="" class="date" />
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
