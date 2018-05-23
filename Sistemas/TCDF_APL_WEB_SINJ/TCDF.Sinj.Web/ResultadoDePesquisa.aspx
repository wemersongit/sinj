<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="ResultadoDePesquisa.aspx.cs" Inherits="TCDF.Sinj.Web.ResultadoDePesquisa" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/jquery.highlight.js"></script>
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/funcoes_norma.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/funcoes_resultado_de_pesquisa.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>    
    <script type="text/javascript">
        var date_now = '<%= DateTime.Now.ToString("dd/MM/yyyy") %>';
    </script>
        
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div>
        <div class="divIdentificadorDePagina">
	        <label>.: Resultado de Pesquisa</label>
	    </div>
        <div class="w-100-pc">
            <div id="div_ds_pesquisa" class="w-70-pc mauto" style="margin-left:30%;"> <span id="span_ds_historico"></span> </div>    
        </div>
        <div id="modal_texto" style="display:none;">
            <div class="loading" style="display:none;"></div>
            <div class="notify" style="display:none;"></div>
            <div class="text"></div>
        </div>
        <div>
            <div style="height:32px;">
                <a href="javascript:void(0);" onclick="javascript:RelatorioPDF();" class="fr" title="Salvar planilha" id="a_relatorio"><img alt="xls" src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_xls_p.png"/></a>
            </div>
            <form id="form_highlight" name="form_highlight" method="post" action="#" onsubmit="return fn_submit_highlight()">
                <div id="tabs_pesquisa" class="tabs_datatable"></div>
                <input type="hidden" name="highlight" id="highlight" value="" />
            </form>
        </div>
    </div>
    <form id="form_relatorio" name="form_relatorio" action="./RelatorioDePesquisa.aspx" method="post" target="iframe_relatorio"></form>
    <iframe name="iframe_relatorio" src="" style="display:none;"></iframe>
</asp:Content>
