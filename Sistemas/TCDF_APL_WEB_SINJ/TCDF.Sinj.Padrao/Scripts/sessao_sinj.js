// necessário carregar:'jquery-1.11.0.min.js', 'js.ashx', 'jquery.cookie.js'

////////////////////////////
//Verificar sessão do sinj//
////////////////////////////
var time_CheckLogin = setInterval(f_CheckLogin, 60000);
$(document).ready(function () {
    f_CheckLogin();
});

function f_CheckLogin() {
    $.cookie.json = false;
    if (IsNotNullOrEmpty($.cookie('neoLightSinjLook'))) {
        var sucesso = function (data) {
            if (data == false) { 
                f_AbortCheckLogin();
                $('<div id="modal_erro_sessao" />').modallight({
                    sTitle: "Usuário não conectado",
                    sContent: "Faça o login para continuar.",
                    sType: "error",
                    fnClose: function () {
                    	//Redireciona para a pagina de Login passando a pagina atual como parametro
                    	var pagina_atual = document.URL;
                    	var pagina_atual_array = pagina_atual.split("/");
                    	pagina_atual = pagina_atual_array[pagina_atual_array.length-1];
                    	document.location.href = "./Login.aspx?redirecionar="+pagina_atual;
                	}
                });
            }
        }
        $.ajaxlight({
            sUrl: './ashx/Login/checkLogin.ashx',
            sType: "POST",
            fnError: null,
            bAsync: true,
            fnSuccess: sucesso
        });
    }
}

function f_AbortCheckLogin() {
    clearInterval(time_CheckLogin);
}
