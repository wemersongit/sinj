<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TextoArquivoNorma.aspx.cs" Inherits="TCDF.Sinj.Web.TextoArquivoNorma" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Texto do Arquivo da Norma</title>
    <meta http-equiv="X-UA-Compatible" content="IE=Edge" />
    <meta http-equiv="Content-Language" content="pt-br" />
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <meta name="robots" content="noindex,nofollow" />
    <meta name="googlebot" content="noindex,nofollow" />
    
    <link rel="shortcut icon" type="image/x-icon" href="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/favicon.png" />


    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/jquery-1.11.0.min.js"></script>
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/platform.js"></script>
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/jquery.printElement.js"></script>

    <link rel="stylesheet" type="text/css" href="<%= TCDF.Sinj.Util._urlPadrao %>/Styles/sinj.css?<%= TCDF.Sinj.Util.MostrarVersao() %>" />

    <script type="text/javascript" language="javascript">
        function Print() {
            $('#div_texto').printElement();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div style="text-align:right;margin-right:50px;">
            <a id="a_print" runat="server" href="javascript:void(0);" onclick="javascript:Print();" title="Imprimir"><img alt="print" src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_print_p.png"/></a>
        </div>
        <div id="div_texto" runat="server"></div>
    </form>
</body>
</html>
