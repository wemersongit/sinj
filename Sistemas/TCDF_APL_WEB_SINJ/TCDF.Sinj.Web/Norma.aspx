<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Norma.aspx.cs" Inherits="TCDF.Sinj.Web.Norma" %>
<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=Edge" />
    <meta name="robots" content="noindex,nofollow" />
    <meta name="googlebot" content="noindex,nofollow" />
    <meta http-equiv="Content-Language" content="pt-br" />
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <link rel="shortcut icon" type="image/x-icon" href=<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/favicon.png />
    <title><%= !string.IsNullOrEmpty(title) ? title : "Arquivo da Norma" %></title>
    <asp:PlaceHolder runat="server" id="placeHolderHeader"></asp:PlaceHolder>
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/jquery-1.11.0.min.js"></script>
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/platform.js"></script>
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/liblight.js"></script>

    <script type="text/javascript" language="javascript">
        function Print() {
            $('div.control').hide();
            window.print();
            $('div.control').show();
        }
        function exibirAlteracoes(el) {
            if (el.getAttribute('refresh')) {
                location.reload();
            }
            else {

                // NOTE: Essa funcionalidade faz com o que o texto alterado "se transforme" 
                // em compilado (basicamente remove informações de vide), mas define 
                // um comportamento diferenciado para para os parágrafos que contiverem 
                // vides com os seguintes textos no link "revogado pelo(a)" ou 
                // "declarado(a) inconstitucional pelo(a)". By Questor
                var regex = undefined;
                regex = new RegExp(/revogado.*?pelo\(a\)/);
                $('p[replaced_by]:contains("revogado")').each(function (k, p) {
                    if (regex.test($(p).text())) {
                        var aLinkVide = $('a.link_vide', $(p));
                        if (aLinkVide.length > 0) {
                            var aTexto = $(aLinkVide[aLinkVide.length - 1]).text();
                            if (regex.test(aTexto)) {
                                var ds = generateDescriptionToParagraph($(p).text());
                                $(p).text(ds + ' ');
                                $(aLinkVide[aLinkVide.length - 1]).appendTo($(p));
                                $(p).removeAttr('replaced_by');
                            }
                        }
                    }
                });
                regex = new RegExp(/Inconstitucional.*?pelo\(a\)/);
                $('p[replaced_by]:contains("Inconstitucional")').each(function (k, p) {
                    if (regex.test($(p).text())) {
                        var aLinkVide = $('a.link_vide', $(p));
                        if (aLinkVide.length > 0) {
                            var aTexto = $(aLinkVide[aLinkVide.length - 1]).text();
                            if (regex.test(aTexto)) {
                                var ds = generateDescriptionToParagraph($(p).text());
                                $(p).text(ds + ' ');
                                $(aLinkVide[aLinkVide.length - 1]).appendTo($(p));
                                $(p).removeAttr('replaced_by');
                            }
                        }
                    }
                });
                $('p[replaced_by]').remove();
                $(el).attr('refresh', '1');
                $(el).text('Exibir Alterações');
            }
        }

        //Verifica os links de LECO, ordena e insere o link 'exibir mais...' e 'exibir menos...' caso exista mais de 3 links de LECO
        function verificarLinks() {
            var aP = $('#div_texto > p');
            var i = 0;
            var length = aP.length;
            var pAfter = null;
            var aOrder = [];
            for (; i < length; i++) {
                if ($('a[href]', aP[i]).length == 1 && $('a[href]', aP[i]).text().toLowerCase().indexOf('legislação correlata') == 0) {
                    $('a[href]', aP[i]).attr('show', '1').attr('leco', 'leco');
                    aOrder.push({ 'text': $('a[href]', aP[i]).text(), 'href': $('a[href]', aP[i]).attr('href') });
                    if (i >= 3) {
                        $('a[href]', aP[i]).attr('show', '0');
                        pAfter = aP[i];
                    }
                }
                else {
                    break;
                }
            }
            aOrder.sort(function (a1, a2) {
                return a1.text > a2.text ? 1 : a1.text < a2.text ? -1 : 0;
            });
            $('#div_texto a[leco="leco"]').each(function (i, o) {
                o.innerText = aOrder[i].text;
                o.href = aOrder[i].href;
            });
            if (pAfter != null && $('a[show="0"]').length > 3) {
                $('a[show="0"]').closest('p').hide();
                $('<p id="p_show"><a href="javascript:void(0);" onclick="exibirLinks()">Exibir mais...</a></p>').insertAfter($(pAfter));
                $('<p id="p_hide" style="display:none"><a href="javascript:void(0);" onclick="esconderLinks()">Exibir menos...</a></p>').insertAfter($(pAfter));
            }
        }
        function exibirLinks() {
            $('a[show="0"]').closest('p').show();
            $('#p_hide').show();
            $('#p_show').hide();
        }
        function esconderLinks() {
            $('a[show="0"]').closest('p').hide();
            $('#p_hide').hide();
            $('#p_show').show();
        }

        function generateDescriptionToParagraph(text) {
            var ds = '';
            text = $.trim(text);
            if (text != "") {
                var palavras = text.split(' ');
                if (palavras[0] == 'ANEXO' || palavras[0] == 'TÍTULO' || palavras[0] == 'CAPÍTULO' || palavras[0] == 'Art.' || (palavras[0] == 'Parágrafo' && palavras[1] == 'Único') || palavras[0] == '§') {
                    ds = palavras[0] + ' ' + palavras[1];
                }
                else if (ehInciso(palavras[0]) || ehAlinea(palavras[0]) || ehNum(palavras[0])) {
                    ds = palavras[0];
                }
                else {
                    ds = "...";
                }
            }
            return ds;
        }

        function ehInciso(termo) {
            var lastIndex = termo.indexOf("-");
            if (lastIndex > 0) {
                termo = termo.substring(0, lastIndex);
            }
            return termo.match(/^[IVXLCDM]+$/i);
        }

        function ehAlinea(termo) {
            var lastIndex = termo.indexOf(")");
            if (lastIndex < 0) {
                return false;
            }
            if (termo.length == (lastIndex + 1)) {
                termo = termo.substring(0, lastIndex);
                return termo.match(/[a-z]/i);
            }
            return false;
        }

        function ehNum(termo) {
            var lastIndex = termo.indexOf(".");
            if (lastIndex < 0) {
                return false;
            }
            if (termo.length == (lastIndex + 1)) {
                termo = termo.substring(0, lastIndex);
                return isInt(termo);
            }
            return false;
        }

        $(document).ready(function () {
            if ($('#div_erro').length > 0) {
                $('#div_norma').hide();
            }
            else {
                if ($('p[replaced_by]').length <= 0 || $('p').length == $('p[replaced_by]').length) {
                    $('.compilado').hide();
                }
                verificarLinks();
            }

            if ($('#div_texto').length == 1 && window.location.href.indexOf('#') > -1) {
                console.log("entrou no if 1");
                var i = window.location.href.split('#');
                var cap;
                console.log(i);
                if (typeof i[1] != undefined) {
                    console.log("entrou no if 2");
                    if (i[1] != "") {
                        console.log("entrou em outro if");
                        var disAfetado = $(`#${i[1]}`);
                        if (disAfetado.length > 1 && disAfetado.is(':visible')) {
                            cap = i[1];
                            console.log(cap);
                        } else {
                            var divToCap;
                            divToCap = i[1].split('_');
                            cap = divToCap[0];
                            console.log(cap);
                        }
                    }
                }

                $('html, body').animate({
                    scrollTop: $(`#${cap}`).offset().top
                }, 600);
            }
        });
    </script>
    <style type="text/css">
        .control{width:100%; float:left;}
        .print{float:right;}
        .compilado{float:left;}
        div.strike { text-decoration: line-through; }
        body, p, h1{font-family:Tahoma;font-size:14px;}
        p[nota=nota] {font-size:12px; border: 1px solid #777; padding: 5px;}
    </style>
</head>
<body>
    <div id="div_norma">
        <a href="<%= ResolveUrl("~/") %>" title="Visitar o SINJ-DF - Sistema Integrado de Normas Jurídicas do DF"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/logo_sinj.png" alt="SINJ-DF" /></a>
        <div class="control">
            <div class="compilado">
                <a href="javascript:void(0)" onclick="javascript:exibirAlteracoes(this);">
                    Texto Compilado
                </a>
            </div>
            <div class="print">
                <a href="javascript:void(0);" onclick="javascript:Print();" title="Imprimir">
                    <img alt="print" src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_print_p.png"/>
                </a>
            </div>
        </div>
        <div id="div_texto" runat="server"></div>
    </div>
</body>
</html>
