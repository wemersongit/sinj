function ExpandirTabelaDeNormas() {
    if ($('#a_selecionar_normas').hasClass('opened')) {
        $('#a_selecionar_normas').removeClass('opened');
        $('#a_selecionar_normas').addClass('closed');
        $('#a_selecionar_normas img').prop('src', _urlPadrao + '/Imagens/ico_arrow_down.png');
        $('#div_datatable_normas').hide();
    }
    else {
        $('#a_selecionar_normas').removeClass('closed');
        $('#a_selecionar_normas').addClass('opened');
        $('#div_datatable_normas').show();
        $('#a_selecionar_normas img').prop('src', _urlPadrao+'/Imagens/ico_arrow_up.png');
        $("#div_datatable_normas").dataTablesLight({
            sAjaxUrl: './ashx/Datatable/NormaDatatable.ashx',
            aoColumns: _columns_norma_vide,
            sIdTable: 'datatable_normas',
            bFilter: true
        });
    }
}

function RemoverNormaVide() {
    $('.label_norma_vide_alterada').text("");
    $('#ch_norma_vide').val("");
    $('#a_removerNormaVide').hide();
}

function SelecionarNormaForaDoSistema() {
    if ($('#in_norma_fora_do_sistema').is(':checked')) {
        $('#line_norma_fora_do_sistema').show();
        $('#line_norma_dentro_do_sistema').hide();
        $("#ch_norma_alterada").attr("obrigatorio", "");
        $("#ch_norma_alterada").val("");
        $("#label_norma_vide_alterada").text("");
    }
    else {
        $('#line_norma_fora_do_sistema').hide();
        $('#line_norma_dentro_do_sistema').show();
        $("#ch_norma_alterada").attr("obrigatorio", "sim");
    }
}

function AutocompleteTipoDeRelacao(in_acao) {
    $('#div_autocomplete_tipo_relacao').autocompletelight({
        sKeyDataName: "ch_tipo_relacao",
        sValueDataName: "nm_tipo_relacao",
        sInputHiddenName: "ch_tipo_relacao",
        sInputName: "nm_tipo_relacao",
        sAjaxUrl: "./ashx/Autocomplete/TipoDeRelacaoAutocomplete.ashx" + (in_acao == "true" ? "?in_relacao_de_acao=true" : ""),
        bLinkAll: true,
        sLinkName: "a_tipo_relacao",
        dOthersHidden: [
                        { campo_app: "ds_texto_para_alterador", campo_base: "ds_texto_para_alterador" },
                        { campo_app: "ds_texto_para_alterado", campo_base: "ds_texto_para_alterado" }
                    ]
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

function ConstruirControlesDinamicos() {
    AutocompleteTipoDeRelacao(GetParameterValue("in_acao"));
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
    SelecionarNormaForaDoSistema();
}

function AbrirModalSelecionarNorma(b_norma_alteradora) {
    $('#b_norma_alteradora').val(b_norma_alteradora);
    $('#modal_norma').modallight({
        sTitle:"Consultar Norma",
        sWidth:"800",
        oButtons:[]
    });
}

function PesquisarNorma() {
    var b_norma_alteradora = $('#b_norma_alteradora').val();
    $("#datatable_normas_modal").dataTablesLight({
        sAjaxUrl: './ashx/Datatable/NormaDatatable.ashx?ch_tipo_norma=' + $('#ch_tipo_norma_modal').val() + '&nr_norma=' + $('#nr_norma_modal').val() + '&dt_assinatura=' + $('#dt_assinatura_modal').val(),
        aoColumns: _columns_norma_vide,
        sIdTable: 'table_normas_modal'
    });
}

function SelecionarNormaVide(ch_norma, ds_norma, dt_assinatura, nm_tipo_norma, st_acao) {
    var b_norma_alteradora = $('#b_norma_alteradora').val();
    if (IsNotNullOrEmpty(ch_norma)) {
        if (b_norma_alteradora == "true") {
            AutocompleteTipoDeRelacao(st_acao ? "true" : "false");
            $('#ch_norma_alteradora').val(ch_norma);
            $('#dt_assinatura_alteradora').val(dt_assinatura);
            $('#nm_tipo_norma_alteradora').val(nm_tipo_norma);
            $('#label_norma_vide_alteradora').text(ds_norma);
            resetCaput('alteradora');
        }
        else {
            $('#ch_norma_alterada').val(ch_norma);
            $('#dt_assinatura_alterada').val(dt_assinatura);
            $('#nm_tipo_norma_alterada').val(nm_tipo_norma);
            $('#label_norma_vide_alterada').text(ds_norma);
            resetCaput('alterada');
        }
        InverterAlteradaAlteradora();
        $('#modal_norma').dialog('close');
    }
}

function resetCaput(nm_sufixo) {
    $('.line_caput_norma_' + nm_sufixo).show();
    $('.line_ds_caput_norma_' + nm_sufixo).hide();
    $('#caput_norma_vide_' + nm_sufixo).val('');
    $('#ds_caput_norma_' + nm_sufixo).val('');
    $('#label_caput_norma_' + nm_sufixo).text('');

}

// Se a data de assinatura da norma alteradora for inferior à data de assinatura da alterada,
// deve-se inverter a seleção de alterada/alteradora.
// A unica exceção eh quando o tipo da norma for ADI.
function InverterAlteradaAlteradora() {
    if (IsNotNullOrEmpty($('#dt_assinatura_alteradora').val()) && IsNotNullOrEmpty($('#dt_assinatura_alterada').val())) {
        if ($('#nm_tipo_norma_alteradora').val() != "ADI") {
            if (convertStringToDateTime($('#dt_assinatura_alteradora').val()) < convertStringToDateTime($('#dt_assinatura_alterada').val())) {
                var aux_dt_assinatura = $('#dt_assinatura_alteradora').val();
                var aux_nm_tipo_norma = $('#nm_tipo_norma_alteradora').val();
                var aux_ch_norma = $('#ch_norma_alteradora').val();
                var aux_label = $('#label_norma_vide_alteradora').text();
                $('#dt_assinatura_alteradora').val($('#dt_assinatura_alterada').val());
                $('#nm_tipo_norma_alteradora').val($('#nm_tipo_norma_alterada').val());
                $('#ch_norma_alteradora').val($('#ch_norma_alterada').val());
                $('#label_norma_vide_alteradora').text($('#label_norma_vide_alterada').text());

                $('#dt_assinatura_alterada').val(aux_dt_assinatura);
                $('#nm_tipo_norma_alterada').val(aux_nm_tipo_norma);
                $('#ch_norma_alterada').val(aux_ch_norma);
                $('#label_norma_vide_alterada').text(aux_label);

                $('#div_notificacao_inversao').messagelight({
                    sType: "alert",
                    sContent: "A seleção de normas foi invertida.<br/>A norma alteradora deve ser a mais recente."
                });
            }
        }
    }
}

function abrirSelecionarCaput(nm_sufixo, id_button) {
    $('#div_cad_caput_' + nm_sufixo + ' div.arquivos_norma').html('');
    limparCaputSelecionado(nm_sufixo);
    var ch_norma = $('#ch_norma_' + nm_sufixo).val();
    if (IsNotNullOrEmpty(ch_norma)) {
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
                $('#div_cad_caput_' + nm_sufixo + ' div.arquivos_norma').append(data.ar_atualizado.filename + ' <input type="checkbox" id_file="' + data.ar_atualizado.id_file + '" filename="' + data.ar_atualizado.filename + '" ch_norma="' + data.ch_norma + '" path="atlz" ds_norma="' + getIdentificacaoDeNorma(data) + '" onchange="javascript:' + ( id_button ? "selecionarArquivoCaputCopiar(\'"+ id_button +"\'," : "selecionarArquivoCaput(") + 'this, \'' + nm_sufixo + '\')" style="vertical-align:middle;"><br/>');
                $('#div_cad_vide').hide();
                $('#div_cad_caput_' + nm_sufixo).show();
                $('#div_cad_caput_' + nm_sufixo + ' div.arquivos_norma input[type="checkbox"]').prop('checked', 'checked').change();
            }
            else if (IsNotNullOrEmpty(data, 'fontes')) {
                var ar_fonte = {};
                var iFonte = 0;
                var nm_tipo_publicacao = '';
                for (var i = 0; i < data.fontes.length; i++) {
                    nm_tipo_publicacao = data.fontes[i].nm_tipo_publicacao.toLowerCase();
                    if (nm_tipo_publicacao == 'publicação' || nm_tipo_publicacao == 'pub') {
                        ar_fonte = data.fontes[i].ar_fonte;
                        iFonte = i;
                        continue;
                    }
                    if (nm_tipo_publicacao == 'republicação' || nm_tipo_publicacao == 'rep' || nm_tipo_publicacao == 'retificação' || nm_tipo_publicacao == 'ret') {
                        ar_fonte = data.fontes[i].ar_fonte;
                        iFonte = i;
                        break;
                    }
                    //$('#arquivos_norma').append('<button class="clean" type="button" id_file="' + data.fontes[i].ar_fonte.id_file + '" filename="' + data.fontes[i].ar_fonte.filename + '" ch_norma="' + data.ch_norma + '" path="fontes/' + i + '" onclick="javascript:selecionarArquivoCaput(this)">' + data.fontes[i].ar_fonte.filename + ' <img src="' + _urlPadrao + '/Imagens/ico_check_p.png" /></button><br/>');
                }
                if (IsNotNullOrEmpty(ar_fonte, 'id_file')) {
                    $('#div_cad_caput_' + nm_sufixo + ' div.arquivos_norma').append(ar_fonte.filename + ' <input type="checkbox" id_file="' + ar_fonte.id_file + '" filename="' + ar_fonte.filename + '" ch_norma="' + data.ch_norma + '" path="fontes/' + iFonte + '" ds_norma="' + getIdentificacaoDeNorma(data) + '" onchange="javascript:' + (id_button ? "selecionarArquivoCaputCopiar(\'" + id_button + "\', " : "selecionarArquivoCaput(") + 'this, \'' + nm_sufixo + '\')" style="vertical-align:middle;"><br/>');
                    $('#div_cad_vide').hide();
                    $('#div_cad_caput_' + nm_sufixo).show();
                    $('#div_cad_caput_' + nm_sufixo + ' div.arquivos_norma input[type="checkbox"]').prop('checked', 'checked').change();
                }
                else {
                    alert('Erro ao selecionar o arquivo de publicação da norma.');
                }
            }
            else {
                $('#div_notificacao_norma').messagelight({
                    sTitle: "Erro",
                    sContent: "Ocorreu um erro não identificado.",
                    sType: "error",
                    sWidth: "",
                    iTime: null
                });
            }
        }
        $.ajaxlight({
            sUrl: './ashx/Visualizacao/NormaDetalhes.ashx?id_norma=' + ch_norma,
            sType: "GET",
            fnSuccess: sucesso,
            fnComplete: gComplete,
            fnBeforeSend: gInicio,
            bAsync: true,
            iTimeout: 40000
        });
    }
}

function selecionarTexto(nm_sufixo, _a) {
    var text = '';
    var parentNode;
    if (IsNotNullOrEmpty(_a)) {
        text = _a.innerText;
        parentNode = _a.parentNode;
    }
    else{
        text = window.getSelection().toString();
        parentNode = window.getSelection().baseNode.parentNode;
    }
    if (text != '') {
        var linkname = parentNode.getAttribute('linkname');
        if ($('p[linkname="' + linkname + '"]').text().length < text.length) {
            alert('Erro! Não selecione mais de um parágrafo por vez.');
            return false;
        }
        if (IsNotNullOrEmpty(linkname)) {
            $('#div_cad_caput_' + nm_sufixo + ' div.div_caputs_selecionados input[name="caput"][value="' + linkname + '"]').remove();
            $('#div_cad_caput_' + nm_sufixo + ' div.div_caputs_selecionados input[name="caput_texto_' + linkname + '"]').remove();
            $('#div_cad_caput_' + nm_sufixo + ' div.div_caputs_selecionados').prepend('<input type="hidden" name="caput" value="' + linkname + '" />');
            $('#div_cad_caput_' + nm_sufixo + ' div.div_caputs_selecionados').prepend('<input type="hidden" name="caput_texto_' + linkname + '" value="' + text + '" />');

            selecionarCaput(nm_sufixo);

        }
    }
}

function selecionarTextoCopiar(id_button, nm_sufixo) {
    var text = window.getSelection().toString();
    if (text != '') {
        $('#' + id_button).parent().find('textarea').text(text);
        fecharSelecionarCaput(nm_sufixo);
    }
}

function selecionarArquivoCaput(el, nm_sufixo) {
    limparCaputSelecionado(nm_sufixo);
    if(!$(el).prop('checked')){
        return;
    }
    var id_file = $(el).attr('id_file');
    if(IsNotNullOrEmpty(id_file)){
        var sucesso = function (data) {

            $('#div_cad_caput_' + nm_sufixo + ' div.div_conteudo_arquivo').html(window.unescape(data.fileencoded));

            if ($('#div_cad_caput_' + nm_sufixo + ' div.div_conteudo_arquivo p').length > 0) {
                //regex para localizar o link da revogação caso haja
                var regex = new RegExp(/revogado.*?pelo\(a\)/);
                //itera em todos os paragrafos com o atributo replaced que é o que identifica se um paragrafo sofre ou não uma alteração
                $('#div_cad_caput_' + nm_sufixo + ' div.div_conteudo_arquivo p[replaced_by]').each(function (k, p) {
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
                //remove todos os parágrafos alterados, renumerados, etc., menos os revogados
                $('p[replaced_by]').remove();

                $.each($('#div_cad_caput_' + nm_sufixo + ' div.div_conteudo_arquivo p'), function (key_p, value_p) {
                    var linkname = $(value_p).attr('linkname');
                    if (IsNotNullOrEmpty(linkname)) {
                        if (nm_sufixo == "alterada") {
                            $(value_p).prepend('<button title="Selecionar caput inteiro" linkname="' + linkname + '" class="buttoncaput clean" type="button" onclick="javascript:clickButtonCaput(this, \'' + nm_sufixo + '\');"><img src="' + _urlPadrao + '/Imagens/ico_anchor_p_clear.png" alt="caput" /></a>');
                        }
                        else {
                            $(value_p).prepend('<button title="No texto alterador deve ser selecionada somente parte do paragrafo, ou seja, o texto que servirá como link para o caput alterado." linkname="' + linkname + '" class="buttoncaput clean" type="button" onclick="javascript:alert(this.title);" ><img src="' + _urlPadrao + '/Imagens/ico_anchor_p_clear.png" alt="caput" /></a>');
                        }
                    }
                });
                if (nm_sufixo == "alteradora") {
                    $('#div_cad_caput_' + nm_sufixo + ' div.div_conteudo_arquivo p[linkname]').mouseup(function () {
                        selecionarTexto(nm_sufixo);
                    });
                    $('#div_cad_caput_' + nm_sufixo + ' div.div_conteudo_arquivo p[linkname] a[href]').attr('href', 'javascript:void(0);').click(function () {
                        selecionarTexto(nm_sufixo, this);
                    });
                }

                $('#div_cad_caput_' + nm_sufixo + ' div.line_conteudo_arquivo label').text('Selecione o Link:');
                $('#div_cad_caput_' + nm_sufixo + ' div.line_conteudo_arquivo').show();
                $('#div_cad_caput_' + nm_sufixo + ' div.line_buttons').show();
            }
        }
        $.ajaxlight({
            sUrl: "./ashx/Arquivo/HtmlFileEncoded.ashx?nm_base=sinj_norma&id_file=" + id_file,
            sType: "GET",
            fnSuccess: sucesso,
            bAsync: true
        });
    }
}

function selecionarArquivoCaputCopiar(id_button, el, nm_sufixo) {
    limparCaputSelecionado(nm_sufixo);
    if (!$(el).prop('checked')) {
        return;
    }
    var id_file = $(el).attr('id_file');
    if (IsNotNullOrEmpty(id_file)) {
        var sucesso = function (data) {

            $('#div_cad_caput_' + nm_sufixo + ' div.div_conteudo_arquivo').html(window.unescape(data.fileencoded));

            if ($('#div_cad_caput_' + nm_sufixo + ' div.div_conteudo_arquivo p').length > 0) {
                $.each($('#div_cad_caput_' + nm_sufixo + ' div.div_conteudo_arquivo p'), function (key_p, value_p) {
                    var pname = 'p' + key_p;
                    $(value_p).attr('pname', pname);
                });
                $('#div_cad_caput_' + nm_sufixo + ' div.div_conteudo_arquivo p[pname]').mouseup(function () {
                    selecionarTextoCopiar(id_button, nm_sufixo);
                });
                $('#div_cad_caput_' + nm_sufixo + ' div.line_conteudo_arquivo label').text('Selecione o texto novo:');
                $('#div_cad_vide').hide();
                $('#div_cad_caput_' + nm_sufixo).show();
                $('#div_cad_caput_' + nm_sufixo + ' div.line_conteudo_arquivo').show();
            }
        }
        $.ajaxlight({
            sUrl: "./ashx/Arquivo/HtmlFileEncoded.ashx?nm_base=sinj_norma&id_file=" + id_file,
            sType: "GET",
            fnSuccess: sucesso,
            bAsync: true
        });
    }
}

function clickButtonCaput(el, nm_sufixo) {
    var linkname = $(el).attr('linkname');
    if (IsNotNullOrEmpty(linkname)) {
        $('#div_cad_caput_' + nm_sufixo + ' div.div_caputs_selecionados input[name="caput"][value="' + linkname + '"]').remove();
        $('#div_cad_caput_' + nm_sufixo + ' div.div_caputs_selecionados input[name="caput_texto_' + linkname + '"]').remove();
        if ($(el).attr('selected') != 'selected') {
            $('#div_cad_caput_' + nm_sufixo + ' div.div_caputs_selecionados').prepend('<input type="hidden" name="caput" value="' + linkname + '" />');
            var p = $(el.parentNode).clone();
            $(p).find('a>sup').remove();
            $(p).find('a.link_vide').remove();
            var text = $(p).text();
            $('#div_cad_caput_' + nm_sufixo + ' div.div_caputs_selecionados').prepend('<input type="hidden" name="caput_texto_' + linkname + '" value="' + text + '" />');

            $(el).attr('selected', 'selected');
            $('img', el).attr('src', _urlPadrao + '/Imagens/ico_anchor_p.png');
        }
        else {
            el.removeAttribute('selected');
            $('img', el).attr('src', _urlPadrao + '/Imagens/ico_anchor_p_clear.png');
        }
    }
}

function removerCaput(nm_sufixo) {
    if (IsNotNullOrEmpty(nm_sufixo)) {
        $('#caput_norma_vide_' + nm_sufixo).val('');
        $('#label_caput_norma_' + nm_sufixo).text('');
        $('#a_selecionar_caput_norma_' + nm_sufixo).attr('onclick', 'javascript:abrirSelecionarCaput(\'' + nm_sufixo + '\');').attr('title', 'Selecionar o caput da norma ' + nm_sufixo).html('<img src="' + _urlPadrao + '/Imagens/ico_edit_dir.png" alt="adicionar" width="16px" height="16px" />');
        if (nm_sufixo == "alterada") {
            $('#ds_caput_norma_' + nm_sufixo).val('');
            $('#texto_antigo').text('');
            $('div.line_texto_caput').hide();
            $('div.line_ds_caput_norma_alterada').hide();
        }
        else {
            $('#a_texto_link').text('');
            $('div.line_link_caput').hide();
        }
        limparCaputSelecionado(nm_sufixo);
    }
}

function removerCaputParagrafo(el) {
    var posicao = el.getAttribute('posicao');

    var sCaput = $('#caput_norma_vide_alterada').val();
    var jCaput = JSON.parse(sCaput);
    jCaput.caput.splice(posicao, 1);
    jCaput.texto_antigo.splice(posicao, 1);
    var nm_tipo_relacao = $('#nm_tipo_relacao').val();
    //ToDo: se for acrescimo a descrição dos dispositivos é definida de outra forma
    if (nm_tipo_relacao.toLowerCase() != 'acrescimo') {
        $('#ds_caput_norma_alterada').val(getDescricaoDoCaput(jCaput.caput).replaceAll(';', '\n'));
    }
    $('#caput_norma_vide_alterada').val(JSON.stringify(jCaput));


    var buttons = $('div.caputedit button[posicao]');
    var iposicao = parseInt(posicao);
    for (; iposicao < buttons.length; iposicao++) {
        $(buttons[iposicao]).attr('posicao', iposicao - 1);
    }
    $(el).closest('div.caputedit').remove();
}

function limparCaputSelecionado(nm_sufixo) {
    $('#div_cad_caput_' + nm_sufixo + ' div.div_conteudo_arquivo').html('');
    $('#div_cad_caput_' + nm_sufixo + ' div.div_caputs_selecionados').html('');
    $('#div_cad_caput_' + nm_sufixo + ' div.line_conteudo_arquivo').hide();
    $('#div_cad_caput_' + nm_sufixo + ' div.line_buttons').hide();
}

function selecionarCaput(nm_sufixo) {
    var $el = $('#div_cad_caput_' + nm_sufixo + ' div.arquivos_norma input[type="checkbox"]:checked');
    var $inputCaput = $('#div_cad_caput_' + nm_sufixo + ' div.div_caputs_selecionados input[name="caput"]').get().reverse();
    if ($el.length == 1 && $inputCaput.length > 0) {
        var texto = [];
        var caput = [];
        var link = "";
        var nm_tipo_relacao = $('#nm_tipo_relacao').val();
        if (nm_sufixo == "alterada") {
            for (var i = 0; i < $inputCaput.length; i++) {
                caput.push($inputCaput[i].value);
                var inputCaputText = $('#div_cad_caput_' + nm_sufixo + ' div.div_caputs_selecionados input[name="caput_texto_' + $inputCaput[i].value + '"').val();
                if (IsNotNullOrEmpty(inputCaputText)) {
                    texto.push(inputCaputText);
                }
            }
        }
        else {
            caput.push($inputCaput[0].value);
            var inputCaputText = $('#div_cad_caput_' + nm_sufixo + ' div.div_caputs_selecionados input[name="caput_texto_' + $inputCaput[0].value + '"').val();
            if (IsNotNullOrEmpty(inputCaputText)) {
                link = inputCaputText;
            }
        }
        var jCaput = {
            caput: caput,
            id_file: $el.attr('id_file'),
            ch_norma: $el.attr('ch_norma'),
            ds_norma: $el.attr('ds_norma'),
            path: $el.attr('path'),
            linkname: $($inputCaput[0]).val(),
            texto_antigo: texto,
            link: link,
            filename: $el.attr('filename')
        }
        $('#caput_norma_vide_' + nm_sufixo).val(JSON.stringify(jCaput));
        $('#label_caput_norma_' + nm_sufixo).text(jCaput.ds_norma + '#' + jCaput.linkname + ' ');

        $('#a_selecionar_caput_norma_' + nm_sufixo).attr('onclick', 'javascript:removerCaput(\'' + nm_sufixo + '\');').attr('title', 'Remover o caput da norma ' + nm_sufixo).html('<img src="'+_urlPadrao+'/Imagens/ico_del_dir.png" alt="remover" width="16px" height="16px" />');

        if (nm_sufixo == "alterada") {
            $('div.line_texto_caput').show();
            $('div.line_texto_caput').html($('<div class="column w-100-pc"></div>'));
            if (IsNotNullOrEmpty(jCaput.texto_antigo)) {
                $('div.line_ds_caput_norma_alterada').show();
                var labeltextoantigo = 'Texto Antigo';
                var labeltextonovo = 'Texto Novo';
                if (nm_tipo_relacao.toLowerCase() == 'acrescimo') {
                    labeltextoantigo = 'Após o Texto';
                    labeltextonovo = 'Inserir o Texto';
                } else {
                    if (nm_tipo_relacao.toLowerCase() == 'renumeração') {
                        labeltextoantigo = 'Renumerar o Texto';
                        labeltextonovo = 'Texto Renumerado';
                    }
                    $('#ds_caput_norma_' + nm_sufixo).val(getDescricaoDoCaput(jCaput.caput).replaceAll(';', '\n'));
                }
                for (var i = 0; i < jCaput.texto_antigo.length; i++) {
                    $('div.line_texto_caput>div.column').append(
                        '<div class="table w-100-pc caputedit">' +
                            '<div class="line">' +
                                '<div class="column w-30-pc">' +
                                    '<div class="cell w-100-pc labeltextoantigo">' + labeltextoantigo + ':</div>' +
                                '</div>' +
                                '<div class="column w-70-pc">' +
                                    '<div class="cell w-100-pc">' +
                                        '<div id="texto_antigo" class="w-95-pc">' + jCaput.texto_antigo[i] + '</div>' +
                                        '<button type="button" class="del clean" onclick="javascript:removerCaputParagrafo(this);" posicao="'+i+'"><img src="'+_urlPadrao+'/Imagens/ico_delete_p.png" width="12px" /></button>' +
                                    '</div>' +
                                '</div>' +
                            '</div>' +
                            '<div class="line">' +
                                '<div class="column w-30-pc">' +
                                    '<div class="cell w-100-pc labeltextonovo">' + labeltextonovo + ':</div>' +
                                '</div>' +
                                '<div class="column w-70-pc">' +
                                    '<div class="cell w-100-pc">' +
                                        '<button id="button_texto_antigo_'+i+'" type="button" class="clean" onclick="javascript:abrirSelecionarCaputCopiar(this);" title="Abre o texto da norma alteradora para copia e colar o texto novo no campo abaixo."><img src="'+_urlPadrao+'/Imagens/ico_copy.png" width="15px" /></button>' +
                                        '<textarea name="texto_novo" class="w-95-pc" rows="5"></textarea>' +
                                    '</div>' +
                                '</div>' +
                            '</div>' +
                        '</div>'
                    );
                }
            }
        }
        else {
            $('div.line_link_caput').show();
            if (IsNotNullOrEmpty(jCaput.link)) {
                $('#a_texto_link').text(jCaput.link);
            }
        }
        fecharSelecionarCaput(nm_sufixo);
    }
}

function fecharSelecionarCaput(nm_sufixo) {
    $('#div_cad_vide').show();
    $('#div_cad_caput_' + nm_sufixo).hide();
}

function abrirSelecionarCaputCopiar(el_paste) {
    var nm_sufixo = 'alteradora';
    var id_button = el_paste.getAttribute('id');
    var $div_conteudo_arquivo = $('#div_cad_caput_' + nm_sufixo + ' div.div_conteudo_arquivo');
    if ($div_conteudo_arquivo.text().trim() == '') {
        abrirSelecionarCaput(nm_sufixo, id_button);
    }
    else {
        $('#div_cad_caput_' + nm_sufixo + ' div.arquivos_norma input[type="checkbox"][id_file]').attr('onchange', 'javascript:selecionarArquivoCaputCopiar(\''+id_button+'\', this, \'' + nm_sufixo + '\')');
        var $checked = $('#div_cad_caput_' + nm_sufixo + ' div.arquivos_norma input[type="checkbox"][id_file]:checked');
        if ($checked.length > 0) {
            selecionarArquivoCaputCopiar(id_button, $checked[0], nm_sufixo);
        }
    }

}

function getDescricaoDoCaput(caput) {
    var descricao = "";
    var descricao_linha = "";
    var descricao_caput = "";
    var cSplited = [];
    if(caput != null && caput.length > 0)
    {
        for(var c in caput)
        {
            descricao_linha = "";
            cSplited = caput[c].split('_');
            for(var cs in cSplited){
                descricao_caput = getDescricaoDoElemento(cSplited[cs]);
                if(IsNotNullOrEmpty(descricao_caput)){
                    descricao_linha += (IsNotNullOrEmpty(descricao_linha) ? ", " : "") + descricao_caput;
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

function getDescricaoDoElemento(caput)
{
    var caput1 = caput.substring(0, 3).toLowerCase();
    var caput2 = "";
    var caput3 = "";
    if(caput.length > 3){
        caput2 = caput.substring(3);
        if(isInt(caput2)){
            if(parseInt(caput2) < 10){
                caput2 += 'º';
            }
        }
    }
    if (caput1 == "ane" && IsNotNullOrEmpty(caput2)) {
        caput3 = "Anexo " + caput2.toUpperCase();
    }
    else if (caput1 == "art" && IsNotNullOrEmpty(caput2)){
        caput3 = "Art. " + caput2;
    }
    else if (caput1 == "par") {
        caput3 = (IsNotNullOrEmpty(caput2) ? "§ " + caput2 : "Parágrafo Único");
    }
    else if (caput1 == "inc" && IsNotNullOrEmpty(caput2)) {
        caput3 = "inc. " + caput2;
    }
    else if (caput1 == "num" && IsNotNullOrEmpty(caput2)) {
        caput3 = caput2 + ".";
    }
    else if (caput1 == "ali" && IsNotNullOrEmpty(caput2)) {
        caput3 = caput2 + ")";
    }
    else if (caput1 == "let" && IsNotNullOrEmpty(caput2)) {
        caput3 = caput2 + ")";
    }
    return caput3;
}