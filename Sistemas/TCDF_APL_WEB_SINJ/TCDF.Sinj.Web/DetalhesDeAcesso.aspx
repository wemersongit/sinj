<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="DetalhesDeAcesso.aspx.cs" Inherits="TCDF.Sinj.Web.DetalhesDeAcesso" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/funcoes_auditoria.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript" language="javascript">
        <%
            var data = new TCDF.Sinj.Web.ashx.Visualizacao.AcessoDetalhes().DocAcesso(HttpContext.Current);
        %>
        var data = <%= data != "" ? data : "\"\"" %>;
        $(document).ready(function () {
            DetalhesAcesso(data);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <%
        var _app = Request["app"];
    %>
    <div class="divIdentificadorDePagina">
	    <label>.: Pesquisar Acessos do <%= _app == "push" ? "Push" : "Sistema" %></label>
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
        <div id="div_acesso" class="loaded">
            <div id="div_notificacao" class="notify" style="display:none;"></div>
            <fieldset class="w-60-pc">
                <!--<legend>Acesso</legend>-->
                <div class="mauto table">
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>Aplicação:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div id="div_ds_login" class="cell w-60-pc">
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>Usuário:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div id="div_nm_user_acesso" class="cell w-60-pc">
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>Ip do Usuário:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div id="div_nr_ip_usuario" class="cell w-60-pc">
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>Data do Acesso:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div id="div_dt_acesso" class="cell w-60-pc">
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>Ip do Servidor (Sistema):</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div id="div_ip_servidor_porta" class="cell w-60-pc">
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>Navegador:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div id="div_ds_browser" class="cell w-60-pc">
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-100-pc">
                            <div class="cell w-100-pc">
                                <div id="div_ds_obs_login" class="w-80-pc mauto">
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </fieldset>
        </div>
    </div>
</asp:Content>
