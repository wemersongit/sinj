// necessário carregar: 'jquery-1.11.0.min.js', ' jquery-ui-1.10.4.custom.min.js', 'script.js'  e 'js.ashx'
$(function () {
    (function ($) {
        $.fn.dataTablesLight = function (options) {
            var defaults = {
                bJQueryUI: true,
                responsive: {
                    "breakpoints": [
                        { "name": 'desktop', "width": Infinity },
                        { "name": 'tablet-l', "width": 1024 },
                        { "name": 'tablet-p', "width": 768 },
                        { "name": 'mobile-l', "width": 480 },
                        { "name": 'mobile-p', "width": 320 }
                    ]
                },
                bSelect: true,
                bProcessing: false,
                bServerSide: true,
                bPaginate: true,
                sPageLast: false,
                bInfo: true,
                iDisplayLength: 10,
                aLengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "Todos"]],
                sAjaxUrl: null,
                bAutoWidth: false,
                aoColumns: [],
                data: {},
                aoData: [],
                bSorting: true,
                aaSorting: [],
                sSearch: "Pesquisa:",
                bFilter: false,
                bPrint: false,
                sIdTable: "datatable",
                fnCreatedRow: null,
                sUrlPadrao: _urlPadrao,
                bStateSave: true,
                fnServerDataAggregation: null,
                fnStateSaveCallback: function (settings, data) {
                    try {
                        localStorage.setItem("DataTables_" + settings.sTableId + document.location.pathname, JSON.stringify(data));
                    }
                    catch (ex) { }
                },
                fnStateLoadCallback: function (settings) {
                    try {
                        var retorno = null;
                        var sDataCookie = localStorage.getItem("DataTables_" + settings.sTableId + document.location.pathname);
                        if (IsNotNullOrEmpty(sDataCookie)) {
                            var sBack_history_aux = $.cookie('back_history_aux_datatable');
                            if (IsNotNullOrEmpty(sBack_history_aux)) {
                                var oBack_history_aux = JSON.parse(sBack_history_aux);
                                if (IsNotNullOrEmpty(oBack_history_aux, 'referrer') && oBack_history_aux.referrer == document.location.pathname) {
                                    retorno = JSON.parse(sDataCookie);
                                    selectOrderStateLoad(settings.sTableId, retorno);
                                }
                                $.removeCookie('back_history_aux_datatable', { path: '/' });
                            }
                            localStorage.removeItem("DataTables_" + settings.sTableId + document.location.pathname);
                        }
                        if (IsNotNullOrEmpty(retorno)) {
                            return retorno;
                        }
                    }
                    catch (ex) { }
                    return false;
                }

            }
            var settings = $.extend({}, defaults, options);
            var $element = $(this);
            return this.each(function () {
                $element.html(MontaTable(settings.sIdTable, settings.sUrlPadrao, settings.aoColumns.length));
                if (!IsNotNullOrEmpty(settings.aaSorting) && settings.bSorting) {
                    settings.aaSorting = Sorting(settings.aoColumns);
                }
                var tableOptions = {
                    "responsive": settings.responsive,
                    "bJQueryUI": settings.bJQueryUI,
                    "bProcessing": settings.bProcessing,
                    "bServerSide": settings.bServerSide,
                    "bPaginate": settings.bPaginate,
                    "sPageLast": settings.sPageLast,
                    "bInfo": settings.bInfo,
                    "iDisplayLength": settings.iDisplayLength,
                    "aLengthMenu": settings.aLengthMenu,
                    "sAjaxSource": settings.sAjaxUrl,
                    "data": settings.aoData,
                    "bAutoWidth": settings.bAutoWidth,
                    "aoColumns": settings.aoColumns,
                    "bFilter": settings.bFilter,
                    "aaSorting": settings.aaSorting,
                    "sDom": '<"H"lpifr>t<"F"ip>',
                    "ordering": IsNotNullOrEmpty(settings.aaSorting),
                    "sPaginationType": "full_numbers",
                    "fnServerData": function (sSource, aoData, fnCallback) {
                        var sucesso = function (data, textStatus, xmlHttpRequest) {
                            if (data == null || data.aaData == null) {
                                data = { "aaData": [], iTotalRecords: 0, iTotalDisplayRecords: 0 };
                            }
                            fnCallback(data);
                            if (typeof settings.fnServerDataAggregation === "function") {
                                settings.fnServerDataAggregation(data);
                            }
                            return;
                        };
                        var aoData = $.extend([], aoData, settings.data);
                        $.ajaxlight({
                            sUrl: parseURL(sSource).urlnoquery,
                            sType: "POST",
                            oData: new_aoData(aoData, sSource),
                            fnSuccess: sucesso,
                            fnBeforeSend: gInicio,
                            fnComplete: gComplete,
                            fnError: null,
                            bAsync: true
                        });
                    },
                    "fnHeaderCallback": function (nHead, aData, iStart, iEnd, aiDisplay) {
                        var ths = $('th', $(nHead));
                        for (var i = 0; i < ths.length; i++) {
                            if (IsNotNullOrEmpty(settings.aoColumns[i].sToolTip)) {
                                $(ths[i]).attr('title', settings.aoColumns[i].sToolTip);
                            }
                            $(ths[i]).attr('nm', settings.aoColumns[i].mData);
                        }
                        //$('.tooltip').tooltip();
                        $('.tooltip').tooltip({ container: "#div_tooltip", trigger: "hover" });
                    },
                    "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                        if (!IsNotNullOrEmpty(aData) || !IsNotNullOrEmpty(aData._metadata) || !IsNotNullOrEmpty(aData._metadata.id_doc)) {
                            $(nRow).attr("id", "Row_" + iDisplayIndex);
                        }
                        else {
                            $(nRow).attr("id", "Row_" + aData._metadata.id_doc);
                        }
                        return nRow;
                    },
                    "fnCreatedRow": settings.fnCreatedRow,
                    "oLanguage": {
                        "oPaginate": { "sFirst": "<<", "sLast": ">>", "sNext": ">", "sPrevious": "<" },
                        "sEmptyTable": "Não foram encontrados registros",
                        "sInfo": "<span>Exibindo de <b>_START_</b> até <b>_END_</b> de <b>_TOTAL_</b> registros encontrados.</span>",
                        "sInfoEmpty": " ",
                        "sInfoFiltered": "",
                        "sInfoThousands": ".",
                        "sLengthMenu": "Exibir _MENU_ registros",
                        "sLoadingRecords": "Carregando...",
                        "sProcessing": "<span><b>Processando...</b></span>",
                        "sSearch": settings.sSearch,
                        "sZeroRecords": "Não foram encontrados registros"
                    },
                    "stateSave": settings.bStateSave,
                    "stateSaveCallback": settings.fnStateSaveCallback,
                    "stateLoadCallback": settings.fnStateLoadCallback
                };
                $.extend(tableOptions, options.jOptions);
                var tableLight = $("#" + settings.sIdTable).dataTable(tableOptions);
                if (settings.bSelect) {
                    $('#' + settings.sIdTable + ' tbody').on('click', 'tr', function () {
                        tableLight.$('tr.selected').removeClass('selected');
                        $(this).addClass('selected');
                    });
                }
                //nota: Adicionado para que o filtro retornasse a pesquisa após a consulta
                //Se for pesquisa de norma ou de diario o 'blur' não será acionado. Caso contrario será adicionado aos eventos by wemerson/jimmy
                if("datatable_normas" === settings.sIdTable || "datatable_diarios" === settings.sIdTable){
                  $("#" + settings.sIdTable + "_filter input").unbind().bind('keyup', function (e) {
                    if (e.keyCode == 13) {
                        $(e.target).blur();
                    }});
                } else {                  
                    $("#" + settings.sIdTable + "_filter input").unbind().bind('keyup', function (e) {
                    if (e.keyCode == 13) {
                        $(e.target).blur();
                    }
                    }).bind('blur', function (e) {
                        tableLight.fnFilter(this.value);
                    });
                }
            });
        }
    })(jQuery);
});

function selectOrderStateLoad(id_datatable, jDatatable) {
    var select = $('select[table="' + id_datatable + '"]');
    if (select.length == 1) {
        var order = jDatatable.order;
        if (order.length == 2) {
            select.val(order[0] + ',' + order[1]);
        }
        if (order.length == 1) {
            select.val(order[0][0] + ',' + order[0][1]);
        }
    }
}

function new_aoData(_aoData, _sSource) {
    var a = document.createElement('a');
    a.href = _sSource;
    var seg = urldecode(a.search.replace(/^\?/, '')).split('&'),
    len = seg.length, id = _aoData.length, i = 0, s;
    for (; i < len; i++) {
        if (!seg[i]) { continue; }
        s = seg[i].split('=');
        _aoData[i + id] = { "name": s[0], "value": s[1] };
    }
    return _aoData;
}

function Sorting(colunas) {
    var aasorting = [];
    if (IsNotNullOrEmpty(colunas)) {
        var i = 0;
        for (var c = 0; c < colunas.length; c++) {
            if (colunas[c].bSortable != false && i == 0) {
                i = c;
                aasorting = [[i, "desc"]];
            }
            if (colunas[c].bSorting == true) {
                aasorting = [[c, "desc"]];
                break;
            }
            if (colunas[c].sorting) {
                aasorting = [[c, colunas[c].sorting]];
                break;
            }
        }
    }
    return aasorting;
}

function fnReport(sUrl, sType) {

}

function MontaTable(id_tabela, urlPadrao, iColunas) {
    return '<table cellpadding="0" width="100%" cellspacing="0" border="0" class="display" id="' + id_tabela + '">\n<thead>\n</thead>\n<tbody>\n<tr>\n<td colspan="' + iColunas + '" class="dataTables_empty">\n<div id="divLoading" class="linha ajusteMargin " style="display: inline; text-align: center;">\n\n</div>\n</td>\n</tr>\n</tbody>\n</table>';
}
