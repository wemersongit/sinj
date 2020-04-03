let tipoDeRelacao;
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
            </div>
        </div>
    </div>`,
    html: true,
    placement: "right"
};
function buscarTipoDeRelacao(ch_tipo_relacao) {
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
function selecionarNormaEditar(norma){
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
            selecionarNormaAlteradaForaSistemaEditar();
        }
        else {
            selecionarNormaAlteradaEditar({ ch_norma: norma.ch_norma, nr_norma: norma.nr_norma, dt_assinatura: norma.dt_assinatura, nm_tipo_norma: norma.nm_tipo_norma, dispositivos: vide.alteracao_texto_vide.dispositivos_norma_vide });
        }
        selecionarNormaAlteradoraEditar({ ch_norma: vide.ch_norma_vide, nr_norma: vide.nr_norma_vide, dt_assinatura: vide.dt_assinatura_norma_vide, nm_tipo_norma: vide.nm_tipo_norma_vide, dispositivos: vide.alteracao_texto_vide.dispositivos_norma_vide_outra });
    }
    else {
        if (vide.in_norma_fora_sistema) {
            selecionarNormaAlteradaForaSistemaEditar();
        }
        else {
            selecionarNormaAlteradaEditar({ ch_norma: vide.ch_norma_vide, nr_norma: vide.nr_norma_vide, dt_assinatura: vide.dt_assinatura_norma_vide, nm_tipo_norma: vide.nm_tipo_norma_vide, dispositivos: vide.alteracao_texto_vide.dispositivos_norma_vide_outra });
        }
        selecionarNormaAlteradoraEditar({ ch_norma: norma.ch_norma, nr_norma: norma.nr_norma, dt_assinatura: norma.dt_assinatura, nm_tipo_norma: norma.nm_tipo_norma, dispositivos: vide.alteracao_texto_vide.dispositivos_norma_vide });
    }
    selecionarArquivosEditar();
}

function selecionarNormaAlteradoraEditar(norma){
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

function selecionarArquivosEditar(){
    if(!IsNotNullOrEmpty(normaAlteradora.dispositivos) && vide.ch_tipo_relacao != '9'){
        gComplete();
        showMessageNormaIncompativel(normaAlteradora);
        return;
    }
    let deferredAlteradora = $.Deferred();
    let deferredAlterada = $.Deferred();
    $.when(deferredAlteradora, deferredAlterada).done(gComplete);
    selecionarArquivoDaNormaEditar(Object.assign({}, normaAlteradora, {sufixo: 'alteradora'}), deferredAlteradora);
    if(!normaAlterada.in_norma_fora_sistema){
        if(!IsNotNullOrEmpty(normaAlterada.dispositivos) && !ehRelacaoDeAlteracaoCompleta(vide.ch_tipo_relacao) && !ehRelacaoQueDesfazAlteracaoCompleta(vide.ch_tipo_relacao) && vide.ch_tipo_relacao != '9'){
            gComplete();
            showMessageNormaIncompativel(normaAlterada);
            return;
        }
        selecionarArquivoDaNormaEditar(Object.assign({}, normaAlterada, {sufixo: 'alterada'}), deferredAlterada);
    }
    else{
        deferredAlterada.resolve();
    }
    
}

function selecionarNormaAlteradaEditar(norma){
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

function selecionarNormaAlteradaForaSistemaEditar(){
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

function selecionarArquivoDaNormaEditar(norma, deferred) {

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
    $.ajaxlight({
        sUrl: './ashx/Visualizacao/NormaDetalhes.ashx?id_norma=' + norma.ch_norma,
        sType: "GET",
        fnSuccess: sucesso,
        fnComplete: null,
        bAsync: true,
        iTimeout: 2000
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
                            if(linkname == norma.dispositivos[i].linkname || linkname == `${norma.dispositivos[i].linkname}_replaced` || linkname.indexOf('_add_') > -1){
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
        $('#div_cad_dispositivo_' + norma.sufixo + ' div.line_conteudo_arquivo').show();
    }
}

function habilitarEdicaoDoDispositivo(linkname, texto){
    switch(vide.ch_tipo_relacao){
        case '1':
            if(texto.indexOf('\n')){
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
                $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[linkname="${linkname}_replaced"]`).addClass('alterado');
                if(texto){
                    $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[linkname="${linkname}"]`).addClass('adicionado');
                    $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[linkname="${linkname}"]`).prepend(`<button type="button" class="clean" onclick="desfazerAlteracaoDoDispositivoEditar('${linkname}')"><img src="${_urlPadrao}/Imagens/ico_undo_p.png" width="14px" height="14px" /></button>`);
                }
                else{
                    $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[linkname="${linkname}_replaced"]`).prepend(`<button type="button" class="clean" onclick="desfazerAlteracaoDoDispositivoEditar('${linkname}')"><img src="${_urlPadrao}/Imagens/ico_undo_p.png" width="14px" height="14px" /></button>`);
                }

            }
            break;
    }
}

function clickAlterarDispositivoEditar(){
    let dispositivoAlterado = {}
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

                const $element = $(`<p linkname="${dispositivoAlterado.linkname}" class="adicionado"><button type="button" class="clean" onclick="desfazerAlteracaoDoDispositivoEditar('${dispositivoAlterado.linkname}')"><img src="${_urlPadrao}/Imagens/ico_undo_p.png" width="14px" height="14px" /></button><a name="${dispositivoAlterado.linkname}"></a>${textosAcrescidos[i]}<a href="(_link_sistema_)Norma/${normaAlteradora.ch_norma}/${normaAlteradora.arquivo.filename}#${normaAlteradora.dispositivos[0].linkname}">(${dispositivoAlterado.nm_linkName ? dispositivoAlterado.nm_linkName + ' ' : ''}${tipoDeRelacao.ds_texto_para_alterador}(a) pelo(a) ${normaAlteradora.ds_norma})</a></p>`);
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
            $buttonSelected.hide();
            dispositivoAlterado.linkname= $elementoAlterado.attr('linkname');
            dispositivoAlterado.nm_linkName = getNomeDoLinkName(dispositivoAlterado.linkname);
            dispositivoAlterado.ds_linkname = getDescricaoDoLinkname(dispositivoAlterado.linkname);
            dispositivoAlterado.texto = texto;
            $elementoAlterado.attr('linkname', `${dispositivoAlterado.linkname}_replaced`);
            $elementoAlterado.addClass('alterado');
            $elementoAlterado.find(`a[name=${dispositivoAlterado.linkname}]`).attr('name', `${dispositivoAlterado.linkname}_replaced`);
            $elementoAlterado.attr('replaced_by', normaAlteradora.ch_norma);
            if(dispositivoAlterado.texto){
                $elementoAlterado.after(`<p linkname="${dispositivoAlterado.linkname}" class="adicionado"><button type="button" class="clean" onclick="desfazerAlteracaoDoDispositivoEditar('${dispositivoAlterado.linkname}')"><img src="${_urlPadrao}/Imagens/ico_undo_p.png" width="14px" height="14px" /></button><a name="${dispositivoAlterado.linkname}"></a>${dispositivoAlterado.texto}<a href="(_link_sistema_)Norma/${normaAlteradora.ch_norma}/${normaAlteradora.arquivo.filename}#${normaAlteradora.dispositivos[0].linkname}">(${dispositivoAlterado.nm_linkName ? dispositivoAlterado.nm_linkName + ' ' : ''}${tipoDeRelacao.ds_texto_para_alterador}(a) pelo(a) ${normaAlteradora.ds_norma})</a></p>`);
                $elementoAlterado.html(`<s>${$elementoAlterado.html()}</s>`);
            }
            else{
                $elementoAlterado.html(`<button type="button" class="clean" onclick="desfazerAlteracaoDoDispositivoEditar('${dispositivoAlterado.linkname}')"><img src="${_urlPadrao}/Imagens/ico_undo_p.png" width="14px" height="14px" /></button><s>${$elementoAlterado.html()}</s><a href="(_link_sistema_)Norma/${normaAlteradora.ch_norma}/${normaAlteradora.arquivo.filename}#${normaAlteradora.dispositivos[0].linkname}">(${dispositivoAlterado.nm_linkName ? dispositivoAlterado.nm_linkName + ' ' : ''}${tipoDeRelacao.ds_texto_para_alterador}(a) pelo(a) ${normaAlteradora.ds_norma})</a>`);
            }
            normaAlterada.dispositivos.push(dispositivoAlterado);
        }
}

function desfazerAlteracaoDoDispositivoEditar(linkname){
    let removeIndex = -1;
    for(let i in normaAlterada.dispositivos){
        removeIndex = i;
        if(normaAlterada.dispositivos[i].linkname == linkname){
            switch(vide.ch_tipo_relacao){
                case '1':
                    $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[linkname="${linkname}"]`).remove();
                    normaAlterada.dispositivos.splice(removeIndex, 1);
                    break;
                default:
                    let $elementoAlterado = $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[linkname="${linkname}_replaced"]`);
                    const html = $elementoAlterado.find('s').html();
                    $elementoAlterado.find('s').remove();
                    $elementoAlterado.html(html);
                    if(normaAlterada.dispositivos[i].texto){
                        $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[linkname="${linkname}"]`).remove();
                    }
                    $elementoAlterado.attr('linkname', linkname);
                    $elementoAlterado.removeAttr('replaced_by');
                    $elementoAlterado.removeClass('alterado');
                    $elementoAlterado.find('button.select').show().tooltip(optionsTooltip);
                    normaAlterada.dispositivos.splice(removeIndex, 1);
                    break;
            }
            break;
        }
    }
}

function salvarVideEditar(sucessoVide){
    $('#vide').val(JSON.stringify({
            ch_vide: vide.ch_vide,
            norma_alteradora: normaAlteradora,
            norma_alterada: normaAlterada
        }));
    return fnSalvar("form_vide_editar", "", sucessoVide);
}

function salvarArquivosVideEditar(sucessoVide){
    if(normaAlteradora.arquivo && normaAlterada.arquivo){
        if(!IsNotNullOrEmpty(normaAlterada.dispositivos) || normaAlterada.dispositivos.length <= 0){
            alert('Não há dispositivos alterados. Se a intenção é removê-los use a função excluir.');
            return false;
        }
        limparFormatacao();

        if(!normaAlterada.in_norma_fora_do_sistema){
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
                    salvarVideEditar(sucessoVide);
                }
            }
            $.ajaxlight({
                sFormId: 'form_arquivo_norma_alterada',
                sUrl: './ashx/Arquivo/UploadHtml.ashx',
                sType: "POST",
                fnSuccess: sucessoNormaAlterada,
                fnComplete: null,
                fnBeforeSubmit: null,
                bAsync: true
            });
        }
        else{
            salvarVideEditar(sucessoVide);
        }
    }
    else{
        salvarVideEditar(sucessoVide);
    }
    
    return false;
}
