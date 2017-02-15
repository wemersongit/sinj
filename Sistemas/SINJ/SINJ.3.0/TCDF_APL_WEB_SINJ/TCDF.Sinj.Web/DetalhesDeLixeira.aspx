<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="DetalhesDeLixeira.aspx.cs" Inherits="TCDF.Sinj.Web.DetalhesDeLixeira" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/funcoes_auditoria.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript" language="javascript">
        <%
            var data = new TCDF.Sinj.Web.ashx.Visualizacao.LixeiraDetalhes().DocLixeira(HttpContext.Current);
        %>
        var data = <%= data != "" ? data : "\"\"" %>;
        $(document).ready(function () {
            DetalhesLixeira(data);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
<div class="divIdentificadorDePagina">
	    <label>.: Detalhes de Registro da Lixeira</label>
	</div>
    <div id="div_controls" class="control">
        <div class="div-light-button">
            <a href="./PesquisarLixeira.aspx"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_loupe_p.png"> Pesquisar Lixeira </a>
        </div>
    </div>
    <div class="form">
        <div id="div_lixeira" class="loaded">
            <div id="div_notificacao" class="notify" style="display:none;"></div>
            <fieldset class="w-60-pc">
                <legend>Regitro da Lixeira</legend>
                <div class="mauto table">
                    <div class="w-100-pc fr" style="margin-top:10px;">
                        <div class="fl" style="margin-right:5px;margin-left:15px;">
                            <button type="button" onclick="javascript:RestaurarExcluido(data);"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_restore_p.png"> Restaurar</button>
                        </div>
                    </div>
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
                                <label>Data de Exclusão:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div id="div_dt_exclusao" class="cell w-60-pc">
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
                            <div id="div_usuario_exclusao" class="cell w-60-pc">
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-100-pc">
                            <div class="cell w-100-pc">
                                <div id="div_registro" class="w-80-pc mauto">
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </fieldset>
        </div>
    </div>
</asp:Content>
