// necessário carregar:'jquery-1.11.0.min.js'
// 
var file = window.location.pathname.substring(window.location.pathname.lastIndexOf("/") + 1) || "Default.aspx";

function GetParameterValue(nm_parameter) {
    var query = unescape(window.location.search).substring(1);
    var params = query.split('&');
    var values = [];
    for (var i = 0; i < params.length; i++) {
        var keyvalue = params[i].split('=');
        if (keyvalue[0].toLowerCase() == nm_parameter) {
            values.push(keyvalue[1]);
        }
    }
    return values.join();
}

function GetParameterValueDecode(nm_parameter) {
    var query = decodeURIComponent(window.location.search).substring(1);
    var params = query.split('&');
    var values = [];
    for (var i = 0; i < params.length; i++) {
        var keyvalue = params[i].split('=');
        if (keyvalue[0].toLowerCase() == nm_parameter) {
            values.push(keyvalue[1]);
        }
    }
    return values.join();
}

function GetText(value, param) {
    if (IsNotNullOrEmpty(value) && IsNotNullOrEmpty(param)) {
        if (IsNotNullOrEmpty(eval('value.' + param))) {
            return eval('value.' + param);
        }
    }
    else if (IsNotNullOrEmpty(value)) {
        return value;
    }
    return "";
}

function escapeRegExp(s) {
    return s.replace(/([.*+?^=!:${}()|\[\]\/\\])/g, "\\$1");
}

//ReplaceAll function para reescrever várias partes de uma string
String.prototype.replaceAll = function (de, para) {
    return this.replace(new RegExp(escapeRegExp(de), 'g'), para);
};

function Nada() { return; }

var cookies_aux = ['back_history_aux_datatable'];

function voltar(){
	if(IsNotNullOrEmpty(cookies_aux) && isArray(cookies_aux)){
		var referrer = document.referrer.replace(/^[^:]+:\/\/[^\/]+/, '').replace(/#.*/, '').replace(/\?.*/, '');
		for(var i = 0; i < cookies_aux.length; i++){
			$.cookie(cookies_aux[i], JSON.stringify({ "referrer": referrer, "last_page": document.location.pathname }), { expires: 1, path: '/' });
		}
	}
	history.go(-1);
    return;
}

//function voltar() {
//    history.go(-1);

//    return;
//}

function btCad() {
    document.location.href = './Cad' + pegaPagina(file) + "?time=" + new Date().getTime();
    return;
}

function btPgPesquisar() {
    document.location.href = './Pesquisar' + pegaPagina(file) + "?time=" + new Date().getTime();
    return;
}

function OnEnter(evt) {
    var key_code = evt.keyCode ? evt.keyCode : evt.charCode ? evt.charCode : evt.which ? evt.which : void 0;
    if (key_code == 13) { return true; }
}

//json_parametros_adicionais deve ser um json com a seguinte estrutura:
//[{"name":"nome_do_campo","value":"valor_do_campo"},{"name":"nome_do_campo","value":"valor_do_campo"},...]
function btDetalhes(_id_doc, _pagina, s_return, json_parametros_adicionais) {
    if (_pagina == null) _pagina = pegaPagina(file);
    var parametros_adicionais = "";
    if (json_parametros_adicionais != null) {
        for (var i = 0; i < json_parametros_adicionais.length; i++) {
            if (json_parametros_adicionais[i].name != null && json_parametros_adicionais[i].name != "" && json_parametros_adicionais[i].value != null && json_parametros_adicionais[i].value != "") {
                parametros_adicionais = parametros_adicionais + "&" + json_parametros_adicionais[i].name + "=" + json_parametros_adicionais[i].value;
            }
        }
    }
    var s_pagina = './Detalhes' + _pagina + "?id_doc=" + _id_doc + parametros_adicionais + "&time=" + new Date().getTime();
    if (s_return) return s_pagina;
    document.location.href = s_pagina;
    return;
}

function btAlterar(_id_doc, _pagina, s_return) {
    if (_pagina == null) _pagina = pegaPagina(file);
    var s_pagina = './Editar' + _pagina + "?id_doc=" + _id_doc + "&time=" + new Date().getTime();
    if (s_return) return s_pagina;
    document.location.href = s_pagina;
    return;
}

function btReplicar(_id_doc, _pagina, s_return) {
    if (_pagina == null) _pagina = pegaPagina(file);
    var s_pagina = './Cad' + _pagina + "?id_doc=" + _id_doc + "&time=" + new Date().getTime();
    if (s_return) return s_pagina;
    document.location.href = s_pagina;
    return;
}

function btExcluir(_id_doc, _pagina, s_return) {
    if (_pagina == null) _pagina = pegaPagina(file);
    var s_pagina = './JustificativaExclusao' + _pagina + "?id_doc=" + _id_doc + "&time=" + new Date().getTime();
    if (s_return) return s_pagina;
    document.location.href = s_pagina;
    return;
}

function btTramitar(_id_doc_app, _app, s_return) {
    var agora = new Date();
    var s_pagina = _urlDocsPro + '/Tramitar.aspx?id_doc_app=' + _id_doc_app + '&app=' + _app + "&time=" + agora.getTime();
    if (s_return) return s_pagina;
    document.location.href = s_pagina;
    return;
}

function btTramitarDocs(_id_doc, s_return) {
    var s_pagina = _urlDocsPro + '/Tramitar.aspx?id_doc=' + _id_doc + "&time=" + new Date().getTime();
    if (s_return) return s_pagina;
    document.location.href = s_pagina;
    return;
}
function pegaPagina(pagina) {
    return pagina.replace("Cad", "").replace("Pesquisar", "").replace("Editar", "").replace("Detalhes", "").replace("ResultadoPesquisa", "").replace("JustificativaExclusao", "").replace("Excluir", "").replace("NumeracaoOrgaos", "OrgaoInterno");
}

function fTitle(pagina) {
    var mapPaginaAPP = {
        'Usuario.aspx': "Usuário",
        'TipoOperacao.aspx': "Tipo Operação",
        'AutoridadeClassificadora.aspx': "Autoridade Classificadora",
        'TipoDoc.aspx': "Tipo Documento",
        'Status.aspx': "Status",
        'GrupoOrgao.aspx': "Grupo de Orgaos",
        'Sigilo.aspx': "Sigilo",
        'Providencia.aspx': "Providência",
        'OrgaoInterno.aspx': "Orgão Interno",
        'OrgaoExterno.aspx': "Orgão Externo",
        'OrgaoExternoCache.aspx': "Orgão Externo Cache",
        'MarcacaoEspecial.aspx': "Marcação Especial",
        'Grupo.aspx': "Grupo",
        'Emitente.aspx': "Emitente",
        'AssuntoDescendente.aspx': "Assunto Descendente",
        'Assunto.aspx': "Assunto",
        'Aplicacao.aspx': "Aplicação",
        'Acesso.aspx': "Acesso",
        'Erro.aspx': "Erro",
        'Excluido.aspx': "Excluido",
        'Operacao.aspx': "Operação",
        'Configuracoes.aspx': "Configurações",
        'Recebidos.aspx': "Recebidos",
        'Expedidos.aspx': "Expedidos",
        'DashBoard.aspx': "Meus Documentos",
        'DocsPro.aspx': "PGFNDocs"
    };
    return mapPaginaAPP[pagina]
}

function urldecode(url) {
    try { return decodeURIComponent(url.replace(/\+/g, ' ')); } catch (e) { return url; }
}

function parseURL(url) {
    var a = document.createElement('a');
    a.href = url;
    return {
        source: url,
        protocol: a.protocol.replace(':', ''),
        host: a.hostname,
        port: a.port,
        query: a.search,
        params: (function () {
            var ret = {},
                seg = urldecode(a.search.replace(/^\?/, '')).split('&'),
                len = seg.length, i = 0, s;
            for (; i < len; i++) {
                if (!seg[i]) { continue; }
                s = seg[i].split('=');
                ret[s[0]] = s[1];
            }
            return ret;
        })(),
        aoData:(function () {
            var ret = [],
                seg = urldecode(a.search.replace(/^\?/, '')).split('&'),
                len = seg.length, i = 0, s;
            for (; i < len; i++) {
                if (!seg[i]) { continue; }
                s = seg[i].split('=');
                ret[i] = { "name": s[0], "value": s[1] };
            }
            return ret;
        })(),
        highLight: (function () {
            var ret = new Array(),
                seg = urldecode(a.search.replace(/^\?/, '')).split('&'),
                len = seg.length, i = 0, s;
            for (; i < len; i++) {
                if (!seg[i]) { continue; }
                s = seg[i].split('=');
                ret[i] = { "Campo": fNameHighLightCampo(s[0]), "Valor": [s[1]] }
            }
            return ret;
        })(),
        file: (a.pathname.match(/\/([^\/?#]+)$/i) || [, ''])[1],
        hash: a.hash.replace('#', ''),
        path: a.pathname.replace(/^([^\/])/, '/$1'),
        relative: (a.href.match(/tps?:\/\/[^\/]+(.+)/) || [, ''])[1],
        segments: a.pathname.replace(/^\//, '').split('/'),
        urlnoquery: (url) ? url.split("?")[0] : ''
    };
}

function fNameHighLightCampo(nomeCampo) {
    if (nomeCampo == "txtbuscalivre") {
        return "allfield";
    }
    return nomeCampo;
}

/*Convert dd/mm/aaaa para um objeto do tipo Date
Exemplo:
18/10/2013 converte para 
Thu Oct 18 2013 00:00:00 GMT-0200
*/
function convertStringToDateTime(dataString) {
    var dataSplit = dataString.split("/");
    var dia = dataSplit[0];
    var mes = dataSplit[1];
    var ano = dataSplit[2];
    return new Date(ano + "/" + mes + "/" + dia);
}

function dateIsDateFuture(data) {
    return (data > new Date());
}

function IsEmptyCombo(Combo) {
    return (Combo.length == 0);
}

function ComboClearSelected(Combo) {
    if ((Combo.length > 0) && (Combo.selectedIndex >= 0)) { Combo.remove(Combo.selectedIndex); }
}

function JaExiste(Combo, sValue) {
    for (var i = 0; i <= Combo.length - 1; i++) { if (Combo.options[i].value == sValue) { return true; } }
}

function comboRemoveItem(Combo, sValue) {
    for (var i = 0; i <= Combo.length - 1; i++) {
        if (Combo.options[i].value == sValue) {
            Combo.remove(i); return;
        }
    }
}

function ComboSelectedAll(Combo) {
    for (var i = 0; i <= Combo.length - 1; i++) Combo.options[i].selected = true;
}

function ComboSelectedValues(Combo) {
    var saida = "";
    for (var i = 0; i <= Combo.length - 1; i++) { saida += Combo.options[i].value + "$"; }
    return saida;
}

//--------------------------------------------------------------------
// Função: mascara_cpf_cnpj()
// Descrição: Formata a máscara dos campos CPF/CNPJ do sistema
//--------------------------------------------------------------------
function mascara_cpf_cnpj(input) {
    campo = input.value //Número dígitado
    campo = campo.replace(".", "");
    campo = campo.replace(".", "");
    campo = campo.replace("-", "");
    campo = campo.replace("/", "");
    if (campo.length == "11" || campo.length == "10") {
        input.value = campo.substring(0, 3) + "." + campo.substring(3, 6) + "." + campo.substring(6, 9) + "-" + campo.substring(9, 11)
    }
    else if (campo.length == "14") {
        input.value = campo.substring(0, 2) + "." + campo.substring(2, 5) + "." + campo.substring(5, 8) + "/" + campo.substring(8, 12) + "-" + campo.substring(12, 14)

    }
}

//--------------------------------------------------------------------
// Função: mascara_cpf_cnpj()
// Descrição: Formata a máscara do CPF/CNPJ
//--------------------------------------------------------------------
function mascara_cpf_cnpj_string(valor) {

    valor = valor.replace(".", "");
    valor = valor.replace(".", "");
    valor = valor.replace("-", "");
    valor = valor.replace("/", "");

    var retorno = "";

    if (valor.length == "11") {
        retorno = valor.substring(0, 3) + "." + valor.substring(3, 6) + "." + valor.substring(6, 9) + "-" + valor.substring(9, 11)
    }
    else if (valor.length == "14") {
        retorno = valor.substring(0, 2) + "." + valor.substring(2, 5) + "." + valor.substring(5, 8) + "/" + valor.substring(8, 12) + "-" + valor.substring(12, 14)

    }
    return retorno;
}

//--------------------------------------------------------------------
// Função: dataAtualFormatada()
// Descrição: Retorna a data com o formato dd/mm/yyyy
//--------------------------------------------------------------------
function dataAtualFormatada(hora) {
    var data = new Date();
    var dia = data.getDate();
    if (dia.toString().length == 1)
        dia = "0" + dia;
    var mes = data.getMonth() + 1;
    if (mes.toString().length == 1)
        mes = "0" + mes;
    var ano = data.getFullYear();

    today = new Date();
    var hoursStr = today.getHours();
    var minuteStr = today.getMinutes();
    var secondStr = today.getSeconds();

    if (secondStr.toString().length == 1) {
        secondStr = "0" + today.getSeconds();
        secondStr = secondStr.substr(-2);
    }
    if (minuteStr.toString().length == 1) {
        minuteStr = "0" + today.getMinutes();
        minuteStr = minuteStr.substr(-2);
    }
    if (hoursStr.toString().length == 1) {
        hoursStr = "0" + today.getHours();
        hoursStr = hoursStr.substr(-2);
    }

    if (hora) {
        return dia + "/" + mes + "/" + ano + " " + hoursStr + ":" + minuteStr + ":" + secondStr;
    }
    else {
        return dia + "/" + mes + "/" + ano;
    }
}

/* #=============================================================================== */
/* # Funções que dependem de JQuery
/* #=============================================================================== */

// existe elemento jQuery
jQuery.fn.exists = function () {
    return jQuery(this).length > 0 ? true : false;
};

//Posiciona a página no elemento
jQuery.fn.goTo = function (element_scroll) {
    if (!IsNotNullOrEmpty(element_scroll)) {
        element_scroll = 'html, body';
    }
    jQuery(element_scroll).animate({
        scrollTop: $(this).offset().top + 'px'
    }, 'fast');
    return this; // for chaining...
}

function ativa_enter() {
    $("input").keypress(function (event) { if (OnEnter(event)) { event.preventDefault(); btPesquisar(); } });
}

function btPesquisar() {
    $('#divDados').hide();
    $('#divBotoes').hide();
    $('#divLoading').show();
    if (validarCampos()) {
        var agora = new Date();
        var pagina = './Resultado' + pegaPagina(file);
        var data = JSON.stringify($("form").find('input, textarea, select').not('input:hidden[name=__VIEWSTATE]').serialize()).replace(/\"/g, "");
        document.location.href = './ResultadoPesquisa' + pegaPagina(file) + '?' + data + "&time=" + agora.getTime();
    } else {
        $('#divDados').show();
        $('#divBotoes').show();
        $('#divLoading').hide();
    }
}

function PaginaInicial() {
    if (file != null) {
        var contemCad = file.indexOf("Cad");
        var contemPesquisa = file.indexOf("Pesquisar");
        var contemRelatorio = file.indexOf("Relatorio");
        var contemPesquisaAvancada = file.indexOf("PesquisaAvancada");
        var contemDashBoard = file.indexOf("DashBoard");

        if (file.toLowerCase().indexOf("inicial.aspx") > -1 && window.location.pathname.toLowerCase().indexOf("/portal/") == -1) {
            if ($('#divMenu').length) { $("#divMenu").hide(); } else { $("#divMenu").show(); }
        } else {
            if ($('#linkManuPesq').length) $("#linkManuPesq").attr({ href: './Pesquisar' + pegaPagina(file), title: 'Pesquisar ' + fTitle(pegaPagina(file)) });
            if ($('#linkManuCad').length) $("#linkManuCad").attr({ href: './Cad' + pegaPagina(file), title: 'Cadastrar ' + fTitle(pegaPagina(file)) });
            if ($('#divMenu').length) { $("#divMenu").show(); }
        }

        if (contemCad == 0) {
            if ($('#ulManuCad').length) {
                $("#ulManuCad").hide();
            }
        }
        else {
            if (pegaPagina(file) == "Usuario.aspx") { if (!IsUserGroupMember("MA.US.IN")) { $("#ulManuCad").hide(); } }
            if (pegaPagina(file) == "Grupo.aspx") { if (!IsUserGroupMember("MA.GR.IN")) { $("#ulManuCad").hide(); } }
            if (pegaPagina(file) == "OrgaoInterno.aspx") { if (!IsUserGroupMember("MA.OR.IN.IN")) { $("#ulManuCad").hide(); } }
            if (pegaPagina(file) == "OrgaoExterno.aspx") { if (!IsUserGroupMember("MA.OR.EX.IN")) { $("#ulManuCad").hide(); } }
            if (pegaPagina(file) == "Sigilo.aspx") { if (!IsUserGroupMember("MA.SI.IN")) { $("#ulManuCad").hide(); } }
            if (pegaPagina(file) == "TipoDoc.aspx") { if (!IsUserGroupMember("MA.TD.IN")) { $("#ulManuCad").hide(); } }
            if (pegaPagina(file) == "Status.aspx") { if (!IsUserGroupMember("MA.SD.IN")) { $("#ulManuCad").hide(); } }
            if (pegaPagina(file) == "TipoOperacao.aspx") { if (!IsUserGroupMember("MA.OP.IN")) { $("#ulManuCad").hide(); } }
            if (pegaPagina(file) == "PesquisarAutoridadeClassificadora.aspx") { if (!IsUserGroupMember("MA.AC.IN")) { $("#ulManuCad").hide(); } }
            if (pegaPagina(file) == "PesquisarGrupoOrgao.aspx") { if (!IsUserGroupMember("MA.GO.IN")) { $("#ulManuCad").hide(); } }
            if (pegaPagina(file) == "Providencia.aspx") { if (!IsUserGroupMember("MA.PR.IN")) { $("#ulManuCad").hide(); } }
            if (pegaPagina(file) == "Assunto.aspx") { if (!IsUserGroupMember("MA.AS.IN")) { $("#ulManuCad").hide(); } }
            if (pegaPagina(file) == "AssuntoDescendente.aspx") { if (!IsUserGroupMember("MA.AD.IN")) { $("#ulManuCad").hide(); } }
            if (pegaPagina(file) == "Aplicacao.aspx") { if (!IsUserGroupMember("MA.AP.IN")) { $("#ulManuCad").hide(); } }
            if (pegaPagina(file) == "MarcacaoEspecial.aspx") { if (!IsUserGroupMember("MA.ME.IN")) { $("#ulManuCad").hide(); } }
            if (pegaPagina(file) == "OrgaoExternoCache.aspx") { if (!IsUserGroupMember("MA.OR.CA.IN")) { $("#ulManuCad").hide(); } }
        }
        if (contemPesquisa == 0) {
            if ($('#ulManuPesq').length) {
                $("#ulManuPesq").hide();
            }
        }
        else {
            if (pegaPagina(file) == "Usuario.aspx") { if (!IsUserGroupMember("MA.US.PE")) { $("#ulManuPesq").hide(); } }
            if (pegaPagina(file) == "Grupo.aspx") { if (!IsUserGroupMember("MA.GR.PE")) { $("#ulManuPesq").hide(); } }
            if (pegaPagina(file) == "OrgaoInterno.aspx") { if (!IsUserGroupMember("MA.OR.IN.PE")) { $("#ulManuPesq").hide(); } }
            if (pegaPagina(file) == "OrgaoExterno.aspx") { if (!IsUserGroupMember("MA.OR.EX.PE")) { $("#ulManuPesq").hide(); } }
            if (pegaPagina(file) == "Sigilo.aspx") { if (!IsUserGroupMember("MA.SI.PE")) { $("#ulManuPesq").hide(); } }
            if (pegaPagina(file) == "TipoDoc.aspx") { if (!IsUserGroupMember("MA.TD.PE")) { $("#ulManuPesq").hide(); } }
            if (pegaPagina(file) == "PesquisarAutoridadeClassificadora.aspx") { if (!IsUserGroupMember("MA.AC.PE")) { $("#ulManuPesq").hide(); } }
            if (pegaPagina(file) == "PesquisarGrupoOrgao.aspx") { if (!IsUserGroupMember("MA.GO.PE")) { $("#ulManuPesq").hide(); } }
            if (pegaPagina(file) == "Status.aspx") { if (!IsUserGroupMember("MA.SD.PE")) { $("#ulManuPesq").hide(); } }
            if (pegaPagina(file) == "TipoOperacao.aspx") { if (!IsUserGroupMember("MA.OP.PE")) { $("#ulManuPesq").hide(); } }
            if (pegaPagina(file) == "Providencia.aspx") { if (!IsUserGroupMember("MA.PR.PE")) { $("#ulManuPesq").hide(); } }
            if (pegaPagina(file) == "Assunto.aspx") { if (!IsUserGroupMember("MA.AS.PE")) { $("#ulManuPesq").hide(); } }
            if (pegaPagina(file) == "AssuntoDescendente.aspx") { if (!IsUserGroupMember("MA.AD.PE")) { $("#ulManuPesq").hide(); } }
            if (pegaPagina(file) == "Aplicacao.aspx") { if (!IsUserGroupMember("MA.AP.PE")) { $("#ulManuPesq").hide(); } }
            if (pegaPagina(file) == "MarcacaoEspecial.aspx") { if (!IsUserGroupMember("MA.ME.PE")) { $("#ulManuPesq").hide(); } }
            if (pegaPagina(file) == "OrgaoExternoCache.aspx") { if (!IsUserGroupMember("MA.OR.CA.PE")) { $("#ulManuPesq").hide(); } }
        }
        if (contemRelatorio == 0)
            if ($('#ulRelatorios').length) $("#ulRelatorios").hide();
        if (contemPesquisaAvancada == 0)
            if ($('#ulPesquisaAvancada').length) $("#ulPesquisaAvancada").hide();
        if (contemDashBoard == 0)
            if ($('#ulMinhaArea').length) $("#ulMinhaArea").hide();
    }
}

function loading(selector) {
    $(selector).html('<div class="loading"></div>');
}

function isNullOrEmpty(obj, parameterToTest) {
    return !IsNotNullOrEmpty(obj, parameterToTest);
}

//Se passada uma string em parameterToTest a função tenta usar o eval para concatenar o obj com o parametro: eval('obj.' + parameterToTest)
function IsNotNullOrEmpty(obj, parameterToTest) {
    try {
        if (typeof parameterToTest == 'string' && parameterToTest !== "null" && parameterToTest !== "undefined" && parameterToTest !== "") obj = eval('obj.' + parameterToTest);
        return (typeof obj == 'boolean' && obj) || (typeof obj == 'undefined' && obj !== undefined) || (typeof obj == 'string' && obj !== "null" && obj !== "undefined" && obj !== "") || (typeof obj == 'object' && typeof obj.length != 'number' && !isEmptyObject(obj)) || (typeof obj == 'object' && typeof obj.length == 'number' && obj.length > 0) || (typeof obj == 'number' && obj > 0);
    } catch (e) {
        return false;
    }
}

function isEmptyObject(obj) {
    for (var prop in obj)
        if (obj.hasOwnProperty(prop))
            return false;
    return true;
}

function focusElement(e) {
    try {
        if (e.disabled == false && e.getAttribute("type") != "hidden" && e.getAttribute("display") != "none") {
            e.focus();
        }
    } catch (c)
    { }
}

function getElement(key) {
    var e = null;
    if (document.getElementById) {
        e = document.getElementById(key);
    } else if (document.getElementsByName) {
        e = document.getElementsByName(key);
    } else if (document.all) {
        e = window.document.all[key];
    } else if (document.layers) {
        e = window.document.layers[key];
    }

    return (e != null) ? e : false;
}

function getVal(str) {
    return ((str) ? str : "");
}

function IsJson(str) {
    try {
        JSON.parse(str);
    } catch (e) {
        return false;
    }
    return true;
}

function Guid(arg) {
    var s4 = function() {
        return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
    }
    if (arg == "N") {
        return (s4() + s4() + "" + s4() + "4" + s4().substr(0, 3) + "" + s4() + "" + s4() + s4() + s4()).toLowerCase();
    }
    return (s4() + s4() + "-" + s4() + "-4" + s4().substr(0, 3) + "-" + s4() + "-" + s4() + s4() + s4()).toLowerCase();
}

function Log(objeto){
    try{
    	console.log(objeto);
    }
    catch(ex){
    	
    }
}

function isArray(obj) {
    return obj.constructor.toString().indexOf("Array") > -1;
}

function ultimoDiaDoMes(year, month) {
    return new Date(year, month, 0).getDate();
}

function getMeses() {
    return [
        { nr_mes: "1", nm_mes: "Janeiro" },
        { nr_mes: "2", nm_mes: "Fevereiro" },
        { nr_mes: "3", nm_mes: "Março" },
        { nr_mes: "4", nm_mes: "Abril" },
        { nr_mes: "5", nm_mes: "Maio" },
        { nr_mes: "6", nm_mes: "Junho" },
        { nr_mes: "7", nm_mes: "Julho" },
        { nr_mes: "8", nm_mes: "Agosto" },
        { nr_mes: "9", nm_mes: "Setembro" },
        { nr_mes: "10", nm_mes: "Outubro" },
        { nr_mes: "11", nm_mes: "Novembro" },
        { nr_mes: "12", nm_mes: "Dezembro" }
    ]
}

function getAnos(min, max) {
    var anos = [];
    for (; min <= max; min++) {
        anos.push({ "ano": min });
    }
    return anos;
}

function getDias(nAno, nMes) {
    var dias = [];
    var min = 1;
    var max = ultimoDiaDoMes(nAno, nMes);
    for (; min <= max; min++) {
        dias.push({ "dia": min });
    }
    return dias;
}

//Testa se é JSON
function isJson(str) {
    try {
        JSON.parse(str);
    } catch (e) {
        return false;
    }
    return true;
}

function isInt(i) {
    if (!isNaN(i)) {
        return i == parseInt(i);
    }
    return false;
}

// ao iniciar ajax chamar 
var gInicio = function () { $('#super_loading').show(); };
// ao completar ajax chamar 
var gComplete = function () { $('#super_loading').hide("slow"); };

//Exibe ou oculta elementos na página se o checkbox estiver marcado ou nao
function toggleCheckedShowHide(select_check, select_show) {
    if ($(select_check).is(':checked')) {
        $(select_show).show();
    }
    else {
        $(select_show).hide();
    }
}

function replaceTextByHtml(element, text, html) {
    var nodes = [];
    var contents = $(element).contents();
    for(var i = 0; i < contents.length; i++){
        if (contents[i].nodeType == 3) {
            var new_html = $('<div></div>').html(contents[i].textContent.replace(text, html)).html();
            nodes.push(new_html);
        }
        else {
            nodes.push(contents[i]);
        }
    }
    if(nodes.length > 0){
        $(element).html(nodes);
    }
}