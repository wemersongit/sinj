
$(function () {
    (function ($) {
        $.loadScript = function (url, callback) {
            jQuery.ajax({
                url: url,
                dataType: 'script',
                success: callback,
                async: true
            });
        };
        $.ajaxlight = function (options) {
            var handleErrorAjax = function (jqXHR, textStatus, errorThrown) {
                var message;
                var statusErrorMap = {
                    '400': "Conte&uacute;do inv&aacute;lido.",
                    '401': "Acesso n&atilde;o autorizado.",
                    '403': "Acesso n&atilde;o autorizado.",
                    '404': "P&aacute;gina n&atilde;o encontrada.",
                    '500': "Erro interno do Servidor.",
                    '503': "Servi&ccedil;o n&atilde;o dispon&iacute;vel.",
                    '998': errorThrown,
                    '999': jqXHR.statusText
                };
                if (jqXHR) {
                    message = statusErrorMap[jqXHR.status];
                    if (jqXHR.status == 500 && jqXHR.responseJSON && jqXHR.responseJSON.ShowErrorServer) {
                        message = jqXHR.responseJSON.responseText;
                    }
                    if (jqXHR.status == 200 && errorThrown == "Error: callback was not called") {
                        return;
                    }
                    else if (jqXHR.status == 0 && errorThrown == "abort") {
                        return;
                    }
                    if (jqXHR && jqXHR.responseText)
                        f_Projeta_Error_Ajax(jqXHR.responseText, jqXHR.url);
                }
                if (textStatus == 'parsererror') {
                    message = "Falha na requisi&ccedil;&atilde;o ao servidor." + (IsNotNullOrEmpty(errorThrown) != null && IsNotNullOrEmpty(errorThrown.message) ? "<br/>" + errorThrown.message : "");
                }
                if (textStatus == 'timeout')
                    message = "Tempo de carregamento esgotado.<br/>Tente novamente mais tarde.";
                if (textStatus == 'abort')
                    message = "Requisi&ccedil;&atilde;o abortada pelo servidor.";
                if (!message)
                    message = "Ocorreu um erro desconhecido.";
                $('<div id="div_modal_erro" />').modallight({
                    sTitle: "Erro",
                    sContent: message,
                    sType: "error",
                    sWidth: "300",
                    jqXHR: jqXHR
                });
                if (settings.sFormId != null && settings.sFormId != "") {
                    $('#' + settings.sFormId + ' .loading').hide();
                    $('#' + settings.sFormId + ' .loaded').show();
                }
                else {
                    $('.loading').hide();
                    $('.loaded').show();
                }
                $('.ui-dialog-buttonpane').show();
                $('#super_loading').hide();
                return;
            }

            var defaults = {
                sUrl: "",
                sType: "POST",
                bAsync: false,
                bCache: false,
                oData: null,
                sFormId: "",
                sDataType: "json",
                iTimeout: 60000,
                fnBeforeSend: null,
                fnBeforeSubmit: null,
                fnComplete: null,
                fnError: handleErrorAjax,
                fnSuccess: null,
                bCallback: false
            }

            var settings = $.extend({}, defaults, options);


            $(document).ajaxComplete(function (event, xhr, settings) {
                if (xhr.status == 200) {
                    if (IsNotNullOrEmpty(xhr.responseText)) {
                        if (IsJson(xhr.responseText)) {
                            var jsonR = JSON.parse(xhr.responseText);
                            if (IsNotNullOrEmpty(jsonR._error_message)) {
                                if (jsonR._error_message == 'Sessao inexistente!!!') {
                                    $.removeCookie('neoLightBaseLook');
                                    $.removeCookie('neoLightBaseLook', { path: '/' });
                                    return;
                                }
                                if (jsonR._error_message.indexOf('Grupo invalido!!!') > -1) {
                                    $('<div id="div_modal_erro" />').modallight({
                                        sTitle: "Erro",
                                        sContent: "Permiss&atilde;o de acesso negado!!!<br> Voc&ecirc; est&aacute; tentando acessar um recurso n&atilde;o permitido.<br>" + jsonR.error_message,
                                        sType: "error",
                                        sWidth: "300",
                                        jqXHR: null
                                    });
                                    return;
                                }
                            }
                        }
                    }
                }
            });

            var dataRetorno = null;

            var jAjaxOption = {
                "url": settings.sUrl,
                "type": settings.sType,
                "cache": settings.bCache,
                "async": settings.bAsync,
                "data": settings.oData,
                "dataType": settings.sDataType,
                "timeout": settings.iTimeout,
                "beforeSend": settings.fnBeforeSend,
                "complete": settings.fnComplete,
                "error": settings.fnError,
                "success": settings.fnSuccess
            };
            if (settings.bCallback) {
                $.extend(jAjaxOption, { "dataType": "jsonp", "jsonpCallback": "callback", "crossDomain": true });
            }
            if (!settings.fnSuccess) {
                $.extend(jAjaxOption, { "success": function (data) { dataRetorno = data; } });
            }
            try {
                if (settings.sFormId != null && settings.sFormId != "") {
                    $.extend(jAjaxOption, { "beforeSubmit": settings.fnBeforeSubmit });
                    $('#' + settings.sFormId).ajaxSubmit(jAjaxOption);
                }
                else {
                    $.ajax(jAjaxOption);
                }
            } catch (e) {
                MostraErro(e);
            }

            return dataRetorno;
        }
        function carregarScript(src) {
            var s = document.createElement('script');
            s.type = 'text/javascript';
            s.async = true;
            s.src = src;
            (document.getElementsByTagName('head')[0] || document.getElementsByTagName('body')[0]).appendChild(s);
        }

        function MostraErro(e) {
            Log(e);
            if (typeof printStackTrace == 'undefined') $.loadScript(((IsNotNullOrEmpty(_urlPadrao)) ? _urlPadrao : "") + '/Scripts/stacktrace.js', function () {
                var trace = printStackTrace({ e: e });
                var erro = '<b>Message:</b> ' + e.message + '<br /><br /><b>Stack trace:</b><br> ' + trace.join('<br /><br />');
                $('<div id="div_modal_erro" />').modallight({
                    sTitle: "Erro",
                    sContent: "Script Erro ajax",
                    sType: "error",
                    sWidth: "300",
                    jqXHR: { "responseText": erro }
                });
            });
        }
        function callback(data) { return data; }

        function f_Projeta_Error_Ajax(sMsg, sUrl) {
            var e = { 'message': sMsg, 'url': sUrl, 'pagina': window.location.href };
            if ((sMsg != "") && (sUrl != ""))
                if (typeof jQuery != 'undefined') $.ajax({ type: 'POST', url: './ashx/ErrorAjax.ashx', data: e, jsonpCallback: "callback", dataType: "jsonp", cache: false, async: true, success: null, error: null });
            return;
        }
    })(jQuery);
});