<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="ResultadoDePesquisaPesquisas.aspx.cs" Inherits="TCDF.Sinj.Web.ResultadoDePesquisaPesquisas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $("#div_resultado").dataTablesLight({
                sAjaxUrl: './ashx/Datatable/HistoricoDePesquisaDatatable.ashx' + window.location.search,
                aoColumns: _columns_historico
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="divIdentificadorDePagina">
	    <label>.: Resultado de Pesquisa de Pesquisas</label>
	</div>
    <div id="div_controls" class="control">
        <div class="div-light-button">
            <a href="./PesquisarPesquisas.aspx"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_loupe_p.png">Pesquisar</a>
        </div>
        <div class="div-light-button">
            <a href="./EstatisticasDePesquisas.aspx"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_loupe_p.png">Estatísticas</a>
        </div>
    </div>
    <div id="div_resultado" class="w-90-pc mauto">
    </div>
</asp:Content>
