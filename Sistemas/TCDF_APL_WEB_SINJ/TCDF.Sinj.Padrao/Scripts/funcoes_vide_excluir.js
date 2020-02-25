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
        dispositivos: []
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
        dispositivos: []
    };
    $('#label_norma_vide_alterada').text(normaAlterada.ds_norma);
    selecionarArquivoDaNorma(Object.assign({}, normaAlterada, {sufixo: 'alterada'}));
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
            if(vide.in_norma_afetada){
                desfazerLinkAlterador(vide.alteracao_texto_vide.dispositivos_norma_vide_outra[0].linkname, vide.alteracao_texto_vide.dispositivos_norma_vide_outra[0].texto);
            }
            else{
                desfazerLinkAlterador(vide.alteracao_texto_vide.dispositivos_norma_vide[0].linkname, vide.alteracao_texto_vide.dispositivos_norma_vide[0].texto);
            }
        }
        else{
            if(vide.in_norma_afetada){
                for(let i = 0; i < vide.alteracao_texto_vide.dispositivos_norma_vide.length; i++){
                    desfazerAlteracaoDoDispositivo(vide.alteracao_texto_vide.dispositivos_norma_vide[i].linkname, vide.alteracao_texto_vide.dispositivos_norma_vide[i].texto);
                }
            }
            else{
                for(let i = 0; i < vide.alteracao_texto_vide.dispositivos_norma_vide_outra.length; i++){
                    desfazerAlteracaoDoDispositivo(vide.alteracao_texto_vide.dispositivos_norma_vide_outra[i].linkname, vide.alteracao_texto_vide.dispositivos_norma_vide_outra[i].texto);
                }
            }
        }
        $('#div_cad_dispositivo_' + norma.sufixo + ' div.line_conteudo_arquivo').show();
    }
}

function desfazerLinkAlterador(linkname, texto){
    $.each($('#div_cad_dispositivo_alteradora div.div_conteudo_arquivo p[linkname='+linkname+'] a'), function (item, value){
        if(value.innerText == texto){
            $(value).text('').after(texto).remove();
        }
    });
}

function desfazerAlteracaoDoDispositivo(linkname, texto){
    switch(vide.ch_tipo_relacao){
        case '1':
            $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[linkname="${linkname}"]`).remove();
            break;
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

function salvarVide(vide, sucessoVide){
    $('#vide').val(JSON.stringify(vide));

    return fnSalvar("form_vide", "", sucessoVide);
}

function salvarArquivosVide(sucessoVide){
    
    let htmlNormaAlteradora = "";
    const $conteudoArquivoNormaAlteradora = $($('#div_cad_dispositivo_alteradora div.div_conteudo_arquivo').html());
    for(let i in $conteudoArquivoNormaAlteradora){
        if($conteudoArquivoNormaAlteradora[i].outerHTML){
            if($conteudoArquivoNormaAlteradora[i].getAttribute('linkname') == normaAlteradora.dispositivos[0].linkname){
                const $link = $($conteudoArquivoNormaAlteradora[i]).find('a[href]');
                for(let a in $link){
                    if($link[a].innerText == normaAlteradora.dispositivos[0].texto){
                        $link[a].setAttribute('href', `(_link_sistema_)Norma/${normaAlterada.ch_norma}/${normaAlterada.arquivo.filename}.html#${normaAlterada.dispositivos[0].linkname}`);
                        break;
                    }
                }
            }
            htmlNormaAlteradora += $conteudoArquivoNormaAlteradora[i].outerHTML;
        }
    }
    $('#form_arquivo_norma_alteradora textarea[name="arquivo"]').val(window.encodeURI(htmlNormaAlteradora));
    $('#form_arquivo_norma_alteradora input[name=nm_arquivo]').val(normaAlteradora.arquivo.filename);

    let htmlNormaAlterada = "";
    const $conteudoArquivoNormaAlterada = $($('#div_cad_dispositivo_alterada div.div_conteudo_arquivo').html());
    for(let i in $conteudoArquivoNormaAlterada){
        if($conteudoArquivoNormaAlterada[i].outerHTML){
            $($conteudoArquivoNormaAlterada[i]).find('button').remove();
            htmlNormaAlterada += $conteudoArquivoNormaAlterada[i].outerHTML;
        }
    }
    $('#form_arquivo_norma_alterada textarea[name="arquivo"]').val(window.encodeURI(htmlNormaAlterada));
    $('#form_arquivo_norma_alterada input[name=nm_arquivo]').val(normaAlterada.arquivo.filename);

    $('#super_loading').hide();

    let completed = false;

    var sucessoNormaAlteradora = function (data) {
        if (data.error_message != null && data.error_message != "") {
            notificar('#div_cad_vide' + id_form, data.error_message, 'error');
        }
        else if (IsNotNullOrEmpty(data, "id_file")) {
            normaAlteradora.arquivo_novo = data;
            if(completed){
                salvarVide({
                    relacao: tipoDeRelacaoSelecionado,
                    norma_alteradora: normaAlteradora,
                    norma_alterada: normaAlterada
                }, sucessoVide);
            }
        }
    }
    var sucessoNormaAlterada = function (data) {
        if (data.error_message != null && data.error_message != "") {
            notificar('#div_cad_vide' + id_form, data.error_message, 'error');
        }
        else if (IsNotNullOrEmpty(data, "id_file")) {
            normaAlterada.arquivo_novo = data;
            if(completed){
                salvarVide({
                    relacao: tipoDeRelacaoSelecionado,
                    norma_alteradora: normaAlteradora,
                    norma_alterada: normaAlterada
                }, sucessoVide);
            }
        }
    }
    let complete = function(){
        if(completed){
            gComplete();
        }else{
            completed = true;
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
    $.ajaxlight({
        sFormId: 'form_arquivo_norma_alterada',
        sUrl: './ashx/Arquivo/UploadHtml.ashx',
        sType: "POST",
        fnSuccess: sucessoNormaAlterada,
        fnComplete: complete,
        fnBeforeSubmit: null,
        bAsync: true
    });
    return false;
}