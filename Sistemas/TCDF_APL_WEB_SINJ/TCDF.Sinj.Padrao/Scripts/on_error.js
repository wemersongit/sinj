// necessário carregar: 'jquery-1.11.0.min.js', 'js.ashx', 'Feedback.js', scripts.js
window.onerror = f_Projeta_Error;

function f_Projeta_Error(sMsg, sUrl, sLine) {
//    var e = { 'message': sMsg, 'url': sUrl, 'linenumber': sLine, 'pagina': window.location.href };
    var e = { 'url': sUrl, 'linenumber': sLine, 'pagina': window.location.href };
    if (sMsg != "Uncaught Error: Error calling method on NPObject.") {
        if ((sMsg != "") && (sUrl != 'undefined') && (sUrl != "") && (sUrl != 'null'))
            if (typeof jQuery != 'undefined')
                var jOptions = { type: 'POST', url: './ashx/ErrorJS.ashx', data: e, dataType: "json", cache: false, async: true, success: null, error: null, timeout: 9000 };
                $.ajax(jOptions);
    }
    return (((_ambiente != 'undefined') ? _ambiente : '') != "DESENVOLVIMENTO_VS");
}
