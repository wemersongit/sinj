let tipoDeRelacao;
let videsDaNormaAlterada = [];
let indexVideAlterado;

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

function selecionarNormaAlteradoraExcluir(norma){
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

function selecionarNormaAlteradaExcluir(norma){
    normaAlterada = {
        ch_norma: norma.ch_norma,
        nr_norma: norma.nr_norma,
        ds_norma: montarDescricaoDaNorma(norma),
        dt_assinatura: norma.dt_assinatura,
        nm_tipo_norma: norma.nm_tipo_norma,
        sem_arquivo: norma.sem_arquivo,
        arquivo: norma.arquivo,
        dispositivos: norma.dispositivos
    };
    $('#label_norma_vide_alterada').text(normaAlterada.ds_norma);
}

function selecionarNormaAlteradaForaSistemaExcluir(videExcluir){
    normaAlterada = {
        in_norma_fora_sistema: true
    }
    $('#line_norma_fora_do_sistema').removeClass('hidden');
    $('#line_norma_dentro_do_sistema').addClass('hidden');
    $('#ch_tipo_norma_vide_fora_do_sistema').val(videExcluir.ch_tipo_norma_vide);
    $('#nm_tipo_norma_vide_fora_do_sistema').val(videExcluir.nm_tipo_norma_vide);
    $('#nr_norma_vide_fora_do_sistema').val(videExcluir.nr_norma_vide);
    $('#ch_tipo_fonte_vide_fora_do_sistema').val(videExcluir.ch_tipo_fonte_norma_vide);
    $('#nm_tipo_fonte_vide_fora_do_sistema').val(videExcluir.nm_tipo_fonte_norma_vide);
    $('#dt_publicacao_norma_vide_fora_do_sistema').val(videExcluir.dt_publicacao_fonte_norma_vide);
    $('#nr_pagina_publicacao_norma_vide_fora_do_sistema').val(videExcluir.pagina_publicacao_norma_vide);
    $('#nr_coluna_publicacao_norma_vide_fora_do_sistema').val(videExcluir.coluna_publicacao_norma_vide);
}

function selecionarArquivoNormaAlteradoraExcluir(){
    let deferredAlteradora = $.Deferred();
    $.when(deferredAlteradora).done(gComplete);

    if(normaAlteradora.sem_arquivo){
        deferredAlteradora.resolve();
    }
    else{
        selecionarArquivoDaNormaExcluir(Object.assign({}, normaAlteradora, {sufixo: 'alteradora'}), deferredAlteradora);
    }
}

function selecionarArquivosExcluir(){
    let deferredAlteradora = $.Deferred();
    let deferredAlterada = $.Deferred();
    $.when(deferredAlteradora, deferredAlterada).done(gComplete);
    if(normaAlteradora.sem_arquivo){
        deferredAlteradora.resolve();
    }
    else{
        selecionarArquivoDaNormaExcluir(Object.assign({}, normaAlteradora, {sufixo: 'alteradora'}), deferredAlteradora);
    }
    if(!normaAlterada.in_norma_fora_sistema && !normaAlterada.sem_arquivo){
        if(!IsNotNullOrEmpty(normaAlterada.dispositivos) && !isRelacaoDeAlteracaoCompleta(tipoDeRelacao.ch_tipo_relacao) && !isRelacaoQueDesfazAlteracao(tipoDeRelacao.ch_tipo_relacao) && !isLecoVide(tipoDeRelacao.ch_tipo_relacao) && !isInfoVide(tipoDeRelacao.ch_tipo_relacao)){
            gComplete();
            showMessageNormaIncompativel(normaAlterada);
            return;
        }
        selecionarArquivoDaNormaExcluir(Object.assign({}, normaAlterada, {sufixo: 'alterada'}), deferredAlterada);
    }
    else{
        deferredAlterada.resolve();
    }
}

function selecionarArquivoDaNormaExcluir(norma, deferred) {
    if (!IsNotNullOrEmpty(norma, 'arquivo.id_file')) {
        return;
    }
    const sucessoArquivo = function(data){
        exibirTextoDoArquivoSemAsAltercoesDoVide(norma, data);
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

function exibirTextoDoArquivoSemAsAltercoesDoVide(norma, arquivo) {
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
                //removerLinkAlterador(norma.dispositivos[0].linkname, norma.dispositivos[0].texto);
                destacarLinkAlterador(norma.dispositivos[0].linkname, norma.dispositivos[0].texto);
            }
            else{
                destacarLinkAlterador();
            }
        }
        else{
            if(norma.dispositivos.length > 0){
                for(let i = 0; i < norma.dispositivos.length; i++){
                    destacarAlteracaoDoDispositivo(norma.dispositivos[i].linkname, norma.dispositivos[i].texto, norma.dispositivos[i].convertido);
                }
            }
            else{
                destacarAlteracaoDoDispositivo();
            }
        }
        $('#div_cad_dispositivo_' + norma.sufixo + ' div.line_conteudo_arquivo').show();
    }
}

function destacarLinkAlterador(linkname, texto){
    if(linkname && texto){
        $.each($('#div_cad_dispositivo_alteradora div.div_conteudo_arquivo p[linkname='+linkname+'] a'), function (item, value){
            if(value.innerText == texto){
                $(value).attr('remover','linkAlterador');
                $(value).addClass('desfazer');
            }
        });
    }
    else{
        if(tipoDeRelacao.ch_tipo_relacao == '9'){
            $(`#div_cad_dispositivo_alteradora div.div_conteudo_arquivo p[ch_norma_info="${normaAlterada.ch_norma}"]`).attr('remover', 'leco').addClass('remover');
        }
    }
}

function removerLinkAlterador(){
    $(`#div_cad_dispositivo_alteradora div.div_conteudo_arquivo p[linkname] a[remover=linkAlterador]`).contents().unwrap();
    $('#div_cad_dispositivo_alteradora div.div_conteudo_arquivo p[remover=leco]').remove();
}

function destacarAlteracaoDoDispositivo(linkname, texto, convertido){
    if(isAcrescimoVide(tipoDeRelacao.ch_tipo_relacao)){
        if(texto.indexOf('\n') > -1){
            const textoSplited = texto.split('\n');
            for(let i = 0; i < textoSplited.length; i++){
                $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[linkname="${linkname}_add_${i}"]`).attr('remover', 'acrescimo').addClass('remover');
            }
        }
        else if(convertido === true){
            $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[linkname="${linkname}_add_0"]`).attr('remover', 'acrescimo').addClass('remover');
        }
        else{
            $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[linkname="${linkname}"]`).attr('remover', 'acrescimo').addClass('remover');
        }
    }
    else if(isLecoVide(tipoDeRelacao.ch_tipo_relacao)){
        if(IsNotNullOrEmpty(linkname)){
            $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[linkname="${linkname}"]`).attr('remover', 'leco-dispositivo').addClass('desfazer');
        }
        else{
            $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[ch_norma_info="${normaAlteradora.ch_norma}"]`).attr('remover', 'leco').addClass('remover');
        }
    }
    else if(isInfoVide(tipoDeRelacao.ch_tipo_relacao)){
        if(IsNotNullOrEmpty(linkname)){
            $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[linkname="${linkname}"]`).attr('remover', 'info-dispositivo').addClass('desfazer');
        }
        else{
            $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[ch_norma_info="${normaAlteradora.ch_norma}"]`).attr('remover', 'info').addClass('remover');
        }
    }
    else if(isRenumVide(tipoDeRelacao.ch_tipo_relacao)){
        const dispositivoRenumerado = getDispositivoAlterado(linkname);
        if(IsNotNullOrEmpty(dispositivoRenumerado) && !IsNotNullOrEmpty(dispositivoRenumerado.texto_antigo)){
            $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[linkname="${linkname}"]`).attr('remover', 'alteracao').addClass('remover').removeAttr('linkname');
            let $elementoDesfazer = $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[linkname="${linkname}_replaced"]`);
            $elementoDesfazer.attr('linkname', linkname);
            $elementoDesfazer.find('a[name]').first().attr('name', linkname);
            $elementoDesfazer.removeAttr('replaced_by');
            $elementoDesfazer.attr('desfazer', 'alteracao').addClass('desfazer');
        }
        else{
            $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[linkname="${linkname}"]`).attr('desfazer', 'renumerado').addClass('desfazer');
        }
    }
    else{
        if(linkname){
            if(isRelacaoQueDesfazAlteracao(tipoDeRelacao.ch_tipo_relacao)){
                let $elementoRefazer = $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[linkname="${linkname}"]`);
                $elementoRefazer.attr('refazer', 'remocao').attr('linkname', linkname.replace(/_undone$/,'')).removeAttr('undone_by').addClass('refazer');
                $elementoRefazer.find('a[name]').first().attr('name', linkname.replace(/_undone$/,''));
            }
            else{
                if(texto){
                    $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[linkname="${linkname}"]`).attr('remover', 'alteracao').addClass('remover').removeAttr('linkname');
                    let $elementoDesfazer = $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[linkname="${linkname}_replaced"]`);
                    $elementoDesfazer.attr('linkname', linkname);
                    $elementoDesfazer.find('a[name]').first().attr('name', linkname);
                    $elementoDesfazer.removeAttr('replaced_by');
                    $elementoDesfazer.attr('desfazer', 'alteracao').addClass('desfazer');

                }
                else{
                    let $elementoDesfazer = $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[linkname="${linkname}"]`);
                    $elementoDesfazer.attr('linkname', linkname.replace(/_replaced+/,''));
                    $elementoDesfazer.find('a[name]').first().attr('name', `${linkname.replace(/_replaced+/,'')}`);
                    $elementoDesfazer.removeAttr('replaced_by');
                    $elementoDesfazer.attr('desfazer', 'alteracao').addClass('desfazer');
                }
            }
        }
        else{
            $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[ch_norma_alteracao_completa=${normaAlteradora.ch_norma}]`).attr('remover', 'identificador-alteracao-completa').addClass('remover');
            const dispositivosIdentificacaoAlteracaoCompleta = $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[ch_norma_alteracao_completa]');
            if(isRelacaoDeAlteracaoCompleta(tipoDeRelacao.ch_tipo_relacao)){
                let dispositivosNormaAlterada = $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo s p[linkname]');
                if(dispositivosIdentificacaoAlteracaoCompleta.length > 1){
                    const lastChTipoRelacao = dispositivosIdentificacaoAlteracaoCompleta[dispositivosIdentificacaoAlteracaoCompleta.length - 2].getAttribute('ch_tipo_relacao');
                    if(!isRelacaoDeAlteracaoCompleta(lastChTipoRelacao) && dispositivosNormaAlterada.length > 0){
                        // $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo s')[0].remove();
                        // let lastSelector = `p[ch_norma_alteracao_completa=${lastChNorma}]`;
                        // for(let i = 0; i < dispositivosNormaAlterada.length; i++){
                        //     $(dispositivosNormaAlterada[i]).attr('desfazer', 'alteracao').addClass('desfazer');
                        //     $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo ${lastSelector}`).after(dispositivosNormaAlterada[i]);
                        //     lastSelector = `p[linkname=${dispositivosNormaAlterada[i].getAttribute('linkname')}]`;
                        // }
                        $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo s').first().attr('desfazer', 'alteracao-completa');
                        for(let i = 0; i < dispositivosNormaAlterada.length; i++){
                            $(dispositivosNormaAlterada[i]).attr('desfazer', 'alteracao-completa').addClass('desfazer');
                        }
                    }
                }
                else{
                    // $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo s')[0].remove();
                    // let lastSelector = 'h1[epigrafe]';
                    // for(let i = 0; i < dispositivosNormaAlterada.length; i++){
                    //     $(dispositivosNormaAlterada[i]).attr('desfazer', 'alteracao').addClass('desfazer');
                    //     $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo ${lastSelector}`).after(dispositivosNormaAlterada[i]);
                    //     lastSelector = `p[linkname=${dispositivosNormaAlterada[i].getAttribute('linkname')}]`;
                    // }
                    $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo s').first().attr('desfazer', 'alteracao-completa');
                    for(let i = 0; i < dispositivosNormaAlterada.length; i++){
                        $(dispositivosNormaAlterada[i]).attr('desfazer', 'alteracao-completa').addClass('desfazer');
                    }
                }
            }
            else if(isRelacaoQueDesfazAlteracao(tipoDeRelacao.ch_tipo_relacao)){
                let dispositivosNormaAlterada = $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo s p[linkname]');
                if(dispositivosIdentificacaoAlteracaoCompleta.length > 1){
                    let lastChNorma = dispositivosIdentificacaoAlteracaoCompleta[dispositivosIdentificacaoAlteracaoCompleta.length - 2].getAttribute('ch_norma_alteracao_completa');
                    const lastChTipoRelacao = dispositivosIdentificacaoAlteracaoCompleta[dispositivosIdentificacaoAlteracaoCompleta.length - 2].getAttribute('ch_tipo_relacao');
                    if(dispositivosNormaAlterada.length <= 0 && isRelacaoDeAlteracaoCompleta(lastChTipoRelacao)){
                        dispositivosNormaAlterada = $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[linkname]');
                        lastChNorma = dispositivosIdentificacaoAlteracaoCompleta[dispositivosIdentificacaoAlteracaoCompleta.length - 1].getAttribute('ch_norma_alteracao_completa');
                        // dispositivosNormaAlterada = $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[linkname]');
                        // $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[linkname]').remove();
                        // $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[ch_norma_alteracao_completa=${lastChNorma}]`).after('<s></s>');
                        // for(let i = 0; i < dispositivosNormaAlterada.length; i++){
                        //     $(dispositivosNormaAlterada[i]).attr('refazer', 'alteracao').addClass('refazer');
                        //     $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo s').append(dispositivosNormaAlterada[i]);
                        // }
                        $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[ch_norma_alteracao_completa=${lastChNorma}]`).after('<div refazer="alteracao-completa"></div>');
                        for(let i = 0; i < dispositivosNormaAlterada.length; i++){
                            $(dispositivosNormaAlterada[i]).attr('refazer', 'alteracao-completa').addClass('refazer');
                            $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo div[refazer=alteracao-completa]').append(dispositivosNormaAlterada[i]);
                        }
    
                    }
                }
            }
        }        
    }
}

function removerAlteracaoDoDispositivo(){
    if(isAcrescimoVide(tipoDeRelacao.ch_tipo_relacao)){
        $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[remover=acrescimo]').remove();
    }
    else if(isLecoVide(tipoDeRelacao.ch_tipo_relacao)){
        $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[remover=leco]').remove();
        $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[remover=leco-dispositivo] a:regex(href, \\(_link_sistema_\\)Norma/${normaAlteradora.ch_norma}.*)`).remove();
        $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[remover=leco-dispositivo]').removeAttr('remover').removeClass('desfazer');
    }
    else if(isInfoVide(tipoDeRelacao.ch_tipo_relacao)){
        $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[remover=info]').remove();
        $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[remover=info-dispositivo] a:regex(href, \\(_link_sistema_\\)Norma/${normaAlteradora.ch_norma}.*)`).remove();
        $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[remover=info-dispositivo]').removeAttr('remover').removeClass('desfazer');
    }
    else if(isRenumVide(tipoDeRelacao.ch_tipo_relacao)){
        let elementosDesfazer = $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[desfazer=alteracao]');
        if(elementosDesfazer.length > 0){
            $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[remover=alteracao]').remove();
            for(let i = 0; i < elementosDesfazer.length; i++){
                html = $(elementosDesfazer[i]).find('s').html();
                $(elementosDesfazer[i]).find('s').remove();
                $(elementosDesfazer[i]).html(html);
                $(elementosDesfazer[i]).removeAttr('desfazer').removeClass('desfazer');
            }
        }
        else{
            let $elementoRenumerado;
            let htmlRenumerado = '';
            let linkname = ''
            for(let i = 0; i < normaAlterada.dispositivos.length; i++){
                linkname = normaAlterada.dispositivos[i].linkname;
                $elementoRenumerado = $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[linkname=${linkname}]`);
                linkname = linkname.replace(/_renum+/,'');
                $elementoRenumerado.attr('linkname', linkname);
                $elementoRenumerado.find('a[name]').first().attr('name', linkname);
                $elementoRenumerado.removeAttr('desfazer').removeClass('desfazer');
                $elementoRenumerado.find(`a:regex(href, \\(_link_sistema_\\)Norma/${normaAlteradora.ch_norma}.*)`).remove();
                htmlRenumerado = $elementoRenumerado.html().replace(/&nbsp;$/,'');
                $elementoRenumerado.html(htmlRenumerado.replace(normaAlterada.dispositivos[i].texto, normaAlterada.dispositivos[i].texto_antigo));
            }
        }
    }
    else{
        let sDesfazerAlteracaoCompleta = $('s[desfazer=alteracao-completa]');
        let divRefazerAlteracaoCompleta = $('div[refazer=alteracao-completa]');
        if(sDesfazerAlteracaoCompleta.length > 0){
            $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[remover=identificador-alteracao-completa]').remove();
            $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo s[desfazer=alteracao-completa]').contents().unwrap();
            $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[desfazer=alteracao-completa]').removeAttr('desfazer').removeClass('desfazer');
        }
        else if(divRefazerAlteracaoCompleta.length > 0){
            $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[remover=identificador-alteracao-completa]').remove();
            const dispositivos = $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo div[refazer=alteracao-completa] p');

            $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo div[refazer=alteracao-completa]').after('<s dispositivos></s>');
            $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo div[refazer=alteracao-completa]').remove();
            for(let i = 0; i < dispositivos.length; i++){
                $(dispositivos[i]).removeAttr('refazer').removeClass('refazer');
                $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo s[dispositivos]').append(dispositivos[i]);
            }
            $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo s[dispositivos]').removeAttr('dispositivos');

        }
        else{
            $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[remover=alteracao]').remove();
            let elementosDesfazer = $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[desfazer=alteracao]');
            let elementosRefazer = $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[refazer=alteracao]');
            let elementosRefazerRemocao = $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[refazer=remocao]');
            let html = '';
            if(elementosDesfazer.length > 0){
                for(let i = 0; i < elementosDesfazer.length; i++){
                    html = $(elementosDesfazer[i]).find('s').html();
                    $(elementosDesfazer[i]).find('s').remove();
                    $(elementosDesfazer[i]).html(html);
                    $(elementosDesfazer[i]).removeAttr('desfazer').removeClass('desfazer');
                }
            }
            else if(elementosRefazerRemocao.length > 0){
                let videRefazer = null;
                for(j = 0; j < indexVideAlterado; j++){
                    if(isRelacaoDeAlteracaoCompleta(videsDaNormaAlterada[j].ch_tipo_relacao)){
                        videRefazer = videsDaNormaAlterada[j];
                    }
                }

                let childrensAlterado = [];
                let htmlAlterado = '';
                for(let i = 0; i < elementosRefazerRemocao.length; i++){
                    childrensAlterado = $(elementosRefazerRemocao[i]).children();
                    $(elementosRefazerRemocao[i]).find('a:regex(href, _link_sistema_)').remove();
                    htmlAlterado = $(elementosRefazerRemocao[i]).html().replace(/&nbsp;/g,'');
                    $(elementosRefazerRemocao[i]).html(`<s>${htmlAlterado}</s>`);
                    for(var j = 0; j < childrensAlterado.length; j++){
                        if($(childrensAlterado[j]).is('a') && $(childrensAlterado[j]).attr('href')?.indexOf(normaAlteradora.ch_norma) > 0){
                            continue;
                        }
                        else if($(childrensAlterado[j]).is('a') && $(childrensAlterado[j]).attr('href')?.indexOf('(_link_sistema_)') >= 0){
                            $(elementosRefazerRemocao[i]).append('&nbsp;');
                            $(elementosRefazerRemocao[i]).append(childrensAlterado[j]);
                        }
                    }
                    $(elementosRefazerRemocao[i]).attr('replaced_by', videRefazer.ch_norma_vide);
                    $(elementosRefazerRemocao[i]).removeAttr('refazer').removeClass('refazer');
                }
            }
            else if(elementosRefazer.length > 0){
                for(let i = 0; i < elementosRefazer.length; i++){
                    const linksDasAlteracoes = $(elementosRefazer[i]).find('a');
                    $.each($(elementosRefazer[i]).find('a'), function(item, value){
                        if($(value).text().indexOf('(') == 0 && $(value).text()[$(value).text().length - 1] == ')'){
                            $(value).remove();
                        }
                    });
                    const html = $(elementosRefazer[i]).html();
                    $(elementosRefazer[i]).html('<s>' + html + '</s>');
                    $.each(linksDasAlteracoes, function(item, value){
                        if($(value).text().indexOf('(') == 0 && $(value).text()[$(value).text().length - 1] == ')'){
                            $(elementosRefazer[i]).append(value);
                        }
                    });
                    $(elementosRefazer[i]).removeAttr('refazer').removeClass('refazer');
                }
            }
        }
    }
}

function salvarVideExcluir(sucessoVide, ch_vide){
    $('#vide').val(JSON.stringify({
            ch_vide: ch_vide,
            norma_alteradora: normaAlteradora,
            norma_alterada: normaAlterada
        }));
    return fnSalvar("form_vide_excluir", "", sucessoVide);
}

function salvarArquivosVideExcluir(sucessoVide, ch_vide){
    const deferredAlteradora = $.Deferred();
    const deferredAlterada = $.Deferred();
    $.when(deferredAlteradora, deferredAlterada).done(function(){
        salvarVideExcluir(sucessoVide, ch_vide);
    });
    gInicio();
    if(!normaAlteradora.sem_arquivo && IsNotNullOrEmpty(normaAlteradora, 'arquivo.id_file') && $('#div_cad_dispositivo_alteradora div.div_conteudo_arquivo p[linkname]').length > 0){
        removerLinkAlterador();
        
        $('#form_arquivo_norma_alteradora textarea[name="arquivo"]').val(window.encodeURI($('#div_cad_dispositivo_alteradora div.div_conteudo_arquivo').html()));
        $('#form_arquivo_norma_alteradora input[name=nm_arquivo]').val(normaAlteradora.arquivo.filename);
        
        var sucessoNormaAlteradora = function (data) {
            if (data.error_message != null && data.error_message != "") {
                notificar('#div_cad_vide' + id_form, data.error_message, 'error');
            }
            else if (IsNotNullOrEmpty(data, "id_file")) {
                normaAlteradora.arquivo_novo = data;
            }
        }
        $.ajaxlight({
            sFormId: 'form_arquivo_norma_alteradora',
            sUrl: './ashx/Arquivo/UploadHtml.ashx',
            sType: "POST",
            fnSuccess: sucessoNormaAlteradora,
            fnComplete: function(){
                deferredAlteradora.resolve();
            },
            fnBeforeSubmit: null,
            bAsync: true
        });
    }
    else{
        deferredAlteradora.resolve();
    }
    if(!normaAlterada.sem_arquivo && IsNotNullOrEmpty(normaAlterada, 'arquivo.id_file') && $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[linkname]').length > 0){
        removerAlteracaoDoDispositivo();
        
        $('#form_arquivo_norma_alterada textarea[name="arquivo"]').val(window.encodeURI($('#div_cad_dispositivo_alterada div.div_conteudo_arquivo').html()));
        $('#form_arquivo_norma_alterada input[name=nm_arquivo]').val(normaAlterada.arquivo.filename);

        var sucessoNormaAlterada = function (data) {
            if (data.error_message != null && data.error_message != "") {
                notificar('#div_cad_vide' + id_form, data.error_message, 'error');
            }
            else if (IsNotNullOrEmpty(data, "id_file")) {
                normaAlterada.arquivo_novo = data;
            }
        }
        $.ajaxlight({
            sFormId: 'form_arquivo_norma_alterada',
            sUrl: './ashx/Arquivo/UploadHtml.ashx',
            sType: "POST",
            fnSuccess: sucessoNormaAlterada,
            fnComplete: function(){
                deferredAlterada.resolve();
            },
            fnBeforeSubmit: null,
            bAsync: true
        });
    }
    else{
        deferredAlterada.resolve();
    }
    return false;
}
