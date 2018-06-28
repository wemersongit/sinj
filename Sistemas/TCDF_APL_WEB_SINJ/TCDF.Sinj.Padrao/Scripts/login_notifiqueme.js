$(document).ready(function () {
    MontarUsuarioNotifiqueme();
});

function MontarUsuarioNotifiqueme() {
    if (IsNotNullOrEmpty(_notifiqueme, 'email_usuario_push')) {
        $('#div_user_notifiqueme').html(_notifiqueme.email_usuario_push + ', <a style="text-decoration:none; color:#FFF; font-weight:bold;" href="javascript:void(0);" onclick="javascript:DeslogarNotifiqueme();" title="Sair do Notifique-me" >sair</a>');
        $('#nm_user_fale_conosco').val(_notifiqueme.nm_usuario_push);
        $('#ds_email_fale_conosco').val(_notifiqueme.email_usuario_push);
    }
    else {
        $('#div_user_notifiqueme').html('');
    }
}

function LogarNotifiqueme() {
    try {
        Validar(form_login_notifiqueme);
        if ($('#email_usuario_push').val().length < 6) {
            throw "A senha deve possuir no mínimo 6 dígitos.";
        }
        var success = function (data) {
            if (IsNotNullOrEmpty(data, 'login_notifiqueme')) {
                var neoLightBasePush = { 'persist': $("#persist").is(":checked"), 'email_usuario_push': $("#email_usuario_push").val() };
                $.cookie(_nm_cookie_push, JSON.stringify(neoLightBasePush), { expires: 10000, path: '/' });
                //Talvez a linha abaixo seja desnecessaria, ja que isso esta sendo feito no aspx
                var redirecionar_notifiqueme = GetParameterValue("redirecionar_notifiqueme");
                var p = GetParameterValue("p");
                var back = GetParameterValue("back");
                var addFav = GetParameterValue("addfav");
                var _ch = GetParameterValue("ch");
                if (back == "1") {
                    top.document.location.href = document.referrer;
                }
                else if (IsNotNullOrEmpty(redirecionar_notifiqueme)) {
                    redirecionar_notifiqueme = redirecionar_notifiqueme.replace(/__EQUAL__/g, "=");
                    redirecionar_notifiqueme = redirecionar_notifiqueme.replace(/__AND__/g, "&");
                    redirecionar_notifiqueme = redirecionar_notifiqueme.replace(/__QUERY__/g, "?");
                    var array_ch_norma = redirecionar_notifiqueme.split("&");
                    var ch_norma = array_ch_norma[array_ch_norma.length - 1];
                    $.ajaxlight({
                        sUrl: "./ashx/Push/NotifiquemeNormaIncluir.ashx?" + ch_norma,
                        sType: "GET",
                        fnError: null,
                        iTimeout: 40000
                    });
                    top.document.location.href = redirecionar_notifiqueme;
                }
                else if (IsNotNullOrEmpty(p)) {
                    top.document.location.href = './' + p;
                }
                else if (addFav == "1" && IsNotNullOrEmpty(_ch)) {

                    $.ajaxlight({
                        sUrl: "./ashx/Push/FavoritosIncluir.ashx?ch=" + _ch,
                        sType: "GET",
                        fnComplete: function () {
                            var url_referrer = document.referrer;
                            var location = url_referrer.split('?')[0];
                            var params = url_referrer.split('?')[1];
                            if (url_referrer.indexOf('success_message') > -1) {
                                params = '';
                                var url_referrer_splited = url_referrer.split('?')[1].split('&');
                                for (var i = 0; i < url_referrer_splited.length; i++) {
                                    if (url_referrer_splited[i].indexOf('success_message') < 0) {
                                        params += (params != '' ? '&' : '') + url_referrer_splited[i];
                                    }
                                }
                            }
                            params += (params != '' ? '&' : '') + 'success_message=A norma foi incluída nos seus favoritos.';
                            top.document.location.href = location + '?' + params;
                        },
                        bAsync: true,
                        iTimeout: 40000
                    });
                }
                else {
                    top.document.location.href = "./Notifiqueme";
                }
            } else {
                gComplete();
                if (data.error_message != null) {
                    $('#div_notificacao_login_notifiqueme').messagelight({
                        sTitle: "Erro",
                        sContent: data.error_message,
                        sType: "error",
                        sWidth: "",
                        iTime: null
                    });
                } else {
                    $('#div_notificacao_login_notifiqueme').messagelight({
                        sTitle: "Erro",
                        sContent: "Não foi possível determinar erro!!!",
                        sType: "error",
                        sWidth: "",
                        iTime: null
                    });
                }
            }
        };
        $.ajaxlight({
            sUrl: './ashx/Push/NotifiquemeLogin.ashx',
            sType: "POST",
            sFormId: "form_login_notifiqueme",
            fnSuccess: success,
            fnBeforeSubmit: gInicio(),
            bAsync: true
        });
    }
    catch (ex) {
        $('#div_notificacao_login_notifiqueme').messagelight({
            sTitle: "Erro nos dados informados",
            sContent: ex,
            sType: "error"
        });
    }
    return false;
}

function DeslogarNotifiqueme() {
    //ShowMessage("PanelNotificacao", " Sistema sendo finalizado com segurança!!!", "alert");
    var sucesso = function (data) {
        if (data.excluido) {
            if (file == "Notifiqueme.aspx") {
                top.document.location.href = "./LoginNotifiqueme.aspx";
            }
            else {
                location.reload();
            }
            return;
        } else {
            if (data.error_message != null) {
                $('<div id="modal_erro_logout" />').modallight({
                    sTitle: "Erro ao encerrar sessão.",
                    sContent: data.error_message,
                    sType: "error",
                    fnClose: function () { $(this).remove(); }
                });
            } else {
                $('<div id="modal_erro_logout" />').modallight({
                    sTitle: "Erro ao encerrar sessão.",
                    sContent: "Não foi possível determinar o erro!!!",
                    sType: "error",
                    fnClose: function () { $(this).remove(); }
                });
            }
        }
    };
    var inicio = function () {
        $('#div_user_notifiqueme').html('<div class="loading-p"></div>');
    }
    $.ajaxlight({
        sUrl: './ashx/Push/NotifiquemeSessaoEnd.ashx',
        sType: "POST",
        fnSuccess: sucesso,
        fnBeforeSend: inicio,
        fnError: null,
        bAsync: true
    });
}

function NotificarNoLogin(ch_norma){
    $.ajaxlight({
        sUrl: "./ashx/Push/NotifiquemeNormaIncluir.ashx?ch_norma=" + ch_norma,
        sType: "GET",
        fnError: null,
        bAsync: false,
        iTimeout: 40000
    });
}

function NotificarAoCriar(redirecionar_notifiqueme){
    $('#a_cadastrar_notifiqueme').attr("href", "./CadastrarNotifiqueme.aspx?redirecionar_notifiqueme="+redirecionar_notifiqueme);
}