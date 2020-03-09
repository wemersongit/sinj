let normaAlteradora = {};
let normaAlterada = {};
let vide = {};

function selecionarNorma(norma){
    if(vide.caput_norma_vide){
        vide.alteracao_texto_vide.dispositivos_norma_vide = [];
        for(var i = 0; i < vide.caput_norma_vide.caput.length; i++){
            vide.alteracao_texto_vide.dispositivos_norma_vide.push({
                linkname: vide.caput_norma_vide.caput[i],
                texto: (IsNotNullOrEmpty(vide.caput_norma_vide, 'texto_novo['+i+']') ? vide.caput_norma_vide.texto_novo[i] : IsNotNullOrEmpty(vide.caput_norma_vide.link) ? vide.caput_norma_vide.link : '')
            });
        }
    }
    if(vide.caput_norma_vide_outra){
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
    let deferredAlteradora = $.Deferred();
    let deferredAlterada = $.Deferred();
    $.when(deferredAlteradora, deferredAlterada).done(gComplete);
    selecionarArquivoDaNorma(Object.assign({}, normaAlteradora, {sufixo: 'alteradora'}), deferredAlteradora);
    if(!normaAlterada.in_norma_fora_sistema){
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
            exibirTextoDoArquivo(norma, data);
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

function exibirTextoDoArquivo(norma, arquivo) {
    $('#div_cad_dispositivo_' + norma.sufixo + ' div.div_conteudo_arquivo').html(window.unescape(arquivo.fileencoded));
    $('#filename_arquivo_norma_alteradora').val(arquivo.filename);
    
    if ($('#div_cad_dispositivo_' + norma.sufixo + ' div.div_conteudo_arquivo p').length > 0) {
        if(norma.sufixo == 'alteradora'){
            removerLinkAlterador(norma.dispositivos[0].linkname, norma.dispositivos[0].texto);
        }
        else{
            if(norma.dispositivos.length > 0){
                for(let i = 0; i < norma.dispositivos.length; i++){
                    removerAlteracaoDoDispositivo(norma.dispositivos[i].linkname, norma.dispositivos[i].texto);
                }
            }
            else{
                removerAlteracaoDoDispositivo();
            }
        }
        $('#div_cad_dispositivo_' + norma.sufixo + ' div.line_conteudo_arquivo').show();
    }
}

function removerLinkAlterador(linkname, texto){
    $.each($('#div_cad_dispositivo_alteradora div.div_conteudo_arquivo p[linkname='+linkname+'] a'), function (item, value){
        if(value.innerText == texto){
            $(value).text('').after(texto).remove();
        }
    });
}

function removerAlteracaoDoDispositivo(linkname, texto){
    switch(vide.ch_tipo_relacao){
        case '1':
            if(texto.indexOf('\n')){
                const textoSplited = texto.split('\n');
                for(let i = 0; i < textoSplited.length; i++){
                    $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[linkname="${linkname}_add_${i}"]`).remove();
                }
            }
            else{
                $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[linkname="${linkname}"]`).remove();
            }
            break;
        case '9':
            $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[ch_norma_info="${normaAlteradora.ch_norma}"]`).remove();
            break
        case '36':
            
            break;
        default:
            let $elementoAlterado = $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[linkname="${linkname}_replaced"]`);
            const html = $elementoAlterado.find('s').html();
            $elementoAlterado.find('s').remove();
            $elementoAlterado.html(html);
            if(texto){
                $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[linkname="${linkname}"]`).remove();
            }
            $elementoAlterado.attr('linkname', linkname);
            $elementoAlterado.removeAttr('replaced_by');
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
