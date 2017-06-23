<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Norma.aspx.cs" Inherits="TCDF.Sinj.Portal.Web.Norma" %>
<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Arquivo da Norma</title>
    <asp:PlaceHolder runat="server" id="placeHolderHeader"></asp:PlaceHolder>
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/jquery-1.11.0.min.js"></script>
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/platform.js"></script>

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
                $('p[replaced_by]').remove();
                $(el).attr('refresh', '1');
                $(el).text('Exibir Alterações');
            }
        }
        function verificarLinks() {
            var aP = $('#div_texto > p');
            var i = 0;
            var length = aP.length;
            var pAfter = null;
            var aText = [];
            for (; i < length; i++) {
                if ($('a', aP[i]).length == 1 && $('a', aP[i]).text().toLowerCase().indexOf('legislação correlata') == 0) {
                    aText.push($('a', aP[i]).attr('show', '1').attr('leco', 'leco').text());
                    if (i >= 3) {
                        $('a', aP[i]).attr('show', '0');
                        pAfter = aP[i];
                    }
                }
                else {
                    break;
                }
            }
            aText.sort(function (text1, text2) {
                return text1 > text2 ? 1 : text1 < text2 ? -1 : 0;
            });
            $('#div_texto a[leco="leco"]').each(function (i, o) {
                o.innerText = aText[i];
            });
            if (pAfter != null && $('a[show="0"]').length > 3) {
                $('a[show="0"]').hide();
                $('<p id="p_show"><a href="javascript:void(0);" onclick="exibirLinks()">Exibir mais...</a></p>').insertAfter($(pAfter));
                $('<p id="p_hide" style="display:none"><a href="javascript:void(0);" onclick="esconderLinks()">Exibir menos...</a></p>').insertAfter($(pAfter));
            }
        }
        function exibirLinks() {
            $('a[show="0"]').show();
            $('#p_hide').show();
            $('#p_show').hide();
        }
        function esconderLinks() {
            $('a[show="0"]').hide();
            $('#p_hide').hide();
            $('#p_show').show();
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
        });
    </script>
    <style type="text/css">
        .control{width:100%; float:left;}
        .print{float:right;}
        .compilado{float:left;}
        body, p, h1{font-family:Tahoma;font-size:14px;}
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