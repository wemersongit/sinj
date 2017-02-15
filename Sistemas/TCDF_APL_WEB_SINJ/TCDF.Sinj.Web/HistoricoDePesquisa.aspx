<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="HistoricoDePesquisa.aspx.cs" Inherits="TCDF.Sinj.Web.HistoricoDePesquisa" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/funcoes_historico_de_pesquisa.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $('#div_historico').dataTablesLight({
                sAjaxUrl: './ashx/Datatable/HistoricoDePesquisaDatatable.ashx?chave=' + $.cookie('sinj_ch_history'),
                aoColumns: _columns_historico,
                sIdTable: 'datatable_historico'
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div>
        <div class="divIdentificadorDePagina">
	        <label>.: Histórico de Pesquisa</label>
	    </div>
        <div>
            <div id="div_historico" class="w-90-pc mauto"></div>
        </div>
    </div>
</asp:Content>
