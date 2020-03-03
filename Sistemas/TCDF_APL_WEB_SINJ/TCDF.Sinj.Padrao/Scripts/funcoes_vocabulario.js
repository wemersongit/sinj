var _ch_termo = "";

function PreencherVocabularioEdicao() {
    var id_doc = GetParameterValue("id_doc");
    if (id_doc != "") {
        var sucesso = function (data) {
            if (IsNotNullOrEmpty(data)) {
                if (data.error_message != null && data.error_message != "") {
                    $('#div_notificacao_vocabulario').messagelight({
                        sTitle: "Erro",
                        sContent: data.error_message,
                        sType: "error",
                        sWidth: "",
                        iTime: null
                    });
                }
                if (data.ch_termo != null && data.ch_termo != "") {
                    $('#id_doc').val(data._metadata.id_doc);
                    $('#ch_tipo_termo option[value="' + data.ch_tipo_termo + '"]').attr('selected', 'selected');
                    SelecionarTipoDeTermo(true);
                    $('#nm_termo').val(data.nm_termo);
                    if (data.eh_descritor) {
                        if (IsNotNullOrEmpty(data, 'termos_gerais')) {
                            for (var i = 0; i < data.termos_gerais.length; i++) {
                                AdicionarTermo(data.termos_gerais[i].ch_termo_geral, data.termos_gerais[i].nm_termo_geral, "TG");
                            }
                        }
                        if (IsNotNullOrEmpty(data, 'termos_especificos')) {
                            for (var i = 0; i < data.termos_especificos.length; i++) {
                                AdicionarTermo(data.termos_especificos[i].ch_termo_especifico, data.termos_especificos[i].nm_termo_especifico, "TE");
                            }
                        }
                        if (IsNotNullOrEmpty(data, 'termos_relacionados')) {
                            for (var i = 0; i < data.termos_relacionados.length; i++) {
                                AdicionarTermo(data.termos_relacionados[i].ch_termo_relacionado, data.termos_relacionados[i].nm_termo_relacionado, "TR");
                            }
                        }
                    }
                    if (data.eh_lista_auxiliar) {
                        if (IsNotNullOrEmpty(data, 'ch_lista_superior')) {
                            $('#nm_lista_superior').val(data.nm_lista_superior);
                        }
                    }
                    if (IsNotNullOrEmpty(data, 'termos_nao_autorizados')) {
                        for (var i = 0; i < data.termos_nao_autorizados.length; i++) {
                            AdicionarTermo(data.termos_nao_autorizados[i].ch_termo_nao_autorizado, data.termos_nao_autorizados[i].nm_termo_nao_autorizado, "TNA");
                        }
                        $('.div_termos_nao_use').show();
                    }
                    $('#ds_nota_explicativa').text(GetText(data.ds_nota_explicativa));
                    $('#ds_fontes_pesquisadas').text(GetText(data.ds_fontes_pesquisadas));
                    $('#ds_texto_fonte').text(GetText(data.ds_texto_fonte));
                }
            }
        };
        var inicio = function () {
            $('#div_loading_vocabulario').show();
            $('#div_vocabulario').hide();
        }
        var complete = function () {
            $('#div_loading_vocabulario').hide();
            $('#div_vocabulario').show();
        }
        var teste = $.ajaxlight({
            sUrl: MostrarPaginaAjax('VIS') + window.location.search,
            sType: "POST",
            fnSuccess: sucesso,
            fnComplete: complete,
            fnBeforeSend: inicio,
            fnError: null,
            bAsync: true
        });
    }

}

function ConstruirControlesDinamicos() {
    $('#button_salvar_vocabulario').click(function () {
        return fnSalvar("form_vocabulario");
    });
    $('#button_criar_lista').click(function () {
        return fnSalvar("form_criar_lista");
    });
    var alfabeto = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'];
    for (var i = 0; i < alfabeto.length; i++) {
        var letra = alfabeto[i];
        $('#ul-alfabeto').append('<li class="li-alfabeto"><a href="javascript:void(0);" onclick="javascript:PesquisarAlfabeto(this);">' + alfabeto[i] + '</a></li>');
    }
    $('#div_autocomplete_lista_superior').autocompletelight({
        sKeyDataName: "ch_termo",
        sValueDataName: "nm_termo",
        sInputHiddenName: "ch_lista_superior",
        sInputName: "nm_lista_superior",
        sAjaxUrl: "./ashx/Autocomplete/VocabularioAutocomplete.ashx?id_doc_ignore=" + GetParameterValue("id_doc") + "&in_lista=true",
        bLinkAll: true,
        sLinkName: "a_lista_superior"
    });
    $('#div_pesquisa').autocompletelight({
        sKeyDataName: "ch_termo",
        sValueDataName: "nm_termo",
        sInputHiddenName: "input_ch_termo",
        bAddInexistente: true,
        sInputName: "input_nm_termo",
        sAjaxUrl: "./ashx/Autocomplete/VocabularioAutocomplete.ashx",
        bLinkAll: false
    });
    $('#div_resultado').hide();
}

function PesquisarAlfabeto(element) {
    $('#input_letra').val($(element).text());
    $('#form_vocabulario').submit();
}

function PesquisarVocabularioClick() {
    $('#form_vocabulario').submit();
}

function PesquisarVocabulario(filtro) {
    var search = window.location.search;
    $("#div_resultado").show();
    $("#div_resultado").dataTablesLight({
        sAjaxUrl: './ashx/Datatable/VocabularioDatatable.ashx' + (IsNotNullOrEmpty(search) ? search + (IsNotNullOrEmpty(filtro) ? '&' + filtro : '') : (IsNotNullOrEmpty(filtro) ? '?' + filtro : '')),
        aoColumns: _columns_vocabulario,
        bFilter: true
    });
}


function SelecionarTipoDeTermo(b_edicao) {
    $('.AU').hide();
    $('.DE').hide();
    $('.LA').hide();
    $('.ES').hide();
    $('.no-hide').hide();
    var selected = $('#ch_tipo_termo').val();
    if(IsNotNullOrEmpty(selected)){
        $('.' + selected).show();
        $('.no-hide').show();
        if (!b_edicao) {
            if (selected == "AU") {
                $('#nm_termo').blur(function () {
                    $('#img_alerta_existente').remove();
                    if ($('#nm_termo').val() != "") {
                        var sucesso = function (data) {
                            if (IsNotNullOrEmpty(data)) {
                                if (data.results != null && data.results.length > 0) {
                                    var orgaos = "";
                                    for (var i = 0; i < data.results.length; i++) {
                                        orgaos += (i + 1) + ". " + data.results[i].sg_hierarquia + " - " + data.results[i].nm_orgao + "<br/>";
                                    }
                                    if (orgaos != "") {
                                        $("#div_notificacao_vocabulario").modallight({
                                            sTitle: "Nome Duplicado!",
                                            oPosition: { my: "center", at: "center", of: "#nm_termo" },
                                            sContent: "Um ou mais Órgãos já possuem esse nome:<br/>" + orgaos,
                                            sType: "alert",
                                            sWidth: "350"
                                        });
                                        $('#nm_termo').parent().append('<img id="img_alerta_existente" src="' + _urlPadrao + '/Imagens/ico_alert_p.png" alt="atenção" title="Existe um órgão com este nome"/>');
                                    }
                                }
                            }
                        };
                        $.ajaxlight({
                            sUrl: './ashx/Consulta/OrgaoConsulta.ashx?nm_orgao=' + $('#nm_termo').val().replace(/\(.+?\)/, ""),
                            sType: "GET",
                            fnSuccess: sucesso,
                            bAsync: true
                        });
                    }
                });
            }
        }
    }
}

function DetalhesVocabulario() {
    var id_doc = GetParameterValue("id_doc");
    var ch_termo = GetParameterValue("ch_termo");
    if (id_doc != "" || ch_termo != "") {
        var sucesso = function (data) {
            if (IsNotNullOrEmpty(data)) {
                if (data.error_message != null && data.error_message != "") {
                    $('#div_notificacao_vocabulario').messagelight({
                        sTitle: "Erro",
                        sContent: data.error_message,
                        sType: "error",
                        sWidth: "",
                        iTime: null
                    });
                }
                if (data.ch_termo != null && data.ch_termo != "") {
                    _ch_termo = data.ch_termo;
                    if (ValidarPermissao(_grupos.voc_edt)) {
                        $('#div_controls_detalhes').append(
                            '<a title="Editar Termo" href="./EditarVocabulario.aspx?id_doc=' + data._metadata.id_doc + '"><img alt="editar" src="'+_urlPadrao+'/Imagens/ico_pencil_p.png"/></a> &nbsp;'
                        );
                    }
                    if (ValidarPermissao(_grupos.voc_exc)) {
                        if (!data.st_excluir) {
                            $('#div_controls_detalhes').append(
                                '<a id="quarentena" title="Excluir Termo" href="javascript:void(0);" onclick="javascript:MoverTermoParaQuarentena(' + data._metadata.id_doc + ', false);" ><img alt="excluir" src="'+_urlPadrao+'/Imagens/ico_trash_p.png"/></a>'
                            );
                        }
                        else if (ValidarPermissao(_grupos.voc_ger)) {
                            $('#div_controls_detalhes').append(
                                '<a id="quarentena" title="Restaurar Termo" href="javascript:void(0);" onclick="javascript:MoverTermoParaQuarentena(' + data._metadata.id_doc + ', true);" ><img alt="restaurar" src="'+_urlPadrao+'/Imagens/ico_undo_p.png"/></a>'
                            );
                        }
                    }
                    $('#label_tipo_termo').text(data.nm_tipo_termo);
                    $('#label_nm_termo').text(data.nm_termo);
                    $('#img_status').css('visibility', data.st_aprovado ? 'hidden' : 'visible');
                    if (data.eh_descritor) {
                        if (IsNotNullOrEmpty(data, 'termos_gerais')) {
                            for (var i = 0; i < data.termos_gerais.length; i++) {
                                $('#ul_termos_gerais').append('<li><a href="./DetalhesDeVocabulario.aspx?ch_termo=' + data.termos_gerais[i].ch_termo_geral + '" title="Visualizar detalhes do termo">' + data.termos_gerais[i].nm_termo_geral + '<img alt="detalhes" src="'+_urlPadrao+'/Imagens/ico_loupe_p.png" /></a></li>');
                            }
                            $('.div_termos_gerais').show();
                        }
                        if (IsNotNullOrEmpty(data, 'termos_especificos')) {
                            for (var i = 0; i < data.termos_especificos.length; i++) {
                                $('#ul_termos_especificos').append('<li><a href="./DetalhesDeVocabulario.aspx?ch_termo=' + data.termos_especificos[i].ch_termo_especifico + '" title="Visualizar detalhes do termo">' + data.termos_especificos[i].nm_termo_especifico + '<img alt="detalhes" src="'+_urlPadrao+'/Imagens/ico_loupe_p.png" /></a></li>');
                            }
                            $('.div_termos_especificos').show();
                        }
                        if (IsNotNullOrEmpty(data, 'termos_relacionados')) {
                            for (var i = 0; i < data.termos_relacionados.length; i++) {
                                $('#ul_termos_relacionados').append('<li><a href="./DetalhesDeVocabulario.aspx?ch_termo=' + data.termos_relacionados[i].ch_termo_relacionado + '" title="Visualizar detalhes do termo">' + data.termos_relacionados[i].nm_termo_relacionado + '<img alt="detalhes" src="'+_urlPadrao+'/Imagens/ico_loupe_p.png" /></a></li>');
                            }
                            $('.div_termos_relacionados').show();
                        }
                    }
                    if (data.eh_lista_auxiliar) {
                        if (IsNotNullOrEmpty(data, 'sublistas')) {
                            for (var i = 0; i < data.sublistas.length; i++) {
                                $('#ul_termos_sublista').append('<li><a href="./DetalhesDeVocabulario.aspx?ch_termo=' + data.sublistas[i].ch_termo + '" title="Visualizar detalhes do termo">' + data.sublistas[i].nm_termo + '<img alt="detalhes" src="'+_urlPadrao+'/Imagens/ico_loupe_p.png" /></a></li>');
                            }
                            $('.div_termos_sublista').show();
                        }
                        if (IsNotNullOrEmpty(data, 'itens')) {
                            for (var i = 0; i < data.itens.length; i++) {
                                $('#ul_termos_itens').append('<li><a href="./DetalhesDeVocabulario.aspx?ch_termo=' + data.itens[i].ch_termo + '" title="Visualizar detalhes do termo">' + data.itens[i].nm_termo + '<img alt="detalhes" src="'+_urlPadrao+'/Imagens/ico_loupe_p.png" /></a></li>');
                            }
                            $('.div_termos_itens').show();
                        }
                        if (data.lista != null && data.lista.nm_termo) {
                            $('#ul_termos_lista').append('<li><a href="./DetalhesDeVocabulario.aspx?ch_termo=' + data.lista.ch_termo + '" title="Visualizar detalhes do termo">' + data.lista.nm_termo + '<img alt="detalhes" src="'+_urlPadrao+'/Imagens/ico_loupe_p.png" /></a></li>');
                            $('.div_termos_lista').show();
                        }
                    }
                    if (IsNotNullOrEmpty(data.nm_termo_use)) {
                        $('#ul_termos_use').append('<li><a href="./DetalhesDeVocabulario.aspx?ch_termo=' + data.ch_termo_use + '" title="Visualizar detalhes do termo">' + data.nm_termo_use + '<img alt="detalhes" src="'+_urlPadrao+'/Imagens/ico_loupe_p.png" /></a></li>');
                        $('.div_termos_use').show();
                    }
                    if (IsNotNullOrEmpty(data, 'termos_nao_autorizados')) {
                        for (var i = 0; i < data.termos_nao_autorizados.length; i++) {
                            $('#ul_termos_nao_use').append('<li><a href="./DetalhesDeVocabulario.aspx?ch_termo=' + data.termos_nao_autorizados[i].ch_termo_nao_autorizado + '" title="Visualizar detalhes do termo">' + data.termos_nao_autorizados[i].nm_termo_nao_autorizado + '<img alt="detalhes" src="'+_urlPadrao+'/Imagens/ico_loupe_p.png" /></a></li>');
                        }
                        $('.div_termos_nao_use').show();
                    }
                    $('#div_nota_explicativa').text(GetText(data.ds_nota_explicativa));
                    $('#div_fontes_pesquisadas').text(GetText(data.ds_fontes_pesquisadas));
                    $('#div_texto_fonte').text(GetText(data.ds_texto_fonte));
                    if (ValidarPermissao(_grupos.voc_ger)) {
                        if (data.st_ativo) {
                            $('#div_controls_detalhes').prepend('<a title="Inativar Termo" href="javascript:void(0);" onclick="javascript:InativarTermo(' + data._metadata.id_doc + ',false,event, true);"><img alt="inativar" src="'+_urlPadrao+'/Imagens/ico_unlock_p.png"/></a> &nbsp;');
                        }
                        else {
                            $('#div_controls_detalhes').prepend('<a title="Ativar Termo" href="javascript:void(0);" onclick="javascript:InativarTermo(' + data._metadata.id_doc + ',true,event, true);"><img alt="ativar" src="'+_urlPadrao+'/Imagens/ico_lock_p.png"/></a> &nbsp;');
                        }
                        if (data.st_aprovado) {
                            $('#div_controls_detalhes').prepend('<a title="Tornar Pendente" href="javascript:void(0);" onclick="javascript:AprovarTermo(' + data._metadata.id_doc + ',false,event, null, true);"><img alt="aprovado" src="'+_urlPadrao+'/Imagens/ico_ok_p.png"/></a> &nbsp;');
                        }
                        else {
                            $('#div_controls_detalhes').prepend('<a title="Aprovar Termo" href="javascript:void(0);" onclick="javascript:AprovarTermo(' + data._metadata.id_doc + ',true,event, null, true);"><img alt="pendente" src="'+_urlPadrao+'/Imagens/ico_alert_p.png"/></a> &nbsp;');
                        }
                        $('#div_controls_detalhes').append("<div class='loading-p' style='display:none;'></div>");
                    }
                }
            }
        };
        var inicio = function () {
            $('#div_loading_vocabulario').show();
            $('#div_vocabulario').hide();
        }
        var complete = function () {
            $('#div_loading_vocabulario').hide();
            $('#div_vocabulario').show();
        }
        $.ajaxlight({
            sUrl: MostrarPaginaAjax('VIS') + window.location.search,
            sType: "POST",
            fnSuccess: sucesso,
            fnComplete: complete,
            fnBeforeSend: inicio,
            fnError: null,
            bAsync: true
        });
    }
}

function ExpandirTabelaDeOrgaos() {
    if ($('#a_selecionar_origens img').hasClass('opened')) {
        $('#a_selecionar_origens img').removeClass('opened');
        $('#a_selecionar_origens img').addClass('closed');
        $('#a_selecionar_origens img').prop('src', _urlPadrao + '/Imagens/ico_arrow_down.png');
        $('#div_datatable_origens').hide();
    }
    else {
        $('#a_selecionar_origens img').removeClass('closed');
        $('#a_selecionar_origens img').addClass('opened');
        $('#div_datatable_origens').show();
        $('#a_selecionar_origens img').prop('src', _urlPadrao + '/Imagens/ico_arrow_up.png');
        $("#div_datatable_origens").dataTablesLight({
            sAjaxUrl: './ashx/Datatable/OrgaoDatatable.ashx?st_autoridade=false',
            aoColumns: _columns_autoridade,
            sIdTable: 'datatable_selecionar_origens',
            bFilter: true
        });
    }
}

function SelecionarOrgaoAutoridade(ch_orgao, nm_orgao, sg_orgao, dt_inicio_vigencia, dt_fim_vigencia) {
    if (ch_orgao != null && nm_orgao != null && ch_orgao != "" && nm_orgao != "") {
        var anoInicio = "";
        var anoFim = "";
        var vigencia = "";
        if (IsNotNullOrEmpty(dt_inicio_vigencia)) {
            var dt_inicio_split = dt_inicio_vigencia.split('/');
            if (dt_inicio_split.length == 3) {
                anoInicio = dt_inicio_split[2];
            }
        }
        if (IsNotNullOrEmpty(dt_fim_vigencia)) {
            var dt_fim_split = dt_fim_vigencia.split('/');
            if (dt_fim_split.length == 3) {
                anoFim = dt_fim_split[2];
            }
        }
        if (anoInicio != "" || anoFim != "") {
            vigencia = " " + anoInicio + "-" + anoFim;
        }
        $('#ch_orgao').val(ch_orgao);
        $('#nm_termo').val(nm_orgao + "(" + sg_orgao + vigencia + ")");
        $('#img_alerta_existente').remove();
        $('#nm_termo').prop("disabled", true);
        $('#a_removerOrgaoAutoridade').show();
    }
}

function RemoverOrgaoAutoridade() {
    $('#ch_orgao').val("");
    $('#nm_termo').val("");
    $('#nm_termo').prop("disabled", false);
    $('#a_removerOrgaoAutoridade').hide();
}

function ExpandirTabelaDeTermos() {
    if ($('#a_selecionar_termos img').hasClass('opened')) {
        $('#a_selecionar_termos img').removeClass('opened');
        $('#a_selecionar_termos img').addClass('closed');
        $('#a_selecionar_termos img').prop('src', _urlPadrao + '/Imagens/ico_arrow_down.png');
        $('#div_datatable_termos').hide();
    }
    else {
        $('#a_selecionar_termos img').removeClass('closed');
        $('#a_selecionar_termos img').addClass('opened');
        $('#div_datatable_termos').show();
        $('#a_selecionar_termos img').prop('src', _urlPadrao + '/Imagens/ico_arrow_up.png');
        $("#div_datatable_termos").dataTablesLight({
            sAjaxUrl: './ashx/Datatable/VocabularioDatatable.ashx?id_doc_ignored=' + GetParameterValue("id_doc") + '&ch_tipo_termo=' + $('#ch_tipo_termo').val(),
            aoColumns: _columns_vocabulario_selecionar,
            sIdTable: 'datatable_selecionar_termos',
            bFilter: true
        });
    }
}

function ExpandirTabelaDeNormas() {
    if ($('#a_selecionar_normas img').hasClass('opened')) {
        $('#a_selecionar_normas img').removeClass('opened');
        $('#a_selecionar_normas img').addClass('closed');
        $('#a_selecionar_normas img').prop('src', _urlPadrao + '/Imagens/ico_arrow_down.png');
        $('#div_datatable_normas').hide();
    }
    else {
        $('#a_selecionar_normas img').removeClass('closed');
        $('#a_selecionar_normas img').addClass('opened');
        $('#div_datatable_normas').show();
        $('#a_selecionar_normas img').prop('src', _urlPadrao + '/Imagens/ico_arrow_up.png');
        $("#div_datatable_normas").dataTablesLight({
            sAjaxUrl: './ashx/Datatable/NormaDatatable.ashx?ch_termo=' + _ch_termo,
            aoColumns: _columns_norma_associada_vocabulario,
            sIdTable: 'datatable_normas',
            bFilter: true
        });
    }
}

function VerificarSeTermoNaoEstaAdicionado(ch_termo) {
    var selectors = ['input[name="termos_nao_autorizados"]', 'input[name="termos_gerais"]', 'input[name="termos_especificos"]', 'input[name="termos_relacionados"]'];
    for (var s = 0; s < selectors.length; s++) {
        var inputsTermosSelecionados = $(selectors[s]);
        for (var i = 0; i < inputsTermosSelecionados.length; i++) {
            if (IsNotNullOrEmpty(inputsTermosSelecionados[i].value)) {
                var chave = inputsTermosSelecionados[i].value.split('#')[0];
                if(chave == ch_termo) return false;
            }
        }
    }
    return true;
}

function AdicionarTermo(ch_termo, nm_termo, ch_relacao) {
    if (IsNotNullOrEmpty(ch_termo) && IsNotNullOrEmpty(nm_termo) && IsNotNullOrEmpty(ch_relacao)) {
        var value = ch_termo + "#" + nm_termo;
        var sufixo = ch_relacao == "TNA" ? "termos_nao_autorizados" : ch_relacao == "TR" ? "termos_relacionados" : ch_relacao == "TE" ? "termos_especificos" : ch_relacao == "TG" ? "termos_gerais" : "";
        if ($('input[name="'+sufixo+'"][value="' + value + '"]').length > 0) {
            return false;
        }
        $('div#' + sufixo).append('<div><input type="hidden" name="' + sufixo + '" value="' + value + '" />' + nm_termo + '&nbsp;<a href="javascript:void();" onclick="javascript:RemoverDaLista(this,\'' + ch_termo + '\');"><img src="'+_urlPadrao+'/Imagens/ico_delete_p.png" /></a></div>');
        $('div#' + sufixo).show();
        $('a[ch_termo="' + ch_termo + '"]').hide();
    }
}

function RemoverDaLista(a, value) {
    var div = a.parentNode.parentNode;
    $(a.parentNode).remove();
    var testar = $(div).text().replace(/\s/gi, '');
    if(IsNotNullOrEmpty(value)){
        $('a[ch_termo="' + value + '"]').show();
    }
}

function fnQuarentena(_id_doc, _bRestaurar) {
    var sucesso = function (data) {
        if (IsNotNullOrEmpty(data, 'id_doc_success')) {
            $('#div_notificacao_vocabulario').messagelight({
                sTitle: "Sucesso",
                sContent: (_bRestaurar ? "Restaurado com sucesso." : "Movido para quarentena com sucesso."),
                sType: "success",
                sWidth: ""
            });
            $('img', $('#quarentena')).prop('src', './Imagens/' + (!_bRestaurar ? "ico_undo_p" : "ico_trash_p") + '.png');
            $('img', $('#quarentena')).prop('alt', (!_bRestaurar ? "restaurar" : "excluir"));
            $('#quarentena').attr('onclick', 'javascript:MoverTermoParaQuarentena(' + _id_doc + ',' + !_bRestaurar + ')');
            $('#quarentena').attr('title', (!_bRestaurar ? "Restaurar Termo" : "Mover Termo para Quarentena"));
            $('#modal_termo_quarentena').dialog("close");
        }
        else if (IsNotNullOrempty(data, 'error_message')) {
            $('.notify', $('#modal_termo_quarentena')).messagelight({
                sTitle: "Erro",
                sContent: data.error_message,
                sType: "error",
                sWidth: ""
            });
        }
        else {
            $('.notify', $('#modal_termo_quarentena')).messagelight({
                sTitle: "Erro",
                sContent: "Ocorreu um erro não identificado.",
                sType: "error",
                sWidth: ""
            });
        }
    }
    var inicio = function () {
        $('.loaded', $('#modal_termo_quarentena')).hide();
        $('.loading', $('#modal_termo_quarentena')).show();
    }
    var complete = function () {
        $('.loaded', $('#modal_termo_quarentena')).show();
        $('.loading', $('#modal_termo_quarentena')).hide();
    }
    $.ajaxlight({
        sUrl: './ashx/Path/VocabularioPath.ashx?id_doc=' + _id_doc + '&path=st_excluir&value=' + !_bRestaurar,
        sType: "GET",
        fnSuccess: sucesso,
        fnComplete: complete,
        fnBeforeSend: inicio,
        bAsync: true
    });
}

function fnCancelarQuarentena() {
    $('#modal_termo_quarentena').dialog('close');
}

function MoverTermoParaQuarentena(id_doc, bRestaurar) {
    var html = '<div class="mauto table">' +
                    '<div class="line">' +
                        '<div class="column w-100-pc">' +
                            '<div class="cell w-100-pc">' +
                                (bRestaurar ? 'Deseja Restaurar o Termo?' : 'O Termo será movido para quarentena. Deseja continuar?') +
                            '</div>' +
                        '</div>' +
                    '</div>' +
                '</div>' +
                '<div class="notify" style="display:none;">' +
                '</div>' +
                '<div class="loading" style="display:none;"></div>' +
                '<div style="width:220px; margin:auto;" class="loaded">' +
                    '<button onclick="javascript:fnQuarentena(' + id_doc + ',' + bRestaurar + ');">' +
                        '<img src="'+_urlPadrao+'/Imagens/ico_ok_p.png" />Sim' +
                    '</button>' +
                    '<button onclick="javascript:fnCancelarQuarentena();">' +
                        '<img src="'+_urlPadrao+'/Imagens/ico_delete_p.png" />Nao' +
                    '</button>' +
                '</div>';
    if ($('#modal_termo_quarentena').length > 0) {
        $('#modal_termo_quarentena').html(html);
        $('#modal_termo_quarentena').dialog('open');
    }
    else {
        $('<div id="modal_termo_quarentena" />').modallight({
            sTitle: "Quarentena",
            sType: "default",
            sContent: html,
            sWidth: 500,
            oButtons: []
        });
    }
}

//Funcoes Gerenciar Vocabulario

function PesquisarTermosTrocarTermo(filtro) {
    $("#div_datatable_torcar_termos").dataTablesLight({
        sAjaxUrl: './ashx/Datatable/VocabularioDatatable.ashx' + (IsNotNullOrEmpty(filtro) ? '?' + filtro : ''),
        aoColumns: _columns_vocabulario_trocar,
        bFilter: true,
        sIdTable: 'datatable_trocar'
    });
}

function ClicarAbaTrocarTermo() {
    PesquisarTermosTrocarTermo();
    $('#id_doc_termo_antigo_autocomplete').val('');
    $('#ch_tipo_termo_antigo_autocomplete').val('');
    $('#nm_termo_antigo_autocomplete').val('');
    $('#id_doc_termo_novo_autocomplete').val('');
    $('#nm_termo_novo_autocomplete').val('');
    $('#nm_termo_antigo_autocomplete').prop('disabled', true);
    $('.termo-antigo').hide();
    $('.termo-novo').hide();
    $('.cancelar').hide();
    $('#div_trocar_termo').hide();
    $('#div_datatable_torcar_termos').show();
}

function ClicarAbaQuarentena(filtro) {
    $("#div_datatable_termos_quarentena").dataTablesLight({
        sAjaxUrl: './ashx/Datatable/VocabularioDatatable.ashx?st_excluir=true' + (IsNotNullOrEmpty(filtro) ? '&' + filtro : ''),
        aoColumns: _columns_vocabulario_quarentena,
        bFilter: true,
        sIdTable: 'datatable_quarentena'
    });
}

function ClicarAbaPendentes(filtro) {
    $("#div_datatable_termos_pendentes").dataTablesLight({
        sAjaxUrl: './ashx/Datatable/VocabularioDatatable.ashx?st_aprovado=false' + (IsNotNullOrEmpty(filtro) ? '&' + filtro : ''),
        aoColumns: _columns_vocabulario_pendentes,
        bFilter: true,
        sIdTable: 'datatable_pendentes'
    });
}

function ClicarAbaInativos(filtro) {
    $("#div_datatable_termos_inativos").dataTablesLight({
        sAjaxUrl: './ashx/Datatable/VocabularioDatatable.ashx?st_ativo=false' + (IsNotNullOrEmpty(filtro) ? '&' + filtro : ''),
        aoColumns: _columns_vocabulario_inativos,
        bFilter: true,
        sIdTable: 'datatable_inativos'
    });
}

function ClicarAbaRestaurados(filtro) {
    $("#div_datatable_termos_restaurados").dataTablesLight({
        sAjaxUrl: './ashx/Datatable/VocabularioDatatable.ashx?st_restaurado=true' + (IsNotNullOrEmpty(filtro) ? '?' + filtro : ''),
        aoColumns: _columns_vocabulario_restaurados,
        bFilter: true,
        sIdTable: 'datatable_restaurados'
    });
}

function fnExcluirTermo(id_datatable) {
    $('#div_notificacao_modal_exclusao').html('');
    try {
        Validar("form_exclusao");
        var sucesso = function (data) {
            if (IsNotNullOrEmpty(data)) {
                $('#div_loading_modal_exclusao').hide();
                $('#div_buttons_modal_exclusao').show();
                if (data.error_message != null && data.error_message != "") {
                    $('#div_notificacao_modal_exclusao').messagelight({
                        sTitle: "Erro",
                        sContent: data.error_message,
                        sType: "error",
                        sWidth: "",
                        iTime: null
                    });
                }
                else if (data.excluido != null && data.excluido == true) {
                    $('#' + id_datatable + ' tr.selected').remove();
                    $('#div_notificacao_termos_quarentena').messagelight({
                        sTitle: "Sucesso",
                        sContent: "Excluído com sucesso.",
                        sType: "success",
                        sWidth: "",
                        iTime: 5000
                    });
                    $('#modal_exclusao').dialog("close");
                }
                else {
                    $('#div_notificacao_modal_exclusao').messagelight({
                        sTitle: "Erro",
                        sContent: "Erro na exclusão.",
                        sType: "error",
                        sWidth: "",
                        iTime: null
                    });
                }
            }
        }
        var beforeSubmit = function () {
            $('#div_loading_modal_exclusao').show();
            $('#div_buttons_modal_exclusao').hide();
        };
        $.ajaxlight({
            sUrl: './ashx/Exclusao/VocabularioExcluir.ashx',
            sType: "POST",
            fnSuccess: sucesso,
            sFormId: "form_exclusao",
            fnBeforeSubmit: beforeSubmit
        });
    }
    catch (ex) {
        $("#div_notificacao_modal_exclusao").messagelight({
            sTitle: "Erro nos dados informados",
            sContent: ex,
            sType: "error"
        });
    }
    return false;
}

function ExcluirTermo(id_doc, id_datatable) {
    var html = '<form id="form_exclusao" name="formExclusao" action="#" method="POST">' +
        '<input id="id_doc" type="hidden" value="' + id_doc + '" label="ID" obrigatorio="sim" name="id_doc" />' +
        '<input type="hidden" value="true" name="force" />' +
        '<div class="mauto table">' +
            '<div class="line">' +
                '<div class="column w-20-pc">' +
                    '<div class="cell fr">' +
                        '<label>Justificativa:</label>' +
                    '</div>' +
                '</div>' +
                '<div class="column w-70-pc">' +
                    '<div class="cell w-100-pc">' +
                        '<textarea label="Justificativa" obrigatorio="sim" id="justificativa" name="justificativa" class="w-90-pc" cols="100" rows="5" style="max-width:100%;" ></textarea>' +
                    '</div>' +
                '</div>' +
            '</div>' +
        '</div>' +
        '<div id="div_notificacao_modal_exclusao" style="display:none;">' +
        '</div>' +
        '<div id="div_loading_modal_exclusao" class="loading" style="display:none;"></div>' +
        '<div id="div_buttons_modal_exclusao" style="width:220px; margin:auto;" class="loaded">' +
            '<button id="button_excluir" onclick="javascript:return fnExcluirTermo(\'' + id_datatable + '\');">' +
                '<img src="'+_urlPadrao+'/Imagens/ico_disk_p.png" />Excluir' +
            '</button>' +
            '<button type="reset">' +
                '<img src="'+_urlPadrao+'/Imagens/ico_eraser_p.png" />Limpar' +
            '</button>' +
        '</div>' +
    '</form>';
    if ($('#modal_exclusao').length > 0) {
        $('#modal_exclusao').html(html);
        $('#modal_exclusao').dialog('open');
    }
    else {
        $('<div id="modal_exclusao" />').modallight({
            sTitle: "Excluir",
            sType: "default",
            sContent: html,
            sWidth: 500,
            oButtons: []
        });
    }
}

function fnRestaurar(_id_doc, _id_datatable, _event) {
    var td = $(_event.target).closest('td');
    var sucesso = function (data) {
        if (IsNotNullOrEmpty(data, 'id_doc_success')) {
            $('#div_notificacao_termos_quarentena').messagelight({
                sTitle: "Sucesso",
                sContent: "Termo restaurado com sucesso.",
                sType: "success",
                sWidth: "",
                iTime: 5000
            });
            $('#' + _id_datatable + ' tr.selected').remove();
            $('#modal_termo_quarentena').dialog("close");
        }
        else if (IsNotNullOrempty(data, 'error_message')) {
            $('.notify', $('#modal_termo_quarentena')).messagelight({
                sTitle: "Erro",
                sContent: data.error_message,
                sType: "error",
                sWidth: ""
            });
        }
        else {
            $('.notify', $('#modal_termo_quarentena')).messagelight({
                sTitle: "Erro",
                sContent: "Ocorreu um erro não identificado.",
                sType: "error",
                sWidth: ""
            });
        }
    }
    var inicio = function () {
        $('.loaded', $('#modal_termo_quarentena')).hide();
        $('.loading', $('#modal_termo_quarentena')).show();
    }
    var complete = function () {
        $('.loaded', $('#modal_termo_quarentena')).show();
        $('.loading', $('#modal_termo_quarentena')).hide();
    }
    $.ajaxlight({
        sUrl: './ashx/Path/VocabularioPath.ashx?id_doc=' + _id_doc + '&path=st_excluir&value=false',
        sType: "GET",
        fnSuccess: sucesso,
        fnComplete: complete,
        fnBeforeSend: inicio,
        bAsync: true
    });
}

function fnCancelarRestaurar(){
    $('#modal_termo_quarentena').dialog('close');
}

function RestaurarTermo(id_doc, id_datatable, event){
    var html = '<div class="mauto table">' +
                    '<div class="line">' +
                        '<div class="column w-100-pc">' +
                            '<div class="cell w-100-pc">' +
                                'Deseja restaurar o termo?' +
                            '</div>' +
                        '</div>' +
                    '</div>' +
                '</div>' +
                '<div class="notify" style="display:none;">' +
                '</div>' +
                '<div class="loading" style="display:none;"></div>' +
                '<div style="width:220px; margin:auto;" class="loaded">' +
                    '<button onclick="javascript:fnRestaurar(' + id_doc + ',\'' + id_datatable + '\',event);">' +
                        '<img src="'+_urlPadrao+'/Imagens/ico_ok_p.png" />Sim' +
                    '</button>' +
                    '<button onclick="javascript:fnCancelarRestaurar();">' +
                        '<img src="'+_urlPadrao+'/Imagens/ico_delete_p.png" />Nao' +
                    '</button>' +
                '</div>';
    if ($('#modal_termo_quarentena').length > 0) {
        $('#modal_termo_quarentena').html(html);
        $('#modal_termo_quarentena').dialog('open');
    }
    else {
        $('<div id="modal_termo_quarentena" />').modallight({
            sTitle: "Quarentena",
            sType: "default",
            sContent: html,
            sWidth: 500,
            oButtons: []
        });
    }
}

function AprovarTermo(id_doc, bAprovar, event, id_datatable, bRefresh) {
    var parent = $(event.target).closest('td');
    if (parent.length <= 0) {
        parent = $(event.target).closest('div');
    }
    $('<div id="modal_confirmar_termos_pendentes" />').modallight({
        sTitle: (bAprovar ? "Aprovar" : "Tornar pendente"),
        sContent: "Deseja " + (bAprovar ? "aprovar o Termo" : "tornar o Termo pendente") + "?",
        sType: "default",
        oButtons: [
            { text: "Sim", click: function () {
                var sucesso = function (data) {
                    if (IsNotNullOrEmpty(data, 'id_doc_success')) {
                        $('#div_notificacao_termos_pendentes').messagelight({
                            sTitle: "Sucesso",
                            sContent: "Termo " + (bAprovar ? "aprovado" : "pendido") + " com sucesso.",
                            sType: "success",
                            sWidth: "",
                            iTime: 5000
                        });
                        if (IsNotNullOrEmpty(id_datatable)) {
                            $('#' + id_datatable + ' tr.selected').remove();
                        }
                        else if (bRefresh) {
                            document.location.reload();
                        }
                        else {
                            parent.html(fn_botoes_pendentes(id_doc, bAprovar));
                        }
                    }
                    else if (IsNotNullOrempty(data, 'error_message')) {
                        $('#div_notificacao_termos_pendentes').messagelight({
                            sTitle: "Erro",
                            sContent: data.error_message,
                            sType: "error",
                            sWidth: "",
                            iTime: 5000
                        });
                    }
                    else {
                        $('#div_notificacao_termos_pendentes').messagelight({
                            sTitle: "Erro",
                            sContent: "Ocorreu um erro não identificado.",
                            sType: "error",
                            sWidth: "",
                            iTime: 5000
                        });
                    }
                }
                var inicio = function () {
                    if (!IsNotNullOrEmpty(id_datatable)) {
                        $('a', parent).hide();
                        $('.loading-p', parent).show();
                    }
                }
                var complete = function () {
                    if (!IsNotNullOrEmpty(id_datatable)) {
                        $('a', parent).show();
                        $('.loading-p', parent).hide();
                    }
                }
                $.ajaxlight({
                    sUrl: './ashx/Path/VocabularioPath.ashx?id_doc=' + id_doc + '&path=st_aprovado&value=' + bAprovar,
                    sType: "GET",
                    fnSuccess: sucesso,
                    fnComplete: complete,
                    fnBeforeSend: inicio,
                    bAsync: true
                });
                $(this).dialog("close");
            }
            },
            { text: "Não", click: function () { $(this).dialog('close'); } }]
    });
}

function InativarTermo(id_doc, bAtivar, event, bRefresh) {
    var parent = $(event.target).closest('td');
    if (parent.length <= 0) {
        parent = $(event.target).closest('div');
    }
    var sucesso = function (data) {
        if (IsNotNullOrEmpty(data, 'id_doc_success')) {
            $('#div_notificacao_termos_inativos').messagelight({
                sTitle: "Sucesso",
                sContent: "Termo " + (bAtivar ? "ativado" : "inativado") + " com sucesso.",
                sType: "success",
                sWidth: "",
                iTime: 5000
            });
            if (IsNotNullOrEmpty(bRefresh) && bRefresh) {
                document.location.reload();
            }
            else {
                parent.html(fn_botoes_inativar(id_doc, bAtivar));
            }
        }
        else if (IsNotNullOrempty(data, 'error_message')) {
            $('#div_notificacao_termos_inativos').messagelight({
                sTitle: "Erro",
                sContent: data.error_message,
                sType: "error",
                sWidth: "",
                iTime: 5000
            });
        }
        else {
            $('#div_notificacao_termos_inativos').messagelight({
                sTitle: "Erro",
                sContent: "Ocorreu um erro não identificado.",
                sType: "error",
                sWidth: "",
                iTime: 5000
            });
        }
    }
    var inicio = function () {
        $('a', parent).hide();
        $('.loading-p', parent).show();
    }
    var complete = function () {
        $('a', parent).show();
        $('.loading-p', parent).hide();
    }
    $.ajaxlight({
        sUrl: './ashx/Path/VocabularioPath.ashx?id_doc=' + id_doc + '&path=st_ativo&value=' + bAtivar,
        sType: "GET",
        fnSuccess: sucesso,
        fnComplete: complete,
        fnBeforeSend: inicio,
        bAsync: true
    });
}

function SelecionarTrocarTermo(id_doc, ch_termo, nm_termo, ch_tipo_termo) {
    $('.termo-antigo').show();
    $('.termo-novo').show();
    $('.cancelar').show();
    $('#id_doc_termo_antigo_autocomplete').val(id_doc);
    $('#ch_tipo_termo_antigo_autocomplete').val(ch_tipo_termo);
    $('#nm_termo_antigo_autocomplete').val(nm_termo);
    $('#div_datatable_torcar_termos').hide();
    $('#a_termo_novo').show();
    $('#div_autocomplete_termo_novo').autocompletelight({
        sKeyDataName: "_metadata.id_doc",
        sValueDataName: "nm_termo",
        sInputHiddenName: "id_doc_termo_novo_autocomplete",
        sInputName: "nm_termo_novo_autocomplete",
        sAjaxUrl: "./ashx/Autocomplete/VocabularioAutocomplete.ashx?ch_termo_ignore=" + ch_termo,
        bLinkAll: true,
        sLinkName: "a_termo_novo",
        bAddInexistente: true,
        jPagination: { bPaginate: true,
                iDisplayLength: 20,
                bButtonsPaginate: true
            }
    });
}

function CancelarTrocarTermo() {
    $('.termo-antigo').hide();
    $('.termo-novo').hide();
    $('.cancelar').hide();
    $('#id_doc_termo_antigo_autocomplete').val('');
    $('#ch_tipo_termo_antigo_autocomplete').val('');
    $('#nm_termo_antigo_autocomplete').val('');
    $('#div_datatable_torcar_termos').show();
    PesquisarTermosTrocarTermo();
    $('#div_trocar_termo').hide();
    return false;
}

function TrocarTipoTermoNovo() {
    $('#div_modal_trocar_tipo_termo_novo').modallight({
        sTitle: "Trocar Tipo do Termo",
        sWidth: '400',
        oButtons: [{
            text: "Ok",
            click: function () {
                if ($('#select_trocar_tipo_termo_novo').val() != "") {
                    $('#ch_tipo_termo_novo').val($('#select_trocar_tipo_termo_novo').val());
                    $('#div_nm_tipo_termo').html($('#select_trocar_tipo_termo_novo option[value="' + $('#select_trocar_tipo_termo_novo').val() + '"]').text() + '<a id="a_trocar_tipo_termo_novo" href="javascript:void(0);" onclick="javascript:TrocarTipoTermoNovo();" title="Trocar o Tipo de Termo" ><img src="' + _urlPadrao + '/Imagens/ico_pencil_p.png"  alt="trocar" /></a>');
                    $(this).dialog('close');
                }
            }
        },
        {
            text: "Cancelar",
            click: function () {
                    $(this).dialog('close');
                }
        }]
    });
}

function ContinuarTrocarTermo() {
    var id_doc_antigo = $('#id_doc_termo_antigo_autocomplete').val();
    var id_doc_novo = $('#id_doc_termo_novo_autocomplete').val();
    if (IsNotNullOrEmpty(id_doc_antigo)) {
        var sucesso = function (data) {
            if (IsNotNullOrEmpty(data)) {
                if (data.error_message != null && data.error_message != "") {
                    $('#div_notificacao_trocar_termo').messagelight({
                        sTitle: "Erro",
                        sContent: data.error_message,
                        sType: "error",
                        sWidth: "",
                        iTime: null
                    });
                }
                if (IsNotNullOrEmpty(data.termo_antigo)) {
                    $('.termo-antigo').hide();
                    $('.termo-novo').hide();
                    $('.cancelar').hide();
                    $('#ch_tipo_termo_novo').val(data.termo_antigo.ch_tipo_termo);
                    $('#div_nm_tipo_termo').html(data.termo_antigo.nm_tipo_termo + '<a id="a_trocar_tipo_termo_novo" href="javascript:void(0);" onclick="javascrpit:TrocarTipoTermoNovo();" title="Trocar o Tipo de Termo" style="display:none;" ><img src="'+_urlPadrao+'/Imagens/ico_pencil_p.png"  alt="trocar" /></a>');
                    $('#id_doc_termo_antigo').val(data.termo_antigo._metadata.id_doc);
                    if (data.termo_antigo.eh_descritor) {
                        var value = "";
                        if (IsNotNullOrEmpty(data.termo_antigo.termos_gerais)) {
                            for (var i = 0; i < data.termo_antigo.termos_gerais.length; i++) {
                                value = data.termo_antigo.termos_gerais[i].ch_termo_geral + "#" + data.termo_antigo.termos_gerais[i].nm_termo_geral;
                                if ($('input[name="termos_gerais"][value="' + value + '"]', $('#termos_gerais_trocar_termo')).length <= 0) {
                                    $('#termos_gerais_trocar_termo').append('<div><input type="hidden" name="termos_gerais_antigo" value="' + value + '" />' + data.termo_antigo.termos_gerais[i].nm_termo_geral + '&nbsp;<a href="javascript:void();" onclick="javascript:RemoverDaLista(this);"><img src="'+_urlPadrao+'/Imagens/ico_delete_p.png" /></a></div>');
                                }
                            }
                        }
                        if (IsNotNullOrEmpty(data.termo_antigo.termos_especificos)) {
                            for (var i = 0; i < data.termo_antigo.termos_especificos.length; i++) {
                                value = data.termo_antigo.termos_especificos[i].ch_termo_especifico + "#" + data.termo_antigo.termos_especificos[i].nm_termo_especifico;
                                if ($('input[name="termos_especificos"][value="' + value + '"]', $('#termos_especificos_trocar_termo')).length <= 0) {
                                    $('#termos_especificos_trocar_termo').append('<div><input type="hidden" name="termos_especificos_antigo" value="' + value + '" />' + data.termo_antigo.termos_especificos[i].nm_termo_especifico + '&nbsp;<a href="javascript:void();" onclick="javascript:RemoverDaLista(this);"><img src="' + _urlPadrao + '/Imagens/ico_delete_p.png" /></a></div>');
                                }
                            }
                        }
                        if (IsNotNullOrEmpty(data.termo_antigo.termos_relacionados)) {
                            for (var i = 0; i < data.termo_antigo.termos_relacionados.length; i++) {
                                value = data.termo_antigo.termos_relacionados[i].ch_termo_relacionado + "#" + data.termo_antigo.termos_relacionados[i].nm_termo_relacionado;
                                if ($('input[name="termos_relacionados"][value="' + value + '"]', $('#termos_relacionados_trocar_termo')).length <= 0) {
                                    $('#termos_relacionados_trocar_termo').append('<div><input type="hidden" name="termos_relacionados_antigo" value="' + value + '" />' + data.termo_antigo.termos_relacionados[i].nm_termo_relacionado + '&nbsp;<a href="javascript:void();" onclick="javascript:RemoverDaLista(this);"><img src="' + _urlPadrao + '/Imagens/ico_delete_p.png" /></a></div>');
                                }
                            }
                        }
                    }
                    if (IsNotNullOrEmpty(data.termo_antigo.termos_nao_autorizados)) {
                        for (var i = 0; i < data.termo_antigo.termos_nao_autorizados.length; i++) {
                            value = data.termo_antigo.termos_nao_autorizados[i].ch_termo_nao_autorizado + "#" + data.termo_antigo.termos_nao_autorizados[i].nm_termo_nao_autorizado;
                            if ($('input[name="termos_nao_autorizados"][value="' + value + '"]', $('#termos_nao_autorizados_trocar_termo')).length <= 0) {
                                $('#termos_nao_autorizados_trocar_termo').append('<div><input type="hidden" name="termos_nao_autorizados_antigo" value="' + value + '" />' + data.termo_antigo.termos_nao_autorizados[i].nm_termo_nao_autorizado + '&nbsp;<a href="javascript:void();" onclick="javascript:RemoverDaLista(this);"><img src="' + _urlPadrao + '/Imagens/ico_delete_p.png" /></a></div>');
                            }
                        }
                    }
                    $('#div_nota_explicativa_trocar_termo').text(data.termo_antigo.ds_nota_explicativa);
                    $('#div_fontes_pesquisadas_trocar_termo').text(data.termo_antigo.ds_fontes_pesquisadas);
                    $('#div_texto_fonte_trocar_termo').text(data.termo_antigo.ds_texto_fonte);
                }
                if (IsNotNullOrEmpty(data.termo_novo)) {
                    $('#id_doc_termo_novo').val(data.termo_novo._metadata.id_doc);
                    $('#ch_tipo_termo_novo').val(data.termo_novo.ch_tipo_termo);
                    $('#div_nm_tipo_termo').text(data.termo_novo.nm_tipo_termo);
                    $('#div_nm_termo_novo').text(data.termo_novo.nm_termo);
                    $('#nm_termo_novo').val(data.termo_novo.nm_termo);
                    if (data.termo_novo.eh_descritor) {
                        var value = "";
                        $('div.DE').show();
                        if (IsNotNullOrEmpty(data.termo_novo.termos_gerais)) {
                            for (var i = 0; i < data.termo_novo.termos_gerais.length; i++) {
                                value = data.termo_novo.termos_gerais[i].ch_termo_geral + "#" + data.termo_novo.termos_gerais[i].nm_termo_geral;
                                if ($('input[name="termos_gerais"][value="' + value + '"]', $('#termos_gerais_trocar_termo')).length <= 0) {
                                    $('#termos_gerais_trocar_termo').append('<div><input type="hidden" name="termos_gerais_novo" value="' + value + '" />' + data.termo_novo.termos_gerais[i].nm_termo_geral + '&nbsp;<a href="javascript:void();" onclick="javascript:RemoverDaLista(this);"><img src="'+_urlPadrao+'/Imagens/ico_delete_p.png" /></a></div>');
                                }
                            }
                        }
                        if (IsNotNullOrEmpty(data.termo_novo.termos_especificos)) {
                            for (var i = 0; i < data.termo_novo.termos_especificos.length; i++) {
                                value = data.termo_novo.termos_especificos[i].ch_termo_especifico + "#" + data.termo_novo.termos_especificos[i].nm_termo_especifico;
                                if ($('input[name="termos_especificos"][value="' + value + '"]', $('#termos_especificos_trocar_termo')).length <= 0) {
                                    $('#termos_especificos_trocar_termo').append('<div><input type="hidden" name="termos_especificos_novo" value="' + value + '" />' + data.termo_novo.termos_especificos[i].nm_termo_especifico + '&nbsp;<a href="javascript:void();" onclick="javascript:RemoverDaLista(this);"><img src="' + _urlPadrao + '/Imagens/ico_delete_p.png" /></a></div>');
                                }
                            }
                        }
                        if (IsNotNullOrEmpty(data.termo_novo.termos_relacionados)) {
                            for (var i = 0; i < data.termo_novo.termos_relacionados.length; i++) {
                                value = data.termo_novo.termos_relacionados[i].ch_termo_relacionado + "#" + data.termo_novo.termos_relacionados[i].nm_termo_relacionado;
                                if ($('input[name="termos_relacionados"][value="' + value + '"]', $('#termos_relacionados_trocar_termo')).length <= 0) {
                                    $('#termos_relacionados_trocar_termo').append('<div><input type="hidden" name="termos_relacionados_novo" value="' + value + '" />' + data.termo_novo.termos_relacionados[i].nm_termo_relacionado + '&nbsp;<a href="javascript:void();" onclick="javascript:RemoverDaLista(this);"><img src="' + _urlPadrao + '/Imagens/ico_delete_p.png" /></a></div>');
                                }
                            }
                        }
                    }
                    if (IsNotNullOrEmpty(data.termo_novo.termos_nao_autorizados)) {
                        for (var i = 0; i < data.termo_novo.termos_nao_autorizados.length; i++) {
                            value = data.termo_novo.termos_nao_autorizados[i].ch_termo_nao_autorizado + "#" + data.termo_novo.termos_nao_autorizados[i].nm_termo_nao_autorizado;
                            if ($('input[name="termos_nao_autorizados"][value="' + value + '"]', $('#termos_nao_autorizados_trocar_termo')).length <= 0) {
                                $('#termos_nao_autorizados_trocar_termo').append('<div><input type="hidden" name="termos_nao_autorizados_novo" value="' + value + '" />' + data.termo_novo.termos_nao_autorizados[i].nm_termo_nao_autorizado + '&nbsp;<a href="javascript:void();" onclick="javascript:RemoverDaLista(this);"><img src="' + _urlPadrao + '/Imagens/ico_delete_p.png" /></a></div>');
                            }
                        }
                    }
                    $('#div_nota_explicativa_trocar_termo').text(data.termo_novo.ds_nota_explicativa);
                    $('#div_fontes_pesquisadas_trocar_termo').text(data.termo_novo.ds_fontes_pesquisadas);
                    $('#div_texto_fonte_trocar_termo').text(data.termo_novo.ds_texto_fonte);
                }
                else {
                    $('#div_nm_termo_novo').text($('#nm_termo_novo_autocomplete').val());
                    $('#nm_termo_novo').val($('#nm_termo_novo_autocomplete').val());
                    $('#a_trocar_tipo_termo_novo').show();
                }
            }
        };
        var inicio = function () {
            $('#a_trocar_tipo_termo_novo').hide();

            $('#div_loading_trocar_termo').show();
            $('#div_trocar_termo').hide();

            $('#id_doc_termo_antigo').val('');
            $('#id_doc_termo_novo').val('');
            $('#nm_termo_novo').val('');
            $('#ch_tipo_termo_novo').val('');
            $('#div_nm_termo_novo').text('');

            $('#termos_gerais_trocar_termo').text('');
            $('#termos_especificos_trocar_termo').text('');
            $('#termos_relacionados_trocar_termo').text('');
            $('#termos_nao_autorizados_trocar_termo').text('');

            $('#div_nota_explicativa_trocar_termo').text('');
            $('#div_fontes_pesquisadas_trocar_termo').text('');
            $('#div_texto_fonte_trocar_termo').text('');
        }
        var complete = function () {
            $('#div_loading_trocar_termo').hide();
            $('#div_trocar_termo').show();
        }
        $.ajaxlight({
            sUrl: './ashx/Consulta/VocabularioTrocarTermoConsulta.ashx?id_doc_termo_antigo='+id_doc_antigo+'&id_doc_termo_novo='+id_doc_novo,
            sType: "GET",
            fnSuccess: sucesso,
            fnComplete: complete,
            fnBeforeSend: inicio,
            fnError: null,
            bAsync: true
        });
    }
}

/////////////////////
//Funções de listas//
/////////////////////

function ExibirListas() {
    var ch_termo = GetParameterValue("ch_termo");
    var sucesso = function (data) {
        if (IsNotNullOrEmpty(data)) {
            if (data.error_message != null && data.error_message != "") {
                $('#div_notificacao_vocabulario').messagelight({
                    sTitle: "Erro",
                    sContent: data.error_message,
                    sType: "error",
                    sWidth: "",
                    iTime: null
                });
            }
            if (IsNotNullOrEmpty(data, 'termos')) {
                var ul = PreencherLista(data.termos);
                $('#div_listas').html(ul);
                if (IsNotNullOrEmpty(data, 'termo_selecionado') && IsNotNullOrEmpty(ch_termo)) {
                    var li = PreencherListaComTermoSelecionado(data.termo_selecionado, id_termo, "", "");
                    $('li[termo="' + $(li).attr('termo') + '"]', $('ul', $('#div_listas'))).replaceWith(li);
                }
            }
        }
    };
    var inicio = function () {
        $('#div_loading_vocabulario').show();
        $('#div_vocabulario').hide();
    }
    var complete = function () {
        $('#div_loading_vocabulario').hide();
        $('#div_vocabulario').show();
    }
    $.ajaxlight({
        sUrl: MostrarPaginaAjax('VIS') + window.location.search,
        sType: "POST",
        fnSuccess: sucesso,
        fnComplete: complete,
        fnBeforeSend: inicio,
        bAsync: true
    });
}

function PreencherListaComTermoSelecionado(termo, ch_termo_selecionado, li_montado, ch_termo_montado) {
    var li = "";
    if (IsNotNullOrEmpty(termo) && IsNotNullOrEmpty(ch_termo_selecionado)) {
        li += '<li termo="' + termo.ch_termo + '">' + (termo.Id_Termo == id_termoSelecionado ? '<selected>' + termo.nm_termo + '</selected>' : termo.nm_termo) + (termo.ch_termo ? '<a href="javascript:void(0);" onclick="javascript:ExpandirLista(\'' + termo.ch_termo + '\',event);" title="Expandir"><img src="'+_urlPadrao+'/Imagens/ico_arrow_down.png" /></a>' : '') + '<a href="./DetalhesDeVocabulario.aspx?ch_termo=' + termo.ch_termo + '" title="Visualizar Detalhes"><img src="'+_urlPadrao+'/Imagens/ico_loupe_p.png" alt="detalhes" /></a>';
        var ul = "";
        if (IsNotNullOrEmpty(termo, 'sublistas')) {
            for (var i = 0; i < termo.sublistas.length; i++) {
                if (termo.TermosSublistas[i].ch_termo == ch_termo_montado) {
                    ul += li_montado;
                    continue;
                }
                ul += '<li termo="' + termo.sublistas[i].ch_termo + '">' + termo.sublistas[i].nm_termo + '<a href="javascript:void(0);" onclick="javascript:ExpandirLista(\'' + termo.sublistas[i].ch_termo + '\',event);" title="Expandir"><img src="'+_urlPadrao+'/Imagens/ico_arrow_down.png" style="margin-bottom: -3px;" /></a>' + '<a href="./DetalhesDeVocabulario.aspx?ch_termo=' + termo.sublistas[i].ch_termo + '" title="Visualizar Detalhes"><img src="'+_urlPadrao+'/Imagens/ico_loupe_p.png" alt="detalhes" /></a></li>';
            }
        }
        if (isNotNullOrEmpty(termo, 'itens')) {
            for (var i = 0; i < termo.itens.length; i++) {
                if (termo.itens[i].ch_termo == ch_termo_montado) {
                    ul += li_montado;
                    continue;
                }
                ul += '<li termo="' + termo.itens[i].ch_termo + '">' + termo.itens[i].nm_termo + '<a href="./DetalhesDeVocabulario.aspx?ch_termo=' + termo.itens[i].ch_termo + '" title="Visualizar Detalhes"><img src="'+_urlPadrao+'/Imagens/ico_loupe_p.png" alt="detalhes" /></a></li>';
            }
        }
        if (ul != "") {
            ul = "<ul>" + ul + "</ul>";
        }
        li += ul + '</li>';
        li = PreencherListaComTermoSelecionado(data.lista_pai, ch_termo_selecionado, li, termo.ch_termo);
    }
    if (li == "") {
        li = li_montado;
    }
    return li;
}

function PreencherLista(termos) {
    var ul = "<ul>";
    for (var i = 0; i < termos.length; i++) {
        if (termos[i].In_Excluir) continue;
        var li = '<li termo="' + termos[i].ch_termo + '">' + termos[i].nm_termo + (termos[i].in_lista ? '<a href="javascript:void(0);" onclick="javascript:ExpandirLista(\'' + termos[i].ch_termo + '\',event);" title="Expandir"><img src="'+_urlPadrao+'/Imagens/ico_arrow_down.png" /></a>' : '') + '<a href="./DetalhesDeVocabulario.aspx?ch_termo=' + termos[i].ch_termo + '" title="Visualizar Detalhes"><img src="'+_urlPadrao+'/Imagens/ico_loupe_p.png" alt="detalhes"/></a></li>';
        ul += li;
    }
    ul += '</ul>';
    return ul;
}

function ExpandirLista(ch_termo, e) {
    if (e.target.parentNode.parentNode.getElementsByTagName('ul').length > 0) {
        var uls = e.target.parentNode.parentNode.getElementsByTagName('ul');
        for (var i = 0; i < uls.length; i++) {
            e.target.parentNode.parentNode.removeChild(uls[i]);
        }
        e.target.src = _urlPadrao + "/Imagens/ico_arrow_down.png";
    }
    else {
        if (IsNotNullOrEmpty(ch_termo)) {
            var sucesso = function (data) {
                if (data != null) {
                    if (IsNotNullOrEmpty(data, 'termos')) {
                        var ul = PreencherLista(data.termos);
                        $(e.target.parentNode.parentNode).append(ul);
                        e.target.src = _urlPadrao + "/Imagens/ico_arrow_up.png";
                    }
                    else if (IsNotNullOrEmpty(data, 'error_message')) {
                        $('#div_notificacao_vocabulario').messagelight({
                            sTitle: "Erro",
                            sContent: data.error_message,
                            sType: "error",
                            sWidth: "",
                            iTime: null
                        });
                    }
                }
                else {
                    $('#div_notificacao_vocabulario').messagelight({
                        sTitle: "Erro",
                        sContent: "Não foi possível exibir as listas. Ocorreu um erro desconhecido.",
                        sType: "error",
                        sWidth: "",
                        iTime: null
                    });
                }
            };
            var inicio = function () {
                $('#div_loading_vocabulario').show();
                $('#div_vocabulario').hide();
            }
            var complete = function () {
                $('#div_loading_vocabulario').hide();
                $('#div_vocabulario').show();
            }
            $.ajaxlight({
                sUrl: MostrarPaginaAjax('VIS') + "?ch_lista=" + ch_termo,
                sType: "POST",
                fnSuccess: sucesso,
                fnComplete: complete,
                fnBeforeSend: inicio,
                bAsync: true
            });
        }
    }
}
//function AbrirModalFiltroPendentes() {
//    $('#div_modal_filtro_pendentes').modallight({
//        sTitle: "Filtrar por tipo",
//        sType: "default",
//        oButtons: [{
//            text: "Filtrar",
//            click: function () {
//                var checkboxes = $('input[type="checkbox"]:checked', this);
//                var filtro = $('input[type="checkbox"]:checked', this).serialize();
//                ClicarAbaPendentes(filtro);

//                $(this).dialog("close");
//            }
//        }]
//    });
//}
function AbrirModalFiltrarPorTipo(funcao_pesquisar) {
    $('#div_modal_filtro').modallight({
        sTitle: "Filtrar por tipo",
        sType: "default",
        oButtons: [{
            text: "Filtrar",
            click: function () {
                var checkboxes = $('input[type="checkbox"]:checked', this);
                var filtro = $('input[type="checkbox"]:checked', this).serialize();
                funcao_pesquisar(filtro);
                $(this).dialog("close");
            }
        }]
    });
}

function CriarModalDetalhesDoTermo(id_doc) {
    $('#div_modal_termo').modallight({
        sTitle: "Descrição do Termo",
        sWidth: '600',
        sContent: '<div id="descricao_termo" class="form"></div>',
        fnCreated: function () {
            var sucesso = function (data) {
                $('#descricao_termo').append(
                    '<fieldset class="w-95-pc">'+
                        '<legend id="label_tipo_termo">'+data.nm_tipo_termo+'</legend>' +
                        '<div class="mauto table">' +
                            '<div class="line">' +
                               '<div class="column w-100-pc">' +
                                    '<div class="cell">' +
                                        '<label id="label_nm_termo" style="font-size:16px; margin-left:50px; color:#333;">' + data.nm_termo + '</label>' +
                                    '</div>' +
                                '</div>' +
                            '</div>' +
                            '<div class="line div_termos_gerais" style="display:none;">' +
                                '<div class="column w-100-pc">' +
                                    '<div class="cell relacoes">' +
                                        '<label>TG</label><br />' +
                                        '<ul id="ul_termos_gerais">' +
                                        '</ul>' +
                                    '</div>' +
                                '</div>' +
                            '</div>' +
                            '<div class="line div_termos_especificos" style="display:none;">' +
                                '<div class="column w-100-pc">'+
                                    '<div class="cell relacoes">'+
                                        '<label>TE</label><br />'+
                                        '<ul id="ul_termos_especificos">' +
                                        '</ul>'+
                                    '</div>'+
                                '</div>'+
                            '</div>'+
                            '<div class="line div_termos_relacionados" style="display:none;">' +
                                '<div class="column w-100-pc">'+
                                    '<div class="cell relacoes">'+
                                        '<label>TR</label><br />'+
                                        '<ul id="ul_termos_relacionados">'+
                                        '</ul>'+
                                    '</div>'+
                                '</div>'+
                            '</div>'+
                            '<div class="line div_termos_itens" style="display:none;">'+
                                '<div class="column w-100-pc">'+
                                    '<div class="cell relacoes">'+
                                        '<label>ITENS</label><br />'+
                                        '<ul id="ul_termos_itens">'+
                                        '</ul>'+
                                    '</div>'+
                                '</div>'+
                            '</div>'+
                            '<div class="line div_termos_lista" style="display:none;">'+
                                '<div class="column w-100-pc">'+
                                    '<div class="cell relacoes">'+
                                        '<label>LISTA AUXILIAR</label><br />'+
                                        '<ul id="ul_termos_lista">'+
                                        '</ul>'+
                                    '</div>'+
                                '</div>'+
                            '</div>'+
                            '<div class="line div_termos_sublista" style="display:none;">'+
                                '<div class="column w-100-pc">'+
                                    '<div class="cell relacoes">'+
                                        '<label>SUBLISTA</label><br />'+
                                        '<ul id="ul_termos_sublista">'+
                                        '</ul>'+
                                    '</div>'+
                                '</div>'+
                            '</div>'+
                            '<div class="line div_termos_use" style="display:none;">'+
                                '<div class="column w-100-pc">'+
                                    '<div class="cell relacoes">'+
                                        '<label>USE</label><br />'+
                                        '<ul id="ul_termos_use">'+
                                        '</ul>'+
                                    '</div>'+
                                '</div>'+
                            '</div>'+
                            '<div class="line div_termos_nao_use" style="display:none;">'+
                                '<div class="column w-100-pc">'+
                                    '<div class="cell relacoes">'+
                                        '<label>NÃO USE</label><br />'+
                                        '<ul id="ul_termos_nao_use">'+
                                        '</ul>'+
                                    '</div>'+
                                '</div>'+
                            '</div>'+
                            '<div class="line">'+
                                '<div class="column w-30-pc">'+
                                    '<div class="cell fr">'+
                                        '<label>Nota Explicativa:</label>'+
                                    '</div>'+
                                '</div>'+
                                '<div class="column w-70-pc">'+
                                    '<div id="div_nota_explicativa" class="cell w-60-pc">'+
                                    '</div>'+
                                '</div>'+
                            '</div>'+
                            '<div class="line">'+
                                '<div class="column w-30-pc">'+
                                    '<div class="cell fr">'+
                                        '<label>Fontes Pesquisadas:</label>'+
                                    '</div>'+
                                '</div>'+
                                '<div class="column w-70-pc">'+
                                    '<div id="div_fontes_pesquisadas" class="cell w-60-pc">'+
                                    '</div>'+
                                '</div>'+
                            '</div>'+
                            '<div class="line">'+
                                '<div class="column w-30-pc">'+
                                    '<div class="cell fr">'+
                                        '<label>Norma de Origem:</label>'+
                                    '</div>'+
                                '</div>'+
                                '<div class="column w-70-pc">'+
                                    '<div id="div_texto_fonte" class="cell w-60-pc">'+
                                    '</div>'+
                                '</div>'+
                            '</div>'+
                        '</div>'+
                    '</fieldset>'
                );

                // Preenche os campos dinamicos
                if (data.eh_descritor) {
                    if (IsNotNullOrEmpty(data, 'termos_gerais')) {
                        for (var i = 0; i < data.termos_gerais.length; i++) {
                            $('#ul_termos_gerais').append('<li>' + data.termos_gerais[i].nm_termo_geral + '</li>');
                        }
                        $('.div_termos_gerais').show();
                    }
                    if (IsNotNullOrEmpty(data, 'termos_especificos')) {
                        for (var i = 0; i < data.termos_especificos.length; i++) {
                            $('#ul_termos_especificos').append('<li>' + data.termos_especificos[i].nm_termo_especifico + '</li>');
                        }
                        $('.div_termos_especificos').show();
                    }
                    if (IsNotNullOrEmpty(data, 'termos_relacionados')) {
                        for (var i = 0; i < data.termos_relacionados.length; i++) {
                            $('#ul_termos_relacionados').append('<li>' + data.termos_relacionados[i].nm_termo_relacionado + '</li>');
                        }
                        $('.div_termos_relacionados').show();
                    }
                }
                if (data.eh_lista_auxiliar) {
                    if (IsNotNullOrEmpty(data, 'sublistas')) {
                        for (var i = 0; i < data.sublistas.length; i++) {
                            $('#ul_termos_sublista').append('<li>' + data.sublistas[i].nm_termo + '</li>');
                        }
                        $('.div_termos_sublista').show();
                    }
                    if (IsNotNullOrEmpty(data, 'itens')) {
                        for (var i = 0; i < data.itens.length; i++) {
                            $('#ul_termos_itens').append('<li>' + data.itens[i].nm_termo + '</li>');
                        }
                        $('.div_termos_itens').show();
                    }
                    if (data.lista != null && data.lista.nm_termo) {
                        $('#ul_termos_lista').append('<li>' + data.lista.nm_termo + '</li>');
                        $('.div_termos_lista').show();
                    }
                }
                if (IsNotNullOrEmpty(data.nm_termo_use)) {
                    $('#ul_termos_use').append('<li>' + data.nm_termo_use + '</li>');
                    $('.div_termos_use').show();
                }
                if (IsNotNullOrEmpty(data, 'termos_nao_autorizados')) {
                    for (var i = 0; i < data.termos_nao_autorizados.length; i++) {
                        $('#ul_termos_nao_use').append('<li>' + data.termos_nao_autorizados[i].nm_termo_nao_autorizado + '</li>');
                    }
                    $('.div_termos_nao_use').show();
                }
                $('#div_nota_explicativa').text(GetText(data.ds_nota_explicativa));
                $('#div_fontes_pesquisadas').text(GetText(data.ds_fontes_pesquisadas));
                $('#div_texto_fonte').text(GetText(data.ds_texto_fonte));
                    
            }
            $.ajaxlight({
                sUrl: "./ashx/Visualizacao/VocabularioDetalhes.ashx?id_doc=" + id_doc,
                fnSuccess: sucesso,
                aSync: true
            });
        },
        fnClose: function () {
            LimparModal('div_modal_termo');
        }
    })
}

function fnTrocarTermo() {
    var id_form = 'form_trocar_termo';
    $('#' + id_form + ' .notify').html('');
    try {
        Validar(id_form);
        ValidarRegrasEspecificas(id_form);
        var sucesso = function (data) {
            $('#super_loading').hide();
            if (IsNotNullOrEmpty(data, 'error_message')) {
                $('#' + id_form + ' .notify').messagelight({
                    sContent: data.error_message,
                    sType: "error",
                });
            }
            else if(IsNotNullOrEmpty(data, 'update')){
                var tr_termos = "";
                for(var i = 0; i < data.termos_trocados.length; i++){
                    tr_termos += "<tr class='text-center "+((i % 2 == 0) ? 'EEE' : '' )+"'><td>" + data.termos_trocados[i].nm_termo_trocado + "</td>"+
                        "<td class='td_success text-center'>"+data.termos_trocados[i].id_docs_sucesso.length+"</td>"+
                        "<td class='td_error text-center'>"+data.termos_trocados[i].id_docs_erro.length+"</td>"+
                        "<td class='text-center'>"+data.termos_trocados[i].total_de_normas+"</td>"+
                        "<td class='text-center'>"+(data.termos_trocados[i].bExcluido ? "<img src='"+_urlPadrao+"/Imagens/ico_ok_p.png' width='16px' alt='sim' title='O termo antigo foi excluído com sucesso.'/>" : "<img src='"+_urlPadrao+"/Imagens/ico_delete_p.png' width='16px' alt='sim' title='O termo antigo não pôde ser excluído.'/>")+"</td>"+
                        "<td class='text-center'>"+(data.termos_trocados[i].bAtualizado ? "<img src='"+_urlPadrao+"/Imagens/ico_ok_p.png' width='16px' alt='sim' title='O termo foi atualizado com as novas informações.'/>" : "<img src='"+_urlPadrao+"/Imagens/ico_ok_p.png' width='16px' alt='sim' title='O termo não foi atualizado com as novas informações.'/>" )+"</td>"+
                        "<td>"+getVal(data.termos_trocados[i].error_message)+"</td></tr>";
                }
                var table = "<table class='relatorio_troca' cellspacing='0'><caption>Relatório da troca</caption><tr>"+
                    "<th class='text-center'>Termo</th>"+
                    "<th class='text-center'>Normas/Sucesso</th>"+
                    "<th class='text-center'>Normas/Erro</th>"+
                    "<th class='text-center'>Normas/Total</th>"+
                    "<th class='text-center'>Excluir termo antigo</th>"+
                    "<th class='text-center'>Atualização</th>"+
                    "<th class='text-center'>Observação</th></tr>"+
                        tr_termos+
                    "</table>";
                var mensagem = "Total de normas usando o termo atual: <b>"+data.total_de_normas_usando_o_termo_novo+"</b><br/>"+
                    table;
                $('<div id="modal_notificacao_modal_salvar" />').modallight({
                    sTitle: "Sucesso",
                    sContent: mensagem,
                    sWidth: "800",
                    sType: "success",
                    oButtons: [{ text: "Ok", click: function () { $(this).dialog('close'); } }],
                    fnClose: function () {
                        CancelarTrocarTermo();
                        $('#modal_notificacao_modal_salvar').dialog('destroy');
                        $('#modal_notificacao_modal_salvar').remove();
                    }
                });
            }
            else {
                $('#' + id_form + ' .notify').messagelight({
                    sTitle: "Erro",
                    sContent: "Erro durante a troca.",
                    sType: "error",
                    sWidth: "",
                    iTime: null
                });
            }
        }
        var beforeSubmit = function () {
            $('#super_loading').show();
        }
        $.ajaxlight({
            sUrl: './ashx/Cadastro/VocabularioTrocar.ashx',
            sType: "POST",
            fnSuccess: sucesso,
            sFormId: id_form,
            fnBeforeSubmit: beforeSubmit,
            bAsync: true,
            iTimeout: 600000
        });
    }
    catch (ex) {
        $('#' + id_form + ' .notify').messagelight({
            sTitle: "Erro nos dados informados",
            sContent: ex,
            sType: "error"
        });
        $("html, body").animate({ scrollTop: 0 }, "slow");
    }
    return false;
}



/*******************
TERMOS RESTAURADOS
********************/

function continuarTrocarTermosRestaurados() {
    var ch_termos_antigos = [];
    var inputs = $('#div_termos_antigos_restaurados input[type="hidden"][name="ch_termo_antigo_restaurados"]');
    for (var i = 0; i < inputs.length; i++ ) {
        ch_termos_antigos.push(inputs[i].value);
    }
    var id_doc_novo = $('#id_doc_termo_novo_restaurados_autocomplete').val();
    if (IsNotNullOrEmpty(ch_termos_antigos)) {
        var sucesso = function (data) {
            if (IsNotNullOrEmpty(data, 'error_message')) {
                $('#div_notificacao_trocar_termo_restaurados').messagelight({
                    sTitle: "Erro",
                    sContent: data.error_message,
                    sType: "error",
                    sWidth: "",
                    iTime: null
                });
            }
            else {
                if (IsNotNullOrEmpty(data, 'termos_antigos')) {
                    $('#ch_tipo_termo_novo_restaurados').val(data.termos_antigos[0].ch_tipo_termo);
                    $('#div_nm_tipo_termo_restaurados').html(data.termos_antigos[0].nm_tipo_termo + '<a id="a_trocar_tipo_termo_novo_restaurados" href="javascript:void(0);" onclick="javascrpit:TrocarTipoTermoNovoRestaurados();" title="Trocar o Tipo de Termo" style="display:none;" ><img src="' + _urlPadrao + '/Imagens/ico_pencil_p.png"  alt="trocar" /></a>');

                    for (var t = 0; t < data.termos_antigos.length; t++) {
                        if (data.termos_antigos[t].eh_descritor) {
                            var value = "";
                            if (IsNotNullOrEmpty(data.termos_antigos[t].termos_gerais)) {
                                for (var i = 0; i < data.termos_antigos[t].termos_gerais.length; i++) {
                                    value = data.termos_antigos[t].termos_gerais[i].ch_termo_geral + "#" + data.termos_antigos[t].termos_gerais[i].nm_termo_geral;
                                    if ($('input[name="termos_gerais"][value="' + value + '"]', $('#termos_gerais_trocar_termo_restaurados')).length <= 0) {
                                        $('#termos_gerais_trocar_termo_restaurados').append('<div><input type="hidden" name="termos_gerais_antigo" value="' + value + '" />' + data.termos_antigos[t].termos_gerais[i].nm_termo_geral + '&nbsp;<a href="javascript:void();" onclick="javascript:RemoverDaLista(this);"><img src="' + _urlPadrao + '/Imagens/ico_delete_p.png" /></a></div>');
                                    }
                                }
                            }
                            if (IsNotNullOrEmpty(data.termos_antigos[t].termos_especificos)) {
                                for (var i = 0; i < data.termos_antigos[t].termos_especificos.length; i++) {
                                    value = data.termos_antigos[t].termos_especificos[i].ch_termo_especifico + "#" + data.termos_antigos[t].termos_especificos[i].nm_termo_especifico;
                                    if ($('input[name="termos_especificos"][value="' + value + '"]', $('#termos_especificos_trocar_termo_restaurados')).length <= 0) {
                                        $('#termos_especificos_trocar_termo_restaurados').append('<div><input type="hidden" name="termos_especificos_antigo" value="' + value + '" />' + data.termos_antigos[t].termos_especificos[i].nm_termo_especifico + '&nbsp;<a href="javascript:void();" onclick="javascript:RemoverDaLista(this);"><img src="' + _urlPadrao + '/Imagens/ico_delete_p.png" /></a></div>');
                                    }
                                }
                            }
                            if (IsNotNullOrEmpty(data.termos_antigos[t].termos_relacionados)) {
                                for (var i = 0; i < data.termos_antigos[t].termos_relacionados.length; i++) {
                                    value = data.termos_antigos[t].termos_relacionados[i].ch_termo_relacionado + "#" + data.termos_antigos[t].termos_relacionados[i].nm_termo_relacionado;
                                    if ($('input[name="termos_relacionados"][value="' + value + '"]', $('#termos_relacionados_trocar_termo_restaurados')).length <= 0) {
                                        $('#termos_relacionados_trocar_termo_restaurados').append('<div><input type="hidden" name="termos_relacionados_antigo" value="' + value + '" />' + data.termos_antigos[t].termos_relacionados[i].nm_termo_relacionado + '&nbsp;<a href="javascript:void();" onclick="javascript:RemoverDaLista(this);"><img src="' + _urlPadrao + '/Imagens/ico_delete_p.png" /></a></div>');
                                    }
                                }
                            }
                        }
                        if (IsNotNullOrEmpty(data.termos_antigos[t].termos_nao_autorizados)) {
                            for (var i = 0; i < data.termos_antigos[t].termos_nao_autorizados.length; i++) {
                                value = data.termos_antigos[t].termos_nao_autorizados[i].ch_termo_nao_autorizado + "#" + data.termos_antigos[t].termos_nao_autorizados[i].nm_termo_nao_autorizado;
                                if ($('input[name="termos_nao_autorizados"][value="' + value + '"]', $('#termos_nao_autorizados_trocar_termo_restaurados')).length <= 0) {
                                    $('#termos_nao_autorizados_trocar_termo_restaurados').append('<div><input type="hidden" name="termos_nao_autorizados_antigo" value="' + value + '" />' + data.termos_antigos[t].termos_nao_autorizados[i].nm_termo_nao_autorizado + '&nbsp;<a href="javascript:void();" onclick="javascript:RemoverDaLista(this);"><img src="' + _urlPadrao + '/Imagens/ico_delete_p.png" /></a></div>');
                                }
                            }
                        }
                        $('#div_termos_antigos_restaurados_trocar').append('<input type="hidden" name="ch_termo_antigo_restaurados" value="' + data.termos_antigos[t].ch_termo + '"/>');
                    }
                }

                if (IsNotNullOrEmpty(data.termo_novo)) {

                    $('#id_doc_termo_novo_restaurados').val(data.termo_novo._metadata.id_doc);
                    $('#nm_termo_novo_restaurados').val(data.termo_novo.nm_termo);
                    $('#div_nm_termo_novo_restaurados').text(data.termo_novo.nm_termo);

                    $('#ch_tipo_termo_novo_restaurados').val(data.termo_novo.ch_tipo_termo);
                    $('#div_nm_tipo_termo_restaurados').text(data.termo_novo.nm_tipo_termo);

                    if (data.termo_novo.eh_descritor) {
                        var value = "";
                        $('div.DE').show();
                        if (IsNotNullOrEmpty(data.termo_novo.termos_gerais)) {
                            for (var i = 0; i < data.termo_novo.termos_gerais.length; i++) {
                                value = data.termo_novo.termos_gerais[i].ch_termo_geral + "#" + data.termo_novo.termos_gerais[i].nm_termo_geral;
                                if ($('input[name="termos_gerais"][value="' + value + '"]', $('#termos_gerais_trocar_termo_restaurados')).length <= 0) {
                                    $('#termos_gerais_trocar_termo_restaurados').append('<div><input type="hidden" name="termos_gerais_novo" value="' + value + '" />' + data.termo_novo.termos_gerais[i].nm_termo_geral + '&nbsp;<a href="javascript:void();" onclick="javascript:RemoverDaLista(this);"><img src="' + _urlPadrao + '/Imagens/ico_delete_p.png" /></a></div>');
                                }
                            }
                        }
                        if (IsNotNullOrEmpty(data.termo_novo.termos_especificos)) {
                            for (var i = 0; i < data.termo_novo.termos_especificos.length; i++) {
                                value = data.termo_novo.termos_especificos[i].ch_termo_especifico + "#" + data.termo_novo.termos_especificos[i].nm_termo_especifico;
                                if ($('input[name="termos_especificos"][value="' + value + '"]', $('#termos_especificos_trocar_termo_restaurados')).length <= 0) {
                                    $('#termos_especificos_trocar_termo_restaurados').append('<div><input type="hidden" name="termos_especificos_novo" value="' + value + '" />' + data.termo_novo.termos_especificos[i].nm_termo_especifico + '&nbsp;<a href="javascript:void();" onclick="javascript:RemoverDaLista(this);"><img src="' + _urlPadrao + '/Imagens/ico_delete_p.png" /></a></div>');
                                }
                            }
                        }
                        if (IsNotNullOrEmpty(data.termo_novo.termos_relacionados)) {
                            for (var i = 0; i < data.termo_novo.termos_relacionados.length; i++) {
                                value = data.termo_novo.termos_relacionados[i].ch_termo_relacionado + "#" + data.termo_novo.termos_relacionados[i].nm_termo_relacionado;
                                if ($('input[name="termos_relacionados"][value="' + value + '"]', $('#termos_relacionados_trocar_termo_restaurados')).length <= 0) {
                                    $('#termos_relacionados_trocar_termo_restaurados').append('<div><input type="hidden" name="termos_relacionados_novo" value="' + value + '" />' + data.termo_novo.termos_relacionados[i].nm_termo_relacionado + '&nbsp;<a href="javascript:void();" onclick="javascript:RemoverDaLista(this);"><img src="' + _urlPadrao + '/Imagens/ico_delete_p.png" /></a></div>');
                                }
                            }
                        }
                    }
                    if (IsNotNullOrEmpty(data.termo_novo.termos_nao_autorizados)) {
                        for (var i = 0; i < data.termo_novo.termos_nao_autorizados.length; i++) {
                            value = data.termo_novo.termos_nao_autorizados[i].ch_termo_nao_autorizado + "#" + data.termo_novo.termos_nao_autorizados[i].nm_termo_nao_autorizado;
                            if ($('input[name="termos_nao_autorizados"][value="' + value + '"]', $('#termos_nao_autorizados_trocar_termo_restaurados')).length <= 0) {
                                $('#termos_nao_autorizados_trocar_termo_restaurados').append('<div><input type="hidden" name="termos_nao_autorizados_novo" value="' + value + '" />' + data.termo_novo.termos_nao_autorizados[i].nm_termo_nao_autorizado + '&nbsp;<a href="javascript:void();" onclick="javascript:RemoverDaLista(this);"><img src="' + _urlPadrao + '/Imagens/ico_delete_p.png" /></a></div>');
                            }
                        }
                    }
                }
                else {
                    $('#div_nm_termo_novo_restaurados').text($('#nm_termo_novo_autocomplete').val());
                    $('#nm_termo_novo_restaurados').val($('#nm_termo_novo_autocomplete').val());
                    $('#a_trocar_tipo_termo_novo_restaurados').show();
                }

                $('#div_selecionados_trocar_termos_restaurados').hide();
                $('#form_trocar_termos_restaurados').show();
            }
        };
        var inicio = function () {
            $('#a_trocar_tipo_termo_novo_restaurados').hide();
            $('#super_loading').show();

            $('#div_termos_antigos_restaurados_trocar').html('');

            $('#id_doc_termo_novo_restaurados').val('');
            $('#nm_termo_novo_restaurados').val('');
            $('#ch_tipo_termo_novo_restaurados').val('');
            $('#div_nm_termo_novo_restaurados').text('');

            $('#termos_gerais_trocar_termo_restaurados').text('');
            $('#termos_especificos_trocar_termo_restaurados').text('');
            $('#termos_relacionados_trocar_termo_restaurados').text('');
            $('#termos_nao_autorizados_trocar_termo_restaurados').text('');

        }
        var complete = function () {
            $('#super_loading').hide();
        }
        $.ajaxlight({
            sUrl: './ashx/Consulta/VocabularioTrocarTermoConsulta.ashx?ch_termos_antigos=' + ch_termos_antigos + '&id_doc_termo_novo=' + id_doc_novo,
            sType: "GET",
            fnSuccess: sucesso,
            fnComplete: complete,
            fnBeforeSend: inicio,
            fnError: null,
            bAsync: true
        });
    }
}

function trocarTipoTermoNovoRestaurados() {
    $('#div_modal_trocar_tipo_termo_novo').modallight({
        sTitle: "Trocar Tipo do Termo",
        sWidth: '400',
        oButtons: [{
            text: "Ok",
            click: function () {
                if ($('#select_trocar_tipo_termo_novo').val() != "") {
                    $('#ch_tipo_termo_novo_restaurados').val($('#select_trocar_tipo_termo_novo').val());
                    $('#div_nm_tipo_termo_restaurados').html($('#select_trocar_tipo_termo_novo option[value="' + $('#select_trocar_tipo_termo_novo').val() + '"]').text() + '<a id="a_trocar_tipo_termo_novo_restaurados" href="javascript:void(0);" onclick="javascript:trocarTipoTermoNovoRestaurados();" title="Trocar o Tipo de Termo" ><img src="' + _urlPadrao + '/Imagens/ico_pencil_p.png"  alt="trocar" /></a>');
                    $(this).dialog('close');
                }
            }
        },
        {
            text: "Cancelar",
            click: function () {
                $(this).dialog('close');
            }
        }]
    });
}

function trocarTermosRestaurados(ch_termo, nm_termo) {
    var chaves = [];
    if (!IsNotNullOrEmpty(ch_termo)) {
        var inputs = $('input[type="checkbox"][name="ch_termo"]:checked');
        for (var i = 0; i < inputs.length; i++) {
            chaves.push({ ch_termo: $(inputs[i]).attr('ch_termo'), nm_termo: $(inputs[i]).attr('nm_termo') });
        }
    }
    else {
        chaves.push({ ch_termo: ch_termo, nm_termo: nm_termo });
    }
    if (chaves.length > 0) {
        var nm_termos = '';
        var ch_termos = '';
        for(var i = 0; i < chaves.length; i++){
            $('#div_termos_antigos_restaurados').append('<input type="hidden" name="ch_termo_antigo_restaurados" value="' + chaves[i].ch_termo + '"/>');
            nm_termos += chaves[i].nm_termo + '<br/>';
            ch_termos += (ch_termos != "" ? "," : "") + chaves[i].ch_termo;
        }
        $('#div_termos_antigos_restaurados').append(nm_termos);
        $('#div_autocomplete_termo_novo_restaurados').autocompletelight({
            sKeyDataName: "_metadata.id_doc",
            sValueDataName: "nm_termo",
            sInputHiddenName: "id_doc_termo_novo_restaurados_autocomplete",
            sInputName: "nm_termo_novo_restaurados_autocomplete",
            sAjaxUrl: "./ashx/Autocomplete/VocabularioAutocomplete.ashx?ch_termo_ignoreds=" + ch_termos,
            bLinkAll: true,
            sLinkName: "a_termo_novo_restaurados",
            bAddInexistente: true,
            jPagination: { bPaginate: true,
                    iDisplayLength: 20,
                    bButtonsPaginate: true
                }
        });
        $('#div_selecionar_termos_restaurados').hide();
        $('#div_selecionados_trocar_termos_restaurados').show();
        $('#form_trocar_termos_restaurados').hide();
    }
}

function fnTrocarTermosRestaurados() {
    var id_form = 'form_trocar_termos_restaurados';
    $('#' + id_form + ' .notify').html('');
    try {
        Validar(id_form);
        ValidarRegrasEspecificas(id_form);
        var sucesso = function (data) {
            $('#super_loading').hide();
            if (IsNotNullOrEmpty(data, 'error_message')) {
                $('#' + id_form + ' .notify').messagelight({
                    sContent: data.error_message,
                    sType: "error",
                });
            }
            else if(IsNotNullOrEmpty(data, 'update')){
                var tr_termos = "";
                for(var i = 0; i < data.termos_trocados.length; i++){
                    tr_termos += "<tr class='text-center "+((i % 2 == 0) ? 'EEE' : '' )+"'><td>" + data.termos_trocados[i].nm_termo_trocado + "</td>"+
                        "<td class='td_success text-center'>"+data.termos_trocados[i].id_docs_sucesso.length+"</td>"+
                        "<td class='td_error text-center'>"+data.termos_trocados[i].id_docs_erro.length+"</td>"+
                        "<td class='text-center'>"+data.termos_trocados[i].total_de_normas+"</td>"+
                        "<td class='text-center'>"+(data.termos_trocados[i].bExcluido ? "<img src='"+_urlPadrao+"/Imagens/ico_ok_p.png' width='16px' alt='sim' title='O termo antigo foi excluído com sucesso.'/>" : "<img src='"+_urlPadrao+"/Imagens/ico_delete_p.png' width='16px' alt='sim' title='O termo antigo não pôde ser excluído.'/>")+"</td>"+
                        "<td class='text-center'>"+(data.termos_trocados[i].bAtualizado ? "<img src='"+_urlPadrao+"/Imagens/ico_ok_p.png' width='16px' alt='sim' title='O termo foi atualizado com as novas informações.'/>" : "<img src='"+_urlPadrao+"/Imagens/ico_ok_p.png' width='16px' alt='sim' title='O termo não foi atualizado com as novas informações.'/>" )+"</td>"+
                        "<td>"+getVal(data.termos_trocados[i].error_message)+"</td></tr>";
                }
                var table = "<table class='relatorio_troca' cellspacing='0'><caption>Relatório da troca</caption><tr>"+
                    "<th class='text-center'>Termo</th>"+
                    "<th class='text-center'>Normas/Sucesso</th>"+
                    "<th class='text-center'>Normas/Erro</th>"+
                    "<th class='text-center'>Normas/Total</th>"+
                    "<th class='text-center'>Excluir termo antigo</th>"+
                    "<th class='text-center'>Atualização</th>"+
                    "<th class='text-center'>Observação</th></tr>"+
                        tr_termos+
                    "</table>";
                var mensagem = "Total de normas usando o termo atual: <b>"+data.total_de_normas_usando_o_termo_novo+"</b><br/>"+
                    table;
                $('<div id="modal_notificacao_modal_salvar" />').modallight({
                    sTitle: "Sucesso",
                    sContent: mensagem,
                    sWidth: "800",
                    sType: "success",
                    oButtons: [{ text: "Ok", click: function () { $(this).dialog('close'); } }],
                    fnClose: function () {
                        cancelarTrocarTermosRestaurados();
                        $('#modal_notificacao_modal_salvar').dialog('destroy');
                        $('#modal_notificacao_modal_salvar').remove();
                    }
                });
            }
            else {
                $('#' + id_form + ' .notify').messagelight({
                    sTitle: "Erro",
                    sContent: "Erro durante a troca.",
                    sType: "error",
                    sWidth: "",
                    iTime: null
                });
            }
        }
        var beforeSubmit = function () {
            $('#super_loading').show();
        }
        $.ajaxlight({
            sUrl: './ashx/Cadastro/VocabularioTrocar.ashx',
            sType: "POST",
            fnSuccess: sucesso,
            sFormId: id_form,
            fnBeforeSubmit: beforeSubmit,
            bAsync: true,
            iTimeout: 600000
        });
    }
    catch (ex) {
        $('#' + id_form + ' .notify').messagelight({
            sTitle: "Erro nos dados informados",
            sContent: ex,
            sType: "error"
        });
        $("html, body").animate({ scrollTop: 0 }, "slow");
    }
    return false;
}

function cancelarTrocarTermosRestaurados(id_doc, nm_termo, ch_tipo_termo) {
    $('#div_selecionar_termos_restaurados').show();
    ClicarAbaRestaurados();
    $('#div_selecionados_trocar_termos_restaurados').hide();
    $('#form_trocar_termos_restaurados').hide();
    
    $('#div_termos_antigos_restaurados').html('');

    $('#div_termos_antigos_restaurados_trocar').html('');


    $('#id_doc_termo_novo_restaurados_autocomplete').val('');
    $('#nm_termo_novo_restaurados_autocomplete').val('');

    
    $('#form_trocar_termos_restaurados input').val('');

    $('#div_nm_tipo_termo_restaurados').html('');
    $('#div_nm_tipo_termo_restaurados').text('');

    $('#div_termos_antigos_restaurados_trocar').html('');
    $('#div_termos_antigos_restaurados_trocar').text('');

    $('#div_nm_termo_novo_restaurados').html('');
    $('#div_nm_termo_novo_restaurados').text('');

    $('#termos_gerais_trocar_termo_restaurados').html('');
    $('#termos_gerais_trocar_termo_restaurados').text('');

    $('#termos_nao_autorizados_trocar_termo_restaurados').html('');
    $('#termos_nao_autorizados_trocar_termo_restaurados').text('');

    $('#termos_relacionados_trocar_termo_restaurados').html('');
    $('#termos_relacionados_trocar_termo_restaurados').text('');

    $('#termos_especificos_trocar_termo_restaurados').html('');
    $('#termos_especificos_trocar_termo_restaurados').text('');

    return false;
}
