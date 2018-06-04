<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="FaleConosco.aspx.cs" Inherits="TCDF.Sinj.Web.FaleConosco" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
		<script type="text/javascript">
		    $(document).ready(function () {
		        pesquisarChamados("Novo","");
		    });
		    function pesquisarChamados(st_atendimento, ds_assunto) {
		        var htmlButtonsAssunto = '<div class="div_todos" style="margin-left:25px;">' +
                            '<div style="border-bottom: 1px solid #CCC;">' +
                                '<button style="padding:10px" class="clean Crítica w-100-pc" onclick="pesquisarChamados(\'' + st_atendimento + '\', \'Crítica\')">Críticas</button>' +
                            '</div>' +
                            '<div style="border-bottom: 1px solid #CCC;">'+
                                    '<button style="padding:10px" class="clean Dúvida w-100-pc" onclick="pesquisarChamados(\'' + st_atendimento + '\', \'Dúvida\')">Dúvidas</button>' +
                            '</div>' +
                            '<div style="border-bottom: 1px solid #CCC;">' +
                                '<button style="padding:10px" class="clean Elogio w-100-pc" onclick="pesquisarChamados(\'' + st_atendimento + '\', \'Elogio\')">Elogios</button>' +
                            '</div>' +
                            '<div style="border-bottom: 1px solid #CCC;">'+
                                '<button style="padding:10px" class="clean Sugestão w-100-pc" onclick="pesquisarChamados(\'' + st_atendimento + '\', \'Sugestão\')">Sugestões</button>' +
                            '</div>'+
                        '</div>';
                var targetAppend = st_atendimento;
                var classSelecionar = st_atendimento;

                if (IsNotNullOrEmpty(ds_assunto)) {
                    classSelecionar = ds_assunto;
                }

                $('div.div_todos').remove();
                $('button.' + targetAppend).closest('div.div_button').append(htmlButtonsAssunto);

                $('button').removeClass('selected');
                $('button.' + classSelecionar).addClass('selected');
                $("#div_datatable_fale_conosco").dataTablesLight({
		            sAjaxUrl: "./ashx/Datatable/FaleConoscoDatatable.ashx?st_atendimento=" + (st_atendimento == 'Todos' ? '' : st_atendimento )+ "&ds_assunto=" + ds_assunto,
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
                    <div class="column" style="width:150px;">
                        <div class="div_button">
                            <div class="w-90-pc" style="border-bottom: 1px solid #CCC;border-top: 1px solid #CCC;">
                                <button style="padding:10px" class="clean w-100-pc Novo" onclick="pesquisarChamados('Novo', '')">Novos</button>
                            </div>
                        </div>
                        <div class="div_button">
                            <div class="w-90-pc" style="border-bottom: 1px solid #CCC;">
                                <button style="padding:10px" class="clean w-100-pc Recebido" onclick="pesquisarChamados('Recebido', '')">Recebidos</button>
                            </div>
                        </div>
                        <div class="div_button">
                            <div class="w-90-pc" style="border-bottom: 1px solid #CCC;">
                                <button style="padding:10px" class="clean w-100-pc Finalizado" onclick="pesquisarChamados('Finalizado', '')">Finalizados</button>
                            </div>
                        </div>
                        <div class="div_button">
                            <div class="w-90-pc" style="border-bottom: 1px solid #CCC;">
                                <button style="padding:10px" class="clean w-100-pc Todos" onclick="pesquisarChamados('Todos', '')">Todos</button>
                            </div>
                        </div>
                        
                    </div>
                    <div class="column w-80-pc" style="padding-bottom: 5px;">
                        <div style="border-top: 1px solid #CCC;" id="div_datatable_fale_conosco"></div>
                    </div>
                </div>
        </div>
    </div>
</asp:Content>
