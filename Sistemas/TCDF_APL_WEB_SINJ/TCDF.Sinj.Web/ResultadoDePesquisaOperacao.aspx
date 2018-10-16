<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="ResultadoDePesquisaOperacao.aspx.cs" Inherits="TCDF.Sinj.Web.ResultadoDePesquisaOperacao" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/funcoes_auditoria.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $("#div_resultado").dataTablesLight({
                sAjaxUrl: './ashx/Datatable/OperacaoDatatable.ashx' + window.location.search,
                aoColumns: _columns_operacao
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="divIdentificadorDePagina">
	    <label>.: Resultado de Pesquisa de Operação</label>
	</div>
    <div id="div_controls" class="control">
        <div class="div-light-button">
            <a href="./PesquisarOperacao.aspx"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_loupe_p.png">Pesquisar Operação</a>
        </div>
    </div>
    <div id="div_resultado" class="w-90-pc mauto">
    </div>
</asp:Content>
