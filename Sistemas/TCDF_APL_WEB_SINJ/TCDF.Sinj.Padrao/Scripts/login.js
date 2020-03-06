function Logar() {
    try {
        Validar(form_login);
        if ($('#senha').val().length < 6) {
            throw "A senha deve possuir no mínimo 6 dígitos.";
        }
        var success = function (data) {
            if (IsNotNullOrEmpty(data, 'login')) {
                var neoLightBaseSinj = { 'persist': $("#persist").is(":checked"), 'login': data.login };
                $.cookie('neoLightSinj', JSON.stringify(neoLightBaseSinj), { expires: 10000, path: '/' });
                //Caso essa variavel exista, o login deve redirecionar para essa pagina

                if (window.location.search.indexOf('redirecionar=') > -1){
	                var redirecionar = unescape(window.location.search);
	                redirecionar = redirecionar.substring(redirecionar.indexOf('redirecionar=')+13);
           	    	top.document.location.href = redirecionar;
                }
                //Se não, o login redireciona para a pagina inicial definida pelo usuario
                else{
	                var s_url = ((IsNotNullOrEmpty(data.pagina_inicial)) ? getVal(data.pagina_inicial) : "");
	                if (IsNotNullOrEmpty(s_url)){
	           	    	top.document.location.href = s_url;
	                }
	                else{
	           	    	top.document.location.href = "./";
	                }
                }
            } else {
                $('.loaded').show();
                $('.loading').hide();
                if (data.error_message != null) {
                    $('#div_notificacao_login').messagelight({
                        sTitle: "Erro",
                        sContent: data.error_message,
                        sType: "error",
                        sWidth: "",
                        iTime: null
                    });
                } else {
                    $('#div_notificacao_login').messagelight({
                        sTitle: "Erro",
                        sContent: "Não foi possível determinar erro!!!",
                        sType: "error",
                        sWidth: "",
                        iTime: null
                    });
                }
            }
        };
        var beforeSubmit = function () {
            $('.loading').show();
            $('.loaded').hide();
        };
        $.ajaxlight({
            sUrl: './ashx/Login/Login.ashx',
            sType: "POST",
            sFormId: "form_login",
            fnSuccess: success,
            fnBeforeSubmit: beforeSubmit,
            bAsync: true
        });
    }
    catch (ex) {
        $('#div_notificacao_login').messagelight({
            sTitle: "Erro nos dados informados",
            sContent: ex,
            sType: "error"
        });
    }
    return false;
}

function Deslogar() {
    //ShowMessage("PanelNotificacao", " Sistema sendo finalizado com segurança!!!", "alert");
    var sucesso = function (data) {
        if (data.excluido) {
            document.location.href = "./Login.aspx?exit=3";
            return;
        } else {
            $('#a_logout').show();
            $('#div_logout_loading').hide();
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
                    sContent: "Não foi possível determinar erro!!!",
                    sType: "error",
                    fnClose: function () { $(this).remove(); }
                });
            }
        }
    };

    var inicio = function () {
        $('#a_logout').hide();
        $('#div_logout_loading').show();
    }
    $.ajaxlight({
        sUrl: './ashx/Login/SessaoEnd.ashx',
        sType: "POST",
        fnSuccess: sucesso,
        fnBeforeSend: inicio,
        fnError: null,
        bAsync: true
    });
}
