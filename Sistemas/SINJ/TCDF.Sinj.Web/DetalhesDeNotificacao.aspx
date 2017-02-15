<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="DetalhesDeNotificacao.aspx.cs" Inherits="TCDF.Sinj.Web.DetalhesDeNotificacao" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/funcoes_auditoria.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript" language="javascript">
        <%
            var data = new TCDF.Sinj.Web.ashx.Visualizacao.OperacaoDetalhes().DocOperacao(HttpContext.Current);
        %>
        var data = <%= data != "" ? data : "\"\"" %>;
        $(document).ready(function () {
            DetalhesNotificacao(data);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="divIdentificadorDePagina">
	    <label>.: Notificação</label>
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
        <div id="div_notificacao" class="notify" style="display:none;"></div>
        <div id="div_notifiqueme" class="loaded">
            <fieldset class="w-60-pc">
                <div class="mauto table">
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>E-mails:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div id="div_emails" class="cell w-100-pc">
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>Data da Notificação:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div id="div_dt_inicio" class="cell w-100-pc">
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>Executado por:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div id="div_nm_user_operacao" class="cell w-100-pc">
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>Assunto do E-mail:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div id="div_assunto" class="cell w-100-pc">
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>Mensagem do E-mail:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div id="div_mensagem" class="cell w-100-pc">
                            </div>
                        </div>
                    </div>
                </div>
            </fieldset>
        </div>
    </div>
</asp:Content>
