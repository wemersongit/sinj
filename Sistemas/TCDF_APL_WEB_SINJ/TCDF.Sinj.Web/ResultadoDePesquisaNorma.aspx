<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="ResultadoDePesquisaNorma.aspx.cs" Inherits="TCDF.Sinj.Web.ResultadoDePesquisaNorma" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/funcoes_norma.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $("#div_resultado").dataTablesLight({
                sAjaxUrl: './ashx/Datatable/NormaDatatable.ashx' + window.location.search,
                aoColumns: _columns_norma,
                sIdTable: 'datatable_normas'
            });
        });
        function RelatorioPDF() {
            if ($("#datatable_normas tbody tr").length > 0 && $("#datatable_normas .dataTables_empty").length <= 0) {
                var order = ""
                var asc = $("#datatable_normas thead tr th.sorting_asc").attr('nm');
                var desc = $("#datatable_normas thead tr th.sorting_desc").attr('nm');
                if (IsNotNullOrEmpty(asc)) {
                    order = asc + ",asc";
                }
                else if (IsNotNullOrEmpty(desc)) {
                    order = desc + ",desc";
                }
                window.open('./RelatorioDePesquisa.aspx' + window.location.search + '&tp=lb&bbusca=sinj_norma&order=' + order, '_blank', 'width=400,height=200');
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="divIdentificadorDePagina">
	    <label>.: Resultado de Pesquisa de Norma</label>
	</div>
    <div id="div_controls" class="control">
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
    <div id="modal_descricao_origem" style="display:none;"></div>
</asp:Content>
