<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="PesquisarOrgao.aspx.cs" Inherits="TCDF.Sinj.Web.PesquisarOrgao" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/funcoes_orgao.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>

    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $("#div_resultado").dataTablesLight({
                sAjaxUrl: './ashx/Datatable/OrgaoDatatable.ashx',
                aoColumns: _columns_orgao,
                bFilter: true
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="divIdentificadorDePagina">
	    <label>.: Pesquisar Órgão </label>
	</div>
    <div id="div_controls" class="control">
    </div>
    <div id="div_resultado" class="w-90-pc mauto">
    </div>
    <div id="modal_descricao_hierarquia" style="display:none;"></div>       
</asp:Content>
