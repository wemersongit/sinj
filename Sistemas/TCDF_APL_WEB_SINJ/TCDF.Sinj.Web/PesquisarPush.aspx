<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="PesquisarOperacao.aspx.cs" Inherits="TCDF.Sinj.Web.PesquisarOperacao" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('#div_autocomplete_user').autocompletelight({
                sKeyDataName: "email_usuario_push",
                sValueDataName: "nm_usuario_push",
                sInputHiddenName: "email_usuario_push",
                sInputName: "nm_usuario_push",
                sAjaxUrl: './ashx/Autocomplete/NotifiquemeAutocomplete.ashx',
                bLinkAll: true,
                sLinkName: "a_user",
                jPagination: { bPaginate: true,
                    iDisplayLength: 20,
                    bButtonsPaginate: true
                }
            });
            $('#div_autocomplete_tipo_norma').autocompletelight({
                sKeyDataName: "ch_tipo_norma",
                sValueDataName: "nm_tipo_norma",
                sInputHiddenName: "ch_tipo_norma",
                sInputName: "nm_tipo_norma",
                sAjaxUrl: "./ashx/Autocomplete/TipoDeNormaAutocomplete.ashx",
                bLinkAll: true,
                sLinkName: "a_tipo_norma"
            });
            $('#div_autocomplete_origem').autocompletelight({
                sKeyDataName: "ch_orgao",
                sValueDataName: "get_sg_hierarquia_nm_vigencia",
                sInputHiddenName: "ch_orgao",
                sInputName: "sg_hierarquia_nm_vigencia",
                sAjaxUrl: './ashx/Autocomplete/OrgaoAutocomplete.ashx',
                bLinkAll: true,
                sLinkName: "a_origem"
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="divIdentificadorDePagina">
	    <label>.: Pesquisar Usuário Push</label>
	</div>
    <div id="div_controls" class="control">
        <div class="div-light-button">
            <a href="./PesquisarPush.aspx"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_loupe_p.png">Usuários Push</a>
        </div>
        <div class="div-light-button">
            <a href="./PesquisarNotificacao.aspx"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_loupe_p.png">Notificações</a>
        </div>
    </div>
    <div class="form">
        <form id="form_pesquisa_operacao" name="formPesquisaOperacao" action="./ResultadoDePesquisaPush.aspx" method="get">
            <div id="div_notificacao" class="notify" style="display:none;"></div>
            <div id="div_pesquisa">
                <fieldset>
                    <!--<legend>Pesquisa</legend>-->
                    <div class="mauto table">
                        <div class="line">
                            <div class="column w-20-pc">
                                <div class="cell fr">
                                    <label>Tipo de Norma:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div id="div_autocomplete_tipo_norma" class="cell w-100-pc">
                                    <input id="ch_tipo_norma" name="ch_tipo_norma" type="hidden" value="" />
                                    <input id="nm_tipo_norma" type="text" value="" class="w-80-pc" /><a title="Listar" id="a_tipo_norma"></a>
                                </div>
                            </div>
                        </div>
                        <div class="line">
                            <div class="column w-20-pc">
                                <div class="cell fr">
                                    <label>Número da Norma:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div class="cell w-80-pc">
                                    <input name="nr_norma" type="text" value="" class="w-20-pc" />
                                </div>
                            </div>
                        </div>
                        <div class="line">
                            <div class="column w-20-pc">
                                <div class="cell fr">
                                    <label>Origem da Norma:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div id="div_autocomplete_origem" class="cell w-100-pc">
                                    <input id="ch_orgao" name="ch_orgao" type="hidden" value="" />
                                    <input id="sg_hierarquia_nm_vigencia" type="text" value="" class="w-80-pc" /><a title="Listar" id="a_origem"></a>
                                </div>
                            </div>
                        </div>
                        <div class="line">
                            <div class="column w-20-pc">
                                <div class="cell fr">
                                    <label>Data de Criação:</label>
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
                                    <input name="dt_doc" type="text" value="" class="date" />
                                </div>
                                <div id="div_intervalo" class="cell" style="display:none">
                                    &nbsp;até&nbsp;
                                    <input name="dt_doc_fim" type="text" value="" class="date" />
                                </div>
                            </div>
                        </div>
                        <div class="line">
                            <div class="column w-20-pc">
                                <div class="cell fr">
                                    <label>Data de Alteração:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div class="cell">
                                    <select name="op_intervalo_dt_last_up" onchange="javascript:selecionarOperador(this)" id_target="div_intervalo_dt_last_up" class="fl">
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
                                    <input name="dt_last_up" type="text" value="" class="date" />
                                </div>
                                <div id="div_intervalo_dt_last_up" class="cell" style="display:none">
                                    &nbsp;até&nbsp;
                                    <input name="dt_last_up_fim" type="text" value="" class="date" />
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
                                <div id="div_autocomplete_user" class="cell w-100-pc">
                                    <input id="email_usuario_push" name="email_usuario_push" type="hidden" value="" />
                                    <input id="nm_usuario_push" type="text" value="" class="w-80-pc" /><a title="Listar" id="a_user"></a>
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
                                <div class="cell w-100-pc">
                                    <select name="st_push">
                                        <option value=""></option>
                                        <option value="true">Ativo</option>
                                        <option value="false">Inativo</option>
                                    </select>
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
        </form>
    </div>
</asp:Content>
