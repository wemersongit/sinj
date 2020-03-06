<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="DetalhesDeErroSistema.aspx.cs" Inherits="TCDF.Sinj.Web.DetalhesDeErroSistema" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/funcoes_auditoria.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript" language="javascript">
        <%
            var data = new TCDF.Sinj.Web.ashx.Visualizacao.ErroSistemaDetalhes().DocErroSistema(HttpContext.Current);
        %>
        var data = <%= data != "" ? data : "\"\"" %>;
        $(document).ready(function () {
            DetalhesErroSistema(data);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="divIdentificadorDePagina">
	    <label>.: Detalhes de Erro de Sistema</label>
	</div>
    <div id="div_controls" class="control">
        <div class="div-light-button">
            <a href="./PesquisarErroSistema.aspx"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_loupe_p.png">Erros de Sistema</a>
        </div>
        <div class="div-light-button">
            <a href="./PesquisarErroExtracao.aspx"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_loupe_p.png">Erros de Extração</a>
        </div>
        <div class="div-light-button">
            <a href="./PesquisarErroIndexacao.aspx"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_loupe_p.png">Erros de Indexação</a>
        </div>
    </div>
    <div class="form">
        <div id="div_notificacao" class="notify" style="display:none;"></div>
        <div id="div_erro_sistema" class="loaded">
            <fieldset class="w-60-pc">
                <!--<legend>Erro de Sistema</legend>-->
                <div class="mauto table w-90-pc">
                    <div class="line">
                        <div class="column w-20-pc">
                            <div class="cell fr">
                                <label>Operação:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div id="div_operacao" class="cell w-60-pc">
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-20-pc">
                            <div class="cell fr">
                                <label>Tipo de Erro:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div id="div_tipo_erro" class="cell w-60-pc">
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
                            <div id="div_dt_log_erro" class="cell w-60-pc">
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
                            <div id="div_nm_user_erro" class="cell w-60-pc">
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-20-pc">
                            <div class="cell fr">
                                <label>Ip do Usuario:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div id="div_nr_ip_usuario" class="cell w-60-pc">
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-20-pc">
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
                                <div id="div_ds_erro" class="w-80-pc mauto">
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </fieldset>
        </div>
    </div>
</asp:Content>
