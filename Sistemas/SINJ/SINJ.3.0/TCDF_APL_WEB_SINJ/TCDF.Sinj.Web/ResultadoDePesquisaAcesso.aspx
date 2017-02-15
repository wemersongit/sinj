<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="ResultadoDePesquisaAcesso.aspx.cs" Inherits="TCDF.Sinj.Web.ResultadoDePesquisaAcesso" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/columns_datatable.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript" language="javascript">
        
        $(document).ready(function () {
            $("#div_resultado").dataTablesLight({
                sAjaxUrl: './ashx/Datatable/AcessoDatatable.ashx' + window.location.search,
                aoColumns: _columns_acesso
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <%
        var _app = Request["app"];
        if(string.IsNullOrEmpty(_app)){
            _app = "sistema";
        }
    %>
    <div class="divIdentificadorDePagina">
	    <label>.: Resultado de Pesquisa de Acessos do <%= _app == "push" ? "Push" : "Sistema" %></label>
	</div>
    <div id="div_controls" class="control">
        <div class="div-light-button">
            <a href="./PesquisarAcesso.aspx?app=sistema"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_loupe_p.png">Acessos do Sistema</a>
        </div>
        <div class="div-light-button">
            <a href="./PesquisarAcesso.aspx?app=push"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_loupe_p.png">Acessos do Push</a>
        </div>
    </div>
    <div id="div_resultado" class="w-90-pc mauto">
    </div>
</asp:Content>
