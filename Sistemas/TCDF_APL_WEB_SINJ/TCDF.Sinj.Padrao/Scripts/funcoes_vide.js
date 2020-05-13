let normaAlteradora = {};
let normaAlterada = {};
let vide = {};
let videSemArquivo = false;
let ahTextoIncompativel = false;
const dt_controle_alteracao = new Date();

function ehRelacaoDeAlteracaoCompleta(chTipoRelacao){
    const relacoesAlteracaoCompleta = ['21', '4', 'a8ed93396fcc4959b9b8e82808880f2a', '25'];
    return relacoesAlteracaoCompleta.indexOf(chTipoRelacao) > -1;
}

function ehRelacaoQueDesfazAlteracaoCompleta(chTipoRelacao){
    const relacoesQueDesfazAlteracaoCompleta = ['18', 'b4fe69f9b8d748b19e41be8a2071dbdd'];
    return relacoesQueDesfazAlteracaoCompleta.indexOf(chTipoRelacao) > -1;
}

function showMessageNormaIncompativel(norma){
    $('#div_notificacao_vide').messagelight({
        sTitle: "Erro",
        sContent: `O texto da norma ${norma.ds_norma} não é compatível com o módulo de edição de vides.`,
        sType: "error",
        sWidth: "",
        iTime: null
    });
}

function selecionarTextoCopiar() {
    var text = window.getSelection().toString();
    window.getSelection().empty();
    if (text != '') {
        while (text.indexOf('\n\n') > -1) {
            text = text.replace(/\n\n/g, '\n');
        }
        $('#div_tooltip_dispositivo textarea').val(text);
        $('#div_tooltip_dispositivo textarea').removeClass('hidden');

        $('#div_tooltip_dispositivo button.copy').removeClass('selected');
        $('#div_tooltip_dispositivo button.copy').hide();
        $('#div_cad_dispositivo_alteradora div.div_conteudo_arquivo').removeClass('copy-enabled');
        $('#div_tooltip_dispositivo textarea').focus();
    }
}

function clickButtonSelecionarDispositivo(el) {
    if(normaAlteradora.dispositivos.length <= 0){
        alert('Selecione o dispositivo alterador.');
        return;
    }
    var linkname = $(el).attr('linkname');
    if (IsNotNullOrEmpty(linkname)) {
        if ($(el).hasClass('select')) {
            if($('#div_tooltip_dispositivo div.tooltip').length > 0){
                return false;
            }
            var p = $(el.parentNode).clone();
            $(p).find('a>sup').remove();
            $(p).find('a.link_vide').remove();
            var text = $(p).text();

            $(el).removeClass('select').addClass('selected');
            $(el).tooltip('show');
            $('#div_cad_dispositivo_alteradora div.div_conteudo_arquivo').addClass('copy-enabled');
        }
        else {
            closeTooltipDispositivoAlterado(el);
        }
    }
}

function clickCancelarAlterarDispositivo(el){
    closeTooltipDispositivoAlterado($(`button[aria-describedby=${$('#div_tooltip_dispositivo>div.tooltip').attr('id')}]`));
}

function closeTooltipDispositivoAlterado(el){
    $(el).removeClass('selected').addClass('select');
    $(el).tooltip('hide');
    $('#div_cad_dispositivo_alteradora div.div_conteudo_arquivo').removeClass('copy-enabled');
}

function selecionarTextoNormaAlteradoraEditar() {
    if($('#div_cad_dispositivo_alteradora div.div_conteudo_arquivo').hasClass('copy-enabled')){
        selecionarTextoCopiar();
        return;
    }
}

function adicionarDescricaoDispositivosAlterados(ds_dispositivo_alterado){
    let ds_dispositivos_alterados = $('textarea[name=ds_dispositivos_alterados]').val();
    ds_dispositivos_alterados += (IsNotNullOrEmpty(ds_dispositivos_alterados) ? '\n' : '') + ds_dispositivo_alterado;
    $('textarea[name=ds_dispositivos_alterados]').val(ds_dispositivos_alterados);
}

function removerDescricaoDispositivosAlterados(ds_dispositivo_alterado){
    let ds_dispositivos_alterados = $('textarea[name=ds_dispositivos_alterados]').val();
    const dispositivosSplited = ds_dispositivos_alterados.split('\n');
    ds_dispositivos_alterados = "";
    for(let i = 0; i < dispositivosSplited.length; i++){
        if(dispositivosSplited[i] == ds_dispositivo_alterado){
            continue;
        }
        ds_dispositivos_alterados += (IsNotNullOrEmpty(ds_dispositivos_alterados) ? '\n' : '') + dispositivosSplited[i];
    }
    $('textarea[name=ds_dispositivos_alterados]').val(ds_dispositivos_alterados);
    
}

function getNomeDoLinkName(linkname){
    const sufixo = linkname.substring(0, 3);
    const linknames = linkname.split('_');
    if(linknames > 1){
        sufixo = linknames[linknames.length - 1].substring(0, 3);
    }
    const nomes = {
        'art': 'Artigo',
        'ane': 'Anexo',
        'par': 'Anexo',
        'inc': 'Anexo',
        'num': '',
        'ali': 'Alínea',
        'let': ''
    };
    return nomes[sufixo];
}

function getDescricaoDoLinkname(linkname) {
    let descricao = '';
    const linknameSplited = linkname.split('_');
    let descricaoDispositivo = '';
    for(var item of linknameSplited){
        descricaoDispositivo = getDescricaoDoElemento(item);
        if (IsNotNullOrEmpty(descricaoDispositivo)) {
            descricao += (IsNotNullOrEmpty(descricao) ? ', ' : '') + descricaoDispositivo;
        }
    }
    const lastItem = linknameSplited[linknameSplited.length - 1];
    
    if(/art[0-9]|par[0-9]|inc[MDCLXVI]|ali[a-z]/.test(lastItem) && $(`#div_cad_dispositivo_alterada p[linkname^=${linkname}_]`).length > 0){
        descricao += ', caput'
    }
    return descricao;
}

function getDescricaoDoDispositivo(dispositivo) {
    var descricao = "";
    var descricao_linha = "";
    var descricao_dispositivo = "";
    var cSplited = [];
    if (dispositivo != null && dispositivo.length > 0)
    {
        for (var c in dispositivo)
        {
            descricao_linha = "";
            cSplited = dispositivo[c].split('_');
            for(var cs in cSplited){
                descricao_dispositivo = getDescricaoDoElemento(cSplited[cs]);
                if (IsNotNullOrEmpty(descricao_dispositivo)) {
                    descricao_linha += (IsNotNullOrEmpty(descricao_linha) ? ", " : "") + descricao_dispositivo;
                }
            }
            if (IsNotNullOrEmpty(descricao_linha)) {
                descricao += (IsNotNullOrEmpty(descricao) ? ";" : "") + descricao_linha;
            }
        }
        //descricao += IsNotNullOrEmpty(descricao) ? "." : "";
    }
    return descricao;
}

function getDescricaoDoElemento(dispositivo)
{
    var dispositivo1 = dispositivo.substring(0, 3).toLowerCase();
    var dispositivo2 = "";
    var dispositivo3 = "";
    if (dispositivo.length > 3) {
        dispositivo2 = dispositivo.substring(3);
        if (isInt(dispositivo2)) {
            if (parseInt(dispositivo2) < 10 && (dispositivo1 == 'art' || dispositivo1 == 'par')) {
                dispositivo2 += 'º';
            }
        }
    }
    if (dispositivo1 == "ane" && IsNotNullOrEmpty(dispositivo2)) {
        dispositivo3 = "Anexo " + dispositivo2.toUpperCase();
    }
    else if (dispositivo1 == "art" && IsNotNullOrEmpty(dispositivo2)) {
        dispositivo3 = "Art. " + dispositivo2;
    }
    else if (dispositivo1 == "par") {
        dispositivo3 = (IsNotNullOrEmpty(dispositivo2) ? "§ " + dispositivo2 : "Parágrafo Único");
    }
    else if (dispositivo1 == "inc" && IsNotNullOrEmpty(dispositivo2)) {
        dispositivo3 = "inc. " + dispositivo2;
    }
    else if (dispositivo1 == "num" && IsNotNullOrEmpty(dispositivo2)) {
        dispositivo3 = 'núm. ' + dispositivo2;
    }
    else if (dispositivo1 == "ali" && IsNotNullOrEmpty(dispositivo2)) {
        dispositivo3 = 'alí. "' + dispositivo2 + '"';
    }
    else if (dispositivo1 == "let" && IsNotNullOrEmpty(dispositivo2)) {
        dispositivo3 = 'let. "' + dispositivo2 + '"';
    }
    return dispositivo3;
}

function limparFormatacao(){
    $('div.div_conteudo_arquivo a.link-alterador').removeAttr('title');
    $('div.div_conteudo_arquivo a.link-alterador').removeAttr('id');
    $('div.div_conteudo_arquivo a.link-alterador').removeClass('link-alterador');

    $('div.div_conteudo_arquivo p.alterado').removeClass('alterado');
    $('div.div_conteudo_arquivo p.adicionado').removeClass('adicionado');
    
    $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo').find('button').remove();
    $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo p:hidden').show();
    $.each($('#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[replaced_by_revogado]'), function(index,value){
        value.setAttribute('replaced_by', value.getAttribute('replaced_by_revogado'));
        value.removeAttribute('replaced_by_revogado');
    });
}

function habilitarEmailPesquisa(id_doc, hsp, hemail){
    if (hemail != undefined && hsp != undefined) {
        var sucesso = function(data) {
            
        }
        var inicio = function () {
            $('#super_loading').show();
        }
        var complete = function (data) {
            $('#super_loading').hide();
        }
        $.ajaxlight({
            sUrl: './ashx/Cadastro/NormaEditarCampo.ashx?id_doc=' + id_doc + "&st_habilita_pesquisa=" + hsp + "&st_habilita_email=" + hemail,
            sType: "GET",
            fnSuccess: sucesso,
            fnComplete: complete,
            fnBeforeSend: inicio,
            fnError: null,
            bAsync: true
        });
    }
}