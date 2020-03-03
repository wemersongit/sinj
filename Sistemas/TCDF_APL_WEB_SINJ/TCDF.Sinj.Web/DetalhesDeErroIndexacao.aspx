<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="DetalhesDeErroIndexacao.aspx.cs" Inherits="TCDF.Sinj.Web.DetalhesDeErroIndexacao" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/funcoes_auditoria.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript" language="javascript">
        <%
            var data = new TCDF.Sinj.Web.ashx.Visualizacao.ErroIndexacaoDetalhes().DocErroIndexacao(HttpContext.Current);
        %>
        var data = <%= data != "" ? data : "\"\"" %>;
        $(document).ready(function () {
            DetalhesErroIndexacao(data);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
<div class="divIdentificadorDePagina">
	    <label>.: Detalhes de Erro de Indexação</label>
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
        <div id="div_erro_indexacao" class="loaded">
            <div id="div_notificacao" class="notify" style="display:none;"></div>
            <fieldset class="w-60-pc">
                <!--<legend>Erro de Indexação</legend>-->
                <div class="mauto table">
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>Base:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div id="div_nm_base" class="cell w-60-pc">
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>Documento:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div id="div_link_documento" class="cell w-60-pc">
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>Data do Erro:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div id="div_dt_error" class="cell w-60-pc">
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>Erro:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div id="div_error_msg" class="cell w-60-pc">
                            </div>
                        </div>
                    </div>
                </div>
            </fieldset>
        </div>
    </div>
</asp:Content>
