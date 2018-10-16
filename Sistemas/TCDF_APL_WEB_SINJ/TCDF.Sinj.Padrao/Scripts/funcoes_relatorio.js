function ConstruirControlesDinamicos() {
    ConstruirOrgaosCadastradores();
    $('#div_autocomplete_tipo_norma').autocompletelight({
        sKeyDataName: "ch_tipo_norma",
        sValueDataName: "nm_tipo_norma",
        sInputHiddenName: "ch_tipo_norma",
        sInputName: "nm_tipo_norma",
        sAjaxUrl: "./ashx/Autocomplete/TipoDeNormaAutocomplete.ashx",
        bLinkAll: true,
        sLinkName: "a_tipo_norma"
    });
    $('#div_autocomplete_origem_modal').autocompletelight({
        sKeyDataName: "ch_orgao",
        sValueDataName: "get_sg_hierarquia_nm_vigencia",
        sInputHiddenName: "ch_orgao",
        sInputName: "sg_hierarquia_nm_vigencia",
        sAjaxUrl: './ashx/Autocomplete/OrgaoAutocomplete.ashx',
        bLinkAll: true,
        sLinkName: "a_origem"
    });
    $('#div_autocomplete_usuario_cadastrador_modal').autocompletelight({
        sKeyDataName: "nm_login_usuario",
        sValueDataName: "nm_usuario",
        sInputHiddenName: "nm_login_usuario",
        sInputName: "nm_usuario",
        sAjaxUrl: './ashx/Autocomplete/UsuarioAutocomplete.ashx',
        bLinkAll: true,
        sLinkName: "a_usuario"
    });
    $('#div_autocomplete_usuario_alterador_modal').autocompletelight({
        sKeyDataName: "nm_login_usuario",
        sValueDataName: "nm_usuario",
        sInputHiddenName: "nm_login_usuario_alterador",
        sInputName: "nm_usuario_alterador",
        sAjaxUrl: './ashx/Autocomplete/UsuarioAutocomplete.ashx',
        bLinkAll: true,
        sLinkName: "a_usuario_alterador"
    });


    $('#button_pesquisar_norma').click(function () {
        return PesquisarNormas();
    });
    $('#button_termos_por_tipo').click(function () {
        ConsultarTotalDeTermos();
    });
    $('#button_termos_homonimos').click(function () {
        ConsultarTermosHomonimos();
    });
    $('#button_termos_nao_autorizados').click(function () {
        ConsultarTermosNaoAutorizados();
    });
    $('#button_notifiqueme').click(function () {
        ConsultarUsuariosNotifiqueme();
    });
}

function PesquisarNormas() {
    $('#form_relatorio_norma .notify').html('');
    try {
        ValidarPesquisa('form_relatorio_norma');
        ValidarRegrasEspecificas('form_relatorio_norma');
        $("#div_relatorio_norma").dataTablesLight({
            sAjaxUrl: './ashx/Datatable/NormaDatatable.ashx?' + $('#form_relatorio_norma').serialize(),
            aoColumns: _columns_norma,
            sIdTable: "tabela_relatorio_norma"
        });
        return false;
    } catch (ex) {
        $('#form_relatorio_norma .notify').messagelight({
            sTitle: "Erro",
            sContent: ex,
            sType: "error"
        });
    }
    return false;
}

function GerarPlanilhaTermos(tipo) {
    window.open('./RelatorioDeVocabulario.aspx?tipo=' + tipo, '_blank', 'width=400,height=200');
}

function ConsultarTotalDeTermos() {
    $('#div_termos_por_tipo .notify').html('');
    $("#div_total_termos_por_tipo").html('');
    var sucesso = function (data) {
        if (IsNotNullOrEmpty(data.error_message)) {
            $('#div_termos_por_tipo .notify').messagelight({
                sTitle: "Erro",
                sContent: data.error_message,
                sType: "error",
                sWidth: "",
                iTime: null
            });
        }
        else if (IsNotNullOrEmpty(data.total)) {
            var totais = [{ "name": "Descritor", "total": data.de, "tipo": "DE" }, { "name": "Especificador", "total": data.es, "tipo": "ES" }, { "name": "Autoridade", "total": data.au, "tipo": "AU" }, { "name": "Lista Auxiliar", "total": data.la, "tipo": "LA" }, { name: "Total", total: data.total, "tipo": "*"}];
            $("#div_total_termos_por_tipo").dataTablesLight({
                bServerSide: false,
                aoData: totais,
                aoColumns: _columns_total_termos_tipo,
                sIdTable: "tabela_termos_por_tipo"
            });
        }
        else {
            $('#div_termos_por_tipo .notify').messagelight({
                sTitle: "Erro",
                sContent: "Ocorreu um erro desconhecido.",
                sType: "error",
                sWidth: "",
                iTime: null
            });
        }
    }
    var inicio = function () {
        $('#div_termos_por_tipo .loading').show();
        $('#div_termos_por_tipo .loaded').hide();
    }
    var complete = function () {
        $('#div_termos_por_tipo .loading').hide();
        $('#div_termos_por_tipo .loaded').show();
    }
    $.ajaxlight({
        sUrl: './ashx/Consulta/VocabularioTotalConsulta.ashx',
        sType: "GET",
        fnSuccess: sucesso,
        fnComplete: complete,
        fnBeforeSend: inicio,
        bAsync: true
    });
    return false;
}

function ConsultarTermosHomonimos() {
    $('#div_termos_homonimos .notify').html('');
    $("#div_tabela_termos_homonimos").html('');
    var sucesso = function (data) {
        if (IsNotNullOrEmpty(data, 'error_message')) {
            $('#div_termos_homonimos .notify').messagelight({
                sTitle: "Erro",
                sContent: data.error_message,
                sType: "error",
                sWidth: "",
                iTime: null
            });
        }
        else if (data.termos_homonimos != null) {
            $("#div_tabela_termos_homonimos").dataTablesLight({
                bServerSide: false,
                aoData: data.termos_homonimos,
                aoColumns: _columns_termos_homonimos,
                sIdTable: "tabela_termos_homonimos"
            });
        }
        else {
            $('#div_termos_homonimos .notify').messagelight({
                sTitle: "Erro",
                sContent: "Ocorreu um erro desconhecido.",
                sType: "error",
                sWidth: "",
                iTime: null
            });
        }
    }
    var inicio = function () {
        $('#div_termos_homonimos .loading').show();
        $('#div_termos_homonimos .loaded').hide();
    }
    var complete = function () {
        $('#div_termos_homonimos .loading').hide();
        $('#div_termos_homonimos .loaded').show();
    }
    $.ajaxlight({
        sUrl: './ashx/Consulta/VocabularioHomonimosConsulta.ashx',
        sType: "GET",
        fnSuccess: sucesso,
        fnComplete: complete,
        fnBeforeSend: inicio,
        bAsync: true
    });
    return false;
}

function ConsultarTermosNaoAutorizados(filtro) {
    var search = window.location.search;
    $("#div_tabela_termos_nao_autorizados").dataTablesLight({
        sAjaxUrl: './ashx/Datatable/VocabularioDatatable.ashx?in_nao_autorizado=true' + (IsNotNullOrEmpty(filtro) ? '&' + filtro : ''),
        aoColumns: _columns_vocabulario_nao_autorizados,
        sIdTable: "tabela_termos_nao_autorizados"
    });
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



function BuscarUsuarioInativo() {
    //Alteração
    var autocomplete_usuario_alteracao = {
        sKeyDataName: "nm_login_usuario",
        sValueDataName: "nm_usuario",
        sInputHiddenName: "nm_login_usuario_alterador",
        sInputName: "nm_usuario_alterador",
        sAjaxUrl: './ashx/Autocomplete/UsuarioAutocomplete.ashx',
        bLinkAll: true,
        sLinkName: "a_usuario_alterador",
        jDataSumit: { "inativo": $('#checkbox_alteracao_inativo').prop("checked") }
    };
    $('#div_autocomplete_usuario_alterador_modal').autocompletelight(autocomplete_usuario_alteracao);
    // Cadastro
    var autocomplete_usuario_cadastro = {
        sKeyDataName: "nm_login_usuario",
        sValueDataName: "nm_usuario",
        sInputHiddenName: "nm_login_usuario",
        sInputName: "nm_usuario",
        sAjaxUrl: './ashx/Autocomplete/UsuarioAutocomplete.ashx',
        bLinkAll: true,
        sLinkName: "a_usuario",
        jDataSumit: { "inativo": $('#checkbox_cadastro_inativo').prop("checked") }
    };
    $('#div_autocomplete_usuario_cadastrador_modal').autocompletelight(autocomplete_usuario_cadastro);
}
