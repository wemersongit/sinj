// necessário carregar: 'jquery-1.11.0.min.js', ' jquery-ui-1.10.4.custom.min.js', 'scripts.js'
/*==========================================================================================
Arquivo js responsável por todos os autocomplete das paginas;
Implementação:
Adicione espe js no projeto;
Chame a função autoCompleteDrop na pagina que tem o imput que vai ter o autocomplete;
Passando os parametros, Id do Imput, id do botão que vai ficar ao lado do imput, a url do arquivo ashx ou um json com um array composto de id e value, 
id do Hidden field, nome do atributo que vai ser o value, nome do atributo que vai ser a key;
Exemplo:
<script type="text/javascript">

        var Div5 = {
            sID: "Div5", 
            sInputName: "user5",
            iInputSize: 30,
            sInputClass: "",
            sInputHiddenName: "cpf5",
            bLinkAll: true,
            sPlaceholder: "Digite aqui para pesquisar",
            sLinkName: "",
            sAjaxUrl: _urlManutencao + "/ashx/AC/UsuarioAC.ashx",
            jData: null,
            sKeyDataName: "nr_cpf",
            sValueDataName: "nm_usuario",
            dOthersHidden: [],
            bAddInexistente: false,
            jPagination: { 
                bPaginate: false,
                iDisplayLength: 100,
                bButtonsPaginate: false
            }
        };

        $("#Div5").autocompletelight(Div5);

</script>
    
Obs:. Adicionar o <a id=""></a> na frente do input para o autocomplete também ser um dropdown e passar o id deste no para o parâmetro idLink. 
isso fará também com que o usuário seja obrigado a selecionar um dos valores retornados no dropdown, apagando o conteúdo caso não selecionando. Para ignorar essa funcionalidade
não adicione o <a id=""></a> e passe null para idLink.
=============================================================================================*/
$(function () {
    //AUTOCOMPLETE
    (function ($) {

        $.fn.autocompletelight = function (options) {
            var defaults = {
                sID: "",
                sInputName: "",
                iInputSize: 40,
                sInputClass: null,
                sInputHiddenName: "",
                bLinkAll: false,
                sLinkName: "",
                sAjaxUrl: null,
                jDataSumit: null,
                sPlaceholder: null,
                bCross: false,
                jData: null,
                sKeyDataName: null,
                sValueDataName: null,
                sKeyOthersHidden: [],
                dOthersHidden: [],
                bAddInexistente: false,
                appendTo: null,
                jPagination: { bPaginate: false,
                    iDisplayLength: -1,
                    bButtonsPaginate: false
                }
            };

            //  buscar os valores passados pela variável options e mesclar com os valores definidos 
            // na variável defaults, armazenando em outra variável chamada settings
            var settings = $.extend({}, defaults, options);

            var $Input = null, $LinkAll = null, $InputHiden = null, $Limit = null, $OffSet = null, $Count = null, $TermoBusca = null;

            // métodos internos  - init

            var initElements = function (id) {
                var objName = (id) ? '#' + id : 'body';
                settings.sID = (id) ? id : 'body';

                // tratando input
                if (!$('#' + settings.sInputName).exists()) {
                    if (settings.sInputName == "") settings.sInputName = id + "_Input";
                    if (settings.sID != 'body') $('<input type="text" id="' + settings.sInputName + '" name="' + settings.sInputName + '" />').appendTo(objName);
                }
                $Input = $("#" + settings.sInputName);
                if (($Input.attr("size") == null || $Input.attr("size") == "" || $Input.attr("size") == 'undefined') && settings.iInputSize != null) {
                    $Input.attr("size", settings.iInputSize);
                }
                if (settings.sInputClass != null)
                    $Input.addClass(settings.sInputClass);
                // add placeholder no input   
                if (($Input.attr("placeholder") == null || $Input.attr("placeholder") == "" || $Input.attr("placeholder") == 'undefined') && settings.sPlaceholder != null) {
                    $Input.attr("placeholder", settings.sPlaceholder);
                }

                // tratando link todos
                if (!$('#' + settings.sLinkName).exists()) {
                    if (settings.sLinkName == "") settings.sLinkName = id + "_LinkAll";
                    if (settings.sID != 'body') $('<a title="Todos" id="' + settings.sLinkName + '"></a>').appendTo(objName);
                }
                $LinkAll = $("#" + settings.sLinkName);

                if (settings.bLinkAll) {
                    $LinkAll.addClass("ui-autocompletelight-btn ui-button ui-icon ui-button-icon-only").show();
                } else {
                    if (settings.sID != 'body') $LinkAll.hide();
                }
                // tratando Hidden
                if (!$('#' + settings.sInputHiddenName).exists()) {
                    if (settings.sInputHiddenName == "") settings.sInputHiddenName = id + "_Hidden";
                    if (settings.sID != 'body') $('<input type="hidden" id="' + settings.sInputHiddenName + '" name="' + settings.sInputHiddenName + '" value=""/>').appendTo(objName);
                }
                $InputHiden = $("#" + settings.sInputHiddenName);

                // criando outros dOthersHidden caso não existam
                if (settings.dOthersHidden) {
                    for (var m = 0; m < settings.dOthersHidden.length; m++)
                        if (!$("#" + settings.dOthersHidden[m].campo_app).exists()) $('<input type="hidden" id="' + settings.dOthersHidden[m].campo_app + '" name="' + settings.dOthersHidden[m].campo_app + '" value=""/>').appendTo(objName);
                }

                // 'jPag': elementos para paginação
                if (!$("#" + id + "_limit").exists()) $('<input type="hidden" id="' + id + '_limit" value="' + settings.jPagination.iDisplayLength + '" />').appendTo(objName);
                $Limit = $("#" + id + "_limit");

                // inicializa o offset 
                if (!$("#" + id + "_offset").exists()) $('<input type="hidden" id="' + id + '_offset" value="0" />').appendTo(objName);
                $OffSet = $("#" + id + "_offset");

                if (!$("#" + id + "_count").exists()) $('<input type="hidden" id="' + id + '_count" value="0" />').appendTo(objName);
                $Count = $("#" + id + "_count");

                if (!$("#" + id + "_termoBusca").exists()) $('<input type="hidden" id="' + id + '_termoBusca" value="" />').appendTo(objName);
                $TermoBusca = $("#" + id + "_termoBusca");

            };

            // Paginate - init

            var resetControlsPaginate = function () {
                $OffSet.val(0);
                $Count.val(0);
                return false;
            };

            var getDataSubmit = function (sRequestTermoBusca) {
                if (sRequestTermoBusca != $TermoBusca.val()) {
                    $TermoBusca.val(sRequestTermoBusca);
                    if ($TermoBusca.val() != "...")
                        resetControlsPaginate();
                }

                var jDataURL = parseURL(settings.sAjaxUrl).params;
                var jDataDinamic = { "time": new Date().getTime(), "limit": $Limit.val(), "offset": $OffSet.val(), "texto": sRequestTermoBusca };
                return $.extend({}, jDataURL, jDataDinamic, settings.jDataSumit);
            };

            // Pegando informações da Paginação corrente
            var getInfoPaginate = function () {
                var _offset = parseInt($OffSet.val());
                var _limit = (parseInt($Limit.val()) <= 0) ? 1 : parseInt($Limit.val());
                var _count = parseInt($Count.val());
                var totalPage = ((_count % _limit) == 0) ? Math.floor((_count / _limit)) : Math.floor((_count / _limit)) + 1;
                if (totalPage == 0) totalPage = 1;
                var PgAtual = Math.floor(((_offset + _limit) / _limit));
                var RegInit = (_offset == 0) ? 1 : _offset + 1;
                var RegFim = ((PgAtual * _limit) > _count) ? _count : (PgAtual * _limit);
                var iPgLast = (totalPage * _limit) - _limit;
                var iPgNext = ((_offset + _limit) > iPgLast) ? iPgLast + 1 : (_offset + _limit);
                var iPgPrev = ((_offset - _limit) < 0) ? 0 : (_offset - _limit);
                return InfoPaginate = { offset: _offset, limit: _limit, RegTotal: _count, RegInit: RegInit, RegFim: RegFim, PgAtual: PgAtual, PgFrist: 0, PgPrev: iPgPrev, PgNext: iPgNext, PgLast: iPgLast, TotalPage: totalPage };
            };

            // Construindo Botoes de Paginação
            var getButtonsPaginate = function () {
                var InfoPaginate = getInfoPaginate(), id = settings.sID;
                // itens do paginate remover
                $(".ui-menu-item-disabled").remove();

                var info = "Exibindo <b>" + InfoPaginate.RegInit + '</b> até <b>' + InfoPaginate.RegFim + '</b> de <b>' + InfoPaginate.RegTotal + "</b> registros.";
                var frist = "<a title='Primeira Pagina' name='frist_" + id + "' id='frist_" + id + "' value='frist' class='jPagBtn' ><<</a>";
                var prev = "<a title='Anterior Pagina' id='prev_" + id + "' value='prev' class='jPagBtn' ><</a>";
                var nav = "<b>" + InfoPaginate.PgAtual + "</b>";
                var next = "<a title='Proxima Pagina' id='next_" + id + "' value='next' class='jPagBtn' >></a>";
                var last = "<a title='Ultima Pagina' id='last_" + id + "' value='last' class='jPagBtn' >>></a>";

                if (InfoPaginate.PgAtual == 1) {
                    frist = "<span id='frist_" + id + "' value='frist' class='jPagBtnDisable' ><<</span>";
                    prev = "<span id='prev_" + id + "' value='prev' class='jPagBtnDisable' ><</span>";
                }
                if (InfoPaginate.PgAtual == InfoPaginate.TotalPage) {
                    next = "<span id='next_" + id + "' value='next' class='jPagBtnDisable' >></span>";
                    last = "<span id='last_" + id + "' value='last' class='jPagBtnDisable' >>></span>";
                }
                return info + " <span class='floatright'>" + frist + " " + prev + " " + nav + " " + next + " " + last + "</span>";

            };

            var AtiveBusca = function (operacao, e) {
                var InfoPaginate = getInfoPaginate();
                switch (operacao) {
                    case "first":
                        new_offset = InfoPaginate.PgFrist;
                        break;
                    case "prev":
                        new_offset = InfoPaginate.PgPrev;
                        break;
                    case "next":
                        new_offset = InfoPaginate.PgNext;
                        break;
                    case "last":
                        new_offset = InfoPaginate.PgLast;
                        break;
                };
                $OffSet.val(new_offset);
                $Input.autocomplete("search", ($Input.val() == "") ? "..." : $Input.val());
                $Input.focus();

                e.preventDefault();
                return false;
            };

            // Paginate - end


            // métodos internos  - end

            this.each(function () {

                initElements($(this).attr("id"));
                $Input.unbind("autocomplete");
                $LinkAll.unbind("click");

                $Input.autocomplete({
                    delay: 800,
                    minLength: (IsNotNullOrEmpty(settings.sAjaxUrl)) ? 2 : 1,
                    cache: false,
                    source: function (request, response) {
                        $Input.keypress(function () {
                            $InputHiden.val("");
                        });
                        if (request.term == "") {
                            $InputHiden.val("");
                        }
                        if (IsNotNullOrEmpty(settings.sAjaxUrl)) {

                            $Input.focusout(function () {

                            });

                            var success = function (data) {
                                if (data != null) {
                                    //if (data.ErroCallBack) { handleError(data, null, null); return; }
                                    // 'jPag': Preenche controles para proxima pagina;
                                    if (settings.jPagination.bPaginate) {
                                        $OffSet.val(data.offset);
                                        $Count.val(data.result_count);
                                    }
                                    var valores = new Array();
                                    if (data.results) {
                                        if (data.result_count != 0) {
                                            for (var i = 0; i < data.results.length; i++) {
                                                var outrosvalores = new Array();
                                                if (settings.dOthersHidden.length > 0) {
                                                    for (var xt = 0; xt < settings.dOthersHidden.length; xt++) {
                                                        outrosvalores[xt] = eval("data.results[i]." + settings.dOthersHidden[xt].campo_base);
                                                    }
                                                }
                                                valores[i] = { value: eval("data.results[i]." + settings.sValueDataName), id: eval("data.results[i]." + settings.sKeyDataName), option: this, outros: outrosvalores, ds_autocomplete: data.results[i].ds_autocomplete };
                                            }
                                        } else {
                                            $InputHiden.val("");
                                        }
                                    }
                                    response(valores);
                                }
                            };

                            var complete = function (jqXHR, textStatus) {
                                $Input.removeClass("ui-autocomplete-loading");
                            }

                            //Ajax(settings.sAjaxUrl.split("?")[0], "POST", getDataSubmit(request.term), success, null, complete, null, settings.bCross);
                            $.ajaxlight({
                                sUrl: settings.sAjaxUrl.split("?")[0],
                                sType: "POST",
                                oData: getDataSubmit(request.term),
                                fnSuccess: success,
                                fnComplete: complete,
                                fnError: null,
                                bAsync: true,
                                bCallback: settings.bCross
                            });

                        } else {
                            if (settings.jData != null) {
                                var regE = (request.term == "...") ? "" : $.ui.autocomplete.escapeRegex(request.term);
                                var matcher = new RegExp("" + regE, "i");
                                var jBusca = $.grep(settings.jData, function (item, index) {
                                    return matcher.test(eval("item." + settings.sValueDataName));
                                });
                                var source = $.map(jBusca, function (item) {
                                    var outrosvalores = new Array();
                                    if (settings.dOthersHidden.length > 0) {
                                        for (var xt = 0; xt < settings.dOthersHidden.length; xt++) {
                                            outrosvalores[xt] = eval("item." + settings.dOthersHidden[xt].campo_base);
                                        }
                                    }
                                    return { id: eval("item." + settings.sKeyDataName), value: eval("item." + settings.sValueDataName), outros: outrosvalores };
                                });
                                response(source);
                            } else {
                                $InputHiden.val("");
                            }

                        }
                    },
                    open: function () {
                        // ajusta problema do tamanho do autocomplet quando o texto é maior que area quebrando a linha
                        $(this).autocomplete("widget").css({ "width": ($(this).width() + 13 + "px") });


                        if (settings.jPagination.bPaginate && settings.jPagination.bButtonsPaginate) {
                            var InfoPaginate = getInfoPaginate();
                            if (InfoPaginate.RegTotal > 0) {
                                $('.jPagBtn').unbind('click');
                                $('.ui-autocomplete').append('<li class="ui-menu-item-disabled">' + getButtonsPaginate() + '</li>');
                                $('.ui-menu-item-disabled').on('click', ".jPagBtn", function (e) {
                                    $('.jPagBtn').unbind('click');
                                    AtiveBusca($(this).attr("value"), e);
                                    return false;
                                });
                            }
                        }
                        $(this).addClass("ui-corner-top");
                        $Input.removeClass("ui-autocomplete-loading");
                    },
                    close: function () {
                        $(this).removeClass("ui-corner-top");
                    },
                    select: function (e, ui) {
                        // autocomplete: preenche valores selecionado no campo e no hiden 
                        if (ui.item != null) {
                            $Input.val(ui.item.value);
                            $InputHiden.val(ui.item.id).trigger('change');
                            if (settings.dOthersHidden.length > 0) {
                                for (var q = 0; q < settings.dOthersHidden.length; q++) {
                                    $control = $("#" + settings.dOthersHidden[q].campo_app);
                                    if ($control.get(0).tagName == "LABEL") {
                                        $control.text(ui.item.outros[q] != null ? ui.item.outros[q] : "")
                                    }
                                    else {
                                        $control.val(ui.item.outros[q] != null ? ui.item.outros[q] : "");
                                    }
                                    $control.trigger('change');
                                }
                            }
                        }
                        $Input.blur();
                        e.preventDefault();
                        return false;
                    }
                });

                if (IsNotNullOrEmpty(settings.appendTo)) {
                    $Input.autocomplete("option", "appendTo", "#modal_filtros_norma")
                }

                $LinkAll.click(function (e) {
                    $Input.val("");
                    $InputHiden.val("").trigger("change");
                    if (settings.dOthersHidden) {
                        for (var q = 0; q < settings.dOthersHidden.length; q++) {
                            $("#" + settings.dOthersHidden[q].campo_app).val("").trigger("change");
                        }
                    }

                    resetControlsPaginate();
                    $TermoBusca.val("...");
                    $Input.autocomplete("search", "...");
                    $Input.focus();
                });

                $Input.change(function (e) {

                    //limpa o hidden do id caso limpem o campo 
                    if ($.trim($Input.val()) == "") {
                        $InputHiden.val("").trigger("change");
                        if (settings.dOthersHidden) {
                            for (var q = 0; q < settings.dOthersHidden.length; q++) {
                                $("#" + settings.dOthersHidden[q].campo_app).val("").trigger("change");
                            }
                        }
                    }

                    //limpa o campo, caso tenham preenchido um assunto que não exista no autocomplete
                    if (!settings.bAddInexistente)
                        if ($InputHiden.val() == "")
                            $Input.val("");

                    resetControlsPaginate();
                });

                $Input.blur(function (e) {
                    if ($.trim($Input.val()) == "")
                        $InputHiden.trigger("change");

                    $Input.removeClass("ui-autocomplete-loading");
                });
            });

            return this;

        };


    })(jQuery);

    $.extend($.ui.autocomplete.prototype, {
        _renderItem: function (ul, item) {
            var term = this.element.val(), html = item.label;
            if (term != "" && term != "...")
                if (IsNotNullOrEmpty(item.ds_autocomplete)) {
                    html = item.ds_autocomplete;
                }
                else {
                    html = html.replace(new RegExp("(" + term + ")", "gi"), "<b>$&</b>");
                }
            return $("<li></li>").data("item.autocomplete", item).append($("<a></a>").html(html)).appendTo(ul);
        }
    });

});
