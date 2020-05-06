<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="ResultadoDePesquisaDiario.aspx.cs" Inherits="TCDF.Sinj.Web.ResultadoDePesquisaDiario" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/funcoes_diario.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $("#div_resultado").dataTablesLight({
                sAjaxUrl: './ashx/Datatable/DiarioDatatable.ashx' + window.location.search,
                aoColumns: _columns_diario,
                iDisplayLength: 25,
                sIdTable: 'datatable_diarios'
            });
        });
        function RelatorioPDF() {
            if ($("#datatable_diarios tbody tr").length > 0 && $("#datatable_diarios .dataTables_empty").length <= 0) {
                var order = ""
                var asc = $("#datatable_diarios thead tr th.sorting_asc").attr('nm');
                var desc = $("#datatable_diarios thead tr th.sorting_desc").attr('nm');
                if (IsNotNullOrEmpty(asc)) {
                    order = asc + ",asc";
                }
                else if (IsNotNullOrEmpty(desc)) {
                    order = desc + ",desc";
                }
                window.open('./RelatorioDePesquisa.aspx' + window.location.search + '&tp=lb&bbusca=sinj_diario&order=' + order, '_blank', 'width=400,height=200');
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="divIdentificadorDePagina">
	    <label>.: Resultado de Pesquisa de Diário</label>
	</div>
    <div class="control">
        <div class="div-light-button">
            <a href="./CadastrarDiario.aspx"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_add_p.png" />Cadastrar Diário</a>
        </div>
        <div class="div-light-button">
            <a href="./PesquisarDiario.aspx"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_loupe_p.png" />Pesquisar Diário</a>
        </div>
    </div>
    <div id="modal_texto" style="display:none;">
        <div class="loading" style="display:none;"></div>
        <div class="notify" style="display:none;"></div>
        <div class="text"></div>
    </div>
    <div style="width:99%; height:32px;">
            <a href="javascript:void(0);" onclick="javascript:RelatorioPDF();" class="fr" title="Salvar planilha"><img alt="xls" src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_xls_p.png"/></a>
    </div>
    <div id="div_resultado">
                
    </div>
</asp:Content>
