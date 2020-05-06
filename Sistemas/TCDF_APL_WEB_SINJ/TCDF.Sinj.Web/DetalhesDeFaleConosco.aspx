<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="DetalhesDeFaleConosco.aspx.cs" Inherits="TCDF.Sinj.Web.DetalhesDeFaleConosco" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            var ch_chamado = GetParameterValue("ch_chamado");
            if (IsNotNullOrEmpty(ch_chamado)) {
                var sucesso = function (data) {
                    if (IsNotNullOrEmpty(data, 'error_message')) {
                        notificar('#div_chamado', data.error_message, 'error');
                    }
                    else if (IsNotNullOrEmpty(data, 'ch_chamado')) {
                        $('#div_nm_user').text(getVal(data.nm_user));
                        $('#div_ds_email').text(getVal(data.ds_email));
                        $('#div_ds_assunto').text(getVal(data.ds_assunto));
                        $('#div_ds_msg').text(getVal(data.ds_msg));
                        $('#div_dt_inclusao').text(getVal(data.dt_inclusao));
                        $('#div_nm_orgao_cadastrador_atribuido').text(getVal(data.nm_orgao_cadastrador_atribuido));
                        $('#div_st_atendimento').text(getVal(data.st_atendimento));
                        $('#a_ds_url_pagina').text(getVal(data.ds_url_pagina));
                        $('#a_ds_url_pagina').attr('href',getVal(data.ds_url_pagina));
                        if (IsNotNullOrEmpty(data.print)) {
                            $('#img_print').attr('src', data.print);
                            $('div.line.print').show();
                        }
                        if (data.st_atendimento == "Novo") {
                            $('#div_buttons').append('<button title="Receber esse chamado" ch_chamado="' + data.ch_chamado + '" onclick="receberChamado(this);"><img src="' + _urlPadrao + '/Imagens/ico_check_p.png"/> Receber</button>');
                            if (!IsNotNullOrEmpty(data.nm_orgao_cadastrador_atribuido)) {
                                $('#div_buttons').append('<button title="Atibuir para algum Órgão Cadastrador" ch_chamado="' + data.ch_chamado + '" onclick="selecionarOrgaoAtribuir(this);"><img src="' + _urlPadrao + '/Imagens/ico_atribuir_p.png"/> Atribuir</button>');
                            }
                        }
                        else if (IsNotNullOrEmpty(data, 'nm_login_usuario_atendimento') && data.nm_login_usuario_atendimento == _user.nm_login_usuario) {
                            if (data.st_atendimento == "Recebido") {
                                $('#div_buttons').append('<button title="Finalizar esse chamado" ch_chamado="' + data.ch_chamado + '" onclick="finalizarChamado(this);"><img src="' + _urlPadrao + '/Imagens/ico_close.png"/> Finalizar</button>');
                            }
                            $('#div_buttons').append('<button title="Enviar um e-mail para esse usuário" ch_chamado="' + data.ch_chamado + '" ds_email="' + data.ds_email + '" ds_assunto="' + data.ds_assunto + '" onclick="responderChamado(this);"><img src="' + _urlPadrao + '/Imagens/ico_email_p.png"/> Responder</button>');
                        }

                        $('#span_nm_usuario_atendimento').text(getVal(data.nm_usuario_atendimento) + ' - ');
                        $('#span_dt_recebido').text(getVal(data.dt_recebido));
                        $('#div_dt_finalizado').text(getVal(data.dt_finalizado));
                        if (IsNotNullOrEmpty(data.mensagens)) {

                            for (var i = 0; i < data.mensagens.length; i++) {
                                $("#div_mensagens").dataTablesLight({
                                    bServerSide: false,
                                    bPaginate: false,
                                    bInfo: false,
                                    aaSorting: [[0, "asc"]],
                                    aoData: data.mensagens,
                                    aoColumns: _columns_fale_conosco_atendimento_historico,
                                    sIdTable: 'datatable_fale_conosco_atendimento_historico'
                                });
                            }
                        }
                    }
                };
                $.ajaxlight({
                    sUrl: './ashx/Visualizacao/FaleConoscoDetalhes.ashx' + window.location.search,
                    sType: "POST",
                    fnSuccess: sucesso,
                    fnComplete: gComplete,
                    fnBeforeSend: gInicio,
                    fnError: null,
                    bAsync: true
                });
            }
        });
        function selecionarOrgaoAtribuir(el) {
            $('#form_orgao_atribuir input[name=ch_chamado]').val(el.getAttribute('ch_chamado'));
            if ($("#modal_orgao_atribuir").hasClass('ui-dialog-content')) {
                $('#modal_orgao_atribuir').modallight("open");
            }
            else {
                $('#modal_orgao_atribuir').modallight({
                    sTitle: "Atribuir para um Órgão",
                    sType: "default",
                    oButtons: [
                            { html: '<img alt="send" src="' + _urlPadrao + '/Imagens/ico_atribuir_p.png" />Atribuir', click:
                                function () {
                                    $('#form_orgao_atribuir').submit();
                                }
                            },
                            { html: '<img alt="clear" src="' + _urlPadrao + '/Imagens/ico_close.png" />Cancelar', click: function () { $(this).dialog('close'); } }
                        ],
                    sWidth: 500
                });

                $('#form_email textarea[name="mensagem"]').focus();
            }
        }
        function submitOrgaoAtribuir(form) {
            try {
                Validar(form);
                var sucesso = function (data) {
                    gComplete();
                    if (IsNotNullOrEmpty(data, 'error_message')) {
                        notificar('#'+form, data.error_message, 'error');
                    }
                    else if (IsNotNullOrEmpty(data, 'success_message')) {
                        $('#modal_orgao_atribuir').modallight("close");
                        ShowDialog({
                            id_element: "modal_notificacao_success",
                            sTitle: "Sucesso",
                            sContent: data.success_message,
                            sType: "success",
                            oButtons: [{
                                text: "Ok",
                                click: function () {
                                    $(this).dialog('close');
                                }
                            }],
                            fnClose: function () {
                                location.reload();
                            }
                        });
                    }
                    else {
                        notificar('#' + form, 'O sistema não retornou uma mensagem de confirmação.', 'alert');
                    }
                }
                $.ajaxlight({
                    bAsync: true,
                    sUrl: "./ashx/Cadastro/FaleConoscoEditar.ashx",
                    sType: "POST",
                    fnSuccess: sucesso,
                    sFormId: form,
                    fnBeforeSubmit: gInicio,
                    iTimeout: 120000
                });

            }
            catch (ex) {
                notificar('#'+form, ex, 'error');
            }
            return false;
        }
        function receberChamado(el) {
            var ch_chamado = $(el).attr('ch_chamado');
            $.ajaxlight({
                sUrl: './ashx/Cadastro/FaleConoscoEditar.ashx?ch_chamado=' + ch_chamado + '&st_atendimento=Recebido',
                sType: "GET",
                fnSuccess: function (data) {
                    if (IsNotNullOrEmpty(data, 'error_message')) {
                        notificar('#div_chamado', data.error_message, 'error');
                    }
                    else if (IsNotNullOrEmpty(data, 'success_message')) {
                        ShowDialog({
                            id_element: "modal_notificacao_success",
                            sTitle: "Sucesso",
                            sContent: data.success_message,
                            sType: "success",
                            oButtons: [{
                                text: "Ok",
                                click: function () {
                                    $(this).dialog('close');
                                }
                            }],
                            fnClose: function () {
                                location.reload();
                            }
                        });
                    }
                    else {
                        notificar('#div_chamado', 'Erro ao receber chamado.', 'error');
                    }
                },
                fnBeforeSend: gInicio,
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
                        notificar('#div_chamado', data.error_message, 'error');
                    }
                    else if (IsNotNullOrEmpty(data, 'success_message')) {
                        ShowDialog({
                            id_element: "modal_notificacao_success",
                            sTitle: "Sucesso",
                            sContent: data.success_message,
                            sType: "success",
                            oButtons: [{
                                text: "Ok",
                                click: function () {
                                    $(this).dialog('close');
                                }
                            }],
                            fnClose: function () {
                                location.reload();
                            }
                        });
                    }
                    else {
                        notificar('#div_chamado', 'Erro ao finalizar chamado.', 'error');
                    }
                },
                fnBeforeSend: gInicio,
                fnComplete: gComplete,
                bAsync: true
            });
        }
        function responderChamado(el) {
            var ch_chamado = $(el).attr('ch_chamado');
            var ds_assunto = $(el).attr('ds_assunto');
            $('.notify').messagelight("destroy");
            if (IsNotNullOrEmpty(ch_chamado)) {
                $('#form_email input[name="ch_chamado"]').val(ch_chamado);
                $('#form_email input[name="assunto"]').val("Resposta - SINJ - " + ds_assunto);
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

                    $('#form_email textarea[name="mensagem"]').focus();
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
                        $('#modal_email').modallight("close");
                        ShowDialog({
                            id_element: "modal_notificacao_success",
                            sTitle: "Sucesso",
                            sContent: data.success_message,
                            sType: "success",
                            oButtons: [{
                                text: "Ok",
                                click: function () {
                                    $(this).dialog('close');
                                }
                            }],
                            fnClose: function () {
                                location.reload();
                            }
                        });
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
                    fnBeforeSubmit: gInicio,
                    iTimeout: 120000
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
	    <label>.: Detalhes do Chamado</label>
	</div>
    <div id="div_controls" class="control">
        
    </div>
    <div class="form">
        <div id="div_chamado" class="loaded mauto w-80-pc">
            <fieldset>
                <div class="table">
                    <div id="div_buttons" class="text-right">
                    </div>
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>Nome:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div id="div_nm_user" class="cell w-60-pc">
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>E-mail:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div id="div_ds_email" class="cell w-60-pc">
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>Assunto:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div id="div_ds_assunto" class="cell w-60-pc">
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>Mensagem:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div id="div_ds_msg" class="cell w-60-pc">
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>Data:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div id="div_dt_inclusao" class="cell w-60-pc">
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>Órgão atribuído:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div id="div_nm_orgao_cadastrador_atribuido" class="cell w-60-pc">
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>Status:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div id="div_st_atendimento" class="cell w-60-pc">
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>Recebido por:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div class="cell w-60-pc">
                                <span id="span_nm_usuario_atendimento"></span><span id="span_dt_recebido"></span>
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>Finalizado:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div id="div_dt_finalizado" class="cell w-60-pc">
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>Página:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div class="cell w-60-pc">
                                <a id="a_ds_url_pagina" href="" target="_blank"></a>
                            </div>
                        </div>
                    </div>
                    <div class="line print" style="displa:none;">
                        <div class="column w-100-pc text-center" style="background: #FFF;">
                            <img id="img_print" src="" width="700" />
                        </div>
                    </div>
                    <div class=" table mauto fale_conosco_atendimento">
                        <div class="line">
                            <div class="column w-100-pc">
                                <div class="cell w-100-pc">
                                    <div id="div_mensagens" class="table w-90-pc"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </fieldset>
        </div>
    </div>
    <div id="modal_email" style="display:none;">
        <form id="form_email" name="formEmail" action="#" method="post">
            <div id="div_notificacao_email" class="notify" style="display:none;"></div>
            <input type="hidden" name="ch_chamado" value="" />
            <div id="div_email">
                <div class="mauto table w-100-pc">
                    <div class="line">
                        <div class="column w-20-pc">
                            <div class="cell fr">
                                <label>Assunto*:</label>
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
                                <label>Mensagem*:</label>
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
    <div id="modal_orgao_atribuir" style="display:none;">
        <form id="form_orgao_atribuir" name="formOrgaoAtribuir" action="#" method="post" onsubmit="return submitOrgaoAtribuir('form_orgao_atribuir')">
            <div class="notify" style="display:none;"></div>
            <input type="hidden" name="ch_chamado" value="" />
            <div class="mauto table w-100-pc">
                <div class="line">
                    <div class="column w-40-pc">
                        <div class="cell fr">
                            <label>Órgão Cadastrador*:</label>
                        </div>
                    </div>
                    <div class="column w-60-pc">
                        <div class="cell w-100-pc">
                            <select name="nm_orgao_cadastrador_atribuido" class="w-100-px" obrigatorio="sim" label="Órgão Cadastrador">
                                <option value=""></option>
                                <option value="CLDF">CLDF</option>
                                <option value="SEPLAG">SEPLAG</option>
                                <option value="PGDF">PGDF</option>
                                <option value="TCDF">TCDF</option>
                            </select>
                        </div>
                    </div>
                </div>
            </div>
        </form>
    </div>
</asp:Content>
