// necessário carregar:'jquery-1.11.0.min.js', 'js.ashx', 'jquery.cookie.js'

////////////////////////////////////
//Verificar sessão do Notifique-me//
////////////////////////////////////
var time_CheckLoginPush = setInterval(f_CheckLoginPush, 60000);
$(document).ready(function () {
    f_CheckLoginPush();
});
function f_CheckLoginPush() {
    $.cookie.json = false;
    if (IsNotNullOrEmpty($.cookie('neoLightSinjPushLook'))) {
        var sucesso = function (data) {
            if (data == false) {
                f_AbortCheckLoginPush();
                $('<div id="modal_erro_sessao_push" />').modallight({
                    sTitle: "Sessão expirada",
                    sContent: "Sua sessão do Notifique-me expirou. Faça o login para continuar.",
                    sType: "error",
                    fnClose: function () { document.location.href = "./LoginNotifiqueme.aspx"; }
                });
            }
        }
        $.ajaxlight({
            sUrl: './ashx/Push/NotifiquemeCheckLogin.ashx',
            sType: "POST",
            fnError: null,
            bAsync: true,
            fnSuccess: sucesso
        });
    }
}

function f_AbortCheckLoginPush() {
    clearInterval(time_CheckLoginPush);
}
