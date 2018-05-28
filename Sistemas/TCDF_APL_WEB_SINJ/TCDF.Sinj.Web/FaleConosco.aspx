<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="FaleConosco.aspx.cs" Inherits="TCDF.Sinj.Web.FaleConosco" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
		<script type="text/javascript">
		    var st_atendimento_selecionado = "";
		    $(document).ready(function () {
		        pesquisarChamados("Novo");
		        $('#nrTotalNovos').text(_nr_total_novos_chamados);
		    });
		    function pesquisarChamados(st_atendimento) {
		        st_atendimento_selecionado = st_atendimento;
                var classSelecionar=st_atendimento;
                if (!IsNotNullOrEmpty(st_atendimento)) {
                    classSelecionar = 'Todos';
                }
                $('button').removeClass('selected');
                $('button.' + classSelecionar).addClass('selected');
		        $("#div_datatable_fale_conosco").dataTablesLight({
		            sAjaxUrl: "./ashx/Datatable/FaleConoscoDatatable.ashx?st_atendimento=" + st_atendimento,
		            aoColumns: _columns_fale_conosco,
		            sIdTable: 'datatable_fale_conosco',
		            bFilter: true,
		            aaSorting: [0, 'desc'],
		            responsive: null
		        });
		    }
		    
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="divIdentificadorDePagina">
	    <label>.: Fale Conosco </label>
	</div>
    <div class="form">
        <div id="div_contato" class="loaded mauto table w-80-pc">
                <div class="line">
                    <div class="column">
                        <div style="border: 1px solid #CCC;">
                            <button style="padding:10px" class="clean w-100-pc Novo" onclick="pesquisarChamados('Novo')">Novos (<span id="nrTotalNovos"></span>)</button>
                        </div>
                        <div style="border: 1px solid #CCC; border-top:none;">
                            <button style="padding:10px" class="clean w-100-pc Recebido" onclick="pesquisarChamados('Recebido')">Recebidos</button>
                        </div>
                        <div style="border: 1px solid #CCC; border-top:none;">
                            <button style="padding:10px" class="clean w-100-pc Finalizado" onclick="pesquisarChamados('Finalizado')">Finalizados</button>
                        </div>
                        <div style="border: 1px solid #CCC; border-top:none;">
                            <button style="padding:10px" class="clean w-100-pc Todos" onclick="pesquisarChamados('')">Todos</button>
                        </div>
                    </div>
                    <div class="column w-80-pc" style="padding-bottom: 5px;">
                        <div style="border-top: 1px solid #CCC;" id="div_datatable_fale_conosco"></div>
                    </div>
                </div>
        </div>
    </div>
</asp:Content>
