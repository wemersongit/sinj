<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="ResultadoDePesquisaOrgao.aspx.cs" Inherits="TCDF.Sinj.Web.ResultadoDePesquisaOrgao" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/funcoes_orgao.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $("#div_resultado").dataTablesLight({
                sAjaxUrl: './ashx/Datatable/OrgaoDatatable.ashx' + window.location.search,
                aoColumns: _columns_orgao,
                bFilter: true
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="divIdentificadorDePagina">
	    <label>.: Resultado de Pesquisa de Órgao</label>
	</div>
    <div class="control">
        <div class="div-light-button">
            <a href="./CadastrarOrgao.aspx"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_add_p.png" >Cadastrar Órgão</a>
        </div>
        <div class="div-light-button">
            <a href="./PesquisarOrgao.aspx"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_loupe_p.png" >Pesquisar Órgão</a>
        </div>
    </div>
    <div id="div_resultado">
                
    </div>
</asp:Content>
