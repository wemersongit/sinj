<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="PesquisarTipoDeNorma.aspx.cs" Inherits="TCDF.Sinj.Web.PesquisarTipoDeNorma" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/funcoes_tipo_de_norma.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $("#div_resultado").dataTablesLight({
                sAjaxUrl: './ashx/Datatable/TipoDeNormaDatatable.ashx',
                aoColumns: _columns_tipo_de_norma,
                bFilter: true
            });

        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">  
    <div class="divIdentificadorDePagina">
	    <label>.: Pesquisar Tipo de Norma</label>
	</div>
    <div id="div_controls" class="control">
    </div>
    <div id="div_resultado">
        
    </div>
</asp:Content>
