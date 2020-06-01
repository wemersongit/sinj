let tipoDeRelacao;
let dsDispositivosAlteradosCopy;
let videsDaNormaAlterada = [];
let indexVideAlterado;
let optionsTooltip = {
    template: `<div class="tooltip" role="tooltip">
        <div class="tooltip-arrow"></div>
            <div class="tooltip-header">
                <div class="table w-100-pc">
                    <div class="head">
                        <div class="title">
                            <h2>Texto Novo</h2>
                        </div>
                    </div>
                </div>
            </div>
        <div class="tooltip-inner"></div>
    </div>`,
    container: '#div_tooltip_dispositivo',
    trigger: 'manual',
    title: `<div class="table w-80-pc mauto">
        <div class="line texto-novo">
            <div class="column w-100-pc">
                <textarea name="textoNovo" class="w-100-pc"></textarea>
            </div>
        </div>
        <div class="line">
            <div class="column w-100-pc text-center">
                <button type="button" class="button" onclick="clickAlterarDispositivoEditar()"><img src="${_urlPadrao}/Imagens/ico_check.png" width="15px" height="15px" /> Ok</button>
                &nbsp;
                <button type="button" class="button" onclick="clickCancelarAlterarDispositivo(this)"><img src="${_urlPadrao}/Imagens/ico_close.png" width="15px" height="15px" /> Cancelar</button>
            </div>
        </div>
    </div>`,
    html: true,
    placement: "right"
};

function buscarTipoDeRelacao(ch_tipo_relacao, deferred) {
    var sucesso = function (data) {
        if (IsNotNullOrEmpty(data)) {
            if (data.error_message != null && data.error_message != "") {
                $('#div_notificacao_vide').messagelight({
                    sTitle: "Erro",
                    sContent: data.error_message,
                    sType: "error",
                    sWidth: "",
                    iTime: null
                });
            }
            else{
                tipoDeRelacao = data;
                $('#tipoDeRelacao').val(data.nm_tipo_relacao);
            }
        }
        deferred.resolve();
    };
    $.ajaxlight({
        sUrl: './ashx/Visualizacao/TipoDeRelacaoDetalhes.ashx?ch_tipo_relacao=' + ch_tipo_relacao,
        sType: "GET",
        fnSuccess: sucesso,
        fnComplete: null,
        fnBeforeSend: null,
        fnError: null,
        bAsync: true
    });
}

function selecionarNormaAlteradoraEditar(norma){

    normaAlteradora = {
        ch_norma: norma.ch_norma,
        nr_norma: norma.nr_norma,
        ds_norma: montarDescricaoDaNorma(norma),
        dt_assinatura: norma.dt_assinatura,
        nm_tipo_norma: norma.nm_tipo_norma,
        sem_arquivo: norma.sem_arquivo,
        arquivo: norma.arquivo,
        dispositivos: norma.dispositivos
    };
    $('#label_norma_vide_alteradora').text(normaAlteradora.ds_norma);
}

function selecionarNormaAlteradaEditar(norma){
    normaAlterada = {
        ch_norma: norma.ch_norma,
        nr_norma: norma.nr_norma,
        ds_norma: montarDescricaoDaNorma(norma),
        dt_assinatura: norma.dt_assinatura,
        nm_tipo_norma: norma.nm_tipo_norma,
        sem_arquivo: norma.sem_arquivo,
        arquivo: norma.arquivo,
        dispositivos: norma.dispositivos,
        in_alteracao_completa: (ehRelacaoDeAlteracaoCompleta(tipoDeRelacao.ch_tipo_relacao) || ehRelacaoQueDesfazAlteracao(tipoDeRelacao.ch_tipo_relacao)) && norma.dispositivos.length == 0
    };
    $('#label_norma_vide_alterada').text(normaAlterada.ds_norma);
}

function selecionarNormaAlteradaForaSistemaEditar(videEditar){
    normaAlterada = {
        in_norma_fora_sistema: true
    }
    $('#line_norma_fora_do_sistema').removeClass('hidden');
    $('#line_norma_dentro_do_sistema').addClass('hidden');
    $('#ch_tipo_norma_vide_fora_do_sistema').val(videEditar.ch_tipo_norma_vide);
    $('#nm_tipo_norma_vide_fora_do_sistema').val(videEditar.nm_tipo_norma_vide);
    $('#nr_norma_vide_fora_do_sistema').val(videEditar.nr_norma_vide);
    $('#ch_tipo_fonte_vide_fora_do_sistema').val(videEditar.ch_tipo_fonte_norma_vide);
    $('#nm_tipo_fonte_vide_fora_do_sistema').val(videEditar.nm_tipo_fonte_norma_vide);
    $('#dt_publicacao_norma_vide_fora_do_sistema').val(videEditar.dt_publicacao_fonte_norma_vide);
    $('#nr_pagina_publicacao_norma_vide_fora_do_sistema').val(videEditar.pagina_publicacao_norma_vide);
    $('#nr_coluna_publicacao_norma_vide_fora_do_sistema').val(videEditar.coluna_publicacao_norma_vide);
}

function selecionarArquivoNormaAlteradoraEditar(){
    let deferredAlteradora = $.Deferred();
    $.when(deferredAlteradora).done(gComplete);
    if(!normaAlteradora.sem_arquivo){
        if(!IsNotNullOrEmpty(normaAlteradora.dispositivos) && tipoDeRelacao.ch_tipo_relacao != '9'){
            gComplete();
            showMessageNormaIncompativel(normaAlteradora);
            return;
        }
        selecionarArquivoDaNormaEditar(Object.assign({}, normaAlteradora, {sufixo: 'alteradora'}), deferredAlteradora);
    }
    else{
        deferredAlteradora.resolve();
    }
    $('#div_cad_dispositivo_alterada').show();
    $('#div_cad_dispositivo_alterada .line_conteudo_arquivo').show();
}

function selecionarArquivosEditar(){
    let deferredAlteradora = $.Deferred();
    let deferredAlterada = $.Deferred();
    $.when(deferredAlteradora, deferredAlterada).done(gComplete);
    if(!normaAlteradora.sem_arquivo){
        if(!IsNotNullOrEmpty(normaAlteradora.dispositivos) && tipoDeRelacao.ch_tipo_relacao != '9'){
            gComplete();
            showMessageNormaIncompativel(normaAlteradora);
            return;
        }
        selecionarArquivoDaNormaEditar(Object.assign({}, normaAlteradora, {sufixo: 'alteradora'}), deferredAlteradora);
    }
    else{
        deferredAlteradora.resolve();
    }
    if(!normaAlterada.in_norma_fora_sistema && !normaAlterada.sem_arquivo){
        if(!IsNotNullOrEmpty(normaAlterada.dispositivos) && !ehRelacaoDeAlteracaoCompleta(tipoDeRelacao.ch_tipo_relacao) && !ehRelacaoQueDesfazAlteracao(tipoDeRelacao.ch_tipo_relacao) && tipoDeRelacao.ch_tipo_relacao != '9'){
            gComplete();
            showMessageNormaIncompativel(normaAlterada);
            return;
        }
        selecionarArquivoDaNormaEditar(Object.assign({}, normaAlterada, {sufixo: 'alterada'}), deferredAlterada);
    }
    else{
        $('#div_cad_dispositivo_alterada').show();
        $('#div_cad_dispositivo_alterada .line_conteudo_arquivo').show();
        deferredAlterada.resolve();
    }
}

function selecionarArquivoDaNormaEditar(norma, deferred) {
    if (!IsNotNullOrEmpty(norma, 'arquivo.id_file')) {
        return;
    }
    const sucessoArquivo = function(data){
        exibirTextoDoArquivoEditar(norma, data);
        $('#div_cad_dispositivo_' + norma.sufixo).show();
    }
    $.ajaxlight({
        sUrl: "./ashx/Arquivo/HtmlFileEncoded.ashx?nm_base=sinj_norma&id_file=" + norma.arquivo.id_file,
        sType: "GET",
        fnSuccess: sucessoArquivo,
        fnBeforeSend: gInicio,
        fnComplete: function(){
            deferred.resolve();
        },
        bAsync: true
    });

}

function exibirTextoDoArquivoEditar(norma, arquivo) {
    const htmlUnescaped = window.unescape(arquivo.fileencoded);
    if(ahTextoIncompativel){
        return;
    }
    if($(htmlUnescaped).closest('h1[epigrafe]').length <= 0){
        showMessageNormaIncompativel(norma);
        ahTextoIncompativel = true;
        return;
    }

    $('#div_cad_dispositivo_' + norma.sufixo + ' div.div_conteudo_arquivo').html(window.unescape(arquivo.fileencoded));
    $('#filename_arquivo_norma_alteradora').val(arquivo.filename);
    
    if ($('#div_cad_dispositivo_' + norma.sufixo + ' div.div_conteudo_arquivo p').length > 0) {
        if(norma.sufixo == 'alteradora'){
            if(norma.dispositivos.length > 0){
                if(norma.dispositivos[0].linkname && norma.dispositivos[0].texto){
                    $.each($('#div_cad_dispositivo_alteradora div.div_conteudo_arquivo p[linkname='+norma.dispositivos[0].linkname+'] a'), function (item, value){
                        if(value.innerText == norma.dispositivos[0].texto){
                            $(value).attr('href','javascript:alert("Não é possível editar o link alterador.")');
                            $(value).addClass('link-alterador');
                        }
                    });
                }
            }
            $('#div_cad_dispositivo_alteradora div.div_conteudo_arquivo p[linkname]').mouseup(function () {
                selecionarTextoNormaAlteradoraEditar();
            });
        }
        else{
            if(norma.dispositivos.length > 0){
                let showButton = true;
                $.each($('#div_cad_dispositivo_alterada div.div_conteudo_arquivo p'), function (index, value) {
                    var linkname = $(value).attr('linkname');
                    if (IsNotNullOrEmpty(linkname)) {
                        showButton = true;
                        for(let i = 0; i < norma.dispositivos.length; i++){
                            if(linkname == norma.dispositivos[i].linkname || linkname == `${norma.dispositivos[i].linkname}_replaced` || linkname.indexOf('_add') > -1){
                                showButton = false;
                                break;
                            }
                        }
                        if(showButton){
                            $(value).prepend('<button data-toggle="tooltip" linkname="' + linkname + '" class="select" type="button" onclick="javascript:clickButtonSelecionarDispositivo(this, \'' + norma.sufixo + '\');"></button>');
                        }
                        else{
                            $(value).find('s').first().prepend('<button data-toggle="tooltip" linkname="' + linkname + '" class="select" type="button" onclick="javascript:clickButtonSelecionarDispositivo(this, \'' + norma.sufixo + '\');" style="display:none;"></button>');
                        }
                    }
                });
                for(let i = 0; i < norma.dispositivos.length; i++){
                    habilitarEdicaoDoDispositivo(norma.dispositivos[i].linkname, norma.dispositivos[i].texto);
                }
                
            }
            $('#div_cad_dispositivo_' + norma.sufixo + ' [data-toggle="tooltip"]').tooltip(optionsTooltip);
        }
        $('#div_cad_dispositivo_' + norma.sufixo + ' div.div_conteudo_arquivo img').filter(function() {
            if($(this).width() > $('#div_vide fieldset').width()/2){
                $(this).addClass('limit-width');
                console.log(this);
            }
        });
        
        $('#div_cad_dispositivo_' + norma.sufixo + ' div.line_conteudo_arquivo').show();
    }
}

function habilitarEdicaoDoDispositivo(linkname, texto){
    switch(tipoDeRelacao.ch_tipo_relacao){
        case '1':
            if(texto.indexOf('\n') > -1){
                const textoSplited = texto.split('\n');
                for(let i = 0; i < textoSplited.length; i++){
                    $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[linkname="${linkname}_add_${i}"]`).addClass('adicionado');
                }
            }
            else{
                $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[linkname="${linkname}"]`).addClass('adicionado');
            }
            break;
        case '9':
            break;
        default:
            if(linkname){
                if(ehRelacaoQueDesfazAlteracao(tipoDeRelacao.ch_tipo_relacao)){
                    $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[linkname="${linkname}"]`).addClass('desfeito');
                }
                else{
                    if(texto){
                        $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[linkname="${linkname}_replaced"]`).addClass('alterado');
                        $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[linkname="${linkname}"]`).addClass('adicionado');
                    }
                    else{
                        $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[linkname="${linkname}"]`).addClass('alterado');
                    }
                }
                $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[linkname="${linkname}"]`).prepend(`<button type="button" class="clean" onclick="desfazerAlteracaoDoDispositivoEditar('${linkname}')"><img src="${_urlPadrao}/Imagens/ico_undo_p.png" width="14px" height="14px" /></button>`);
                

            }
            break;
    }
}

function clickAlterarDispositivoEditar(){
    let dispositivoAlterado = {}
    let linkNormaAlteradora = `(_link_sistema_)Norma/${normaAlteradora.ch_norma}`;
    if(!normaAlteradora.sem_arquivo){
        if(normaAlteradora.dispositivos.length > 0){
            linkNormaAlteradora += `/${normaAlteradora.arquivo.filename}#${normaAlteradora.dispositivos[0].linkname}`;
        }
        else{
            linkNormaAlteradora += `/${normaAlteradora.arquivo.filename}`;
        }
    }
    const texto = $('#div_tooltip_dispositivo textarea[name=textoNovo]').val();
    let $buttonSelected = $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo button.selected');
    $buttonSelected.tooltip('hide');
    $buttonSelected.removeClass('selected').addClass('select');
    if(vide.ch_tipo_relacao == '1'){
        if(!texto){
            return;
        }
        const textosAcrescidos = texto.split('\n');
        let $nextElement = $buttonSelected.parent();
        for(let i = 0; i < textosAcrescidos.length; i++){
            dispositivoAlterado.linkname = generateLinkNameCaput(textosAcrescidos[i]) + '_add';
            dispositivoAlterado.nm_linkName = getNomeDoLinkName(dispositivoAlterado.linkname);
            dispositivoAlterado.ds_linkname = getDescricaoDoLinkname(dispositivoAlterado.linkname);
            dispositivoAlterado.texto = textosAcrescidos[i];
            normaAlterada.dispositivos.push(Object.assign({}, dispositivoAlterado));

            const $element = $(`<p linkname="${dispositivoAlterado.linkname}" class="adicionado"><button type="button" class="clean" onclick="desfazerAlteracaoDoDispositivoEditar('${dispositivoAlterado.linkname}')"><img src="${_urlPadrao}/Imagens/ico_undo_p.png" width="14px" height="14px" /></button><a name="${dispositivoAlterado.linkname}"></a>${textosAcrescidos[i]}&nbsp;<a href="${linkNormaAlteradora}">(${dispositivoAlterado.nm_linkName ? dispositivoAlterado.nm_linkName + ' ' : ''}${tipoDeRelacao.ds_texto_para_alterador}(a) pelo(a) ${normaAlteradora.ds_norma})</a></p>`);
            $nextElement.after($element);
            $nextElement = $element;
        }
    }
    else{
        if(IsNotNullOrEmpty(texto) && texto.indexOf('\n') > -1){
            alert('Esse tipo de relação não permite que a alteração possua mais de um dispositivo por vez.');
            return;
        }
        let $elementoAlterado = $buttonSelected.parent();
        const linkname = $elementoAlterado.attr('linkname');
        let htmlAlterado = $elementoAlterado.find('s').html();
        if(ehRelacaoQueDesfazAlteracao(tipoDeRelacao.ch_tipo_relacao)){
            if(!IsNotNullOrEmpty(htmlAlterado) || linkname.indexOf('_replaced') < 0){
                alert(`A relação ${tipoDeRelacao.nm_tipo_relacao} pode ser aplicada somente a um dispositivo que sofreu alteração.`);
                return;
            }
            $buttonSelected.hide();
            $elementoAlterado.find('s').html('').after(htmlAlterado).remove();
            dispositivoAlterado.linkname = `${linkname}_undone`;
            dispositivoAlterado.nm_linkName = getNomeDoLinkName(linkname.replace(/_replaced|_undone/g,''));
            dispositivoAlterado.ds_linkname = getDescricaoDoLinkname(linkname.replace(/_replaced|_undone/g,''));
            $elementoAlterado.attr('linkname', dispositivoAlterado.linkname);
            $elementoAlterado.find(`a[name=${linkname}]`).attr('name', dispositivoAlterado.linkname);
            $elementoAlterado.removeAttr('replaced_by');
            $elementoAlterado.attr('undone_by', normaAlteradora.ch_norma);
            $elementoAlterado.addClass('desfeito');
            $elementoAlterado.html(`<button type="button" class="clean" onclick="desfazerAlteracaoDoDispositivoEditar('${dispositivoAlterado.linkname}')"><img src="${_urlPadrao}/Imagens/ico_undo_p.png" width="14px" height="14px" /></button>${$elementoAlterado.html()}&nbsp;<a href="${linkNormaAlteradora}">(${dispositivoAlterado.nm_linkName ? dispositivoAlterado.nm_linkName + ' ' : ''}${tipoDeRelacao.ds_texto_para_alterador} pelo(a) ${normaAlteradora.ds_norma})</a>`);
        }
        else{
            const countReplaceds =  $(`#div_cad_dispositivo_alterada p:regex(linkname, ${linkname}.*_replaced)`).length;
            if(!IsNotNullOrEmpty(texto)){
                $buttonSelected.hide();
                dispositivoAlterado.linkname = linkname;
                //adiciona os sufixos replaced no dispositivo alterado para evitar duplicidade
                for(var i = 0; i < countReplaceds; i++){
                    dispositivoAlterado.linkname += '_replaced';
                }
                dispositivoAlterado.linkname += '_replaced';
                dispositivoAlterado.nm_linkName = getNomeDoLinkName(linkname.replace(/_replaced|_undone|_add/g,''));
                dispositivoAlterado.ds_linkname = getDescricaoDoLinkname(linkname.replace(/_replaced|_undone|_add/g,''));
                $elementoAlterado.attr('linkname', dispositivoAlterado.linkname);
                $elementoAlterado.find(`a[name=${linkname}]`).attr('name', dispositivoAlterado.linkname);
                $elementoAlterado.removeAttr('undone_by');
                $elementoAlterado.attr('replaced_by', normaAlteradora.ch_norma);
                $elementoAlterado.addClass('alterado');
                $elementoAlterado.html(`<button type="button" class="clean" onclick="desfazerAlteracaoDoDispositivoEditar('${dispositivoAlterado.linkname}')"><img src="${_urlPadrao}/Imagens/ico_undo_p.png" width="14px" height="14px" /></button><s>${$elementoAlterado.html()}</s>&nbsp;<a href="${linkNormaAlteradora}">(${dispositivoAlterado.nm_linkName ? dispositivoAlterado.nm_linkName + ' ' : ''}${tipoDeRelacao.ds_texto_para_alterador} pelo(a) ${normaAlteradora.ds_norma})</a>`);
            }
            else{
                $buttonSelected.hide();
                let linknameDispAlterado = linkname;
                //adiciona os sufixos replaced no dispositivo alterado para evitar duplicidade
                for(var i = 0; i < countReplaceds; i++){
                    linknameDispAlterado += '_replaced';
                }
                linknameDispAlterado += '_replaced'
                dispositivoAlterado.linkname = linkname;
                dispositivoAlterado.nm_linkName = getNomeDoLinkName(linkname.replace(/_replaced|_undone|_add/g,''));
                dispositivoAlterado.ds_linkname = getDescricaoDoLinkname(linkname.replace(/_replaced|_undone|_add/g,''));
                dispositivoAlterado.texto = texto;
                $elementoAlterado.attr('linkname', linknameDispAlterado);
                $elementoAlterado.find(`a[name=${linkname}]`).attr('name', linknameDispAlterado);
                $elementoAlterado.attr('replaced_by', normaAlteradora.ch_norma);
                $elementoAlterado.addClass('alterado');
                $elementoAlterado.after(`<p linkname="${dispositivoAlterado.linkname}" class="adicionado"><button type="button" class="clean" onclick="desfazerAlteracaoDoDispositivoEditar('${dispositivoAlterado.linkname}')"><img src="${_urlPadrao}/Imagens/ico_undo_p.png" width="14px" height="14px" /></button><a name="${dispositivoAlterado.linkname}"></a>${dispositivoAlterado.texto}&nbsp;<a href="${linkNormaAlteradora}">(${dispositivoAlterado.nm_linkName ? dispositivoAlterado.nm_linkName + ' ' : ''}${tipoDeRelacao.ds_texto_para_alterador} pelo(a) ${normaAlteradora.ds_norma})</a></p>`);
                $elementoAlterado.html(`<s>${$elementoAlterado.html()}</s>`);

            }
        }
        normaAlterada.dispositivos.push(dispositivoAlterado);
    }
    if(IsNotNullOrEmpty(dispositivoAlterado.ds_linkname)){
        adicionarDescricaoDispositivosAlterados(dispositivoAlterado.ds_linkname);
    }
}

function desfazerAlteracaoDoDispositivoEditar(linkname){
    let removeIndex = -1;
    let dispositivoDesfeito = {};
    for(let i in normaAlterada.dispositivos){
        removeIndex = i;
        if(normaAlterada.dispositivos[i].linkname == linkname){
            switch(vide.ch_tipo_relacao){
                case '1':
                    $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[linkname="${linkname}"]`).remove();
                    dispositivoDesfeito = normaAlterada.dispositivos.splice(removeIndex, 1);
                    break;
                default:
                    if(ehRelacaoQueDesfazAlteracao(tipoDeRelacao.ch_tipo_relacao)){
                        let videRefazer = null;
                        for(j = 0; j < indexVideAlterado; j++){
                            if(ehRelacaoDeAlteracaoCompleta(videsDaNormaAlterada[j].ch_tipo_relacao)){
                                videRefazer = videsDaNormaAlterada[j];
                            }
                        }
                        let $elementoAlterado = $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[linkname="${linkname}"]`);
                        const childrensAlterado = $elementoAlterado.children();
                        $elementoAlterado.find('a:regex(href, _link_sistema_)').remove();
                        $elementoAlterado.find('button').remove();
                        const htmlAlterado = $elementoAlterado.html().replace(/&nbsp;/g,'');
                        $elementoAlterado.html(`<s>${htmlAlterado}</s>`);
                        
                        for(var j = 1; j < childrensAlterado.length; j++){
                            if($(childrensAlterado[j]).is('a') && $(childrensAlterado[j]).attr('href')?.indexOf(normaAlteradora.ch_norma) > 0){
                                continue;
                            }
                            if($(childrensAlterado[j]).is('button.select')){
                                $elementoAlterado.prepend(childrensAlterado[j]);
                            }
                            else if($(childrensAlterado[j]).is('a') && $(childrensAlterado[j]).attr('href')?.indexOf('(_link_sistema_)') >= 0){
                                $elementoAlterado.append('&nbsp;');
                                $elementoAlterado.append(childrensAlterado[j]);
                            }
                        }
                        $elementoAlterado.attr('linkname', linkname.replace(/_undone$/,''));
                        $elementoAlterado.removeAttr('undone_by');
                        $elementoAlterado.attr('replaced_by', videRefazer.ch_norma_vide);
                        $elementoAlterado.removeClass('desfeito');
                        $elementoAlterado.find('button.select').show().tooltip(optionsTooltip);
                        dispositivoDesfeito = normaAlterada.dispositivos.splice(removeIndex, 1);
                    }
                    else{
                        if(normaAlterada.dispositivos[i].texto){
                            $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[linkname="${linkname}"]`).remove();
                        }
                        else{
                            linkname = linkname.replace(/_replaced$/,'');
                        }
                        let $elementoAlterado = $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[linkname="${linkname}_replaced"]`);
                        const html = $elementoAlterado.find('s').html();
                        $elementoAlterado.find('s').remove();
                        $elementoAlterado.html(html);
                        $elementoAlterado.attr('linkname', linkname);
                        $elementoAlterado.removeAttr('replaced_by');
                        $elementoAlterado.removeClass('alterado');
                        $elementoAlterado.find('button.select').show().tooltip(optionsTooltip);
                        dispositivoDesfeito = normaAlterada.dispositivos.splice(removeIndex, 1);
                    }
                    break;
            }
            if(IsNotNullOrEmpty(dispositivoDesfeito)){
                removerDescricaoDispositivosAlterados(dispositivoDesfeito[0].ds_linkname);
            }
            break;
        }
    }
}

function salvarVideEditar(sucessoVide, ch_vide){
    $('#vide').val(JSON.stringify({
            ch_vide: ch_vide,
            norma_alteradora: normaAlteradora,
            norma_alterada: normaAlterada
        }));
    return fnSalvar("form_vide_editar", "", sucessoVide);
}

function salvarArquivosVideEditar(sucessoVide, ch_vide){
    if(IsNotNullOrEmpty(normaAlterada, 'arquivo.id_file') && dsDispositivosAlteradosCopy == $('textarea[name=ds_dispositivos_alterados]').val()){
        if(!IsNotNullOrEmpty(normaAlterada.dispositivos) || normaAlterada.dispositivos.length <= 0){
            alert('Não há dispositivos alterados. Se a intenção é removê-los use a função excluir.');
            return false;
        }
    }
    limparFormatacao();
    if(!normaAlterada.in_norma_fora_do_sistema && !normaAlterada.sem_arquivo && !normaAlterada.in_alteracao_completa){
        let htmlNormaAlterada = "";
        const $conteudoArquivoNormaAlterada = $($('#div_cad_dispositivo_alterada div.div_conteudo_arquivo').html());
        for(let i = 0; i < $conteudoArquivoNormaAlterada.length; i++){
            if($conteudoArquivoNormaAlterada[i].outerHTML){
                htmlNormaAlterada += $conteudoArquivoNormaAlterada[i].outerHTML;
            }
        }
        $('#form_arquivo_norma_alterada textarea[name="arquivo"]').val(window.encodeURI(htmlNormaAlterada));
        $('#form_arquivo_norma_alterada input[name=nm_arquivo]').val(normaAlterada.arquivo.filename);

        var sucessoNormaAlterada = function (data) {
            if (data.error_message != null && data.error_message != "") {
                notificar('#div_cad_vide' + id_form, data.error_message, 'error');
            }
            else if (IsNotNullOrEmpty(data, "id_file")) {
                normaAlterada.arquivo_novo = data;
                salvarVideEditar(sucessoVide, ch_vide);
            }
        }
        $.ajaxlight({
            sFormId: 'form_arquivo_norma_alterada',
            sUrl: './ashx/Arquivo/UploadHtml.ashx',
            sType: "POST",
            fnSuccess: sucessoNormaAlterada,
            fnComplete: null,
            fnBeforeSubmit: gInicio,
            bAsync: true
        });
    }
    else{
        salvarVideEditar(sucessoVide, ch_vide);
    }
    
    return false;
}
