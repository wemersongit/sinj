let normaAlteradora = {};
let normaAlterada = {};
let ahTextoIncompativel = false;
let vide = {};

function showMessaNormaIncompativel(norma){
    $('#div_notificacao_vide').messagelight({
        sTitle: "Erro",
        sContent: `O texto da norma ${norma.ds_norma} não é compatível o módulo de edição de vides.`,
        sType: "error",
        sWidth: "",
        iTime: null
    });
}

function selecionarNorma(norma){
    if(IsNotNullOrEmpty(vide.caput_norma_vide, 'caput')){
        vide.alteracao_texto_vide.dispositivos_norma_vide = [];
        for(var i = 0; i < vide.caput_norma_vide.caput.length; i++){
            vide.alteracao_texto_vide.dispositivos_norma_vide.push({
                linkname: vide.caput_norma_vide.caput[i],
                texto: (IsNotNullOrEmpty(vide.caput_norma_vide, 'texto_novo['+i+']') ? vide.caput_norma_vide.texto_novo[i] : IsNotNullOrEmpty(vide.caput_norma_vide.link) ? vide.caput_norma_vide.link : '')
            });
        }
    }
    if(IsNotNullOrEmpty(vide.caput_norma_vide_outra, 'caput')){
        vide.alteracao_texto_vide.dispositivos_norma_vide_outra = [];
        for(var i = 0; i < vide.caput_norma_vide_outra.caput.length; i++){
            vide.alteracao_texto_vide.dispositivos_norma_vide_outra.push({
                    linkname: vide.caput_norma_vide_outra.caput[i],
                    texto: (IsNotNullOrEmpty(vide.caput_norma_vide_outra, 'texto_novo['+i+']') ? vide.caput_norma_vide_outra.texto_novo[i] : IsNotNullOrEmpty(vide.caput_norma_vide_outra.link) ? vide.caput_norma_vide_outra.link : '')
                });
        }
    }
    if (vide.in_norma_afetada) {
        if (vide.in_norma_fora_sistema) {
            selecionarNormaAlteradaForaSistema();
        }
        else {
            selecionarNormaAlterada({ ch_norma: norma.ch_norma, nr_norma: norma.nr_norma, dt_assinatura: norma.dt_assinatura, nm_tipo_norma: norma.nm_tipo_norma, dispositivos: vide.alteracao_texto_vide.dispositivos_norma_vide });
        }
        selecionarNormaAlteradora({ ch_norma: vide.ch_norma_vide, nr_norma: vide.nr_norma_vide, dt_assinatura: vide.dt_assinatura_norma_vide, nm_tipo_norma: vide.nm_tipo_norma_vide, dispositivos: vide.alteracao_texto_vide.dispositivos_norma_vide_outra });
    }
    else {
        if (vide.in_norma_fora_sistema) {
            selecionarNormaAlteradaForaSistema();
        }
        else {
            selecionarNormaAlterada({ ch_norma: vide.ch_norma_vide, nr_norma: vide.nr_norma_vide, dt_assinatura: vide.dt_assinatura_norma_vide, nm_tipo_norma: vide.nm_tipo_norma_vide, dispositivos: vide.alteracao_texto_vide.dispositivos_norma_vide_outra });
        }
        selecionarNormaAlteradora({ ch_norma: norma.ch_norma, nr_norma: norma.nr_norma, dt_assinatura: norma.dt_assinatura, nm_tipo_norma: norma.nm_tipo_norma, dispositivos: vide.alteracao_texto_vide.dispositivos_norma_vide });
    }
    selecionarArquivos();
}

function selecionarNormaAlteradora(norma){
    normaAlteradora = {
        ch_norma: norma.ch_norma,
        nr_norma: norma.nr_norma,
        ds_norma: montarDescricaoDaNorma(norma),
        dt_assinatura: norma.dt_assinatura,
        nm_tipo_norma: norma.nm_tipo_norma,
        dispositivos: norma.dispositivos
    };
    $('#label_norma_vide_alteradora').text(normaAlteradora.ds_norma);
}

function selecionarArquivos(){
    if(!IsNotNullOrEmpty(normaAlteradora.dispositivos) && vide.ch_tipo_relacao != '9'){
        gComplete();
        showMessaNormaIncompativel(normaAlteradora);
        return;
    }
    let deferredAlteradora = $.Deferred();
    let deferredAlterada = $.Deferred();
    $.when(deferredAlteradora, deferredAlterada).done(gComplete);
    selecionarArquivoDaNorma(Object.assign({}, normaAlteradora, {sufixo: 'alteradora'}), deferredAlteradora);
    if(!normaAlterada.in_norma_fora_sistema){
        if(!IsNotNullOrEmpty(normaAlterada.dispositivos) && !ehRelacaoDeAlteracaoCompleta(vide.ch_tipo_relacao) && !ehRelacaoQueDesfazAlteracaoCompleta(vide.ch_tipo_relacao) && vide.ch_tipo_relacao != '9'){
            gComplete();
            showMessaNormaIncompativel(normaAlterada);
            return;
        }
        selecionarArquivoDaNorma(Object.assign({}, normaAlterada, {sufixo: 'alterada'}), deferredAlterada);
    }
    else{
        deferredAlterada.resolve();
    }
    
}

function selecionarNormaAlterada(norma){
    normaAlterada = {
        ch_norma: norma.ch_norma,
        nr_norma: norma.nr_norma,
        ds_norma: montarDescricaoDaNorma(norma),
        dt_assinatura: norma.dt_assinatura,
        nm_tipo_norma: norma.nm_tipo_norma,
        dispositivos: norma.dispositivos
    };
    $('#label_norma_vide_alterada').text(normaAlterada.ds_norma);
}

function selecionarNormaAlteradaForaSistema(){
    normaAlterada = {
        in_norma_fora_sistema: true
    }
    $('#line_norma_fora_do_sistema').removeClass('hidden');
    $('#line_norma_dentro_do_sistema').addClass('hidden');
    $('#ch_tipo_norma_vide_fora_do_sistema').val(vide.ch_tipo_norma_vide);
    $('#nm_tipo_norma_vide_fora_do_sistema').val(vide.nm_tipo_norma_vide);
    $('#nr_norma_vide_fora_do_sistema').val(vide.nr_norma_vide);
    $('#ch_tipo_fonte_vide_fora_do_sistema').val(vide.ch_tipo_fonte_norma_vide);
    $('#nm_tipo_fonte_vide_fora_do_sistema').val(vide.nm_tipo_fonte_norma_vide);
    $('#dt_publicacao_norma_vide_fora_do_sistema').val(vide.dt_publicacao_fonte_norma_vide);
    $('#nr_pagina_publicacao_norma_vide_fora_do_sistema').val(vide.pagina_publicacao_norma_vide);
    $('#nr_coluna_publicacao_norma_vide_fora_do_sistema').val(vide.coluna_publicacao_norma_vide);
}

function selecionarArquivoDaNorma(norma, deferred) {

    var sucesso = function (data) {
        if (IsNotNullOrEmpty(data, 'error_message')) {
            $('#div_notificacao_norma').messagelight({
                sTitle: "Erro",
                sContent: data.error_message,
                sType: "error",
                sWidth: "",
                iTime: null
            });
            return false;
        }
        if (IsNotNullOrEmpty(data, 'ar_atualizado.id_file')) {
            norma.arquivo = data.ar_atualizado;
        }
        else {
            return;
        }
        if(norma.sufixo == 'alteradora'){
            normaAlteradora.arquivo = norma.arquivo;
        }
        else{
            normaAlterada.arquivo = norma.arquivo;
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
    $.ajaxlight({
        sUrl: './ashx/Visualizacao/NormaDetalhes.ashx?id_norma=' + norma.ch_norma,
        sType: "GET",
        fnSuccess: sucesso,
        fnComplete: null,
        bAsync: true,
        iTimeout: 2000
    });

}

function exibirTextoDoArquivoSemAsAltercoesDoVide(norma, arquivo) {
    const htmlUnescaped = window.unescape(arquivo.fileencoded);
    if(ahTextoIncompativel){
        return;
    }
    if($(htmlUnescaped).closest('h1[epigrafe]').length <= 0){
        showMessaNormaIncompativel(norma);
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
                    destacarAlteracaoDoDispositivo(norma.dispositivos[i].linkname, norma.dispositivos[i].texto);
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
        if(vide.ch_tipo_relacao == '9'){
            $(`#div_cad_dispositivo_alteradora div.div_conteudo_arquivo p[ch_norma_info="${normaAlterada.ch_norma}"]`).attr('remover', 'leco').addClass('remover');
        }
    }
}

function removerLinkAlterador(){
    $(`#div_cad_dispositivo_alteradora div.div_conteudo_arquivo p[linkname] a[remover=linkAlterador]`).contents().unwrap();
    $('#div_cad_dispositivo_alteradora div.div_conteudo_arquivo p[remover=leco]').remove();

}

function destacarAlteracaoDoDispositivo(linkname, texto){
    switch(vide.ch_tipo_relacao){
        case '1':
            if(texto.indexOf('\n')){
                const textoSplited = texto.split('\n');
                for(let i = 0; i < textoSplited.length; i++){
                    $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[linkname="${linkname}_add_${i}"]`).attr('remover', 'acrescimo').addClass('remover');
                }
            }
            else{
                $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[linkname="${linkname}"]`).attr('remover', 'acrescimo').addClass('remover');
            }
            break;
        case '9':
            $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[ch_norma_info="${normaAlteradora.ch_norma}"]`).attr('remover', 'leco').addClass('remover');
            break;
        default:
            if(linkname){
                let $elementoDesfazer = $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[linkname="${linkname}_replaced"]`);
                if(texto){
                    $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[linkname="${linkname}"]`).attr('remover', 'alteracao').addClass('remover').removeAttr('linkname');
                }
                $elementoDesfazer.attr('linkname', linkname);
                $elementoDesfazer.find(`a[name=${linkname}_replaced]`).attr('name', `${linkname}`);
                $elementoDesfazer.removeAttr('replaced_by');
                $elementoDesfazer.attr('desfazer', 'alteracao').addClass('desfazer');
            }
            else{
                $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[ch_norma_alteracao_completa=${normaAlteradora.ch_norma}]`).attr('remover', 'identificador-alteracao-completa').addClass('remover');
                const dispositivosIdentificacaoAlteracaoCompleta = $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[ch_norma_alteracao_completa]');
                if(ehRelacaoDeAlteracaoCompleta(vide.ch_tipo_relacao)){
                    let dispositivosNormaAlterada = $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo s p[linkname]');
                    if(dispositivosIdentificacaoAlteracaoCompleta.length > 1){
                        const lastChTipoRelacao = dispositivosIdentificacaoAlteracaoCompleta[dispositivosIdentificacaoAlteracaoCompleta.length - 2].getAttribute('ch_tipo_relacao');
                        if(!ehRelacaoDeAlteracaoCompleta(lastChTipoRelacao) && dispositivosNormaAlterada.length > 0){
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
                else if(ehRelacaoQueDesfazAlteracaoCompleta(vide.ch_tipo_relacao)){
                    let dispositivosNormaAlterada = $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo s p[linkname]');
                    if(dispositivosIdentificacaoAlteracaoCompleta.length > 1){
                        let lastChNorma = dispositivosIdentificacaoAlteracaoCompleta[dispositivosIdentificacaoAlteracaoCompleta.length - 2].getAttribute('ch_norma_alteracao_completa');
                        const lastChTipoRelacao = dispositivosIdentificacaoAlteracaoCompleta[dispositivosIdentificacaoAlteracaoCompleta.length - 2].getAttribute('ch_tipo_relacao');
                        if(dispositivosNormaAlterada.length <= 0 && ehRelacaoDeAlteracaoCompleta(lastChTipoRelacao)){
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
            break;
    }
}

function removerAlteracaoDoDispositivo(){
    switch(vide.ch_tipo_relacao){
        case '1':
            $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[remover=acrescimo]').remove();
            break;
        case '9':
            $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[remover=leco]').remove();
            break;
        default:
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
                if(elementosDesfazer.length > 0){
                    for(let i = 0; i < elementosDesfazer.length; i++){
                        const html = $(elementosDesfazer[i]).find('s').html();
                        $(elementosDesfazer[i]).find('s').remove();
                        $(elementosDesfazer[i]).html(html);
                        $(elementosDesfazer[i]).removeAttr('desfazer').removeClass('desfazer');
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
            break;
    }
}

function salvarVide(sucessoVide){
    $('#vide').val(JSON.stringify({
            ch_vide: vide.ch_vide,
            norma_alteradora: normaAlteradora,
            norma_alterada: normaAlterada
        }));
    return fnSalvar("form_vide_excluir", "", sucessoVide);
}

function salvarArquivosVide(sucessoVide){
    if(normaAlteradora.arquivo && normaAlterada.arquivo){
        removerLinkAlterador();
        removerAlteracaoDoDispositivo();
        $('#form_arquivo_norma_alteradora textarea[name="arquivo"]').val(window.encodeURI($('#div_cad_dispositivo_alteradora div.div_conteudo_arquivo').html()));
        $('#form_arquivo_norma_alteradora input[name=nm_arquivo]').val(normaAlteradora.arquivo.filename);

        const deferredAlteradora = $.Deferred();
        const deferredAlterada = $.Deferred();
        $.when(deferredAlteradora, deferredAlterada).done(function(){
            salvarVide(sucessoVide);
        });
        
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
            fnBeforeSubmit: gInicio,
            bAsync: true
        });
        if(!normaAlterada.in_norma_fora_do_sistema){
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
    }
    else{
        salvarVide(sucessoVide);
    }
    
    return false;
}
