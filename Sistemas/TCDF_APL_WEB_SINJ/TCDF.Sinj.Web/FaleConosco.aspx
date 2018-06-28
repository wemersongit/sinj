<%@ Page Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="FaleConosco.aspx.cs" Inherits="TCDF.Sinj.Web.FaleConosco" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
		<script type="text/javascript">
		    var _nm_orgao_cadastrador_atribuido = '<%= Request["nm_orgao_cadastrador_atribuido"] != null ? Request["nm_orgao_cadastrador_atribuido"] : "" %>';
		    var _st_atendimento = '<%= Request["st_atendimento"] != null ? Request["st_atendimento"] : "" %>';
		    var _ds_assunto = '<%= Request["ds_assunto"] != null ? Request["ds_assunto"] : "" %>';

		    $(document).ready(function () {
		        $('select[name=nm_orgao_cadastrador_atribuido] option[value=' + _nm_orgao_cadastrador_atribuido + ']').prop('selected', 'selected');
		        $('select[name=st_atendimento] option[value=' + _st_atendimento + ']').prop('selected', 'selected');
		        $('select[name=ds_assunto] option[value=' + _ds_assunto + ']').prop('selected', 'selected');
		        pesquisarChamados(_nm_orgao_cadastrador_atribuido, _st_atendimento, _ds_assunto);
		        
		    });
		    function pesquisarChamados(nm_orgao_cadastrador_atribuido, st_atendimento, ds_assunto) {
		        
                $("#div_datatable_fale_conosco").dataTablesLight({
                    sAjaxUrl: "./ashx/Datatable/FaleConoscoDatatable.ashx?nm_orgao_cadastrador_atribuido=" + nm_orgao_cadastrador_atribuido + "&st_atendimento=" + st_atendimento + "&ds_assunto=" + ds_assunto,
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
        <div id="div_contato" >
            <div class="table">
                <div class="line">
                    <div class="column w-400-px">
                        <form id="form_fale_conosco" name="formFaleConosco" action="#" method="get">
                            <fieldset style="margin-top:0px">
                                <div class="table">
                                    <div class="line">
                                        <div class="column w-50-pc">
                                            <div class="cell fr">
                                                <label>Atribuído para o Órgão:</label>
                                            </div>
                                        </div>
                                        <div class="column">
                                            <select name="nm_orgao_cadastrador_atribuido" class="w-100-px">
                                                <option value="">Todos</option>
                                                <option value="CLDF">CLDF</option>
                                                <option value="SEPLAG">SEPLAG</option>
                                                <option value="PGDF">PGDF</option>
                                                <option value="TCDF">TCDF</option>
                                            </select>
                                        </div>
                                    </div>
                                    <div class="line">
                                        <div class="column w-50-pc">
                                            <div class="cell fr">
                                                <label>Status:</label>
                                            </div>
                                        </div>
                                        <div class="column">
                                            <select name="st_atendimento" class="w-100-px">
                                                <option value="">Todos</option>
                                                <option value="Novo">Novo</option>
                                                <option value="Recebido">Recebido</option>
                                                <option value="Finalizado">Finalizado</option>
                                            </select>
                                        </div>
                                    </div>
                                    <div class="line">
                                        <div class="column w-50-pc">
                                            <div class="cell fr">
                                                <label>Assunto:</label>
                                            </div>
                                        </div>
                                        <div class="column">
                                            <select name="ds_assunto" class="w-100-px">
                                                <option value="">Todos</option>
                                                <option value="Dúvida">Dúvida</option>
                                                <option value="Sugestão">Sugestão</option>
                                                <option value="Crítica">Crítica</option>
                                                <option value="Elogio">Elogio</option>
                                            </select>
                                        </div>
                                    </div>
                                    <div class="line">
                                        <div class="column w-100-pc">
                                            <div class="text-center">
                                                <button>
                                                    <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico-lupa-p_green.png" alt="ok" width="15px" height="15px"/>  Pesquisar
                                                </button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </fieldset>
                        </form>
                    </div>
                    <div class="column">
                        <%
                            if (totalNovosOrgao > 0)
                            {
                        %>
                        <div class="line">
                            <div class="column w-100-pc">
                                <a title="Listar os chamados novos atribuídos ao seu Órgão" class="button" id="aFiltarOrgaoUsuario" href="./FaleConosco?nm_orgao_cadastrador_atribuido=<%= nmOrgaoCadastrador %>&st_atendimento=Novo">Atribuídos para <%= nmOrgaoCadastrador%> <b>(<%= totalNovosOrgao%>)</b></a>
                            </div>
                        </div>
                        <%
                            }
                        %>
                        <div class="line">
                            <div class="column w-100-pc">
                                <div style="border-top: 1px solid #CCC;" id="div_datatable_fale_conosco" class="mauto"></div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
