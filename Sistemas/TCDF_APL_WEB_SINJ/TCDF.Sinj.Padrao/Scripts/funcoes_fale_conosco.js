var fnSuccessFaleConosco = function (data) {
    if (IsNotNullOrEmpty(data, 'error_message')) {
        notificar('#form_fale_conosco', data.error_message, 'error');
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
                form_fale_conosco.reset();
                generateCaptcha();
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