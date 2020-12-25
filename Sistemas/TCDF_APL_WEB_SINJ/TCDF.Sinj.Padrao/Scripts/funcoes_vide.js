let normaAlteradora = {};
let normaAlterada = {};
let vide = {};
let ahTextoIncompativel = false;

jQuery.expr[':'].regex = function (elem, index, match) {
    var matchParams = match[3].split(','),
        validLabels = /^(data|css):/,
        attr = {
            method: matchParams[0].match(validLabels) ?
                matchParams[0].split(':')[0] : 'attr',
            property: matchParams.shift().replace(validLabels, '')
        },
        regexFlags = 'ig',
        regex = new RegExp(matchParams.join('').replace(/^\s+|\s+$/g, ''), regexFlags);
    return regex.test(jQuery(elem)[attr.method](attr.property));
}

const dt_controle_alteracao = new Date();

function showMessageNormaIncompativel(norma){
    $('#div_notificacao_vide').messagelight({
        sTitle: "Erro",
        sContent: `O texto da norma ${norma.ds_norma} não é compatível com o módulo de edição de vides.`,
        sType: "error",
        sWidth: "",
        iTime: null
    });
    $(`input[norma=${norma.sufixo}]`).click();
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
    if(!IsNotNullOrEmpty(normaAlteradora, 'dispositivos') && !normaAlteradora.sem_arquivo && !isLecoVide(tipoDeRelacao.ch_tipo_relacao) && !$('#inSemCitacaoNormaAlteradora').is(':checked')){
        notificarErroVide('Erro','Selecione o dispositivo alterador.');
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
    let sufixo = linkname.substring(0, 3);
    const linknames = linkname.split('_');
    if(linknames.length > 1){
        sufixo = linknames[linknames.length - 1].substring(0, 3);
    }
    const nomes = {
        'art': 'Artigo',
        'ane': 'Anexo',
        'par': 'Parágrafo',
        'inc': 'Inciso',
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
    
    // if(/art[0-9]|par[0-9]|inc[MDCLXVI]|ali[a-z]/.test(lastItem) && $(`#div_cad_dispositivo_alterada p[linkname^=${linkname}_]`).length > 0){
    //     descricao += ', caput'
    // }
    if(/art[0-9]/.test(lastItem) && $(`#div_cad_dispositivo_alterada p[linkname^=${linkname}_]`).length > 0){
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
    $('div.div_conteudo_arquivo a.link-alterador').removeAttr('title').removeAttr('id').removeClass('link-alterador');

    $('div.div_conteudo_arquivo p[linkname]').removeClass('alterado').removeClass('desfeito').removeClass('renumerado').removeClass('leco').removeClass('info').removeClass('adicionado');
    
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

function isRelacaoDeAlteracaoCompleta(chTipoRelacao){
    const relacoesAlteracaoCompleta = ['4', '7', '21', '25', 'a8ed93396fcc4959b9b8e82808880f2a', '9a9cc35863a445438a06389420c1833e', 'a4dff3c010b94af3976555da9538d3e6', '215f37b93c1d4f05a357927db3c6885a'];
    return relacoesAlteracaoCompleta.indexOf(chTipoRelacao) > -1;
}

function isRelacaoQueDesfazAlteracao(chTipoRelacao){
    const relacoesQueDesfazAlteracaoCompleta = ['18', 'b4fe69f9b8d748b19e41be8a2071dbdd', '8e7abc807cb94cea93eccbb5ce56d92a'];
    return relacoesQueDesfazAlteracaoCompleta.indexOf(chTipoRelacao) > -1;
}

function isInfoVide(chTipoRelacao){
    // ratificado
    // reeditado
    // regulamentado
    // prorrogado
    // ressalva
    return ['10', '11', '12', '13', '16'].indexOf(chTipoRelacao) > -1;
}

function isRenumVide(chTipoRelacao){
    return chTipoRelacao == '36';
}

function isLecoVide(chTipoRelacao){
    return chTipoRelacao == '9';
}

function isAcrescimoVide(chTipoRelacao){
    return chTipoRelacao == '1';
}

function getDispositivoAlterado(linkname){
    const filtrar = function(disp){
        return disp.linkname == linkname;
    }
    return normaAlterada.dispositivos.filter(filtrar)[0];
}

function getLastLecoSelectorNormaAlteradora(){
    var regex = new RegExp('.*?([0-9]+)?(?: de ([0-9]{2}\/[0-9]{2}\/[0-9]{4}))?$', "i");
    var dispositivos = $('#div_cad_dispositivo_alteradora div.div_conteudo_arquivo').children();
    var selectorInsertBeforeAlterador = '#div_cad_dispositivo_alteradora div.div_conteudo_arquivo h1[epigrafe]';
    if(dispositivos.length > 0){
        for(let i = 0; i < dispositivos.length; i++){
            if(!dispositivos[i].hasAttribute('ch_norma_info')){
                break;
            }
            let resultRegex = regex.exec(dispositivos[i].innerText);
            if(!IsNotNullOrEmpty(resultRegex)){
                continue;
            }
            if(resultRegex[2] && normaAlterada.dt_assinatura){
                if(convertStringToDateTime(normaAlterada.dt_assinatura) > convertStringToDateTime(resultRegex[2])){
                    continue;
                }
                if(convertStringToDateTime(normaAlterada.dt_assinatura) == convertStringToDateTime(resultRegex[2])){
                    if(resultRegex[1] && normaAlterada.nr_norma){
                        if(parseInt(normaAlterada.nr_norma) > parseInt(resultRegex[1])){
                            continue;
                        }
                    }
                }
            }
            selectorInsertBeforeAlterador = `#div_cad_dispositivo_alteradora div.div_conteudo_arquivo p[ch_norma_info=${dispositivos[i].getAttribute('ch_norma_info')}]`;
        }
    }
    return selectorInsertBeforeAlterador;
}

function getLastLecoSelectorNormaAlterada(){
    var regex = new RegExp('.*?([0-9]+)?(?: de ([0-9]{2}\/[0-9]{2}\/[0-9]{4}))?$', "i");
    var dispositivos = $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo').children();
    var selectorInsertBeforeAlterado = '#div_cad_dispositivo_alterada div.div_conteudo_arquivo h1[epigrafe]';
    if(dispositivos.length > 0){
        for(let i = 0; i < dispositivos.length; i++){
            if(!dispositivos[i].hasAttribute('ch_norma_info')){
                break;
            }
            let resultRegex = regex.exec(dispositivos[i].innerText);
            if(!IsNotNullOrEmpty(resultRegex)){
                continue;
            }
            if(resultRegex[2] && normaAlteradora.dt_assinatura){
                if(convertStringToDateTime(normaAlteradora.dt_assinatura) > convertStringToDateTime(resultRegex[2])){
                    continue;
                }
                if(convertStringToDateTime(normaAlteradora.dt_assinatura) == convertStringToDateTime(resultRegex[2])){
                    if(resultRegex[1] && normaAlteradora.nr_norma){
                        if(parseInt(normaAlteradora.nr_norma) > parseInt(resultRegex[1])){
                            continue;
                        }
                    }
                }
            }
            selectorInsertBeforeAlterado = `#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[ch_norma_info=${dispositivos[i].getAttribute('ch_norma_info')}]`;
        }
    }
    return selectorInsertBeforeAlterado;
}

function getLastInfoSelectorNormaAlterada(){
    var selectorInsertAfterAlterado = '#div_cad_dispositivo_alterada div.div_conteudo_arquivo h1[epigrafe]';
    var dispositivosAposEpigrafe = $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo h1[epigrafe]').nextAll('p');
    if(dispositivosAposEpigrafe.length > 0){
        for(let i = 0; i < dispositivosAposEpigrafe.length; i++){
            if(dispositivosAposEpigrafe[i].hasAttribute('ch_norma_info')){
                selectorInsertAfterAlterado = `#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[ch_norma_info=${$(dispositivosAposEpigrafe[i]).attr('ch_norma_info')}]`;
            }
            else if(dispositivosAposEpigrafe[i].hasAttribute('ch_norma_alteracao_completa')){
                selectorInsertAfterAlterado = `#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[ch_norma_alteracao_completa=${$(dispositivosAposEpigrafe[i]).attr('ch_norma_alteracao_completa')}]`;
            }
            else{
                break;
            }
        }
    }
    return selectorInsertAfterAlterado;
}

function notificarSucessoVide(title, body){
    $('#modal_vide').modallight({
        sTitle: title,
        sContent: body,
        sType: "success",
        oButtons: [
            {
                text: "Ok", click: function () {
                    $(this).dialog('close');
                }
            }
        ]
    });
}

function notificarErroVide(title, body){
    $('#modal_vide').modallight({
        sTitle: title,
        sContent: body,
        sType: "error",
        oButtons: [
            {
                text: "Ok", click: function () {
                    $(this).dialog('close');
                }
            }
        ]
    });
}

function montarLinkNorma(norma){
    let linkNorma = `(_link_sistema_)DetalhesDeNorma.aspx?id_norma=${norma.ch_norma}`;
    if(!norma.sem_arquivo){
        if(norma.dispositivos.length > 0){
            linkNorma = `(_link_sistema_)Norma/${norma.ch_norma}/${norma.arquivo.filename}#${norma.dispositivos[0].linkname}`;
        }
        else{
            linkNorma = `(_link_sistema_)Norma/${norma.ch_norma}/${norma.arquivo.filename}`;
        }
    }
    return linkNorma;
}