<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Norma.aspx.cs" Inherits="TCDF.Sinj.Web.Norma" %>
<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="robots" content="noindex,nofollow" />
    <meta name="googlebot" content="noindex,nofollow" />
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
        $(document).ready(function () {
            if ($('#div_erro').length > 0) {
                $('#div_norma').hide();
            }
            else if ($('p[replaced_by]').length <= 0 || $('p').length == $('p[replaced_by]').length) {
                $('.compilado').hide();
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