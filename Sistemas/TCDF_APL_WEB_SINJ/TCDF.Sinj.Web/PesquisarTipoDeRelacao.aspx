<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="PesquisarTipoDeRelacao.aspx.cs" Inherits="TCDF.Sinj.Web.PesquisarTipoDeRelacao" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/funcoes_tipo_de_relacao.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>

    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $("#div_resultado").dataTablesLight({
                sAjaxUrl: './ashx/Datatable/TipoDeRelacaoDatatable.ashx',
                aoColumns: _columns_tipo_de_relacao,
                bFilter: true
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="divIdentificadorDePagina">
	    <label>.: Pesquisar Tipo de Relação</label>
	</div>
    <div id="div_controls" class="control">
    </div>
    <div id="div_resultado" class="w-90-pc mauto">
                
    </div>
</asp:Content>
