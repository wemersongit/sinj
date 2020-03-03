<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Download.aspx.cs" Inherits="TCDF.Sinj.Portal.Web.Download" %>
<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=Edge" />
    <meta name="robots" content="noindex,nofollow" />
    <meta name="googlebot" content="noindex,nofollow" />
    <meta http-equiv="Content-Language" content="pt-br" />
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <title>.:: SINJ-DF - Sistema Integrado de Normas Jurídicas do DF ::.</title>
    <style type="text/css">
        .print{float:right;}
        body, p, h1{font-family:Tahoma;font-size:14px;}
    </style>
    <script type="text/javascript">
        function Print() {
            $('div.control').hide();
            window.print();
            $('div.control').show();
        }
    </script>
</head>
<body>
    <a href="<%= ResolveUrl("~/") %>" title="Visitar o SINJ-DF - Sistema Integrado de Normas Jurídicas do DF"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/logo_sinj.png" alt="SINJ-DF" /></a>
    <div class="print">
        <a href="javascript:void(0);" onclick="javascript:Print();" title="Imprimir">
            <img alt="print" src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_print_p.png"/>
        </a>
    </div>
    <div runat="server" id="div_texto"></div>
</body>
</html>
