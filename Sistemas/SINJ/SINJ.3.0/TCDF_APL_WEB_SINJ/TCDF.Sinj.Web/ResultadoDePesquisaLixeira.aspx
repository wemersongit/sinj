<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="ResultadoDePesquisaLixeira.aspx.cs" Inherits="TCDF.Sinj.Web.ResultadoDePesquisaLixeira" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/columns_datatable.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>

    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $("#div_resultado").dataTablesLight({
                sAjaxUrl: './ashx/Datatable/LixeiraDatatable.ashx' + window.location.search,
                aoColumns: _columns_lixeira
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
<div class="divIdentificadorDePagina">
	    <label>.: Resultado de Pesquisa de Lixeira</label>
	</div>
    <div id="div_controls" class="control">
        <div class="div-light-button">
            <a href="./PesquisarLixeira.aspx"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_loupe_p.png"> Pesquisar Lixeira </a>
        </div>
    </div>
    <div id="div_resultado" class="w-90-pc mauto">
    </div>
</asp:Content>
