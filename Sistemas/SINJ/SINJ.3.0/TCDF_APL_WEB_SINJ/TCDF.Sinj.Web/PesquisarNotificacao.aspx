<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="PesquisarNotificacao.aspx.cs" Inherits="TCDF.Sinj.Web.PesquisarNotificacao" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="divIdentificadorDePagina">
	    <label>.: Pesquisar Notificações</label>
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
        <form id="form_pesquisa_notificacao" name="formPesquisaNotificacao" action="./ResultadoDePesquisaNotificacao.aspx" method="get">
            <input type="hidden" name="push" value="push" />
            <div id="div_notificacao" class="notify" style="display:none;"></div>
            <div id="div_pesquisa">
                <fieldset>
                    <legend>Pesquisa</legend>
                    <div class="mauto table">
                        <div class="line">
                            <div class="column w-20-pc">
                                <div class="cell fr">
                                    <label>Data da Notificação:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div class="cell w-80-pc">
                                    <input name="dt_inicio" type="text" value="" class="w-20-pc date" />
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
