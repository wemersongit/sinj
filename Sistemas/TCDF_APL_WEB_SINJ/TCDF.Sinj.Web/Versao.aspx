<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Versao.aspx.cs" Inherits="TCDF.Sinj.Web.Versao" %>
<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="robots" content="noindex,nofollow" />
    <meta name="googlebot" content="noindex,nofollow" />
    <title>Arquivo da Norma</title>
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/jquery-1.11.0.min.js"></script>
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/platform.js"></script>
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/jquery.printElement.js"></script>

    <script type="text/javascript" language="javascript">
        function Print() {
            $('#div_texto').printElement();
        }
        $(document).ready(function () {
            if ($('#div_erro').length > 0) {
                $('#a_print').hide();
            }
        });
    </script>
</head>
<body>
        <div style="text-align:right;margin-right:50px;">
            <a id="a_print" runat="server" href="javascript:void(0);" onclick="javascript:Print();" title="Imprimir"><img alt="print" src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_print_p.png"/></a>
        </div>
        <div id="div_texto" runat="server"></div>
</body>
</html>
