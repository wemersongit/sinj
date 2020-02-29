let normaAlteradora = {};
let normaAlterada = {};
let vide = {};

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
    selecionarArquivoDaNorma(Object.assign({}, normaAlteradora, {sufixo: 'alteradora'}));
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
    selecionarArquivoDaNorma(Object.assign({}, normaAlterada, {sufixo: 'alterada'}));
    
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

function selecionarArquivoDaNorma(norma) {

    var sucesso = function (data) {
        if (IsNotNullOrEmpty(data, 'error_message')) {
            gComplete();
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
            gComplete();
            $('#div_notificacao_norma').messagelight({
                sTitle: "Erro",
                sContent: `A norma ${norma.sufixo} não possui texto atualizado.`,
                sType: "error",
                sWidth: "",
                iTime: null
            });
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
            gComplete();
        }
        $.ajaxlight({
            sUrl: "./ashx/Arquivo/HtmlFileEncoded.ashx?nm_base=sinj_norma&id_file=" + norma.arquivo.id_file,
            sType: "GET",
            fnSuccess: sucessoArquivo,
            fnBeforeSend: gInicio,
            bAsync: true
        });
    }
    $.ajaxlight({
        sUrl: './ashx/Visualizacao/NormaDetalhes.ashx?id_norma=' + norma.ch_norma,
        sType: "GET",
        fnSuccess: sucesso,
        /*fnBeforeSend: gInicio,*/
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
            $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[linkname="${linkname}"]`).remove();
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
    
    $('#form_arquivo_norma_alteradora textarea[name="arquivo"]').val(window.encodeURI($('#div_cad_dispositivo_alteradora div.div_conteudo_arquivo').html()));
    $('#form_arquivo_norma_alteradora input[name=nm_arquivo]').val(normaAlteradora.arquivo.filename);

    let completed = true;
    let complete = function(){
        if(completed){
            gComplete();
        }else{
            completed = true;
        }
    }
    var sucessoNormaAlteradora = function (data) {
        if (data.error_message != null && data.error_message != "") {
            notificar('#div_cad_vide' + id_form, data.error_message, 'error');
        }
        else if (IsNotNullOrEmpty(data, "id_file")) {
            normaAlteradora.arquivo_novo = data;
            if(completed){
                salvarVide(sucessoVide);
            }
        }
    }
    $.ajaxlight({
        sFormId: 'form_arquivo_norma_alteradora',
        sUrl: './ashx/Arquivo/UploadHtml.ashx',
        sType: "POST",
        fnSuccess: sucessoNormaAlteradora,
        fnComplete: complete,
        fnBeforeSubmit: gInicio,
        bAsync: true
    });
    if(!normaAlterada.in_norma_fora_do_sistema){
        completed = false;
        $('#form_arquivo_norma_alterada textarea[name="arquivo"]').val(window.encodeURI($('#div_cad_dispositivo_alterada div.div_conteudo_arquivo').html()));
        $('#form_arquivo_norma_alterada input[name=nm_arquivo]').val(normaAlterada.arquivo.filename);

        var sucessoNormaAlterada = function (data) {
            if (data.error_message != null && data.error_message != "") {
                notificar('#div_cad_vide' + id_form, data.error_message, 'error');
            }
            else if (IsNotNullOrEmpty(data, "id_file")) {
                normaAlterada.arquivo_novo = data;
                if(completed){
                    salvarVide(sucessoVide);
                }
            }
        }
        $.ajaxlight({
            sFormId: 'form_arquivo_norma_alterada',
            sUrl: './ashx/Arquivo/UploadHtml.ashx',
            sType: "POST",
            fnSuccess: sucessoNormaAlterada,
            fnComplete: complete,
            fnBeforeSubmit: null,
            bAsync: true
        });
    }
    return false;
}