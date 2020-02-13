let tiposDeRelacao = [];
let tipoDeRelacaoSelecionado = [];
let normaAlteradora = {};
let normaAlterada = {};
let flagModalNorma = '';
let optionsTooltip = {
    template: `<div class="tooltip" role="tooltip">
        <div class="tooltip-arrow"></div>
            <div class="tooltip-header">
                <div class="table w-100-pc">
                    <div class="head">
                        <div class="title">
                            <h2></h2>
                        </div>
                    </div>
                </div>
            </div>
        <div class="tooltip-inner"></div>
    </div>`,
    container: '#div_tooltip_dispositivo',
    trigger: 'manual',
    title: `<div class="table w-100-pc">
        <div class="line texto-novo">
            <div class="column w-30-pc">
                <div class="cell fr">
                    <label>Texto novo:</label>
                </div>
            </div>
            <div class="column w-70-pc">
                <button type="button" class="clean copy" onclick="clickEnableCopy()" title="Selecione o texto novo no corpo da norma alteradora."><img src="${_urlPadrao}/Imagens/ico_copy.png" width="15px" height="15px" /></button>
                <button type="button" class="clean edit" onclick="clickEnableEdit()" title="Digite o texto novo."><img src="${_urlPadrao}/Imagens/ico_edit_dir.png" width="15px" height="15px" /></button>
                <textarea name="textoNovo" class="w-100-pc"></textarea>
            </div>
        </div>
        <div class="line">
            <div class="column w-100-pc text-center">
                <button type="button" class="button" onclick="clickAlterarDispositivo()"><img src="${_urlPadrao}/Imagens/ico_check.png" width="15px" height="15px" /> Ok</button>
            </div>
        </div>
    </div>`,
    html: true,
    placement: "right"
};

function selecionarAlteracaoCompleta(){
    const dispositivosNormaAlterada = $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[linkname]');
    const dispositivosAlteracaoCompleta = $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[ch_norma_alteracao_completa]');
    if($('#in_alteracao_completa').is(':checked')){
        $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[linkname]').remove();
        if(dispositivosAlteracaoCompleta.length > 0){
            const lastChNorma = dispositivosAlteracaoCompleta[dispositivosAlteracaoCompleta.length - 1].attr('ch_norma_alteracao_completa');
            $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[ch_norma_alteracao_completa=${lastChNorma}]`).after(`<p ch_norma_alteracao_completa="${normaAlteradora.ch_norma}" style="text-align:center;">
                    <a href="(_link_sistema_)Norma/${normaAlteradora.ch_norma}/${normaAlteradora.arquivo.filename}#${normaAlteradora.dispositivos[0].linkname}" >(${tipoDeRelacaoSelecionado.ds_texto_para_alterador} pelo(a) ${normaAlteradora.ds_norma})</a>
                </p>
                <s></s>`);
        }
        else{
            $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo h1[epigrafe]').after(`<p ch_norma_alteracao_completa="${normaAlteradora.ch_norma}" style="text-align:center;">
                    <a href="(_link_sistema_)Norma/${normaAlteradora.ch_norma}/${normaAlteradora.arquivo.filename}#${normaAlteradora.dispositivos[0].linkname}" >(${tipoDeRelacaoSelecionado.ds_texto_para_alterador} pelo(a) ${normaAlteradora.ds_norma})</a>
                </p>
                <s></s>`);
        }
        for(let i = 0; i < dispositivosNormaAlterada.length; i++){
            $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo s').append(dispositivosNormaAlterada[i]);
        }
        normaAlterada.in_alteracao_completa = true;
    }
    else{
        $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo s')[0].remove();
        $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[ch_norma_alteracao_completa=${normaAlteradora.ch_norma}]`).remove();
        for(let i = 0; i < dispositivosNormaAlterada.length; i++){
            $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo').append(dispositivosNormaAlterada[i]);
        }
        normaAlterada.in_alteracao_completa = false;
    }
}

function selecionarNormaForaDoSistema() {
    if ($('#in_norma_fora_do_sistema').is(':checked')) {
        $('#line_norma_fora_do_sistema').show();
        $('#line_norma_dentro_do_sistema').hide();
        normaAlterada.ch_norma.in_norma_fora_do_sistema = true;
        $("#label_norma_vide_alterada").text("");
    }
    else {
        normaAlterada.ch_norma.in_norma_fora_do_sistema = false;
        $('#line_norma_fora_do_sistema').hide();
        $('#line_norma_dentro_do_sistema').show();
    }
}

function fecharNormaForaDoSistema(){
    $('#in_norma_fora_do_sistema').prop('checked', false);
    selecionarNormaForaDoSistema();
}

function buscarTiposDeRelacao() {
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
            else if (IsNotNullOrEmpty(data.results)) {
                tiposDeRelacao = data.results;
                let selectOptions = `<option value=""></option>`;
                for(let tipo of tiposDeRelacao){
                    selectOptions += `<option value="${tipo.ch_tipo_relacao}">${tipo.nm_tipo_relacao}</option>`;
                }
                $('#selectTipoDeRelacao').html(selectOptions);
            }
        }
    };
    $.ajaxlight({
        sUrl: './ashx/Autocomplete/TipoDeRelacaoAutocomplete.ashx' + (GetParameterValue("in_acao") == "true" ? "?in_relacao_de_acao=true" : ""),
        sType: "GET",
        fnSuccess: sucesso,
        fnComplete: gComplete,
        fnBeforeSend: gInicio,
        fnError: null,
        bAsync: true
    });
}

function verificarTipoSelecionado(el) {
    if (el.value == 'ACRESCIMO') {
        $('div.labeltextoantigo').text('Após o Texto:');
        $('div.labeltextonovo').text('Inserir o Texto:');
    }
    else if (el.value == 'RENUMERAÇÃO') {
        $('div.labeltextoantigo').text('Renumerar o Texto:');
        $('div.labeltextonovo').text('Texto Renumerado:');
    }
    else {
        $('div.labeltextoantigo').text('Texto Antigo:');
        $('div.labeltextonovo').text('Texto Novo:');
    }
}

function construirControlesDinamicos() {
    buscarTiposDeRelacao();
    $('#div_autocomplete_tipo_norma_vide_fora_de_sistema').autocompletelight({
        sKeyDataName: "ch_tipo_norma",
        sValueDataName: "nm_tipo_norma",
        sInputHiddenName: "ch_tipo_norma_vide_fora_do_sistema",
        sInputName: "nm_tipo_norma_vide_fora_do_sistema",
        sAjaxUrl: "./ashx/Autocomplete/TipoDeNormaAutocomplete.ashx",
        bLinkAll: true,
        sLinkName: "a_tipo_norma_vide_fora_do_sistema"
    });
    $('#div_autocomplete_tipo_norma_modal').autocompletelight({
        sKeyDataName: "ch_tipo_norma",
        sValueDataName: "nm_tipo_norma",
        sInputHiddenName: "ch_tipo_norma_modal",
        sInputName: "nm_tipo_norma_modal",
        sAjaxUrl: "./ashx/Autocomplete/TipoDeNormaAutocomplete.ashx",
        bLinkAll: true,
        sLinkName: "a_tipo_norma_modal"
    });
    $('#div_autocomplete_tipo_fonte_vide_fora_do_sistema').autocompletelight({
        sKeyDataName: "ch_tipo_fonte",
        sValueDataName: "nm_tipo_fonte",
        sInputHiddenName: "ch_tipo_fonte_vide_fora_do_sistema",
        sInputName: "nm_tipo_fonte_vide_fora_do_sistema",
        sAjaxUrl: "./ashx/Autocomplete/TipoDeFonteAutocomplete.ashx",
        bLinkAll: true,
        sLinkName: "a_tipo_fonte_vide_fora_do_sistema"
    });
    selecionarNormaForaDoSistema();
}

function abrirModalSelecionarNorma(flag) {
    flagModalNorma = flag;
    form_modal_consultar_norma.reset();
    $('#datatable_normas_modal').html('');
    $('#modal_norma').modallight({
        sTitle:"Consultar Norma",
        sWidth:"800",
        oButtons:[]
    });
}

function pesquisarNorma() {
    $("#datatable_normas_modal").dataTablesLight({
        sAjaxUrl: './ashx/Datatable/NormaDatatable.ashx?ch_tipo_norma=' + $('#ch_tipo_norma_modal').val() + '&nr_norma=' + $('#nr_norma_modal').val() + '&dt_assinatura=' + $('#dt_assinatura_modal').val(),
        aoColumns: _columns_norma_vide,
        sIdTable: 'table_normas_modal'
    });
    return false;
}

function selecionarNormaVide(ch_norma, ds_norma, dt_assinatura, nm_tipo_norma) {
    if (IsNotNullOrEmpty(ch_norma)) {
        if (flagModalNorma == 'alteradora') {
            normaAlteradora = {
                ch_norma: ch_norma,
                ds_norma: ds_norma,
                dt_assinatura: dt_assinatura,
                nm_tipo_norma: nm_tipo_norma,
                dispositivos: []
            };
            $('#label_norma_vide_alteradora').text(ds_norma);
            resetarDispositivo('alteradora');
            selecionarArquivoDaNorma(Object.assign({}, normaAlteradora, {sufixo: 'alteradora'}));
        }
        else {
            normaAlterada = {
                ch_norma: ch_norma,
                ds_norma: ds_norma,
                dt_assinatura: dt_assinatura,
                nm_tipo_norma: nm_tipo_norma,
                dispositivos: []
            };
            $('#label_norma_vide_alterada').text(ds_norma);
            resetarDispositivo('alterada');
            $('#labelNormaForaDoSistema').hide();
            $('#labelAlteracaoCompleta').show();
            selecionarArquivoDaNorma(Object.assign({}, normaAlterada, {sufixo: 'alterada'}));
        }
        InverterAlteradaAlteradora();
        $('#modal_norma').dialog('close');
    }
}

function resetarDispositivo(nm_sufixo) {
    $('.line_dispositivo_norma_' + nm_sufixo).show();
    $('.line_ds_dispositivo_norma_' + nm_sufixo).hide();
    $('#dispositivo_norma_vide_' + nm_sufixo).val('');
    $('#ds_dispositivo_norma_' + nm_sufixo).val('');
    $('#label_dispositivo_norma_' + nm_sufixo).text('');

}

// Se a data de assinatura da norma alteradora for inferior à data de assinatura da alterada,
// deve-se inverter a seleção de alterada/alteradora.
// A unica exceção eh quando o tipo da norma for ADI.
function InverterAlteradaAlteradora() {
    if (IsNotNullOrEmpty(normaAlteradora.dt_assinatura) && IsNotNullOrEmpty(normaAlterada.dt_assinatura)) {
        if (normaAlteradora.nm_tipo_norma != "ADI") {
            if (convertStringToDateTime(normaAlteradora.dt_assinatura) < convertStringToDateTime(normaAlterada.dt_assinatura)) {
                const normaAlteradoraAux = {...normaAlteradora};
                const normaAlteradaAux = {...normaAlterada};
                normaAlteradora = {...normaAlteradaAux};
                normaAlterada = {...normaAlteradoraAux};
                
                $('#label_norma_vide_alteradora').text(normaAlteradora.ds_norma);
                $('#label_norma_vide_alterada').text(normaAlterada.ds_norma);

                $('#div_notificacao_inversao').messagelight({
                    sType: "alert",
                    sContent: "A seleção de normas foi invertida.<br/>A norma alteradora deve ser a mais recente."
                });
            }
        }
    }
}

function selecionarArquivoDaNorma(norma) {
    $('#div_cad_dispositivo_' + norma.sufixo + ' div.arquivos_norma').html('');
    limparDispositivoSelecionado(norma.sufixo);

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
        else if (IsNotNullOrEmpty(data, 'fontes')) {
            var ar_fonte = {};

            ar_fonte = data.fontes[data.fontes.length - 1].ar_fonte; //Adicionado para pegar o ultimo texto by Wemerson/Xandinho

            if (IsNotNullOrEmpty(ar_fonte, 'id_file')) {
                norma.arquivo = data.ar_fonte;
            }
            else {
                alert('Erro ao selecionar o arquivo de publicação da norma. Verifique se a norma possui arquivo em suas fontes de publicação e republicação.');
            }
        }
        else {
            gComplete();
            $('#div_notificacao_norma').messagelight({
                sTitle: "Erro",
                sContent: "Ocorreu um erro não identificado.",
                sType: "error",
                sWidth: "",
                iTime: null
            });
            return;
        }
        if(norma.sufixo == 'alteradora'){
            normaAlteradora.arquivo = norma.arquivo;
            $('.progressBar li.norma-alteradora').addClass('active');
            $('#columnNormaAlterada').removeClass('hidden');
        }
        else{
            normaAlterada.arquivo = norma.arquivo;
            $('.progressBar li.norma-alterada').addClass('active');
            $('#lineComentario').removeClass('hidden');
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

function selecionarTextoNormaAlteradora() {
    if($('#div_cad_dispositivo_alteradora div.div_conteudo_arquivo').hasClass('copy-enabled')){
        selecionarTextoCopiar();
        return;
    }
    var text = window.getSelection().toString();
    var parentNode = $(window.getSelection().baseNode).parents('p[linkname]');
    if (text != '') {
        //Impede que selecione o link da norma alteradora antes de estar com a norma alterada selecionada.
        if ($('#div_cad_dispositivo_alterada div.div_conteudo_arquivo p').length <= 0) {
            alert('Erro! Selecione também o arquivo da norma alterada.');
            return false;
        }

        var linkname = parentNode.attr('linkname');
        if ($('p[linkname="' + linkname + '"]').text().length < text.length) {
            alert('Erro! Não selecione mais de um parágrafo por vez.');
            return false;
        }
        if (IsNotNullOrEmpty(linkname)) {
            normaAlteradora.dispositivos = [{
                linkname: linkname,
                texto: text
            }];

            var a = document.createElement('a');
            const guid = Guid();
            a.setAttribute('href', `javascript: desfazerLinkAlterador('${guid}')`);
            a.setAttribute('title', 'Clique para remover a seleção do link');
            a.setAttribute('id',guid);


            window.getSelection().getRangeAt(0).surroundContents(a);

            window.getSelection().removeAllRanges();

            alert('O texto \'' + text + '\' foi selecionado.');

            $('.progressBar li.dispositivo-alterador').addClass('active');
            $('#divButtons').removeClass('hidden');
        }
    }
}

function selecionarTextoCopiar() {
    var text = window.getSelection().toString();
    window.getSelection().empty();
    if (text != '') {
        while (text.indexOf('\n\n') > -1) {
            text = text.replace(/\n\n/g, '\n');
        }
        $('#div_tooltip_dispositivo textarea').text(text);
        $('#div_tooltip_dispositivo textarea').removeClass('hidden');

        $('#div_tooltip_dispositivo button.copy').removeClass('selected');
        $('#div_tooltip_dispositivo button.copy').hide();
        $('#div_cad_dispositivo_alteradora div.div_conteudo_arquivo').removeClass('copy-enabled');

        
        $('html, body').animate({ scrollTop: $('#div_tooltip_dispositivo textarea').offset().top - 300 }, 1000);
        $('#div_tooltip_dispositivo textarea').focus();

    }
}

function desfazerLinkAlterador(idLink){
    if(normaAlterada.dispositivos > 0){
        alert('A norma alterada já possui alterações vinculadas a este dispositivo. Para continuar desfaça as alterações da mesma.');
        return;
    }
    const texto = $('#'+idLink).text();
    $('#'+idLink).text('').after(texto).remove();
}

function exibirTextoDoArquivo(norma, arquivo) {
    limparDispositivoSelecionado(norma.sufixo);
    $('#div_cad_dispositivo_' + norma.sufixo + ' div.div_conteudo_arquivo').html(window.unescape(arquivo.fileencoded));
    $('#filename_arquivo_norma_alteradora').val(arquivo.filename);
    if ($('#div_cad_dispositivo_' + norma.sufixo + ' div.div_conteudo_arquivo p').length > 0) {
        //regex para localizar o link da revogação caso haja
        var regex = new RegExp(/(revogado\(a\)|declarado\(a\) inconstitucional).*?pelo\(a\)/);
        //itera em todos os paragrafos com o atributo replaced que é o que identifica se um paragrafo sofre ou não uma alteração
        $('#div_cad_dispositivo_' + norma.sufixo + ' div.div_conteudo_arquivo p[replaced_by]').each(function (k, p) {
            //se possuir o texto de revogação
            if (regex.test($(p).text())) {
                var aLinkVide = $('a.link_vide', $(p));
                //se possuir links de vide
                if (aLinkVide.length > 0) {
                    //pega o texto do último link de vide
                    var aTexto = $(aLinkVide[aLinkVide.length - 1]).text();
                    //se o último link de vide no paragrafo for o de revogação então o mesmo não será removido no texto
                    if (regex.test(aTexto)) {
                        $(p).removeAttr('replaced_by');
                    }
                }
            }
        });
        //esconde todos os parágrafos alterados, renumerados, etc., menos os revogados
        $('p[replaced_by]').hide();
        if(norma.sufixo == 'alterada'){
            $.each($('#div_cad_dispositivo_' + norma.sufixo + ' div.div_conteudo_arquivo p'), function (key_p, value_p) {
                var linkname = $(value_p).attr('linkname');
                if (IsNotNullOrEmpty(linkname)) {
                    $(value_p).prepend('<button data-toggle="tooltip" linkname="' + linkname + '" class="select" type="button" onclick="javascript:clickButtonSelecionarDispositivo(this, \'' + norma.sufixo + '\');"></button>');
                }
            });
        }
        else if (norma.sufixo == 'alteradora') {
            $('#div_cad_dispositivo_' + norma.sufixo + ' div.div_conteudo_arquivo p[linkname]').mouseup(function () {
                selecionarTextoNormaAlteradora();
            });
        }
        $('#div_cad_dispositivo_' + norma.sufixo + ' div.line_conteudo_arquivo').show();
        $('#div_cad_dispositivo_' + norma.sufixo + ' div.line_enable_replaced').show();
    }
    $('#div_cad_dispositivo_' + norma.sufixo + ' [data-toggle="tooltip"]').tooltip(optionsTooltip);
    
}

function clickAlterarDispositivo(){
    let dispositivoAlterado = {
        texto_novo: $('#div_tooltip_dispositivo textarea[name="textoNovo"]').text()
    };
    let $buttonSelected = $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo button.selected');
    let $elementoAlterado = $buttonSelected.parent();
    $buttonSelected.tooltip('hide');
    $buttonSelected.removeClass('selected').addClass('select');
    $buttonSelected.hide();

    dispositivoAlterado.linkname= $elementoAlterado.attr('linkname');
    dispositivoAlterado.nm_linkName = getNomeDoLinkName(dispositivoAlterado.linkname);
    dispositivoAlterado.ds_linkname = getDescricaoDoLinkname(dispositivoAlterado.linkname);
    $elementoAlterado.attr('linkname', `${dispositivoAlterado.linkname}_replaced`);
    $elementoAlterado.attr('replaced_by', normaAlteradora.ch_norma);
    if(dispositivoAlterado.texto_novo){
        $elementoAlterado.after(`<p linkname="${dispositivoAlterado.linkname}"><button type="button" class="clean" onclick="desfazerAlteracaoDoDispositivo('${dispositivoAlterado.linkname}')"><img src="${_urlPadrao}/Imagens/ico_undo_p.png" width="14px" height="14px" /></button>${dispositivoAlterado.texto_novo}<a href="(_link_sistema_)Norma/${normaAlteradora.ch_norma}/${normaAlteradora.arquivo.filename}#${normaAlteradora.dispositivos[0].linkname}">(${dispositivoAlterado.nm_linkName ? dispositivoAlterado.nm_linkName + ' ' : ''}${tipoDeRelacaoSelecionado.ds_texto_para_alterador}(a) pelo(a) ${normaAlteradora.ds_norma})</a></p>`);
        $elementoAlterado.html(`<s>${$elementoAlterado.html()}</s>`);
    }
    else{
        $elementoAlterado.html(`<button type="button" class="clean" onclick="desfazerAlteracaoDoDispositivo('${dispositivoAlterado.linkname}')"><img src="${_urlPadrao}/Imagens/ico_undo_p.png" width="14px" height="14px" /></button><s>${$elementoAlterado.html()}</s><a href="(_link_sistema_)Norma/${normaAlteradora.ch_norma}/${normaAlteradora.arquivo.filename}#${normaAlteradora.dispositivos[0].linkname}">(${dispositivoAlterado.nm_linkName ? dispositivoAlterado.nm_linkName + ' ' : ''}${tipoDeRelacaoSelecionado.ds_texto_para_alterador}(a) pelo(a) ${normaAlteradora.ds_norma})</a>`);
    }
    normaAlterada.dispositivos.push(dispositivoAlterado);
    $('.progressBar li.dispositivo-alterado').addClass('active');
}

function desfazerAlteracaoDoDispositivo(linkname){
    let removeIndex = -1;
    for(let i in normaAlterada.dispositivos){
        removeIndex = i;
        if(normaAlterada.dispositivos[i].linkname == linkname){
            let $elementoAlterado = $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[linkname="${linkname}_replaced"]`);
            switch(tipoDeRelacaoSelecionado.ch_tipo_relacao){
                case '1':
                    
                    break;
                case '36':
                    
                    break;
                default:
                    const html = $elementoAlterado.find('s').html();
                    $elementoAlterado.find('s').remove();
                    $elementoAlterado.html(html);
                    if(normaAlterada.dispositivos[i].texto_novo){
                        $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[linkname="${linkname}"]`).remove();
                    }
                    $elementoAlterado.attr('linkname', linkname);
                    $elementoAlterado.removeAttr('replaced_by');
                    $elementoAlterado.find('button.select').show().tooltip(optionsTooltip);
                    break;
            }
            break;
        }
    }
    normaAlterada.dispositivos.splice(removeIndex, 1);
}

function changeTipoRelacao(el){
    normaAlteradora = {};
    normaAlterada = {};
    $('.step').addClass('hidden');
    $('.progressBar li').removeClass('active');
    $('div.div_conteudo_arquivo').html('');
    $('div.div_dispositivos_selecionados').html('');
    $('#labelNormaForaDoSistema').show();
    $('#labelAlteracaoCompleta').hide();
    if(el.value){
        const chTipoDeRelacao = $('#selectTipoDeRelacao').val();
        for(let relacao of tiposDeRelacao){
            if(relacao.ch_tipo_relacao == chTipoDeRelacao){
                tipoDeRelacaoSelecionado = relacao;
                break;
            }
        }
        switch(el.value){
            case '1':
                $('#div_tooltip_dispositivo h2').text('Informe o texto a ser acrescido.');
                break;
            case '36':
                $('#div_tooltip_dispositivo h2').text('Informe o texto renumerado.');
                break;
            default:
                $('#div_tooltip_dispositivo h2').text('Informe o texto novo, caso haja.');
                break;
        }
        $('#lineNormas').removeClass('hidden');
        $('.progressBar li.relacao').addClass('active');
    }
}

// function selecionarArquivoDispositivoCopiar(id_button, el, nm_sufixo) {
//     limparDispositivoSelecionado(nm_sufixo);
//     if (!$(el).prop('checked')) {
//         return;
//     }
//     var id_file = $(el).attr('id_file');
//     if (IsNotNullOrEmpty(id_file)) {
//         var sucesso = function (data) {

//             $('#div_cad_dispositivo_' + nm_sufixo + ' div.div_conteudo_arquivo').html(window.unescape(data.fileencoded));

//             if ($('#div_cad_dispositivo_' + nm_sufixo + ' div.div_conteudo_arquivo p').length > 0) {
//                 $.each($('#div_cad_dispositivo_' + nm_sufixo + ' div.div_conteudo_arquivo p'), function (key_p, value_p) {
//                     var pname = 'p' + key_p;
//                     $(value_p).attr('pname', pname);
//                 });
//                 $('#div_cad_dispositivo_' + nm_sufixo + ' div.div_conteudo_arquivo p[pname]').mouseup(function () {
//                     selecionarTextoCopiar(id_button, nm_sufixo);
//                 });
//                 $('#div_cad_dispositivo_' + nm_sufixo + ' div.line_conteudo_arquivo label').text('Selecione o texto novo:');
//                 $('#div_cad_vide').hide();
//                 $('#div_cad_dispositivo_' + nm_sufixo).show();
//                 $('#div_cad_dispositivo_' + nm_sufixo + ' div.line_conteudo_arquivo').show();
//             }
//         }
//         $.ajaxlight({
//             sUrl: "./ashx/Arquivo/HtmlFileEncoded.ashx?nm_base=sinj_norma&id_file=" + id_file,
//             sType: "GET",
//             fnSuccess: sucesso,
//             bAsync: true
//         });
//     }
// }

function clickButtonSelecionarDispositivo(el) {
    if(normaAlteradora.dispositivos.length <= 0){
        alert('Selecione o dispositivo alterador.');
        return;
    }
    var linkname = $(el).attr('linkname');
    if (IsNotNullOrEmpty(linkname)) {
        $('#div_cad_dispositivo_alterada div.div_dispositivos_selecionados input[name="dispositivo"][value="' + linkname + '"]').remove();
        $('#div_cad_dispositivo_alterada div.div_dispositivos_selecionados input[name="dispositivo_texto_' + linkname + '"]').remove();
        if ($(el).hasClass('select')) {
            if($('#div_tooltip_dispositivo div.tooltip').length > 0){
                return false;
            }
            $('#div_cad_dispositivo_alterada div.div_dispositivos_selecionados').prepend('<input type="hidden" name="dispositivo" value="' + linkname + '" />');
            var p = $(el.parentNode).clone();
            $(p).find('a>sup').remove();
            $(p).find('a.link_vide').remove();
            var text = $(p).text();
            $('#div_cad_dispositivo_alterada div.div_dispositivos_selecionados').prepend('<input type="hidden" name="dispositivo_texto_' + linkname + '" value="' + text + '" />');

            $(el).removeClass('select').addClass('selected');
            // $('img', el).attr('src', _urlPadrao + '/Imagens/ico_anchor_p.png');
            $(el).tooltip('show');
        }
        else {
            $(el).removeClass('selected').addClass('select');
            // $('img', el).attr('src', _urlPadrao + '/Imagens/ico_anchor_p_clear.png');
            $(el).tooltip('hide');
        }
    }
}

// function removerDispositivo(nm_sufixo) {
//     if (IsNotNullOrEmpty(nm_sufixo)) {
//         $('#dispositivo_norma_vide_' + nm_sufixo).val('');
//         $('#label_dispositivo_norma_' + nm_sufixo).text('');
//         $('#a_selecionar_dispositivo_norma_' + nm_sufixo).attr('onclick', 'javascript:abrirSelecionarDispositivo(\'' + nm_sufixo + '\');').attr('title', 'Selecionar o dispositivo da norma ' + nm_sufixo).html('<img src="' + _urlPadrao + '/Imagens/ico_edit_dir.png" alt="adicionar" width="16px" height="16px" />');
//         if (nm_sufixo == "alterada") {
//             $('#ds_dispositivo_norma_' + nm_sufixo).val('');
//             $('#texto_antigo').text('');
//             $('div.line_texto_dispositivo').hide();
//             $('div.line_ds_dispositivo_norma_alterada').hide();
//         }
//         else {
//             $('#a_texto_link').text('');
//             $('div.line_link_dispositivo').hide();
//         }
//         limparDispositivoSelecionado(nm_sufixo);
//     }
// }

// function removerDispositivoParagrafo(el) {
//     var posicao = el.getAttribute('posicao');

//     var sDispositivo = $('#dispositivo_norma_vide_alterada').val();
//     var jDispositivo = JSON.parse(sDispositivo);
//     jDispositivo.dispositivo.splice(posicao, 1);
//     jDispositivo.texto_antigo.splice(posicao, 1);
//     var nm_tipo_relacao = $('#nm_tipo_relacao').val();
//     //ToDo: se for acrescimo a descrição dos dispositivos é definida de outra forma
//     if (nm_tipo_relacao.toLowerCase() != 'acrescimo') {
//         $('#ds_dispositivo_norma_alterada').val(getDescricaoDoDispositivo(jDispositivo.dispositivo).replaceAll(';', '\n'));
//     }
//     $('#dispositivo_norma_vide_alterada').val(JSON.stringify(jDispositivo));


//     var buttons = $('div.dispositivoedit button[posicao]');
//     var iposicao = parseInt(posicao);
//     for (; iposicao < buttons.length; iposicao++) {
//         $(buttons[iposicao]).attr('posicao', iposicao - 1);
//     }
//     $(el).closest('div.dispositivoedit').remove();
// }

function limparDispositivoSelecionado(nm_sufixo) {
    $('#div_cad_dispositivo_' + nm_sufixo + ' div.div_conteudo_arquivo').html('');
    $('#div_cad_dispositivo_' + nm_sufixo + ' div.line_conteudo_arquivo').hide();
    $('#div_cad_dispositivo_' + nm_sufixo + ' div.line_enable_replaced').hide();
}

// function selecionarDispositivo(nm_sufixo) {
//     var $el = $('#div_cad_dispositivo_' + nm_sufixo + ' div.arquivos_norma input[type="checkbox"]:checked');
//     var $inputDispositivo = $('#div_cad_dispositivo_' + nm_sufixo + ' div.div_dispositivos_selecionados input[name="dispositivo"]').get().reverse();
//     if ($el.length == 1 && $inputDispositivo.length > 0) {
//         var texto = [];
//         var dispositivo = [];
//         var link = "";
//         var nm_tipo_relacao = $('#nm_tipo_relacao').val();
//         if (nm_sufixo == "alterada") {
//             for (var i = 0; i < $inputDispositivo.length; i++) {
//                 dispositivo.push($inputDispositivo[i].value);
//                 var inputDispositivoText = $('#div_cad_dispositivo_' + nm_sufixo + ' div.div_dispositivos_selecionados input[name="dispositivo_texto_' + $inputDispositivo[i].value + '"').val();
//                 if (IsNotNullOrEmpty(inputDispositivoText)) {
//                     texto.push(inputDispositivoText);
//                 }
//             }
//         }
//         else {
//             dispositivo.push($inputDispositivo[0].value);
//             var inputDispositivoText = $('#div_cad_dispositivo_' + nm_sufixo + ' div.div_dispositivos_selecionados input[name="dispositivo_texto_' + $inputDispositivo[0].value + '"').val();
//             if (IsNotNullOrEmpty(inputDispositivoText)) {
//                 link = inputDispositivoText;
//             }
//         }
//         var jDispositivo = {
//             dispositivo: dispositivo,
//             id_file: $el.attr('id_file'),
//             ch_norma: $el.attr('ch_norma'),
//             ds_norma: $el.attr('ds_norma'),
//             path: $el.attr('path'),
//             linkname: $($inputDispositivo[0]).val(),
//             texto_antigo: texto,
//             link: link,
//             filename: $el.attr('filename')
//         }
//         $('#dispositivo_norma_vide_' + nm_sufixo).val(JSON.stringify(jDispositivo));
//         $('#label_dispositivo_norma_' + nm_sufixo).text(jDispositivo.ds_norma + '#' + jDispositivo.linkname + ' ');

//         $('#a_selecionar_dispositivo_norma_' + nm_sufixo).attr('onclick', 'javascript:removerDispositivo(\'' + nm_sufixo + '\');').attr('title', 'Remover o dispositivo da norma ' + nm_sufixo).html('<img src="' + _urlPadrao + '/Imagens/ico_del_dir.png" alt="remover" width="16px" height="16px" />');

//         if (nm_sufixo == "alterada") {
//             $('div.line_texto_dispositivo').show();
//             $('div.line_texto_dispositivo').html($('<div class="column w-100-pc"></div>'));
//             if (IsNotNullOrEmpty(jDispositivo.texto_antigo)) {
//                 $('div.line_ds_dispositivo_norma_alterada').show();
//                 var labeltextoantigo = 'Texto Antigo';
//                 var labeltextonovo = 'Texto Novo';
//                 if (nm_tipo_relacao.toLowerCase() == 'acrescimo') {
//                     labeltextoantigo = 'Após o Texto';
//                     labeltextonovo = 'Inserir o Texto';
//                 } else {
//                     if (nm_tipo_relacao.toLowerCase() == 'renumeração') {
//                         labeltextoantigo = 'Renumerar o Texto';
//                         labeltextonovo = 'Texto Renumerado';
//                     }
//                     $('#ds_dispositivo_norma_' + nm_sufixo).val(getDescricaoDoDispositivo(jDispositivo.dispositivo).replaceAll(';', '\n'));
//                 }
//                 for (var i = 0; i < jDispositivo.texto_antigo.length; i++) {
//                     $('div.line_texto_dispositivo>div.column').append(
//                         '<div class="table w-100-pc dispositivoedit">' +
//                             '<div class="line">' +
//                                 '<div class="column w-30-pc">' +
//                                     '<div class="cell w-100-pc labeltextoantigo">' + labeltextoantigo + ':</div>' +
//                                 '</div>' +
//                                 '<div class="column w-70-pc">' +
//                                     '<div class="cell w-100-pc">' +
//                                         '<div id="texto_antigo" class="w-95-pc">' + jDispositivo.texto_antigo[i] + '</div>' +
//                                         '<button type="button" class="del clean" onclick="javascript:removerDispositivoParagrafo(this);" posicao="' + i + '"><img src="' + _urlPadrao + '/Imagens/ico_delete_p.png" width="12px" /></button>' +
//                                     '</div>' +
//                                 '</div>' +
//                             '</div>' +
//                             '<div class="line">' +
//                                 '<div class="column w-30-pc">' +
//                                     '<div class="cell w-100-pc labeltextonovo">' + labeltextonovo + ':</div>' +
//                                 '</div>' +
//                                 '<div class="column w-70-pc">' +
//                                     '<div class="cell w-100-pc">' +
//                                         '<button id="button_texto_antigo_' + i + '" type="button" class="clean" onclick="javascript:abrirSelecionarDispositivoCopiar(this);" title="Abre o texto da norma alteradora para copia e colar o texto novo no campo abaixo."><img src="' + _urlPadrao + '/Imagens/ico_copy.png" width="15px" /></button>' +
//                                         '<textarea name="texto_novo" class="w-95-pc" rows="5"></textarea>' +
//                                     '</div>' +
//                                 '</div>' +
//                             '</div>' +
//                         '</div>'
//                     );
//                 }
//             }
//         }
//         else {
//             $('div.line_link_dispositivo').show();
//             if (IsNotNullOrEmpty(jDispositivo.link)) {
//                 $('#a_texto_link').text(jDispositivo.link);
//             }
//         }
//         fecharSelecionarDispositivo(nm_sufixo);
//     }
// }

// function fecharSelecionarDispositivo(nm_sufixo) {
//     $('#div_cad_vide').show();
//     $('#div_cad_dispositivo_' + nm_sufixo).hide();
// }

function clickEnableCopy(){
    $('#div_tooltip_dispositivo button.copy').addClass('selected');
    $('#div_tooltip_dispositivo button.copy').attr('onclick', 'clickDisableCopy()');
    $('#div_tooltip_dispositivo button.edit').hide();
    $('#div_cad_dispositivo_alteradora div.div_conteudo_arquivo').addClass('copy-enabled');
}

function clickDisableCopy(){
    $('#div_tooltip_dispositivo button.copy').removeClass('selected');
    $('#div_tooltip_dispositivo button.copy').attr('onclick', 'clickEnableCopy()');
    $('#div_tooltip_dispositivo button.edit').show();
    $('#div_cad_dispositivo_alteradora div.div_conteudo_arquivo').removeClass('copy-enabled');
}

// function abrirSelecionarDispositivoCopiar(el_paste) {
//     var nm_sufixo = 'alteradora';
//     var id_button = el_paste.getAttribute('id');
//     var $div_conteudo_arquivo = $('#div_cad_dispositivo_' + nm_sufixo + ' div.div_conteudo_arquivo');
//     if ($div_conteudo_arquivo.text().trim() == '') {
//         abrirSelecionarDispositivo(nm_sufixo, id_button);
//     }
//     else {
//         $('#div_cad_dispositivo_' + nm_sufixo + ' div.arquivos_norma input[type="checkbox"][id_file]').attr('onchange', 'javascript:selecionarArquivoDispositivoCopiar(\'' + id_button + '\', this, \'' + nm_sufixo + '\')');
//         var $checked = $('#div_cad_dispositivo_' + nm_sufixo + ' div.arquivos_norma input[type="checkbox"][id_file]:checked');
//         if ($checked.length > 0) {
//             selecionarArquivoDispositivoCopiar(id_button, $checked[0], nm_sufixo);
//         }
//     }

// }

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
            if (parseInt(dispositivo2) < 10) {
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
        dispositivo3 = dispositivo2 + ".";
    }
    else if (dispositivo1 == "ali" && IsNotNullOrEmpty(dispositivo2)) {
        dispositivo3 = dispositivo2 + ")";
    }
    else if (dispositivo1 == "let" && IsNotNullOrEmpty(dispositivo2)) {
        dispositivo3 = dispositivo2 + ")";
    }
    return dispositivo3;
}


function clickEnableReplaced(check) {
    var enabled = $(check).is(':checked');
    if(enabled){
        $('p[replaced_by]').show();
    }
    else{
        $('p[replaced_by]').hide();
    }
}

function salvarVide(vide, sucessoVide){
    $('#vide').val(JSON.stringify(vide));

    return fnSalvar("form_vide", "", sucessoVide);
}

function salvarArquivosVide(sucessoVide){

    const $conteudoArquivoNormaAlterada = $($('#div_cad_dispositivo_alterada div.div_conteudo_arquivo').html());
    $conteudoArquivoNormaAlterada.find('p[linkname] button').remove();
    
    $('#form_arquivo_norma_alteradora textarea[name="arquivo"]').val(window.encodeURI($('#div_cad_dispositivo_alteradora div.div_conteudo_arquivo').html()));
    $('#form_arquivo_norma_alteradora nm_arquivo').val(normaAlteradora.arquivo.filename);

    $('#form_arquivo_norma_alterada textarea[name="arquivo"]').val(window.encodeURI($conteudoArquivoNormaAlterada.html()));
    $('#form_arquivo_norma_alterada nm_arquivo').val(normaAlterada.arquivo.filename);

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