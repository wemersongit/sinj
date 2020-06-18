// necessário carregar: 'jquery-1.11.0.min.js', ' jquery-ui-1.10.4.custom.min.js', 'script.js'  e 'js.ashx'
$(function () {
    (function ($) {
        $.fn.modallight = function (options) {
            if (typeof options === "string") {
                return $(this).dialog(options);
                return this;
            }
            if (isArray(options) && options.length == 3) {
                return $(this).dialog(options[0], options[1], options[2]);
            }
            var defaults = {
                sTitle: "",
                sContent: "",
                sType: "default",
                sWidth: "350",
                sHeight: "auto",
                oPosition: "center",
                jqXHR: null,
                oButtons: [{ text: "Ok", click: function () { $(this).dialog('close'); } }],
                fnCreated: null,
                fnClose: null,
                autoOpen: true
            }
            var settings = $.extend({}, defaults, options);
            var $element = $(this);
            return this.each(function () {
                if (settings.sContent != "") {
                    $element.html(settings.sContent);
                    if (settings.jqXHR && settings.jqXHR.responseText) {
                        $element
                        .append(
                            $('<a />')
                            .append('...')
                            .click(function (event) {
                                var $parent = $(event.target).parent();
                                var $div_error = $($parent.find('div.error')[0]);
                                if ($div_error.html() == "") {
                                    $div_error.html(settings.jqXHR.responseText);
                                    $element.dialog('option', 'position', 'center');
                                    $element.dialog('option', 'minWidth', '500');
                                    $element.dialog('option', 'height', '350');
                                }
                                else {
                                    $div_error.html("");
                                    $element.dialog('option', 'position', 'center');
                                    $element.dialog('option', 'minWidth', '350');
                                    $element.dialog('option', 'width', '350');
                                    $element.dialog('option', 'height', 'auto');
                                }
                            })
                        )
                        .append(
                            '<div class="word error"/>'
                        );
                    }
                }
                $element.dialog({
                    close: function (event, ui) { if (settings.fnClose != null) { settings.fnClose(); } },
                    hide: "explode",
                    show: "puff",
                    resizable: false,
                    modal: true,
                    dialogClass: settings.sType,
                    minWidth: settings.sWidth,
                    height: settings.sHeight,
                    title: settings.sTitle,
                    buttons: settings.oButtons,
                    autoOpen: settings.autoOpen
                });
                $element.dialog('option', 'position', settings.oPosition);
                if (typeof settings.fnCreated == "function") {
                    settings.fnCreated();
                }
            });

        }
        $.fn.messagelight = function (options) {
            var $element = $(this);
            var execOption = function (op) {
                functions = {
                    "destroy": function () {
                        $element.html('');
                        $element.hide();
                        return $element;
                    }
                }
                return functions[op]();
            }
            if (typeof options === "string") {
                return execOption(options);
            }
            var defaults = {
                sTitle: "", //uma url que retorne o json do portal do cliente.
                sContent: "", //json do portal, uma alternativa é usar o json fornecido pela url em sUrlPortal.
                sType: "default",
                sWidth: "",
                iTime: null
            }
            var settings = $.extend({}, defaults, options);
            var $element = $(this);
            return this.each(function () {
                $element.show();
                $element.html("<div class='ui-state-highlight ui-corner-all " + settings.sType.toLowerCase() + "'" + " style='width:" + settings.sWidth + "'" + "><span class='ui-button-icon-primary ui-icon ui-icon-closethick' style='float:right;' onclick='javascript:fnCloseMessageLight(this)'></span><span class='ui-icon ui-icon-info ui-message'></span>" + settings.sContent + "</div>");
                if (settings.iTime != null) {
                    setTimeout(
                        function(){
                            $element.hide("fade", 3000);
                        },
                        settings.iTime
                    );
                }
            });
        }
    })(jQuery);
});



function fnCloseMessageLight(el) {
    $(el).parent().remove();
}
