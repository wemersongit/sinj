function enviarMsgFaleConosco(id_form) {
    try {
        Validar(id_form);
        $.ajaxlight({
            sFormId: id_form,
            sUrl: './ashx/Cadastro/FaleConoscoMensagemIncluir.ashx',
            sType: "POST",
            fnSuccess: function (data) {
                if (IsNotNullOrEmpty(data, 'error_message')) {
                    notificar('#' + id_form, data.error_message, 'error');
                }
                else if (IsNotNullOrEmpty(data, 'success_message')) {
                    notificar('#' + id_form, data.success_message, 'success');
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
                    notificar('#' + id_form, 'Erro ao enviar. Tente mais tarde.', 'error');
                }
            },
            fnBeforeSubmit: gInicio,
            fnComplete: gComplete,
            bAsync: true
        });

    } catch (ex) {
        notificar('#' + id_form, ex, 'error');
        $("html, body").animate({
            scrollTop: 0
        }, "slow");
    }
    return false;
}