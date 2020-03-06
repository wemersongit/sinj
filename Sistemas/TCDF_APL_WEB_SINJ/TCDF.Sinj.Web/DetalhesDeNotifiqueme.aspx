<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="DetalhesDeNotifiqueme.aspx.cs" Inherits="TCDF.Sinj.Web.DetalhesDeNotifiqueme" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/funcoes_auditoria.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript" language="javascript">
        <%
            var data = new TCDF.Sinj.Web.ashx.Visualizacao.NotifiquemeDetalhes().DocNotifiqueme(HttpContext.Current);
        %>
        var data = <%= data != "" ? data : "\"\"" %>;
        $(document).ready(function () {
            DetalhesPush(data);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="divIdentificadorDePagina">
	    <label>.: Usuário do Notifique-me</label>
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
        <div id="div_notifiqueme" class="loaded">
            <fieldset class="w-60-pc">
                <!--<legend>Usuário do Notifiqueme</legend>-->
                <div class="mauto table">
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>E-mail:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div id="div_email_usuario_push" class="cell w-60-pc">
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
                            <div id="div_nm_usuario_push" class="cell w-60-pc">
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>Status:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div id="div_st_push" class="cell w-60-pc">
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-100-pc">
                            <div class="cell w-100-pc">
                                <table class="w-90-pc mauto">
                                    <caption>Monitoramento de alteração de Normas</caption>
                                    <thead>
                                        <tr>
                                            <th>Norma</th>
                                            <th>Data do cadastro</th>
                                            <th>Ativo</th>
                                        </tr>
                                    </thead>
                                    <tbody id="tbody_normas_monitoradas">
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-100-pc">
                            <div class="cell w-100-pc">
                                <table class="w-90-pc mauto">
                                    <caption>Monitoramento de Criação de Normas</caption>
                                    <thead>
                                        <tr>
                                            <th>Tipo</th>
                                            <th>Conector</th>
                                            <th>Órgão</th>
                                            <th>Conector</th>
                                            <th>Indexação</th>
                                            <th>Ativo</th>
                                        </tr>
                                    </thead>
                                    <tbody id="tbody_criacao_normas">
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-100-pc">
                            <div class="cell w-100-pc">
                                <table class="w-90-pc mauto">
                                    <caption>Monitoramento de Diários</caption>
                                    <thead>
                                        <tr>
                                            <th>Tipo</th>
                                            <th>Texto</th>
                                            <th>Busca</th>
                                            <th>Ativo</th>
                                        </tr>
                                    </thead>
                                    <tbody id="tbody_termos_diarios">
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </fieldset>
        </div>
        <div id="div_notificacao_notifiqueme" class="notify" style="display:none;"></div>
        <div id="div_loading_notifiqueme" class="loading" style="display:none;"></div>
    </div>
</asp:Content>
