<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="FaleConosco.aspx.cs" Inherits="TCDF.Sinj.Web.FaleConosco" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
		<script type="text/javascript">
		    var st_atendimento_selecionado = "";
		    $(document).ready(function () {
		        pesquisarChamados("");
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
		    function receberChamado(el) {
		        var ch_chamado = $(el).attr('ch_chamado');
		        $.ajaxlight({
		            sUrl: './ashx/Cadastro/FaleConoscoEditar.ashx?ch_chamado=' + ch_chamado + '&st_atendimento=Recebido',
		            sType: "GET",
		            fnSuccess: function (data) {
		                if (IsNotNullOrEmpty(data, 'error_message')) {
		                    notificar('#div_contato', data.error_message, 'error');
		                }
		                else if (IsNotNullOrEmpty(data, 'success_message')) {
		                    notificar('#div_contato', data.success_message, 'success');
		                    pesquisarChamados(st_atendimento_selecionado);
		                }
		                else {
		                    notificar('#div_contato', 'Erro ao receber chamado.', 'error');
		                }
		            },
		            fnBeforeSubmit: gInicio,
		            fnComplete: gComplete,
		            bAsync: true
		        });
		    }
		    function finalizarChamado(el) {
		        var ch_chamado = $(el).attr('ch_chamado');
		        $.ajaxlight({
		            sUrl: './ashx/Cadastro/FaleConoscoEditar.ashx?ch_chamado=' + ch_chamado + '&st_atendimento=Finalizado',
		            sType: "GET",
		            fnSuccess: function (data) {
		                if (IsNotNullOrEmpty(data, 'error_message')) {
		                    notificar('#div_contato', data.error_message, 'error');
		                }
		                else if (IsNotNullOrEmpty(data, 'success_message')) {
		                    notificar('#div_contato', data.success_message, 'success');
		                    pesquisarChamados(st_atendimento_selecionado);
		                }
		                else {
		                    notificar('#div_contato', 'Erro ao finalizar chamado.', 'error');
		                }
		            },
		            fnBeforeSubmit: gInicio,
		            fnComplete: gComplete,
		            bAsync: true
		        });
		    }
		    function responderChamado(el) {
		        var ch_chamado = $(el).attr('ch_chamado');
		        $('.notify').messagelight("destroy");
		        if (IsNotNullOrEmpty(ch_chamado)) {
		            $('#form_email input[name="email"]').remove();
		            $('#form_email').append('<input type="hidden" name="ch_chamado" value="' + ch_chamado + '" />');

		            if ($("#modal_email").hasClass('ui-dialog-content')) {
		                $('#modal_email').modallight("open");
		            }
		            else {
		                $('#modal_email').modallight({
		                    sTitle: "E-mail",
		                    sType: "default",
		                    oButtons: [
                        { html: '<img alt="send" src="' + _urlPadrao + '/Imagens/ico_email_p.png" />Enviar', click:
                            function () {
                                enviarEmail();
                            }
                        },
                        { html: '<img alt="clear" src="' + _urlPadrao + '/Imagens/ico_eraser_p.png" />Limpar', click: function () { $(this).dialog('close'); } }
                    ],
		                    sWidth: 500
		                });
		            }
		        }
		    }
		    function enviarEmail() {
		        try {
		            Validar(form_email);
		            var sucesso = function (data) {
		                gComplete();
		                if (IsNotNullOrEmpty(data, 'error_message')) {
		                    notificar('#form_email', data.error_message, 'error');
		                }
		                else if (IsNotNullOrEmpty(data, 'alert_message')) {
		                    notificar('#form_email', data.alert_message, 'alert');
		                }
		                else if (IsNotNullOrEmpty(data, 'info_message')) {
		                    notificar('#form_email', data.info_message, 'info');
		                }
		                else if (IsNotNullOrEmpty(data, 'success_message')) {
		                    notificar('#form_email', data.success_message, 'success');
		                }
		                else {
		                    notificar('#form_email', 'O sistema não retornou uma mensagem de confirmação.', 'alert');
		                }
		            }
		            $.ajaxlight({
		                bAsync: true,
		                sUrl: "./ashx/Email/FaleConoscoEnviarEmail.ashx",
		                sType: "POST",
		                fnSuccess: sucesso,
		                sFormId: "form_email",
		                fnBeforeSubmit: gInicio
		            });
		        }
		        catch (ex) {
		            notificar('#form_email', ex, 'error');
		        }
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
    <div id="modal_email" style="display:none;">
        <form id="form_email" name="formEmail" action="#" method="post">
            <div id="div_notificacao_email" class="notify" style="display:none;"></div>
            <div id="div_email">
                <div class="mauto table w-100-pc">
                    <div class="line">
                        <div class="column w-20-pc">
                            <div class="cell fr">
                                <label>Assunto:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div class="cell w-100-pc">
                                <input label="Assunto" obrigatorio="sim" id="assunto" name="assunto" type="text" value="" class="w-90-pc" />
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-20-pc">
                            <div class="cell fr">
                                <label>Mensagem:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div class="cell w-100-pc">
                                <textarea label="Mensagem" obrigatorio="sim" id="mensagem" name="mensagem" class="w-90-pc" rows="15" cols="50"></textarea>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </form>
    </div>
</asp:Content>
