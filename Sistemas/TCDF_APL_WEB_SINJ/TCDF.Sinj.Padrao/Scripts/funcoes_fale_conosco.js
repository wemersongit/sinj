var fnSuccessFaleConosco = function (data) {
    if (IsNotNullOrEmpty(data, 'error_message')) {
        notificar('#form_fale_conosco', data.error_message, 'error');
        generateCaptcha();
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
                closeFaleConosco();
                if ($('#ch_chamado_fale_conosco').val() != "") {
                    document.location.href = './';
                }
                else {
                    form_fale_conosco.reset();
                    generateCaptcha();
                }
            }
        });
    }
    else {
        notificar('#form_fale_conosco', 'Erro ao enviar. Tente mais tarde.', 'error');
    }
};
function enviarMsgFaleConosco() {
    try {
        Validar("form_fale_conosco");
        $.ajaxlight({
            sFormId: "form_fale_conosco",
            sUrl: './ashx/Cadastro/FaleConoscoMensagemIncluir.ashx',
            sType: "POST",
            fnSuccess: fnSuccessFaleConosco,
            fnBeforeSubmit: gInicio,
            fnComplete: gComplete,
            bAsync: true
        });

    } catch (ex) {
        notificar('#form_fale_conosco', ex, 'error');
        $("html, body").animate({
            scrollTop: 0
        }, "slow");
    }
    return false;
}


function abrirChamado(ch_chamado) {
    var chamado = {};
    for (var i = 0; i < _chamados.length; i++) {
        if (_chamados[i].ch_chamado == ch_chamado) {
            chamado = _chamados[i];
            break;
        }
    }
    if (IsNotNullOrEmpty(chamado)) {
        var html = "";
        html += '<div class="line">' +
                    '<div class="column w-30-pc">' +
                        '<div class="cell fr">' +
                            '<label>Assunto:</label>' +
                        '</div>' +
                     '</div>' +
                    '<div class="column w-70-pc">' +
                             chamado.ds_assunto +
                     '</div>' +
                '</div>';

        html += '<div class="line">' +
                    '<div class="column w-30-pc">' +
                        '<div class="cell fr">' +
                            '<label>Data:</label>' +
                        '</div>' +
                     '</div>' +
                    '<div class="column w-70-pc">' +
                             chamado.dt_inclusao +
                     '</div>' +
                '</div>';
        html += '<div class="line">' +
                    '<div class="column w-30-pc">' +
                        '<div class="cell fr">' +
                            '<label>Mensagem:</label>' +
                        '</div>' +
                     '</div>' +
                    '<div class="column w-70-pc">' +
                             chamado.ds_msg +
                     '</div>' +
                '</div>';

        html += '<div class="line">' +
                    '<div class="column w-100-pc">' +
                        '<div class="cell w-100-pc">' +
                            '<div class="text-right"><button type="button" onclick="enviarMensagem(\'' + chamado.ch_chamado + '\')"><img src="' + _urlPadrao + '/Imagens/ico_email_p.png" width="20px" height="20"/> Enviar Mensagem</button></div>' +
                            '<div id="div_datatable_fale_conosco_atendimento_historico">' +
                            '</div>' +
                        '</div>' +
                    '</div>' +
                '</div>';
        $('#div_detalhes_fale_conosco_atendimento').html(html);
        $("#div_datatable_fale_conosco_atendimento_historico").dataTablesLight({
            bServerSide: false,
            bPaginate: false,
            bInfo: false,
            aaSorting: [[0, "asc"]],
            aoData: chamado.mensagens,
            aoColumns: _columns_fale_conosco_atendimento_historico,
            sIdTable: 'datatable_fale_conosco_atendimento_historico'
        });
        $('html, body').animate({ scrollTop: ($('#div_detalhes_fale_conosco_atendimento').offset().top - 113) }, "slow");
    }
}

function enviarMensagem(ch_chamado) {
    var chamado = {};
    for (var i = 0; i < _chamados.length; i++) {
        if (_chamados[i].ch_chamado == ch_chamado) {
            chamado = _chamados[i];
            break;
        }
    }
    if (IsNotNullOrEmpty(chamado)) {
        $('#ch_chamado_fale_conosco').val(chamado.ch_chamado);
        $('#ds_email_fale_conosco').val(chamado.ds_email);
        $('#nm_user_fale_conosco').val(chamado.nm_user);
        $('#ds_assunto_fale_conosco option[value="' + chamado.ds_assunto + '"]').prop("selected", true);
        fnSuccessFaleConosco = function (data) {
            if (IsNotNullOrEmpty(data, 'error_message')) {
                notificar('#form_fale_conosco', data.error_message, 'error');
            }
            else if (IsNotNullOrEmpty(data, 'success_message')) {
                if (IsNotNullOrEmpty(data, 'msg')) {
                    chamado.mensagens.push(data.msg);
                }
                notificar('#form_fale_conosco', data.success_message, 'success');
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
                        closeFaleConosco();
                        form_fale_conosco.reset();
                        generateCaptcha();
                        abrirChamado(ch_chamado)
                    }
                });
            }
            else {
                notificar('#form_fale_conosco', 'Erro ao enviar. Tente mais tarde.', 'error');
            }
        };
        clickTopoFaleConosco();
        $('#ds_msg_fale_conosco').focus();
    }
}
