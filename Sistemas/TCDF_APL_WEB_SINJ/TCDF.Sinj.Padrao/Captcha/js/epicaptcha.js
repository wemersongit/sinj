(function ($) {
    $.fn.Epicaptcha = function (options) {
        var defaults = {
            width: 179,
            mobile: 0,
            successMessage: "Obrigado! Os dados foram enviados com sucesso!",
            startMessage: "Mova seu cursor sobre a barra acima da esquerda para a direita.",
            startMessageMobile: "Toque em cada c&iacute;rculo da esquerda para a direita para desbloquear.",
            errorMessage: "MOVA seu CURSOR sobre a barra acima da ESQUERDA para a DIREITA.",
            limitMessage: "Voc&ecirc; s&oacute; pode enviar este formul&aacute;rio uma vez.",
            completeMessage: "",
            notCompleteMessage: "Mova seu cursor sobre a barra acima da esquerda para a direita.",
            halfwayMessage: "Validando...",
            threeQuarterMessage: "Validando...",
            theme: "green",   //green,default,fancy,greyscale,lightblue,tribal,basic,bluebar//
            fontColour: "#555",
            errorColour: "#cc0000",
            successColour: "#006898",
            submitUrl: "#",
            theFormID: "contact-form",
            buttonID: "btnEpi",
            debug: false,
            fnSuccess: null
        };
        var settings = $.extend({}, defaults, options);
        var epiElement = $(this);

        var x = 0;
        var y = 0;

        var mousein = 0;
        var checkpoint = 0;
        var ux = 0;
        var uy = 0;
        var restart = 0;
        var comp = 0;
        var up = 0;
        var formName = "";
        var settings;

        var nAgt = navigator.userAgent;
        var browserName = navigator.appName;
        var fullVersion = '' + parseFloat(navigator.appVersion);
        var majorVersion = parseInt(navigator.appVersion, 10);
        var nameOffset, verOffset, ix;

        if ((verOffset = nAgt.indexOf("Opera")) != -1) {
            browserName = "Opera";
            if ((navigator.userAgent.toString().indexOf("Opera Mini")) != -1) {
                browserName = "Opera Mini";
            }
            if ((navigator.userAgent.toString().indexOf("htc")) != -1) {
                browserName = "Opera Mini";
            }

            if ((navigator.userAgent.toString().indexOf("HTC")) != -1) {
                browserName = "Opera Mini";
            }
            fullVersion = nAgt.substring(verOffset + 6);
            if ((verOffset = nAgt.indexOf("Version")) != -1)
                fullVersion = nAgt.substring(verOffset + 8);
        }
        else if ((verOffset = nAgt.indexOf("MSIE")) != -1) {
            browserName = "Microsoft Internet Explorer";
            fullVersion = nAgt.substring(verOffset + 5);
        }
        else if ((verOffset = nAgt.indexOf("Chrome")) != -1) {
            browserName = "Chrome";
            fullVersion = nAgt.substring(verOffset + 7);
        }
        else if ((verOffset = nAgt.indexOf("Safari")) != -1) {
            browserName = "Safari";
            if ((navigator.userAgent.toString().indexOf("Android")) != -1) {
                browserName = "dolphin";
            }

            if ((navigator.userAgent.toString().indexOf("iPhone")) != -1) {
                browserName = "iphone";
            }

            if ((navigator.userAgent.toString().indexOf("iPad")) != -1) {
                browserName = "ipad";
            }
            fullVersion = nAgt.substring(verOffset + 7);
            if ((verOffset = nAgt.indexOf("Version")) != -1)
                fullVersion = nAgt.substring(verOffset + 8);
        }
        else if ((verOffset = nAgt.indexOf("Firefox")) != -1) {
            browserName = "Firefox";
            if ((navigator.userAgent.toString().indexOf("Android")) != -1) {
                browserName = "dolphin";
            }
            fullVersion = nAgt.substring(verOffset + 8);
        }
        else if ((nameOffset = nAgt.lastIndexOf(' ') + 1) < (verOffset = nAgt.lastIndexOf('/'))) {
            browserName = nAgt.substring(nameOffset, verOffset);
            fullVersion = nAgt.substring(verOffset + 1);
            if (browserName.toLowerCase() == browserName.toUpperCase()) {
                browserName = navigator.appName;
            }
        }

        if ((ix = fullVersion.indexOf(";")) != -1) fullVersion = fullVersion.substring(0, ix);
        if ((ix = fullVersion.indexOf(" ")) != -1) fullVersion = fullVersion.substring(0, ix);

        majorVersion = parseInt('' + fullVersion, 10);
        if (isNaN(majorVersion)) {
            fullVersion = '' + parseFloat(navigator.appVersion);
            majorVersion = parseInt(navigator.appVersion, 10);
        }

        var ttsize = settings.width;
        ttsize = ttsize / 7;
        ttsize = Math.round(ttsize / 10) * 10;
        if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
            settings.mobile = 1;
        }
        if (/Trident\/7\./.test(navigator.userAgent)) {
            settings.mobile = 0;
            browserName = "Microsoft Internet Explorer";
        }

        if ((browserName == "Microsoft Internet Explorer" || browserName == "Firefox" || browserName == "Opera" || browserName == "Safari" || browserName == "Chrome") && settings.mobile == 0) {
            $("#" + settings.buttonID).click(function () {
                send();
            });

            idd = $("#" + settings.buttonID).parents("form");
            formName = idd.attr("id");
            epiElement.before("<div class='epicaptcha-clearfix'></div>");
            epiElement.html("<span id='epicaptcha_lock'></span><div id='epicaptcha_captcha'></div>")
            epiElement.attr("style", "width:" + (settings.width + 64) + "px");
            epiElement.find("#epicaptcha_captcha").attr("style", "width:" + settings.width + "px");
            epiElement.find("#epicaptcha_captcha").html("<div id='epicaptcha_select'></div>");
            epiElement.find("#epicaptcha_captcha").after("<div id='epicaptcha_message' style=\"width: " + settings.width + "px\">" + settings.startMessage + "</div>");

            if (settings.debug == true) {
                epiElement.find("#epicaptcha_message").after("<div id='epicaptcha_message' style=\"width: " + settings.width + "px\"><h2 id=\"epicaptcha_status\"></h2></div>");
            }

            var theme = 0
            if (settings.theme == "default") {
                epiElement.find("#epicaptcha_captcha").addClass("epicaptcha_captchaDefault");
                epiElement.find("#epicaptcha_select").addClass("epicaptcha_selectDefault");
                epiElement.find("#epicaptcha_lock").addClass("epicaptcha_lockDefault");
                epiElement.find("#epicaptcha_message").addClass("epicaptcha_messageDefault");
                theme = 1
            }

            if (settings.theme == "fancy") {
                epiElement.find("#epicaptcha_captcha").addClass("epicaptcha_captchaFancy");
                epiElement.find("#epicaptcha_select").addClass("epicaptcha_selectFancy");
                epiElement.find("#epicaptcha_lock").addClass("epicaptcha_lockFancy");
                epiElement.find("#epicaptcha_message").addClass("epicaptcha_messageFancy");
                epiElement.addClass("epicaptchaFancy");
                theme = 1
            }

            if (settings.theme == "greyscale") {
                epiElement.find("#epicaptcha_captcha").addClass("epicaptcha_captchaGreyScale");
                epiElement.find("#epicaptcha_select").addClass("epicaptcha_selectGreyScale");
                epiElement.find("#epicaptcha_lock").addClass("epicaptcha_lockGreyScale");
                epiElement.find("#epicaptcha_message").addClass("epicaptcha_messageGreyScale");
                theme = 1
            }

            if (settings.theme == "green") {
                epiElement.find("#epicaptcha_captcha").addClass("epicaptcha_captchagreen");
                epiElement.find("#epicaptcha_select").addClass("epicaptcha_selectgreen");
                epiElement.find("#epicaptcha_lock").addClass("epicaptcha_lockgreen");
                epiElement.find("#epicaptcha_message").addClass("epicaptcha_messagegreen");
                theme = 1
            }

            if (settings.theme == "lightblue") {
                epiElement.find("#epicaptcha_captcha").addClass("epicaptcha_captchalightblue");
                epiElement.find("#epicaptcha_select").addClass("epicaptcha_selectlightblue");
                epiElement.find("#epicaptcha_lock").addClass("epicaptcha_locklightblue");
                epiElement.find("#epicaptcha_message").addClass("epicaptcha_messagelightblue");
                theme = 1
            }

            if (settings.theme == "tribal") {
                epiElement.find("#epicaptcha_captcha").addClass("epicaptcha_captchatribal");
                epiElement.find("#epicaptcha_select").addClass("epicaptcha_selecttribal");
                epiElement.find("#epicaptcha_lock").addClass("epicaptcha_locktribal");
                epiElement.find("#epicaptcha_message").addClass("epicaptcha_messagetribal");
                theme = 1
            }

            if (settings.theme == "targetcom") {
                epiElement.find("#epicaptcha_captcha").addClass("epicaptcha_captchatargetcom");
                epiElement.find("#epicaptcha_select").addClass("epicaptcha_selecttargetcom");
                epiElement.find("#epicaptcha_lock").addClass("epicaptcha_locktargetcom");
                epiElement.find("#epicaptcha_message").addClass("epicaptcha_messagetargetcom");
                theme = 1
            }

            if (settings.theme == "basic") {
                epiElement.find("#epicaptcha_captcha").addClass("epicaptcha_captchaBasic");
                epiElement.find("#epicaptcha_select").addClass("epicaptcha_selectBasic");
                epiElement.find("#epicaptcha_lock").addClass("epicaptcha_lockBasic");
                epiElement.find("#epicaptcha_message").addClass("epicaptcha_messageBasic");
                theme = 1
            }

            if (settings.theme == "bluebar") {
                epiElement.find("#epicaptcha_captcha").addClass("epicaptcha_captchabluebar");
                epiElement.find("#epicaptcha_captcha").append("<span id='epicaptcha_stripTop'>&nbsp;</span><span id='epicaptcha_stripBtm'>&nbsp;</span><span id='epicaptcha_capL'>&nbsp;</span><span id='epicaptcha_capR'>&nbsp;</span>");
                epiElement.find("#epicaptcha_select").addClass("epicaptcha_selectbluebar");
                epiElement.find("#epicaptcha_select").append("<span id='epicaptcha_containGlow'><span id='epicaptcha_glow'>&nbsp;</span></span><span id='epicaptcha_handle'>&nbsp;</span>");
                epiElement.find("#epicaptcha_lock").addClass("epicaptcha_lockbluebar");
                epiElement.find("#epicaptcha_message").addClass("epicaptcha_messagebluebar");
                settings.glow = 1;
                theme = 1
            }

            if (theme == 0) {
                epiElement.find("#epicaptcha_captcha").addClass("epicaptcha_captchaBasic");
                epiElement.find("#epicaptcha_select").addClass("epicaptcha_selectBasic");
                epiElement.find("#epicaptcha_lock").addClass("epicaptcha_lockBasic");
                epiElement.find("#epicaptcha_message").addClass("epicaptcha_messageBasic");

            }

            epiElement.find("#epicaptcha_message").css("color", settings.fontColour);

            var lf = epiElement.find("#epicaptcha_captcha").offset().left;
            var tp = epiElement.find("#epicaptcha_captcha").offset().top;
            epiElement.find("#epicaptcha_captcha").after("<div id='epicaptcha_captchaO'></div>");

            epiElement.find("#epicaptcha_captchaO").attr("style", "height:32px; width:" + settings.width + "px; position:absolute; Background:url('./Captcha/img/TRANSPARENT.png') repeat; z-index:99;");

            if (settings.theme == "bluebar") {
                glow();
            }

            epiElement.find("#epicaptcha_captchaO").mouseover(function () { go(); }).mouseout(function () { out() });

            $(document).mousemove(function (e) {
                if (settings.debug == true) {
                    epiElement.find('#epicaptcha_status').html(e.pageX + ', ' + e.pageY);
                }

                x = e.pageX;
                y = e.pageY;

                if (ux >= 0 && uy >= 0 && mousein == 1) {
                    var tmpx = x - ux;

                    if (x < ux + 10 & x < ux + ttsize & checkpoint == 0) {
                        validate("x=" + x + "&y=" + y + "&ux=" + ux + "&uy=" + uy + "&mousein=" + mousein + "&mobile=" + settings.mobile + "&send=" + 0 + "&val1=" + 10 + "&val2=" + ttsize);
                        restart = 0;
                        epiElement.find("#epicaptcha_message").html("");
                        checkpoint = 1;
                    }

                    if (x < ux + 10 & x < ux + ttsize & checkpoint <= 0) {
                        failed();
                        validate("x=" + x + "&y=" + y + "&ux=" + ux + "&uy=" + uy + "&mousein=" + mousein + "&mobile=" + settings.mobile + "&send=" + 0 + "&val1=" + 10 + "&val2=" + ttsize);
                    }

                    if (x > ux + ttsize & x < ux + (ttsize * 2) & checkpoint == 1) {
                        validate("x=" + x + "&y=" + y + "&ux=" + ux + "&uy=" + uy + "&mousein=" + mousein + "&mobile=" + settings.mobile + "&send=" + 0 + "&val1=" + 30 + "&val2=" + 60);
                        checkpoint = 2;
                    }

                    if (x > ux + ttsize & x < ux + (ttsize * 2) & checkpoint <= 1) {
                        failed();
                        validate("x=" + x + "&y=" + y + "&ux=" + ux + "&uy=" + uy + "&mousein=" + mousein + "&mobile=" + settings.mobile + "&send=" + 0 + "&val1=" + ttsize + "&val2=" + (ttsize * 2));
                    }

                    if (x > ux + (ttsize * 2) & x < ux + (ttsize * 3) & checkpoint == 2 & restart == 0) {
                        checkpoint = 3;
                        validate("x=" + x + "&y=" + y + "&ux=" + ux + "&uy=" + uy + "&mousein=" + mousein + "&mobile=" + settings.mobile + "&send=" + 0 + "&val1=" + (ttsize * 2) + "&val2=" + (ttsize * 3));
                    }

                    if (x > ux + (ttsize * 2) & x < ux + (ttsize * 3) & checkpoint <= 2) {
                        failed();
                        validate("x=" + x + "&y=" + y + "&ux=" + ux + "&uy=" + uy + "&mousein=" + mousein + "&mobile=" + settings.mobile + "&send=" + 0 + "&val1=" + (ttsize * 2) + "&val2=" + (ttsize * 3));
                    }

                    if (x > ux + (ttsize * 3) & x < ux + (ttsize * 4) & checkpoint == 3 & restart == 0) {
                        checkpoint = 4;
                        epiElement.find("#epicaptcha_message").html(settings.halfwayMessage);
                        validate("x=" + x + "&y=" + y + "&ux=" + ux + "&uy=" + uy + "&mousein=" + mousein + "&mobile=" + settings.mobile + "&send=" + 0 + "&val1=" + (ttsize * 3) + "&val2=" + (ttsize * 4));
                    }

                    if (x > ux + (ttsize * 3) & x < ux + (ttsize * 2) & checkpoint <= 3) {
                        failed();
                        validate("x=" + x + "&y=" + y + "&ux=" + ux + "&uy=" + uy + "&mousein=" + mousein + "&mobile=" + settings.mobile + "&send=" + 0 + "&val1=" + (ttsize * 3) + "&val2=" + (ttsize * 4));
                    }

                    if (x > ux + (ttsize * 4) & x < ux + (ttsize * 5) & checkpoint == 4 & restart == 0) {
                        checkpoint = 5;
                        validate("x=" + x + "&y=" + y + "&ux=" + ux + "&uy=" + uy + "&mousein=" + mousein + "&mobile=" + settings.mobile + "&send=" + 0 + "&val1=" + (ttsize * 4) + "&val2=" + (ttsize * 5));
                    }

                    if (x > ux + (ttsize * 4) & x < ux + (ttsize * 5) & checkpoint <= 4) {
                        failed();
                        validate("x=" + x + "&y=" + y + "&ux=" + ux + "&uy=" + uy + "&mousein=" + mousein + "&mobile=" + settings.mobile + "&send=" + 0 + "&val1=" + (ttsize * 4) + "&val2=" + (ttsize * 5));
                    }

                    if (x > ux + (ttsize * 5) & x < ux + (ttsize * 6) & checkpoint == 5 & restart == 0) {
                        checkpoint = 6;
                        epiElement.find("#epicaptcha_message").html(settings.threeQuarterMessage);
                        validate("x=" + x + "&y=" + y + "&ux=" + ux + "&uy=" + uy + "&mousein=" + mousein + "&mobile=" + settings.mobile + "&send=" + 0 + "&val1=" + (ttsize * 5) + "&val2=" + (ttsize * 6));
                    }

                    if (x > ux + (ttsize * 5) & x < ux + (ttsize * 6) & checkpoint <= 5) {
                        failed();
                        validate("x=" + x + "&y=" + y + "&ux=" + ux + "&uy=" + uy + "&mousein=" + mousein + "&mobile=" + settings.mobile + "&send=" + 0 + "&val1=" + (ttsize * 5) + "&val2=" + (ttsize * 6));
                    }

                    if (x > ux + (settings.width - 9) & checkpoint == 6 & restart == 0) {
                        checkpoint = 7;
                        epiElement.find('#epicaptcha_message').html("" + settings.completeMessage);
                        changelock();
                        comp = 1;
                        epiElement.find("#epicaptcha_select").attr("style", "width:" + settings.width + "px");
                        validate("x=" + x + "&y=" + y + "&ux=" + ux + "&uy=" + uy + "&mousein=" + mousein + "&mobile=" + settings.mobile + "&send=" + 0 + "&val2=" + (settings.width - 9));
                    }

                    if (x > ux + (settings.width - 9) & checkpoint <= 6) {
                        failed();
                        validate("x=" + x + "&y=" + y + "&ux=" + ux + "&uy=" + uy + "&mousein=" + mousein + "&mobile=" + settings.mobile + "&send=" + 0 + "&val2=" + (settings.width - 9));
                    }

                    if (x < ux + 30 || restart == 0) {
                        if (comp == 0) {

                            epiElement.find("#epicaptcha_select").attr("style", "width:" + tmpx + "px");
                        }
                    }
                }
            });
        }
        else {
            $("#" + settings.buttonID).click(function () {
                send();
            });

            idd = $("#" + settings.buttonID).parents("form");
            formName = idd.attr("id");

            epiElement.html("<img id='epicaptcha_lock' src='" + _urlPadrao + "/Captcha/img/lock-icon.png' alt='locked'><div id='epicaptcha_captcham'></div>")

            epiElement.find("#epicaptcha_captcham").attr("style", "width:" + settings.width + "px");
            epiElement.find("#epicaptcha_captcham").html("<div id='epicaptcha_s1' class='epicaptcha_circle'></div><div id='epicaptcha_s2' class='epicaptcha_circle'></div><div id='epicaptcha_s3' class='epicaptcha_circle'></div><div id='epicaptcha_s4' class='epicaptcha_circle'></div><div id='epicaptcha_s5' class='epicaptcha_circle'></div>");
            epiElement.find("#epicaptcha_s5").after("<div id='epicaptcha_message'>" + settings.startMessageMobile + "</div>");
            epiElement.find("#epicaptcha_message").css("color", settings.fontColour);

            epiElement.find('.epicaptcha_circle').click(function () {
                $(this).attr("style", "Background:url('./Captcha/img/mobliecapM.png') no-repeat;");
                level($(this).attr("id"));
            });
        }

        function changelock() {
            var theme = 0
            if (settings.theme == "default") {

                epiElement.find("#epicaptcha_lock").addClass("epicaptcha_lockUnlockDefault");
                epiElement.find("#epicaptcha_lock").removeClass("epicaptcha_lockDefault");


                theme = 1
            }

            if (settings.theme == "fancy") {
                epiElement.find("#epicaptcha_lock").removeClass("epicaptcha_lockfancy");
                epiElement.find("#epicaptcha_lock").addClass("epicaptcha_lockUnlockFancy");



                theme = 1
            }

            if (settings.theme == "greyscale") {
                epiElement.find("#epicaptcha_lock").removeClass("epicaptcha_lockGreyScale");
                epiElement.find("#epicaptcha_lock").addClass("epicaptcha_lockUnlockGreyScale");



                theme = 1
            }

            if (settings.theme == "green") {
                epiElement.find("#epicaptcha_lock").removeClass("epicaptcha_lockgreen");
                epiElement.find("#epicaptcha_lock").addClass("epicaptcha_lockUnlockgreen");



                theme = 1
            }

            if (settings.theme == "lightblue") {
                epiElement.find("#epicaptcha_lock").removeClass("epicaptcha_locklightblue");
                epiElement.find("#epicaptcha_lock").addClass("epicaptcha_lockUnlocklightblue");



                theme = 1
            }

            if (settings.theme == "tribal") {
                epiElement.find("#epicaptcha_lock").removeClass("epicaptcha_locktribal");
                epiElement.find("#epicaptcha_lock").addClass("epicaptcha_lockUnlocktribal");

                theme = 1
            }

            if (settings.theme == "targetcom") {
                epiElement.find("#epicaptcha_lock").removeClass("epicaptcha_locktargetcom");

                epiElement.find("#epicaptcha_lock").addClass("epicaptcha_lockUnlocktargetcom");



                theme = 1
            }

            if (settings.theme == "basic") {
                epiElement.find("#epicaptcha_lock").removeClass("epicaptcha_lockBasic");
                epiElement.find("#epicaptcha_lock").addClass("epicaptcha_lockUnlockBasic");



                theme = 1
            }

            if (settings.theme == "bluebar") {
                epiElement.find("#epicaptcha_lock").removeClass("epicaptcha_lockbluebar");
                epiElement.find("#epicaptcha_lock").addClass("epicaptcha_lockUnlockbluebar");



                theme = 1


            }

            if (theme == 0) {
                epiElement.find("#epicaptcha_lock").removeClass("epicaptcha_lockBasic");
                epiElement.find("#epicaptcha_lock").addClass("epicaptcha_lockUnlockBasic");


            }
        }

        var tempp = 0;

        function glow() {

            if (tempp == 0) {
                tempp = 1;
                epiElement.find("#epicaptcha_glow").animate({ "left": "+=300px" }, 1200);
                setTimeout('norm()', 2000);
            }

        }

        function norm() {
            epiElement.find("#epicaptcha_glow").attr("style", "left : -60px");
            tempp = 0;
            setTimeout('glow()', 50);

        }

        function level(id) {
            if (id == "epicaptcha_s1" && checkpoint > 0) {
                failed2();
                validate("x=" + 1 + "&mobile=" + settings.mobile + "&send=" + 0);
            }
            if (id == "epicaptcha_s1" && checkpoint == 0) {
                checkpoint = 1;
                validate("x=" + 1 + "&mobile=" + settings.mobile + "&send=" + 0);
            }

            if (id == "epicaptcha_s2" && checkpoint != 1) {
                failed2();
                validate("x=" + 2 + "&mobile=" + settings.mobile + "&send=" + 0);
            }
            if (id == "epicaptcha_s2" && checkpoint == 1) {
                checkpoint = 2;
                validate("x=" + 2 + "&mobile=" + settings.mobile + "&send=" + 0);
            }

            if (id == "epicaptcha_s3" && checkpoint != 2) {
                failed2();
                validate("x=" + 3 + "&mobile=" + settings.mobile + "&send=" + 0);
            }
            if (id == "epicaptcha_s3" && checkpoint == 2) {
                checkpoint = 3;
                validate("x=" + 3 + "&mobile=" + settings.mobile + "&send=" + 0);
            }


            if (id == "epicaptcha_s4" && checkpoint != 3) {
                failed2();
                validate("x=" + 4 + "&mobile=" + settings.mobile + "&send=" + 0);
            }
            if (id == "epicaptcha_s4" && checkpoint == 3) {
                checkpoint = 4;
                validate("x=" + 4 + "&mobile=" + settings.mobile + "&send=" + 0);
            }

            if (id == "epicaptcha_s5" && checkpoint != 4) {
                failed2();
                validate("x=" + 5 + "&mobile=" + settings.mobile + "&send=" + 0);
            }
            if (id == "epicaptcha_s5" && checkpoint == 4) {
                checkpoint = 5;
                epiElement.find('#epicaptcha_message').html("" + settings.completeMessage);
                epiElement.find('#epicaptcha_lock').attr("src", "./Captcha/img/GreenTick.png");
                comp = 1;
                validate("x=" + 5 + "&mobile=" + settings.mobile + "&send=" + 0);
            }

        }

        function failed() {
            epiElement.find("#epicaptcha_message").html("" + settings.errorMessage);
            epiElement.find("#epicaptcha_message").css("color", settings.errorColour);
            delayedAlert();
            checkpoint = 0;
            restart = 1;
        }

        function failed2() {
            epiElement.find("#epicaptcha_message").html("" + settings.errorMessage);
            epiElement.find("#epicaptcha_message").css("color", settings.errorColour);
            delayedAlert();
            checkpoint = 0;
            epiElement.find('.epicaptcha_circle').attr("style", "Background:url('./Captcha/img/mobliecap.png') no-repeat;");
        }

        function delayedAlert() {
            timeoutID = window.setTimeout(backred, 3000);
        }

        function backred() {
            epiElement.find("#epicaptcha_message").css("color", settings.fontColour);
        }

        function go() {
            mousein = 1;
            if (ux == 0 & uy == 0) {
                ux = epiElement.find("#epicaptcha_captcha").offset().left;
                uy = epiElement.find("#epicaptcha_captcha").offset().top;
            }
        }

        function out() {
            if (comp == 0) {
                failed();
                mousein = 0;
                validate("x=" + x + "&y=" + y + "&ux=" + ux + "&uy=" + uy + "&mousein=" + mousein + "&mobile=" + settings.mobile + "&send=" + 0);
                checkpoint = 0;
                restart = 1;
                epiElement.find("#epicaptcha_select").attr("style", "width:" + 1 + "px");
            }
        }

        function send() {
            if (comp == 1) {
                //epiElement.find("#epicaptcha_message").html("<center><image src='"+_urlPadrao+"/Captcha/img/loader.gif'/></center>");
                validate("x=" + x + "&y=" + y + "&ux=" + ux + "&uy=" + uy + "&mousein=" + mousein + "&mobile=" + settings.mobile + "&send=" + 1);
            }
            else {
                epiElement.find("#epicaptcha_message").html("" + settings.notCompleteMessage);
                epiElement.find("#epicaptcha_message").css("color", settings.errorColour);
                delayedAlert();
            }
        }

        function validate(input) {
            if (input.indexOf('send=1') > -1) {
                $('#' + settings.theFormID + ' .notify').html('');
                try {
                    Validar(settings.theFormID);
                    ValidarRegrasEspecificas(settings.theFormID);
                    var sucesso = typeof settings.fnSuccess === "function" ? settings.fnSuccess : function (data) {
                        gComplete();
                        if (IsNotNullOrEmpty(data, 'error_message')) {
                            $('#' + settings.theFormID + ' .notify').messagelight({
                                sTitle: "Erro",
                                sContent: data.error_message,
                                sType: "error",
                                sWidth: "",
                                iTime: null
                            });
                        }
                        else if (IsNotNullOrEmpty(data, 'id_doc_success')) {
                            $('<div id="modal_notificacao_modal_salvar" />').modallight({
                                sTitle: "Sucesso",
                                sContent: "Salvo com sucesso.",
                                sType: "success",
                                oButtons: [{ text: "Ok", click: function () { $(this).dialog('close'); } }],
                                fnClose: function () {
                                    if (!Redirecionar('?id_doc=' + data.id_doc_success + "&time=" + new Date().getTime())) {
                                        location.reload();
                                    }
                                }
                            });
                        }
                        else if (IsNotNullOrEmpty(data, 'success_email')) {
                            $('<div id="modal_notificacao_modal_salvar" />').modallight({
                                sTitle: "Sucesso",
                                sContent: "Nova senha enviada para o email " + data.success_email,
                                sType: "success",
                                oButtons: [{ text: "Ok", click: function () { $(this).dialog('close'); } }],
                                fnClose: function () {
                                    document.location.href = "./LoginNotifiqueme.aspx";
                                }
                            });
                        }
                    }
                    $.ajaxlight({
                        sUrl: settings.submitUrl + "?" + input,
                        sType: "POST",
                        fnSuccess: sucesso,
                        sFormId: settings.theFormID,
                        fnBeforeSubmit: gInicio,
                        bAsync: true,
                        iTimeout: 60000
                    });
                }
                catch (ex) {
                    $('#' + settings.theFormID + ' .notify').messagelight({
                        sTitle: "Erro nos dados informados",
                        sContent: ex,
                        sType: "error"
                    });
                }
            }
            return false;
        }

    };
})(jQuery);
