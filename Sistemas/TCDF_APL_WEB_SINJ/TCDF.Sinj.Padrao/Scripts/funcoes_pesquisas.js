function montarPaginaDePesquisa() {
    $('.accordion').accordion({
        animate: 300,
        heightStyle: "content"
    });
    RecuperarAccordion();
    $('#div_autocomplete_tipo_norma').autocompletelight({
        sAjaxUrl: './ashx/Autocomplete/TipoDeNormaAutocomplete.ashx',
        sInputName: 'nm_tipo_norma',
        sInputHiddenName: 'ch_tipo_norma',
        bLinkAll: true,
        sLinkName: 'a_tipo_norma',
        sKeyDataName: 'ch_tipo_norma',
        sValueDataName: 'nm_tipo_norma'
    });


    $('#div_autocomplete_assunto').autocompletelight({
        sKeyDataName: "ch_termo",
        sValueDataName: "nm_termo",
        sInputHiddenName: "ch_termo_assunto",
        sInputName: "nm_termo_assunto",
        sAjaxUrl: './ashx/Autocomplete/VocabularioAutocomplete.ashx',
        bLinkAll: true,
        sLinkName: "a_assunto"
    });
    $('#div_autocomplete_origem').autocompletelight({
        sKeyDataName: "ch_orgao",
        sValueDataName: "get_sg_hierarquia_nm_vigencia",
        sInputHiddenName: "ch_orgao",
        sInputName: "sg_hierarquia_nm_vigencia",
        sAjaxUrl: './ashx/Autocomplete/OrgaoAutocomplete.ashx',
        bLinkAll: true,
        sLinkName: "a_origem",
        dOthersHidden: [
                { campo_app: "ch_hierarquia", campo_base: "ch_hierarquia" }
            ]
    });
    $('#div_autocomplete_tipo_fonte').autocompletelight({
        sKeyDataName: "ch_tipo_fonte",
        sValueDataName: "nm_tipo_fonte",
        sInputHiddenName: "ch_tipo_fonte",
        sInputName: "nm_tipo_fonte",
        sAjaxUrl: './ashx/Autocomplete/TipoDeFonteAutocomplete.ashx',
        bLinkAll: true,
        sLinkName: "a_tipo_fonte"
    });
    $('#div_autocomplete_tipo_edicao').autocompletelight({
        sKeyDataName: "ch_tipo_edicao",
        sValueDataName: "nm_tipo_edicao",
        sInputHiddenName: "ch_tipo_edicao",
        sInputName: "nm_tipo_edicao",
        sAjaxUrl: './ashx/Autocomplete/TipoDeEdicaoAutocomplete.ashx',
        bLinkAll: true,
        sLinkName: "a_tipo_edicao"
    });
    $('#div_autocomplete_campo').autocompletelight({
        sKeyDataName: "ch_campo",
        sValueDataName: "nm_campo",
        sInputHiddenName: "ch_campo",
        sInputName: "nm_campo",
        jData: _campos_pesquisa_avancada,
        bLinkAll: true,
        sLinkName: "a_campo",
        dOthersHidden: [
                { campo_app: "type", campo_base: "type" },
                { campo_app: "data", campo_base: "data" },
                { campo_app: "url_ajax", campo_base: "url_ajax" },
                { campo_app: "sKeyDataName", campo_base: "sKeyDataName" },
                { campo_app: "sValueDataName", campo_base: "sValueDataName" },
                { campo_app: "ch_operador_padrao", campo_base: "ch_operador_padrao" },
                { campo_app: "nm_operador_padrao", campo_base: "nm_operador_padrao" }
            ]
    });
    SelecionarCampo();
    AdicionarArgumento();

    $('#a_pesquisa_geral').click(function () {
        if (Pesquisar('form_pesquisa_geral')) {
            $("#form_pesquisa_geral").submit();
        }
    });
    $('#button_pesquisa_norma').click(function () {
        return Pesquisar('form_pesquisa_norma');
    });
    $('#button_pesquisa_diario').click(function () {
        return Pesquisar('form_pesquisa_diario');
    });
    $('#button_pesquisa_avancada').click(function () {
        return Pesquisar('form_pesquisa_avancada');
    });
    $('#ch_tipo_norma_todas').prop("checked", true);
    $('#line_assuntos').hide();
    SelecionarTipoDeNorma();
    SelecionarOrigem();
    SelecionarOperador();


    var tipo_pesquisa = GetParameterValue("tipo_pesquisa");
    if (IsNotNullOrEmpty(tipo_pesquisa)) {
        RecuperarDoHistorico(tipo_pesquisa);
    }

    // Coloca o parametro (ou substitui) com o valor do accordion ativo
    $('.accordion').on("accordionactivate", function () {
        var accordion_ativo = $(this).accordion("option", "active");
        history.replaceState({ accordion: accordion_ativo }, "", "");
    });
}

function RecuperarDoHistorico(tipo_pesquisa) {
    switch (tipo_pesquisa) {
        case "geral":
            $(".accordion").accordion("option", "active", 0);
            var _qualquer_campo = GetParameterValueDecode("all").replace(/\+/g, " ");
            // Os parametros vem com o caracter "+" no lugar dos espaços. 
            // Isso causa um comportamento inesperado, por isso estão sendo substituídos novamento por um espaço em branco.
            if (IsNotNullOrEmpty(_qualquer_campo)) {
                $('#input_geral_all').val(_qualquer_campo);
            }
            break;
        case "norma":
            $(".accordion").accordion("option", "active", 1);
            var _qualquer_campo = GetParameterValueDecode("all").replace(/\+/g, " ");
            var _ch_tipo_norma = GetParameterValueDecode("ch_tipo_norma");
            var _nm_tipo_norma = GetParameterValueDecode("nm_tipo_norma").replace(/\+/g, " ");
            var _nr_norma = GetParameterValueDecode("nr_norma");
            var _ano_assinatura = GetParameterValueDecode("ano_assinatura");
            var _ch_termo = GetParameterValueDecode("ch_termo");
            var _nm_termo = GetParameterValueDecode("nm_termo").replace(/\+/g, " ");
            var _ch_orgao = GetParameterValueDecode("ch_orgao");
            var _ch_hierarquia = GetParameterValueDecode("ch_hierarquia").replace(/\+/g, " ");
            var _origem_por = GetParameterValueDecode("origem_por").replace(/\+/g, " ");
            var _sg_hierarquia_nm_vigencia = GetParameterValueDecode("sg_hierarquia_nm_vigencia").replace(/\+/g, " ");
            if (IsNotNullOrEmpty(_qualquer_campo)) {
                $('#input_norma_all').val(_qualquer_campo);
            }
            if (IsNotNullOrEmpty(_ch_tipo_norma)) {
                $('#ch_tipo_norma').val(_ch_tipo_norma);
            }
            if (IsNotNullOrEmpty(_nm_tipo_norma)) {
                $('#nm_tipo_norma').val(_nm_tipo_norma);
            }
            if (IsNotNullOrEmpty(_nr_norma)) {
                $('input[name=nr_norma]').val(_nr_norma);
            }
            if (IsNotNullOrEmpty(_ano_assinatura)) {
                $('input[name=ano_assinatura]').val(_ano_assinatura);
            }
            if (IsNotNullOrEmpty(_ch_termo) && IsNotNullOrEmpty(_nm_termo)) {
                var split_ch_termo = _ch_termo.split(",");
                var split_nm_termo = _nm_termo.split(",");
                for (var i = 0; i < split_ch_termo.length; i++) {
                    $('#ch_termo_assunto').val(split_ch_termo[i]);
                    $('#nm_termo_assunto').val(split_nm_termo[i]);
                    AdicionarAssunto();
                }
            }
            if (IsNotNullOrEmpty(_ch_orgao)) {
                $('#ch_orgao').val(_ch_orgao);
            }
            if (IsNotNullOrEmpty(_ch_hierarquia)) {
                $('#ch_hierarquia').val(_ch_hierarquia);
            }
            if (IsNotNullOrEmpty(_sg_hierarquia_nm_vigencia)) {
                $('#sg_hierarquia_nm_vigencia').val(_sg_hierarquia_nm_vigencia);
            }
            if (IsNotNullOrEmpty(_ch_orgao) && IsNotNullOrEmpty(_ch_hierarquia)) {
                SelecionarOrigem();
            }
            if (IsNotNullOrEmpty(_origem_por)) {
                $('select[name=origem_por').val(_origem_por);
            }
            break;
        case "diario":
            $(".accordion").accordion("option", "active", 2);
            var _qualquer_campo = GetParameterValueDecode("all").replace(/\+/g, " ");
            var _ch_tipo_fonte = GetParameterValueDecode("ch_tipo_fonte");
            var _nm_tipo_fonte = GetParameterValueDecode("nm_tipo_fonte").replace(/\+/g, " ");
            var _nr_diario = GetParameterValueDecode("nr_diario");
            var _secao_diario = GetParameterValueDecode("secao_diario").replace(/\+/g, " ");
            var _filetext = GetParameterValueDecode("filetext").replace(/\+/g, " ");
            var _op_dt_assinatura = GetParameterValueDecode("op_dt_assinatura").replace(/\+/g, " ");
            var _dt_assinatura = GetParameterValueDecode("dt_assinatura");

            if (IsNotNullOrEmpty(_qualquer_campo)) {
                $('#input_diario_all').val(_qualquer_campo);
            }
            if (IsNotNullOrEmpty(_ch_tipo_fonte)) {
                $('#ch_tipo_fonte').val(_ch_tipo_fonte);
            }
            if (IsNotNullOrEmpty(_nm_tipo_fonte)) {
                $('#nm_tipo_fonte').val(_nm_tipo_fonte);
            }
            if (IsNotNullOrEmpty(_nr_diario)) {
                $('input[name=nr_diario]').val(_nr_diario);
            }
            if (IsNotNullOrEmpty(_secao_diario)) {
                $('input[name=secao_diario]').val(_secao_diario);
            }
            if (IsNotNullOrEmpty(_filetext)) {
                $('input[name=filetext]').val(_filetext);
            }
            if (IsNotNullOrEmpty(_op_dt_assinatura)) {
                $('#op_dt_assinatura').val(_op_dt_assinatura);
                SelecionarOperador();
            }
            if (IsNotNullOrEmpty(_dt_assinatura)) {
                var split_dt_assinatura = _dt_assinatura.split(",");
                $('input[name=dt_assinatura]').eq(0).val(split_dt_assinatura[0]);
                $('input[name=dt_assinatura]').eq(1).val(split_dt_assinatura[1]);
            }
            break;
        case "avancada":
            $(".accordion").accordion("option", "active", 3);
            var _ch_tipo_norma = GetParameterValueDecode("ch_tipo_norma");
            var _argumento = GetParameterValueDecode("argumento").replace(/\+/g, " ");
            var split_argumento = _argumento.split(",");
            for (var i = 0; i < split_argumento.length; i++) {
                var campos_argumento = split_argumento[i].split("#");
                TestAndAppendArguments(campos_argumento[0], campos_argumento[1], campos_argumento[2], campos_argumento[3], campos_argumento[4], campos_argumento[5], campos_argumento[6], campos_argumento[7]);
            }

            if (_ch_tipo_norma == "todas") {
                $('#ch_tipo_norma_todas').prop("checked", true);
            }
            else {
                $('#ch_tipo_norma_todas').prop("checked", false);
                SelecionarTipoDeNorma();
                var split_ch_tipo_norma = _ch_tipo_norma.split(",");
                for (var i = 0; i < split_ch_tipo_norma.length; i++) {
                    $('input[value=' + split_ch_tipo_norma[i] + ']').prop("checked", true);
                }
            }
            break;
    }
}

function AdicionarAssunto() {
    var ch_termo = $('#ch_termo_assunto').val();
    var nm_termo = $('#nm_termo_assunto').val();
    if (IsNotNullOrEmpty(ch_termo) && IsNotNullOrEmpty(nm_termo)) {
        if ($('input[value="' + ch_termo + '"]', $('#assuntos')).length <= 0) {
            $('#assuntos').append('<div class="assunto"><input type="hidden" name="ch_termo" value="' + ch_termo + '" /><input type="hidden" name="nm_termo" value="' + nm_termo + '" />' + nm_termo + '<a href="javascript:void(0);" onclick="javascript:RemoverAssunto(event);">x</a></div>');
        }
    }
    if ($('input', $('#assuntos')).length > 0) {
        $('#line_assuntos').show();
    }
    $('#ch_termo_assunto').val('');
    $('#nm_termo_assunto').val('');
}

function RemoverAssunto(event) {
    $(event.target).closest("div.assunto").remove();
    if ($('input', $('#assuntos')).length <= 0) {
        $('#line_assuntos').hide();
    }
}

function SelecionarOrigem() {
    if ($('#ch_orgao').val() != "") {
        $('#line_origem_por').show();
    }
    else {
        $('#line_origem_por').hide();
    }
}

function SelecionarOperadorPesquisaAvancada() {
    if ($('#type').val() == 'date' || $('#type').val() == 'datetime' || $('#type').val() == 'number') {
        if ($('#ch_operador').val() == 'intervalo') {
            $('#div_valor').html('<input id="ch_valor" value="" class="w-30-pc" type="text">&nbsp;até&nbsp;<input id="ch_valor_2" value="" class="w-30-pc" type="text">');
        }
        else {
            $('#div_valor').html('<input id="ch_valor" value="" class="w-30-pc" type="text">');
        }
    }
    if ($('#type').val() == 'date' || $('#type').val() == 'datetime') {
        inicializarDatePicker({ element: $('#div_valor input') });
    }
}

function SelecionarOperador() {
    if ($('#op_dt_assinatura').val() == 'intervalo') {
        $('#div_intervalo').show();
    }
    else {
        $('#div_intervalo').hide();
    }
}

function SelecionarCampo() {
    var ch_campo = $('#ch_campo').val();
    var type = $('#type').val();
    var data = $('#data').val();
    var url_ajax = $('#url_ajax').val();
    var sKeyDataName = $('#sKeyDataName').val();
    var sValueDataName = $('#sValueDataName').val();
    var ch_operador_padrao = $('#ch_operador_padrao').val();
    var nm_operador_padrao = $('#nm_operador_padrao').val();
    if (IsNotNullOrEmpty(ch_operador_padrao)) {
        $('#ch_operador').val(ch_operador_padrao);
        $('#nm_operador').val(nm_operador_padrao);
    }
    else {
        $('#ch_operador').val('igual');
        $('#nm_operador').val('igual a');
    }
    $('#div_valor').html('<input id="ch_valor" value="" class="w-90-pc" type="text">');
    if (type != "") {
        $('#ch_operador').prop('disabled', false);
        $('#nm_operador').prop('disabled', false);
        $('#div_autocomplete_operador').autocompletelight({
            sKeyDataName: "ch_operador",
            sValueDataName: "nm_operador",
            sInputHiddenName: "ch_operador",
            sInputName: "nm_operador",
            jData: _operadores_pesquisa_avancada[type],
            bLinkAll: true,
            sLinkName: "a_operador"
        });
        if (type == 'autocomplete') {
            $('#div_valor').html('<input id="ch_valor" type="hidden" value="" /><input id="nm_valor" type="text" value="" class="w-90-pc" /><a title="Listar" id="a_valor"></a>');
            $('#div_valor').autocompletelight({
                sKeyDataName: sKeyDataName,
                sValueDataName: sValueDataName,
                sInputHiddenName: "ch_valor",
                sInputName: "nm_valor",
                jData: eval(data),
                sAjaxUrl: url_ajax,
                bLinkAll: true,
                sLinkName: "a_valor"
            });
        }
        else if (type == 'date' || type == 'datetime') {
            $('#div_valor').html('<input id="ch_valor" value="" class="w-30-pc" type="text"/>');
            inicializarDatePicker({ element: $('#div_valor input') });
        }
        else if (type == 'number') {
            $('#div_valor').html('<input id="ch_valor" value="" class="w-30-pc" type="number"/>');
        }
    }
    else {
        $('#ch_operador').val('');
        $('#nm_operador').val('');
        $('#ch_operador').prop('disabled', true);
        $('#nm_operador').prop('disabled', true);
        $('#a_operador').css('display', 'none');

        $('#ch_valor').val('');
        $('#ch_valor').prop('disabled', true);
    }
}

function SelecionarConector(element) {
    id_hidden_argument = $(element).attr('id_hidden_argument');
    var argument = $('#' + id_hidden_argument).val();
    var argument_splited = argument.split('#');
    if (argument_splited.length > 0) {
        argument_splited[argument_splited.length - 1] = $(element).val();
    }
    $('#' + id_hidden_argument).val(argument_splited.join('#'));
}

function TestAndAppendArguments(type, ch_campo, nm_campo, ch_operador, nm_operador, ch_valor, nm_valor, ch_conector) {
    var valor_concatenado = type + '#' + ch_campo + '#' + nm_campo + '#' + ch_operador + '#' + nm_operador + '#' + ch_valor + '#' + nm_valor;
    var argumentos = $('input[name="argumento"]');
    for (var i = 0; i < argumentos.length; i++) {
        if (argumentos[i].value.indexOf(valor_concatenado) > -1) {
            $('#form_pesquisa_avancada .notify').messagelight({
                sType: 'error',
                sContent: 'Argumento repetido'
            });
            return false;
        }
    }
    if (IsNotNullOrEmpty(ch_campo) && IsNotNullOrEmpty(type) && IsNotNullOrEmpty(ch_operador) && IsNotNullOrEmpty(ch_valor)) {
        var guid = Guid();
        $('#div_argumentos').append(
                    '<div class="line line_argument">' +
                        '<div class="column w-20-pc">' +
                            '<div class="cell w-100-pc">' +
                                '<input id="' + guid + '" type="hidden" name="argumento" value="' + valor_concatenado + '#' + (IsNotNullOrEmpty(ch_conector) ? ch_conector : "E") + '" />' +
                                nm_campo +
                            '</div>' +
                        '</div>' +
                        '<div class="column w-10-pc">' +
                            '<div class="cell w-100-pc">' +
                                nm_operador +
                            '</div>' +
                        '</div>' +
                        '<div class="column w-50-pc">' +
                            '<div class="cell w-100-pc">' +
                                nm_valor +
                            '</div>' +
                        '</div>' +
                        '<div class="column w-10-pc">' +
                            '<div class="cell w-100-pc">' +
                                '<select id_hidden_argument="'+guid+'" onchange="javascript:SelecionarConector(this)"><option value="E" ' + ((ch_conector == "E" || !IsNotNullOrEmpty(ch_conector)) ? 'selected="selected"' : '') + '>E</option><option value="OU" ' + (ch_conector == "OU" ? 'selected="selected"' : '') + '>OU</option><option value="NÃO" ' + (ch_conector == "NÃO" ? 'selected="selected"' : '') + '>NÃO</option></select>' +
                            '</div>' +
                        '</div>' +
                        '<div class="column w-10-pc">' +
                            '<div class="cell w-100-pc">' +
                                '<a href="javascript:void(0);" onclick="javascript:DeletarArgumento(event);"><img alt="remover" src="' + _urlPadrao + '/Imagens/ico_trash_p.png" /></a>' +
                            '</div>' +
                        '</div>' +
                    '</div>'
                );
    }
    ExibirArgumentos();
}

function AdicionarArgumento() {
    $('#form_pesquisa_avancada .notify').html('');
    $('#form_pesquisa_avancada .notify').hide();
    var ch_campo = $('#ch_campo').val();
    var nm_campo = $('#nm_campo').val();
    var type = $('#type').val();
    var ch_operador = $('#ch_operador').val();
    var nm_operador = $('#nm_operador').val();
    var ch_valor = $('#ch_valor').val();
    var nm_valor = $('#ch_valor').val();
    if (type == 'autocomplete') {
        nm_valor = $('#nm_valor').val();
    }
    else if ((type == 'date' || type == 'datetime' || type == 'number') && ch_operador == 'intervalo') {
        ch_valor = $('#ch_valor').val() + "," + $('#ch_valor_2').val();
        nm_valor = $('#ch_valor').val() + " até " + $('#ch_valor_2').val();
    }
    TestAndAppendArguments(type, ch_campo, nm_campo, ch_operador, nm_operador, ch_valor, nm_valor)
}

function ExibirArgumentos() {
    if ($('.line_argument', $('#div_argumentos')).length > 0) {
        $('#form_pesquisa_avancada').show();
        $('.line_argument select').show();
        $('.line_argument:last select').hide();
    }
    else {
        $('#form_pesquisa_avancada').hide();
    }
}

function DeletarArgumento(event) {
    $(event.target).closest("div.line_argument").remove();
    ExibirArgumentos();
}

function ClicarExibirTodosTipos(event) {
    var text = $(event.target).text();
    if (text.trim(' ') == "Todos Tipos") {
        $('#div_tipos_de_norma div.tipos').show();
        $(event.target).text("Principais Tipos");
    }
    else {
        $('#div_tipos_de_norma div.tipos').hide();
        $('#div_tipos_de_norma div.principais').show();
        $(event.target).text("Todos Tipos");
    }
}

function SelecionarTipoDeNorma() {
    if (!$('#ch_tipo_norma_todas').is(':checked')) {
        if ($('#div_tipos_de_norma').html() == "") {
            var sucesso = function (data) {
                if (IsNotNullOrEmpty(data.results)) {
                    for (var i = 0; i < data.results.length; i++) {
                        $('#div_tipos_de_norma').append('<div' + (data.results[i].EhLei || data.results[i].EhDecreto || data.results[i].EhResolucao || data.results[i].EhResolucao ? ' class="principais tipos"' : ' class="tipos"') + ' style="display:none;width:100%;"><label><input type="checkbox" name="ch_tipo_norma" value="' + data.results[i].ch_tipo_norma + '"/>' + data.results[i].nm_tipo_norma + '<label></div>')
                    }
                    $('#div_tipos_de_norma').append('<div style="width:100%;"><a href="javascript:void(0);" onclick="javascript:ClicarExibirTodosTipos(event)">Todos Tipos</a></div>');
                    $('#div_tipos_de_norma div.principais').show();
                }
            }
            var inicio = function () {
                $('#loading_tipos_de_norma').show();
                $('#div_tipos_de_norma').hide();
            }
            var complete = function () {
                $('#loading_tipos_de_norma').hide();
                $('#div_tipos_de_norma').show();
            }
            $.ajaxlight({
                sUrl: './ashx/Consulta/TipoDeNormaConsulta.ashx',
                sType: "GET",
                fnSuccess: sucesso,
                fnComplete: complete,
                fnBeforeSend: inicio
            });
        }
        else {
            $('#div_tipos_de_norma').show();
        }
    }
    else {
        $('#div_tipos_de_norma').hide();
    }
}

function RecuperarAccordion() {
    if (IsNotNullOrEmpty(history, "state.accordion")) {
        try {
            var accordion_historico = history.state.accordion;
        }
        catch (ex) { }
    }
    if (IsNotNullOrEmpty(accordion_historico)) {
        $(".accordion").accordion("option", "active", accordion_historico);
    }
    else {
        $(".accordion").accordion("option", "active", 0);
    }
}
