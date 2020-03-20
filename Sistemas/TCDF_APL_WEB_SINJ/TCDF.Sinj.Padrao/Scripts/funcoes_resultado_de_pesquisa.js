var doc_clicked = "";
var id_file = "";
var text = "";

$(document).ready(function () {
    var tipo_pesquisa = GetParameterValue('tipo_pesquisa');
    var nm_aba = '';
    var nm_base_aux_total = null;
    if (tipo_pesquisa == "geral") {
        $('#tabs_pesquisa').html(
            '<ul>' +
                '<a href="javascript:void(0);" onclick="javascript:RestaurarDefaultDatatable();" class="fr" title="Restaurar Padrão"><img alt="xls" src="' + _urlPadrao + '/Imagens/ico_restore_p.png"/></a>' +
                '<li nm="sinj_norma"><a class="aba" href="#div_resultado_norma" onclick="javascript:ClicarAba(\'sinj_norma\');">Normas <span id="total_sinj_norma" class="total"></span></a></li>' +
                '<li nm="sinj_diario"><a class="aba" href="#div_resultado_diario" onclick="javascript:ClicarAba(\'sinj_diario\');">Diários <span id="total_sinj_diario" class="total"></span></a></li>' +
                '<li nm="cesta"><a class="aba" href="#div_cesta" onclick="javascript:ClicarAba(\'cesta\');">Cesta <span id="total_cesta" class="total"></span> <img alt="cesta" style="height:14px; width:16px;" src="' + _urlPadrao + '/Imagens/ico_basket_p.png" /></a></li>' +
            '</ul>' +
            '<div id="div_resultado_norma" class="result_datatable"><div id="div_sort_sinj_norma" class="sort_datatable" style="display:none;" >Ordenar por <select table="datatable_normas" name="select_order_norma" onchange="javascript:selectOrder(\'datatable_normas\',this);"><option value="1,asc">Menor Relevância</option><option value="1,desc" selected="selected">Maior Relevância</option><option value="2,asc">Tipo e Número (A-Z)</option><option value="2,desc">Tipo e Número (Z-A)</option><option value="3,asc">Mais Antigos</option><option value="3,desc">Mais Recentes</option></select></div><div id="div_aggregations_norma" class="agg_datatable">' + montarFiltro("sinj_norma") + '</div><div id="div_datatable_norma" class="table_datatable"></div><div id="div_sugestoes_sinj_norma" class="sugestoes" style="display:none;">A sua pesquisa não encontrou Normas<span id="span_obs_encontrados_sinj_diario"></span>.<br/>Seguem abaixo algumas sugestões que podem ajudar a encontrar o que procura:<div id="datatable_sugestoes_sinj_norma"></div></div></div>' +
            '<div id="div_resultado_diario" class="result_datatable"><div id="div_sort_sinj_diario" class="sort_datatable" style="display:none;">Ordenar por <select table="datatable_diarios" name="select_order_diario" onchange="javascript:selectOrder(\'datatable_diarios\',this);"><option value="1,asc">Menor Relevância</option><option value="1,desc" selected="selected">Maior Relevância</option><option value="2,asc">Tipo (A-Z)</option><option value="2,desc">Tipo (Z-A)</option><option value="3,asc">Menor Número</option><option value="3,desc">Maior Número</option><option value="4,asc">Menor Seção</option><option value="4,desc">Maior Seção</option><option value="5,asc">Mais Antigos</option><option value="5,desc">Mais Recentes</option></select></div><div id="div_aggregations_diario" class="agg_datatable">' + montarFiltro("sinj_diario") + '</div><div id="div_datatable_diario" class="table_datatable"></div><div id="div_sugestoes_sinj_diario" class="sugestoes" style="display:none;">A sua pesquisa não encontrou Diários<span id="span_obs_encontrados_sinj_norma"></span>.<br/>Seguem abaixo algumas sugestões que podem ajudar a encontrar o que procura:<div id="datatable_sugestoes_sinj_diario"></div></div></div>' +
            '<div id="div_cesta">' +
                '<ul>' +
                    '<a href="javascript:void(0);" onclick="javascript:RelatorioPDF();" class="fr" title="Salvar planilha"><img alt="xls" src="' + _urlPadrao + '/Imagens/ico_xls_p.png"/></a>' +
                    '<a href="javascript:void(0);" onclick="javascript:EsvaziarCesta();" class="fr" title="Limpar Cesta"><img alt="cesta" src="' + _urlPadrao + '/Imagens/ico_trash_m.png" /></a>' +
                    '<li nm="cesta_norma"><a class="a" href="#div_cesta_norma" onclick="javascript:ClicarAba(\'cesta_norma\')">Normas <span id="total_cesta_norma"></span></a></li>' +
                    '<li nm="cesta_diario"><a class="a" href="#div_cesta_diario" onclick="javascript:ClicarAba(\'cesta_diario\')">Diários <span id="total_cesta_diario"></span></a></li>' +
                '</ul>' +
                '<div id="div_cesta_norma"></div>' +
                '<div id="div_cesta_diario"></div>' +
            '</div>'
        ).tabs();
        nm_aba = 'sinj_norma';
    }
	
	    if (tipo_pesquisa == "pendenteDePesquisa") {
        $('#tabs_pesquisa').html(
            '<ul>' +
                '<a href="javascript:void(0);" onclick="javascript:RestaurarDefaultDatatable();" class="fr" title="Restaurar Padrão"><img alt="xls" src="' + _urlPadrao + '/Imagens/ico_restore_p.png"/></a>' +
                '<li nm="sinj_norma"><a class="aba" href="#div_resultado_norma" onclick="javascript:ClicarAba(\'sinj_norma\');">Normas <span id="total_sinj_norma" class="total"></span></a></li>' +
                '<li nm="sinj_diario"><a class="aba" href="#div_resultado_diario" onclick="javascript:ClicarAba(\'sinj_diario\');">Diários <span id="total_sinj_diario" class="total"></span></a></li>' +
                '<li nm="cesta"><a class="aba" href="#div_cesta" onclick="javascript:ClicarAba(\'cesta\');">Cesta <span id="total_cesta" class="total"></span> <img alt="cesta" style="height:14px; width:16px;" src="' + _urlPadrao + '/Imagens/ico_basket_p.png" /></a></li>' +
            '</ul>' +
            '<div id="div_resultado_norma" class="result_datatable"><div id="div_sort_sinj_norma" class="sort_datatable" style="display:none;" >Ordenar por <select table="datatable_normas" name="select_order_norma" onchange="javascript:selectOrder(\'datatable_normas\',this);"><option value="1,asc">Menor Relevância</option><option value="1,desc" selected="selected">Maior Relevância</option><option value="2,asc">Tipo e Número (A-Z)</option><option value="2,desc">Tipo e Número (Z-A)</option><option value="3,asc">Mais Antigos</option><option value="3,desc">Mais Recentes</option></select></div><div id="div_aggregations_norma" class="agg_datatable">' + montarFiltro("sinj_norma") + '</div><div id="div_datatable_norma" class="table_datatable"></div><div id="div_sugestoes_sinj_norma" class="sugestoes" style="display:none;">A sua pesquisa não encontrou Normas<span id="span_obs_encontrados_sinj_diario"></span>.<br/>Seguem abaixo algumas sugestões que podem ajudar a encontrar o que procura:<div id="datatable_sugestoes_sinj_norma"></div></div></div>' +
            '<div id="div_resultado_diario" class="result_datatable"><div id="div_sort_sinj_diario" class="sort_datatable" style="display:none;">Ordenar por <select table="datatable_diarios" name="select_order_diario" onchange="javascript:selectOrder(\'datatable_diarios\',this);"><option value="1,asc">Menor Relevância</option><option value="1,desc" selected="selected">Maior Relevância</option><option value="2,asc">Tipo (A-Z)</option><option value="2,desc">Tipo (Z-A)</option><option value="3,asc">Menor Número</option><option value="3,desc">Maior Número</option><option value="4,asc">Menor Seção</option><option value="4,desc">Maior Seção</option><option value="5,asc">Mais Antigos</option><option value="5,desc">Mais Recentes</option></select></div><div id="div_aggregations_diario" class="agg_datatable">' + montarFiltro("sinj_diario") + '</div><div id="div_datatable_diario" class="table_datatable"></div><div id="div_sugestoes_sinj_diario" class="sugestoes" style="display:none;">A sua pesquisa não encontrou Diários<span id="span_obs_encontrados_sinj_norma"></span>.<br/>Seguem abaixo algumas sugestões que podem ajudar a encontrar o que procura:<div id="datatable_sugestoes_sinj_diario"></div></div></div>' +
            '<div id="div_cesta">' +
                '<ul>' +
                    '<a href="javascript:void(0);" onclick="javascript:RelatorioPDF();" class="fr" title="Salvar planilha"><img alt="xls" src="' + _urlPadrao + '/Imagens/ico_xls_p.png"/></a>' +
                    '<a href="javascript:void(0);" onclick="javascript:EsvaziarCesta();" class="fr" title="Limpar Cesta"><img alt="cesta" src="' + _urlPadrao + '/Imagens/ico_trash_m.png" /></a>' +
                    '<li nm="cesta_norma"><a class="a" href="#div_cesta_norma" onclick="javascript:ClicarAba(\'cesta_norma\')">Normas <span id="total_cesta_norma"></span></a></li>' +
                    '<li nm="cesta_diario"><a class="a" href="#div_cesta_diario" onclick="javascript:ClicarAba(\'cesta_diario\')">Diários <span id="total_cesta_diario"></span></a></li>' +
                '</ul>' +
                '<div id="div_cesta_norma"></div>' +
                '<div id="div_cesta_diario"></div>' +
            '</div>'
        ).tabs();
        nm_aba = 'sinj_norma';
    }
	
    else if (tipo_pesquisa == "norma") {
        $('#tabs_pesquisa').html(
            '<ul>' +
                '<li nm="sinj_norma"><a class="aba" href="#div_resultado_norma" onclick="javascript:ClicarAba(\'sinj_norma\');">Normas <span id="total_sinj_norma" class="total"></span></a></li>' +
                '<li nm="cesta"><a class="aba" href="#div_cesta" onclick="javascript:ClicarAba(\'cesta\');">Cesta <span id="total_cesta" class="total"></span></a></li>' +
            '</ul>' +
            '<div id="div_resultado_norma" class="result_datatable"><div id="div_sort_sinj_norma" class="sort_datatable" style="display:none;">Ordenar por <select table="datatable_normas" name="select_order_norma" onchange="javascript:selectOrder(\'datatable_normas\',this);"><option value="1,asc">Menor Relevância</option><option value="1,desc" selected="selected">Maior Relevância</option><option value="2,asc">Tipo e Número (A-Z)</option><option value="2,desc">Tipo e Número (Z-A)</option><option value="3,asc">Mais Antigos</option><option value="3,desc">Mais Recentes</option></select></div><div id="div_aggregations_norma" class="agg_datatable">' + montarFiltro("sinj_norma") + '</div><div id="div_datatable_norma" class="table_datatable"></div><div id="div_sugestoes_sinj_norma" class="sugestoes" style="display:none;">A sua pesquisa não encontrou Normas.<br/>Seguem abaixo algumas sugestões que podem ajudar a encontrar o que procura:<div id="datatable_sugestoes_sinj_norma"></div></div></div>' +
            '<div id="div_cesta">' +
                '<ul>' +
                    '<li nm="cesta_norma"><a class="a" href="#div_cesta_norma" onclick="javascript:ClicarAba(\'cesta_norma\')">Normas</a></li>' +
                '</ul>' +
                '<div id="div_cesta_norma"></div>' +
            '</div>'
        ).tabs();
        nm_aba = 'sinj_norma';
        nm_base_aux_total = nm_aba;
    }
    else if (tipo_pesquisa == "diario" || tipo_pesquisa == "notifiqueme") {
        $('#tabs_pesquisa').html(
            '<ul>' +
                '<li nm="sinj_diario"><a class="aba" href="#div_resultado_diario" onclick="javascript:ClicarAba(\'sinj_diario\');">Diários <span id="total_sinj_diario" class="total"></span></a></li>' +
                '<li nm="cesta"><a class="aba" href="#div_cesta" onclick="javascript:ClicarAba(\'cesta\');">Cesta <span id="total_cesta" class="total"></span></a></li>' +
            '</ul>' +
            '<div id="div_resultado_diario" class="result_datatable"><div id="div_sort_sinj_diario" class="sort_datatable" style="display:none;">Ordenar por <select table="datatable_diarios" name="select_order_diario" onchange="javascript:selectOrder(\'datatable_diarios\',this);"><option value="1,asc">Menor Relevância</option><option value="1,desc" selected="selected">Maior Relevância</option><option value="2,asc">Tipo (A-Z)</option><option value="2,desc">Tipo (Z-A)</option><option value="3,asc">Menor Número</option><option value="3,desc">Maior Número</option><option value="4,asc">Menor Seção</option><option value="4,desc">Maior Seção</option><option value="5,asc">Mais Antigos</option><option value="5,desc">Mais Recentes</option></select></div><div id="div_aggregations_diario" class="agg_datatable">' + montarFiltro("sinj_diario") + '</div><div id="div_datatable_diario" class="table_datatable"></div><div id="div_sugestoes_sinj_diario" class="sugestoes" style="display:none;">A sua pesquisa não encontrou Diários.<br/>Seguem abaixo algumas sugestões que podem ajudar a encontrar o que procura:<div id="datatable_sugestoes_sinj_diario"></div></div></div>' +
            '<div id="div_cesta">' +
                '<ul>' +
                    '<li nm="cesta_diario"><a class="a" href="#div_cesta_diario" onclick="javascript:ClicarAba(\'cesta_diario\')">Diários</a></li>' +
                '</ul>' +
                '<div id="div_cesta_diario"></div>' +
            '</div>'
        ).tabs();
        nm_aba = 'sinj_diario';
        nm_base_aux_total = nm_aba;
    }
    else if (tipo_pesquisa == "avancada") {
        $('#tabs_pesquisa').html(
            '<ul>' +
                '<li nm="sinj_norma"><a class="aba" href="#div_resultado_norma" onclick="javascript:ClicarAba(\'sinj_norma\');">Normas <span id="total_sinj_norma" class="total"></span></a></li>' +
                '<li nm="cesta"><a class="aba" href="#div_cesta" onclick="javascript:ClicarAba(\'cesta\');">Cesta <span id="total_cesta" class="total"></span></a></li>' +
            '</ul>' +
            '<div id="div_resultado_norma" class="result_datatable"><div id="div_sort_sinj_norma" class="sort_datatable" style="display:none;">Ordenar por <select table="datatable_normas" name="select_order_norma" onchange="javascript:selectOrder(\'datatable_normas\',this);"><option value="1,asc">Menor Relevância</option><option value="1,desc" selected="selected">Maior Relevância</option><option value="2,asc">Tipo e Número (A-Z)</option><option value="2,desc">Tipo e Número (Z-A)</option><option value="3,asc">Mais Antigos</option><option value="3,desc">Mais Recentes</option></select></div><div id="div_aggregations_norma" class="agg_datatable">' + montarFiltro("sinj_norma") + '</div><div id="div_datatable_norma" class="table_datatable"></div><div id="div_sugestoes_sinj_norma" class="sugestoes" style="display:none;">A sua pesquisa não encontrou Normas.<br/>Seguem abaixo algumas sugestões que podem ajudar a encontrar o que procura:<div id="datatable_sugestoes_sinj_norma"></div></div></div>' +
        //            '<div id="div_resultado_norma"><button type="button" class="filtro" onclick="javascript:MostrarFiltros(\'sinj_norma\');"><img alt="filtro" src="' + _urlPadrao + '/Imagens/ico_filter_p.png" /> Refinar pesquisa</button><div id="div_datatable_norma"></div></div>' +
            '<div id="div_cesta">' +
                '<ul>' +
                    '<li nm="cesta_norma"><a class="a" href="#div_cesta_norma" onclick="javascript:ClicarAba(\'cesta_norma\')">Normas</a></li>' +
                '</ul>' +
                '<div id="div_cesta_norma"></div>' +
            '</div>'
        ).tabs();
        nm_aba = 'sinj_norma';
        nm_base_aux_total = nm_aba;
    }
    ConsultarTotal(nm_base_aux_total);
    ConsultarTotalCesta();
});

function sugerirBuscas(nm_base) {
    if ($('#datatable_sugestoes_' + nm_base).html() != "") {
        return;
    }
    var sucesso = function (data) {
        var sugestoes_columns = [
	        { "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Pesquisa", "sWidth": "80%", "sClass": "grid-cell ws", "mData": "ds_sugestao",
	            "mRender": function (data, type, full) {
	                return '<a title="Pesquisar por ' + data + '" href="./ResultadoDePesquisa?' + window.unescape(full.params_sugestao).replaceAll('#', '%23') + '&aba=' + nm_base + '" >' + data + ' <img src="' + _urlPadrao + '/Imagens/ico_loupe_p.png" /></a>';
	            }
	        },
	        { "indice": 1, "isControl": false, "standard_view": true, "sTitle": "Registros", "sWidth": "", "sClass": "grid-cell ws center", "mData": "count" },
	        { "indice": 3, "isControl": false, "standard_view": true, "sTitle": " ", "sWidth": "", "sClass": "grid-cell ws", "mData": "nr_termos", "visible": false }
        ];
	    $("#div_sugestoes_" + nm_base).show();
        if (IsNotNullOrEmpty(data, 'sugestoes')) {
            $("#datatable_sugestoes_" + nm_base).dataTablesLight({
                responsive: null,
                bPaginate: false,
                bInfo: false,
                bSorting: false,
                "sIdTable": "table_sugestoes" + nm_base,
                "aoData": data.sugestoes,
                "bServerSide": false,
                "aoColumns": sugestoes_columns,
                "aLengthMenu": [[10, 25, 50, 100, -1], [10, 25, 50, 100, "Todos"]],
                "aaSorting": []
            });
        }
        else {
            $("#div_sugestoes_" + nm_base).html("A sua pesquisa não encontrou " + data.ds_base + ".");

        }
    }
    $.ajaxlight({
        sUrl: './ashx/Consulta/SugestoesConsulta.ashx?bbusca_sugestoes=' + nm_base,
        oData: parseURL(window.location.href).aoData,
        sType: "POST",
        fnError: null,
        fnBeforeSend: gInicio,
        fnComplete: gComplete,
        bAsync: true,
        fnSuccess: sucesso
    });
}

function ConsultarTotal(nm_base) {
    var abaClicar = GetParameterValue('aba');
    var sucesso = function (data) {
        var nm_by_total = '';
        if (IsNotNullOrEmpty(data, 'counts')) {

            var paramCountsToHistory = '';
            for (var i = 0; i < data.counts.length; i++) {
                $('#total_' + data.counts[i].nm_base).text("");
                if (data.counts[i].count > 0) {
                    $('#total_' + data.counts[i].nm_base).addClass('destaque');
                    $('#total_' + data.counts[i].nm_base).text(data.counts[i].count);
                    $('#span_obs_encontrados_' + data.counts[i].nm_base).html(', mas localizou ' + data.counts[i].count + ' registro(s) no acervo de ' + data.counts[i].ds_base + ', para visualizar <a href="javascript:void(0);" onclick="trocarAba(\'' + data.counts[i].nm_base + '\');">clique aqui</a>')
                }
                paramCountsToHistory += (paramCountsToHistory != "" ? "&" : "") + "total=" + JSON.stringify({ nm_base: data.counts[i].nm_base, ds_base: data.counts[i].ds_base, nr_total: data.counts[i].count });
            }
            SalvarConsultaNoHistorico(paramCountsToHistory);
        }

        //Abrir a ultima aba selecionada ante de mudar de página
        var nm_li_click = '';
        var sAbaCookie = $.cookie("Tabs_" + document.location.pathname);
        if (IsNotNullOrEmpty(sAbaCookie)) {
            var sBack_history_aux = $.cookie('back_history_aux_tab');
            if (IsNotNullOrEmpty(sBack_history_aux)) {
                var oBack_history_aux = JSON.parse(sBack_history_aux);
                if (IsNotNullOrEmpty(oBack_history_aux, 'referrer') && oBack_history_aux.referrer == document.location.pathname) {
                    nm_li_click = sAbaCookie;
                }
                $.removeCookie('back_history_aux_tab', { path: '/' });
            }
            $.removeCookie("Tabs_" + document.location.pathname, { path: document.location.pathname });
        }
        if (IsNotNullOrEmpty(nm_li_click)) {
            if (nm_li_click == "cesta_norma" || nm_li_click == "cesta_diario") {
                $('li[nm="cesta"] a').click();
            }
            $('li[nm="' + nm_li_click + '"] a').click();
        }
        else if (IsNotNullOrEmpty(abaClicar)) {
            $('li[nm="' + abaClicar + '"] a').click();
        }
        else {
            $('a', $('li[nm]')[0]).click();
        }
    };
    var inicio = function () {
        if (IsNotNullOrEmpty(nm_base)) {
            $('#total_' + nm_base).html("<img src='" + _urlPadrao + "/Imagens/loading-p.gif' alt='carregando...' />");
        }
        else {
            $('span.total').html("<img src='" + _urlPadrao + "/Imagens/loading-p.gif' alt='carregando...' />");
        }
    }
    $.ajaxlight({
        sUrl: './ashx/Consulta/TotalConsulta.ashx' + (IsNotNullOrEmpty(nm_base) ? '?bbusca=' + nm_base : ''),
        oData: parseURL(window.location.href).aoData,
        sType: "POST",
        fnError: null,
        bAsync: true,
        fnBeforeSend: inicio,
        fnSuccess: sucesso
    });
}

function trocarAba(abaClicar) {
    $('li[nm="' + abaClicar + '"] a').click();
}

function ClicarAba(nm_base) {
    var total = $('#total_' + nm_base).text();
    if (!IsNotNullOrEmpty(total) && nm_base.indexOf("cesta") < 0) {
        sugerirBuscas(nm_base);
        return;
    }
    $('#div_sort_' + nm_base).show();
    $.cookie("Tabs_" + document.location.pathname, nm_base, { path: document.location.pathname });
    var tipo_pesquisa = GetParameterValue('tipo_pesquisa');
    if (nm_base == "sinj_norma") {
        $('#a_relatorio').show();
        if ($('#datatable_normas').length <= 0) {
            PesquisarDatatable(nm_base, recuperarFiltrosDoCookie());
        }
    }
    else if (nm_base == "sinj_diario") {
        if (tipo_pesquisa == "notifiqueme") {
            $('#a_relatorio').hide();
        }
        else {
            $('#a_relatorio').show();
        }
        if ($('#datatable_diarios').length <= 0) {
            PesquisarDatatable(nm_base, recuperarFiltrosDoCookie());
        }
    }
    else if (nm_base == "cesta") {
        $('#div_cesta').tabs();
        $('#a_relatorio').hide();
        ClicarAba('cesta_norma');
        if (IsNotNullOrEmpty(tipo_pesquisa)) {
            if (tipo_pesquisa == "diario") {
                ClicarAba('cesta_diario');
            }
        }
    }
    else if (nm_base == "cesta_norma") {
        $('#a_relatorio').hide();
        if ($('#datatable_cesta_norma').length <= 0) {
            $('#div_cesta_norma').dataTablesLight({
                sAjaxUrl: './ashx/Datatable/CestaPesquisaDatatable.ashx?tipo_pesquisa=cesta&bbusca=cesta&b=sinj_norma&cesta=' + $.cookie('sinj_basket'),
                aoColumns: _columns_norma_cesta,
                responsive: null,
                sIdTable: 'datatable_cesta_norma',
                aLengthMenu: [[10, 25, 50, 100], [10, 25, 50, 100]]
            });
        }
    }
    else if (nm_base == "cesta_diario") {
        $('#a_relatorio').hide();
        if ($('#datatable_cesta_diario').length <= 0) {
            $('#div_cesta_diario').dataTablesLight({
                sAjaxUrl: './ashx/Datatable/CestaPesquisaDatatable.ashx?tipo_pesquisa=cesta&bbusca=cesta&b=sinj_diario&cesta=' + $.cookie('sinj_basket'),
                aoColumns: _columns_diario_cesta,
                responsive: null,
                sIdTable: 'datatable_cesta_diario',
                aLengthMenu: [[10, 25, 50, 100], [10, 25, 50, 100]]
            });
        }
    }
}

function PesquisarDatatable(nm_base, filtros) {
    if (nm_base == "sinj_norma") {
        var sort = $('select[table="datatable_normas"]').val().split(',');
        var col = sort[0];
        var order = sort[1];
        var table = $('#div_datatable_norma').dataTablesLight({
            sAjaxUrl: './ashx/Datatable/ResultadoDePesquisaNormaDatatable.ashx' + window.location.search + (IsNotNullOrEmpty(filtros) ? '&' + filtros : ''),
            data: [{ "name": "bbusca", "value": nm_base}],
            aoColumns: _columns_norma_es,
            responsive: null,
            bProcessing: false,
            sSearch: "Localizar:",
            bFilter: true,
            sIdTable: 'datatable_normas',
            bSorting: true,
            aaSorting: [parseInt(col), order],
            aLengthMenu: [[10, 25, 50, 100], [10, 25, 50, 100]],
            fnCreatedRow: function (nRow, aData, iDataIndex) {
                aplicarHighlight('.', aData.highlight, nRow);
            },
            fnServerDataAggregation: function (data) {
                var apelidos = { nm_tipo_norma_keyword: "Tipo de Norma", ano_assinatura: "Ano de Assinatura", sg_orgao_keyword: "Origem", nm_situacao_keyword: "Situação" };
                montarAggregations(data, nm_base, apelidos);
            }
        });
        // Caça o input lá aqui, e dá um .remove() nele, ou dá um .hide()
        
    }
    else if (nm_base == "sinj_diario") {
        var sort = $('select[table="datatable_diarios"]').val().split(',');
        var col = sort[0];
        var order = sort[1];
        $('#div_datatable_diario').dataTablesLight({
            sAjaxUrl: './ashx/Datatable/ResultadoDePesquisaDiarioDatatable.ashx' + window.location.search + (IsNotNullOrEmpty(filtros) ? '&' + filtros : ''),
            data: [{ "name": "bbusca", "value": nm_base}],
            aoColumns: _columns_diario_es,
            responsive: null,
            sSearch: "Localizar:",
            bFilter: true,
            sIdTable: 'datatable_diarios',
            bSorting: true,
            aaSorting: [parseInt(col), order],
            aLengthMenu: [[10, 25, 50, 100], [10, 25, 50, 100]],
            fnCreatedRow: function (nRow, aData, iDataIndex) {
                aplicarHighlight('.', aData.highlight, nRow);
            },
            fnServerDataAggregation: function (data) {
                var apelidos = { nm_tipo_fonte_keyword: "Tipo de Diário", ano_assinatura: "Ano de Publicacao"};
                montarAggregations(data, nm_base, apelidos);
            }
        });
    }
}

function RelatorioPDF() {
    $('#form_relatorio input').remove();
    var aba = $("#tabs_pesquisa").tabs("option", "active");
    var nm = $("#tabs_pesquisa").tabs("instance").tabs[aba].getAttribute('nm');
    var sSource = '';
    if (nm == "sinj_norma") {
        if ($("#datatable_normas tbody tr").length > 0 && $("#datatable_normas .dataTables_empty").length <= 0) {
            var order = ""
            var asc = $("#datatable_normas thead tr th.sorting_asc").attr('nm');
            var desc = $("#datatable_normas thead tr th.sorting_desc").attr('nm');
            if (IsNotNullOrEmpty(asc)) {
                order = asc + ",asc";
            }
            else if (IsNotNullOrEmpty(desc)) {
                order = desc + ",desc";
            }
            var search = $('#datatable_normas_filter input[type="search"]').val();
            sSource = window.location.search + '&tp=es&bbusca=sinj_norma&order=' + order + '&search=' + search + '&' + getsFiltrados(nm);
        }
    }
    else if (nm == "sinj_diario") {
        if ($("#datatable_diarios tbody tr").length > 0 && $("#datatable_normas .dataTables_empty").length <= 0) {
            var order = ""
            var asc = $("#datatable_diarios thead tr th.sorting_asc").attr('nm');
            var desc = $("#datatable_diarios thead tr th.sorting_desc").attr('nm');
            if (IsNotNullOrEmpty(asc)) {
                order = asc + ",asc";
            }
            else if (IsNotNullOrEmpty(desc)) {
                order = desc + ",desc";
            }
            var search = $('#datatable_diarios_filter input[type="search"]').val();
            sSource = window.location.search + '&tp=es&bbusca=sinj_diario&order=' + order + '&search=' + search + '&' + getsFiltrados(nm);
        }
    }
    else if (nm == "cesta") {
        var aba_cesta = $("#div_cesta").tabs("option", "active");
        var nm_aba_cesta = $("#div_cesta").tabs("instance").tabs[aba_cesta].getAttribute('nm');
        if (nm_aba_cesta == "cesta_norma") {
            if ($("#datatable_cesta_norma tbody tr").length > 0 && $("#datatable_cesta_norma .dataTables_empty").length <= 0) {
                var order = ""
                var asc = $("#datatable_cesta_norma thead tr th.sorting_asc").attr('nm');
                var desc = $("#datatable_cesta_norma thead tr th.sorting_desc").attr('nm');
                if (IsNotNullOrEmpty(asc)) {
                    order = asc + ",asc";
                }
                else if (IsNotNullOrEmpty(desc)) {
                    order = desc + ",desc";
                }
                sSource = '?tipo_pesquisa=cesta&b=sinj_norma&cesta=' + $.cookie('sinj_basket') + '&bbusca=cesta&order=' + order;
            }
        }
        else if (nm_aba_cesta == "cesta_diario") {
            if ($("#datatable_cesta_diario tbody tr").length > 0 && $("#datatable_cesta_diario .dataTables_empty").length <= 0) {
                var order = ""
                var asc = $("#datatable_cesta_diario thead tr th.sorting_asc").attr('nm');
                var desc = $("#datatable_cesta_diario thead tr th.sorting_desc").attr('nm');
                if (IsNotNullOrEmpty(asc)) {
                    order = asc + ",asc";
                }
                else if (IsNotNullOrEmpty(desc)) {
                    order = desc + ",desc";
                }
                sSource = '?tipo_pesquisa=cesta&b=sinj_diario&cesta=' + $.cookie('sinj_basket') + '&bbusca=cesta&order=' + order;
            }
        }
    }
    var aoData = new_aoData([], sSource);

    for (var i = 0; i < aoData.length; i++) {
        $('#form_relatorio').append($('<input id="input_relatorio_' + aoData[i].name + '_' + i + '" type="hidden" name="' + aoData[i].name + '" value=""/>'));
        $('#input_relatorio_' + aoData[i].name + '_' + i).val(aoData[i].value)
    }
    $('#form_relatorio').submit();
}


// Cesta


function SelecionarTodos(event) {
    var table = $(event.target).closest('table');
    $('.check_cesta', table).each(function () {
        $(this).prop("checked", $(event.target).prop("checked"));
    });
}

function AdicionarNaCesta(id_doc, todas) {
    if (!todas) {
        if (IsNotNullOrEmpty(id_doc)) {
            if (IsNotNullOrEmpty($.cookie('sinj_basket'))) {
                var cesta = $.cookie('sinj_basket').split(',');
                if (cesta.indexOf(id_doc) <= -1) {
                    cesta.push(id_doc);
                    $.cookie('sinj_basket', cesta.join(), { expires: 7, path: '/' });
                }  
            }
            else $.cookie('sinj_basket', id_doc, { expires: 7, path: '/' });
        }
    }
    else {
        var table = $(event.target).closest('table');
        var docs = [];
        var cookie_cesta = $.cookie('sinj_basket');
        $(table).find('a[valor]').each(function () {
            docs.push($(this).attr("valor"));
        })
        if (IsNotNullOrEmpty($.cookie('sinj_basket'))) {
            var cesta = $.cookie('sinj_basket').split(',');
            for (i = 0; i < docs.length; i++) {
                if (cesta.indexOf(docs[i]) <= -1) {
                    cesta.push(docs[i]);
                }
            }
            $.cookie('sinj_basket', cesta.join(), { expires: 7, path: '/' });
        }
        else $.cookie('sinj_basket', docs.join(), { expires: 7, path: '/' });
    }
    ShowDialog({sTitle: 'Cesta', sContent: 'Adicionado à cesta com sucesso.', sType:'success'});
    $('#div_cesta_norma').html('');
    $('#div_cesta_diario').html('');
    ConsultarTotalCesta();
}

function ExcluirDaCesta(doc) {
    var cookie_cesta = $.cookie('sinj_basket');
    if (IsNotNullOrEmpty(cookie_cesta)) {
        var cesta = cookie_cesta.split(',');
        var cesta_aux = [];
        if (cesta.indexOf(doc) > -1) {
            for (var i = 0; i < cesta.length; i++) {
                if (cesta[i] != doc) {
                    cesta_aux.push(cesta[i]);
                }
            }
            $.cookie('sinj_basket', cesta_aux.join(), { expires: 7, path: '/' });
            if (doc.indexOf('norma_') > -1) {
                ClicarAba('cesta_norma');
            }
            else {
                ClicarAba('cesta_diario');
            }
        }
    }
    ShowDialog({ sTitle: 'Cesta', sContent: 'Removido da cesta com sucesso.', sType: 'success' });
    ConsultarTotalCesta();
}

function EsvaziarCesta() {
    $('<div id="modal_esvaziar_cesta" />').modallight({
        sType: "alert",
        sTitle: "Confirmação",
        sContent: "Deseja excluir todos os registros da cesta?",
        oButtons: [
            {
                text: "Ok",
                click: function () {
                    $.removeCookie('sinj_basket', { path: '/' });
                    document.location.reload();
                }
            },
            {
                text: "Cancelar",
                click: function () {
                    $(this).dialog('close');
                }
            }
        ]
    });
    ConsultarTotalCesta();
}

function ConsultarTotalCesta() {
    if (IsNotNullOrEmpty($.cookie('sinj_basket'))) {
        var aCookieCesta = $.cookie('sinj_basket').split(',');
        var total = aCookieCesta.length;
        var total_norma = 0;
        var total_diario = 0;
        for (i = 0; i < total; i++ ) {
            if (aCookieCesta[i].indexOf('norma') > -1) {
                total_norma++;
            }
            else if (aCookieCesta[i].indexOf('diario') > -1) {
                total_diario++;
            }
        }
        if (total > 0) {
            $('#total_cesta').addClass('destaque');
            $('#total_cesta').text(total);
        }
        else {
            $('#total_cesta').removeClass('destaque');
            $('#total_cesta').text('');
        }
        $('#total_cesta_norma').text(total_norma > 0 ? total_norma : '');
        $('#total_cesta_diario').text(total_diario > 0 ? total_diario : '');
    }
    else {
        $('#total_cesta').removeClass('destaque');
        $('#total_cesta').text('');
        $('#total_cesta_norma').text('');
        $('#total_cesta_diario').text('');
    }
}

function selectOrder(id_datatable, el) {
    var col = el.value.split(',')[0];
    var order = el.value.split(',')[1];
    $('#' + id_datatable).DataTable().order([parseInt(col), order]).draw();
}

function montarFiltro(nm_base) {
    return '<div class="table filtro_' + nm_base + ' div-filtros w-100-pc">' +
        '<div class="line">' +
            '<div class="column w-100-pc">' +
                '<div class="table table_filtrados_' + nm_base + ' w-100-pc" style="display:none;">' +
                    '<div class="line">' +
                        '<div class="column">' +
                            '<div class="header_filtrando">Filtrando por</div>' +
                            '<div class="filtrados_' + nm_base + '"></div>' +
                        '</div>' +
                    '</div>' +
                    '<div class="footer_table"></div>' +
                '</div>' +
            '</div>' +
        '</div>' +
        '<div class="line">' +
            '<div class="column w-100-pc">' +
                '<div class="table table_filtros_' + nm_base + ' w-100-pc">' +
                    '<div class="line">' +
                        '<div class="column">' +
                            '<div class="filtros_' + nm_base + '"></div>' +
                        '</div>' +
                    '</div>' +
                    '<div class="footer_table"></div>' +
                '</div>' +
            '</div>' +
        '</div>' +
        '<div class="footer_table"></div>' +
    '</div>';
}

function recuperarFiltrosDoCookie() {
    var filtros = null;
    var sFiltroCookie = $.cookie("Filtros_" + document.location.pathname);
    //se existe a informação da ultima aba clicada então valida se se a ultima página visitada (referrer) é igual à atual (document.location.href)
    //então recarrega aba e limpa os cookies
    if (IsNotNullOrEmpty(sFiltroCookie)) {
        var sBack_history_aux = $.cookie('back_history_aux_filtros');
        if (IsNotNullOrEmpty(sBack_history_aux)) {
            var oBack_history_aux = JSON.parse(sBack_history_aux);
            if (IsNotNullOrEmpty(oBack_history_aux, 'referrer') && oBack_history_aux.referrer == document.location.pathname) {
                var jFiltroCookie = {};
                if (IsJson(sFiltroCookie)) {
                    jFiltroCookie = JSON.parse(sFiltroCookie);
                }
                if (IsNotNullOrEmpty(jFiltroCookie, 'nm_base') && IsNotNullOrEmpty(jFiltroCookie, 'filtros')) {
                    filtros = tratarFiltros(jFiltroCookie);
                }
                else {
                    filtros = sFiltroCookie;
                }
            }
        }
        $.removeCookie("Filtros_" + document.location.pathname, { path: document.location.pathname });
    }
    $.removeCookie('back_history_aux_filtros', { path: '/' });
    return filtros;
}

function tratarFiltros(jFiltro) {
    var filtros = '';
    var nm_base = jFiltro.nm_base;
    var aFiltros = jFiltro.filtros;
    $('.filtrados_' + nm_base).html('');
    $('.table_filtros_' + nm_base).hide();
    if (IsNotNullOrEmpty(aFiltros)) {
        for (var i = 0; i < aFiltros.length; i++) {
            $('.filtrados_' + nm_base).append('<button type="button" id="button_' + nm_base + '_' + aFiltros[i].campo + '" class="clean" title="Remover o filtro \'' + aFiltros[i].valor + '\'." campo="' + aFiltros[i].campo + '" valor="' + aFiltros[i].valor + '" onclick="javascript:removerFiltro(\'' + nm_base + '\',\'' + aFiltros[i].campo + '\');">' + aFiltros[i].valor + '<img alt="filtrar" src="' + _urlPadrao + '/Imagens/ico_fechar.png" height="12px" width="12px" /></button>');
            filtros += (filtros != '' ? '&' : '') + 'filtro=' + aFiltros[i].campo + ':' + aFiltros[i].valor;
        }
        $('.table_filtrados_' + nm_base).show();
    }
    return filtros;
}

function getjFiltrados(nm_base) {
    var filtros = [];
    var filtrados = $('.filtrados_' + nm_base + ' button');
    if (filtrados.length > 0) {
        for (var i = 0; i < filtrados.length; i++) {
            filtros.push({ campo: filtrados[i].getAttribute('campo'), valor: filtrados[i].getAttribute('valor') });
        }
    }
    return filtros;
}

function getsFiltrados(nm_base) {
    var filtros = '';
    var filtrados = $('.filtrados_' + nm_base + ' button');
    if (filtrados.length > 0) {
        for (var i = 0; i < filtrados.length; i++) {
            filtros += (filtros != '' ? '&' : '') + 'filtro=' + filtrados[i].getAttribute('campo') + ':' + filtrados[i].getAttribute('valor');
        }
    }
    return filtros;
}

function filtrar(nm_base, campo, valor) {
    var jFiltro = { nm_base: nm_base, filtros: [] };
    if (IsNotNullOrEmpty(campo) && IsNotNullOrEmpty(valor)) {
        jFiltro.filtros.push({ campo: campo, valor: valor });
    }
    filtrados = getjFiltrados(nm_base);
    for (var i = 0; i < filtrados.length; i++) {
        jFiltro.filtros.push(filtrados[i]);
    }
    
    var filtros = tratarFiltros(jFiltro);
    PesquisarDatatable(nm_base, filtros);
    $.cookie("Filtros_" + document.location.pathname, JSON.stringify(jFiltro), { path: document.location.pathname });
    
    adicionaEventoFiltroBuscaNorma();
    adicionaEventoFiltroBuscaDiario();
}

function removerFiltro(nm_base, campo) {
    $('#button_' + nm_base + '_' + campo).remove();
    if ($('.filtrados_' + nm_base + ' button').length <= 0) {
        $('.table_filtrados_' + nm_base).hide();
        $('.filtrados_' + nm_base).html('');
    }
    filtrar(nm_base);
}

function exibirMais(el) {
    var select = $(el).attr('select');
    $(select).show();
    $(el).attr('onclick', 'javascript:exibirMenos(this)').text('Exibir menos...');
    $(el).attr('title', 'Exibir menos filtros.');
}

function exibirMenos(el) {
    var select = $(el).attr('select');
    $(select).hide();
    $(el).attr('onclick', 'javascript:exibirMais(this)').text('Exibir mais...');
    $(el).attr('title', 'Exibir mais filtros.');
}

function montarAggregations(data, nm_base, apelidos) {
    $('.filtros_' + nm_base).html('');
    $('.filtro_' + nm_base + ' .table' + nm_base).hide();
    $('.filtros_' + nm_base).html('');
    if (IsNotNullOrEmpty(data, 'aggregations')) {
        for (var key in apelidos) {
            if (data.aggregations[key].buckets.length > 1) {
                var ocultar = '';
                for (var i = 0; i < data.aggregations[key].buckets.length; i++) {
                    var key_as_string = data.aggregations[key].buckets[i].key;
                    if (IsNotNullOrEmpty(data.aggregations[key].buckets[i], 'key_as_string')) {
                        key_as_string = data.aggregations[key].buckets[i].key_as_string;
                    }
                    if ($('.filtrados_' + nm_base + ' button[valor="' + key_as_string + '"]').length <= 0) {

                        if ($('.filtros_' + nm_base + ' .filtro_' + key).length <= 0) {
                            $('.filtros_' + nm_base).append('<div class="filtro_' + key + '"><div class="w-100-pc header_filtro">' + apelidos[key] + '</div></div>');
                        }
                        var ocultar = $('.filtros_' + nm_base + ' .filtro_' + key + ' button').length >= 5 ? 'ocultar' : '';
                        $('.filtros_' + nm_base + ' .filtro_' + key).append('<button type="button" title="Filtrar o resultado da pesquisa por \'' + key_as_string + '\'." class="clean ' + ocultar + '" onclick="javascript:filtrar(\'' + nm_base + '\', \'' + key + '\',\'' + key_as_string + '\')">' + key_as_string + ' (' + data.aggregations[key].buckets[i].doc_count + ')</button>');
                    }
                }
                if (ocultar != '') {
                    $('.filtros_' + nm_base + ' .filtro_' + key).append('<a select=".filtros_' + nm_base + ' .filtro_' + key + ' button.ocultar" href="javascript:void(0);" onclick="javascript:exibirMais(this);" title="Exibir mais filtros">Exibir mais...</a>');
                    $('.filtros_' + nm_base + ' .filtro_' + key + ' button.ocultar').hide();
                }
            }
        }
    }
    
    if ($('div.table.filtro_' + nm_base + ' button').length <= 0) {
        $('div.table.filtro_' + nm_base).hide();

        var $agg_datatable = $('.filtro_' + nm_base).closest('.agg_datatable').addClass('no_agg');
        $('.sort_datatable', $agg_datatable.parent()).addClass('no_agg');
        $('.table_datatable', $agg_datatable.parent()).addClass('no_agg');
    }
    else {
        if ($('.filtros_' + nm_base + ' button').length > 0) {
            $('.table_filtros_' + nm_base).show();
        }
        else {
            $('.table_filtros_' + nm_base).hide();
        }
    }
}
